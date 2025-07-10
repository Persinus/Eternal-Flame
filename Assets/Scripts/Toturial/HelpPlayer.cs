using UnityEngine;
using DG.Tweening;

namespace EternalFlame
{
    public class HelpPlayer : MonoBehaviour
    {
        [Header("Thời gian & điểm di chuyển")]
        [SerializeField] private float duration = 0.1f;
        [SerializeField] private Transform startPoint;
        [SerializeField] private Transform midPoint;
        [SerializeField] private GameObject bird;

        [Header("Hệ thống thoại")]
        [SerializeField] private DialogueController dialogueController;

        void Start()
        {
            bird.transform.position = startPoint.position;

            // Di chuyển đến MidPoint
            bird.transform.DOMove(midPoint.position, duration)
                .SetEase(Ease.InOutSine)
                .OnComplete(() =>
                {
                    // Tới Mid → bắt đầu thoại turn 1
                    dialogueController.PlayTurn(1, () =>
                    {
                        // Sau khi thoại xong → quay về lại Start
                        bird.transform.DOMove(startPoint.position, duration).SetEase(Ease.InOutSine);
                    });
                });
        }
    }
}
