using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using TMPro;

namespace EternalFlame
{

   
    public class DialogueController : MonoBehaviour
    {
        [Header("Gắn JSON chứa thoại")]
        public TextAsset dialogueJson;

        [Header("Manager và Transform nhân vật")]
        public BubbleManager bubbleManager;
        public Transform birdTransform;
    
        [Header("UI hiển thị thoại")]
        public TextMeshProUGUI dialogueTextUI;
        private Dictionary<string, Transform> speakerTransforms;
        private List<IGrouping<int, DialogueTurn>> groupedTurns;

        private void Awake()
        {
            speakerTransforms = new Dictionary<string, Transform>
            {
                { "Bird", birdTransform },

            };

            LoadAndGroupDialogue();
        }

        private void LoadAndGroupDialogue()
        {
            List<DialogueTurn> allTurns = JsonUtilityWrapper.FromJsonList<DialogueTurn>(dialogueJson.text);
            groupedTurns = allTurns
                .GroupBy(t => t.turn)
                .OrderBy(g => g.Key)
                .ToList();
        }

        // Gọi từ bên ngoài để bắt đầu lượt
        public void PlayTurn(int turnNumber, Action onComplete = null)
        {
            var turnGroup = groupedTurns.FirstOrDefault(g => g.Key == turnNumber);
            if (turnGroup != null)
            {
                StartCoroutine(PlayDialogueGroup(turnGroup, onComplete));
            }
            else
            {
                Debug.LogWarning("Không tìm thấy lượt thoại: " + turnNumber);
                onComplete?.Invoke();
            }
        }

        private IEnumerator PlayDialogueGroup(IEnumerable<DialogueTurn> turnsInThisGroup, Action onDone)
        {
            foreach (var turn in turnsInThisGroup)
            {
                Transform speakerTransform = GetSpeakerTransform(turn.speaker);

                foreach (var line in turn.lines)
                {
                    bubbleManager.CreateBubble<DialogueBubble>(
                        BubbleType.Dialogue,
                        line,
                        speakerTransform.position
                    );
                    if (dialogueTextUI != null)
                    {
                    dialogueTextUI.text = line;
                    }

                    yield return new WaitForSeconds(2f);
                }
            }

            onDone?.Invoke();
        }

        private Transform GetSpeakerTransform(string speaker)
        {
            return speakerTransforms.TryGetValue(speaker, out var t) ? t : transform;
        }
    }

    // Helper để parse mảng JSON (vì JsonUtility không hỗ trợ mảng gốc)
    public static class JsonUtilityWrapper
    {
        [Serializable]
        private class DialogueTurnListWrapper
        {
            public List<DialogueTurn> items;
        }

        public static List<DialogueTurn> FromJsonList<T>(string json)
        {
            // Bọc vào object "items" để JsonUtility đọc được
            string wrappedJson = "{\"items\":" + json + "}";
            return JsonUtility.FromJson<DialogueTurnListWrapper>(wrappedJson).items;
        }
    }

    [Serializable]
    public class DialogueTurn
    {
        public int turn;
        public string speaker;
        public List<string> lines;
        public bool waitForAction; // ✅ mới
    }
}
