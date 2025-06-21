using UnityEngine;
using AYellowpaper.SerializedCollections;

namespace EternalFlame
{
    [System.Serializable]
    public class AudioClipDictionary : SerializedDictionary<string, AudioClip> { }

    public class AudioCharacter : MonoBehaviour
    {
        [Tooltip("Serialized Dictionary quản lý âm thanh theo trạng thái.")]
        public AudioClipDictionary audioClips = new AudioClipDictionary();

        private AudioSource audioSource;

        private void Awake()
        {
            // Lấy AudioSource từ GameObject hiện tại
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource is required but not found on the GameObject.");
            }
        }

        /// <summary>
        /// Phát âm thanh dựa trên key.
        /// </summary>
        /// <param name="key">Tên key của AudioClip.</param>
        public void PlaySound(string key)
        {
            if (audioSource == null)
            {
                Debug.LogError("AudioSource is not assigned.");
                return;
            }

            if (audioClips.TryGetValue(key, out AudioClip clip))
            {
                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                Debug.LogWarning($"No audio clip found for key: {key}");
            }
        }

        /// <summary>
        /// Dừng phát âm thanh.
        /// </summary>
        public void StopSound()
        {
            if (audioSource != null)
            {
                audioSource.Stop();
            }
        }
    }
}
