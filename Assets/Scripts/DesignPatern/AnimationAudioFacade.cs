using UnityEngine;
using Spine.Unity;

namespace EternalFlame
{
    public class AnimationAudioFacade
    {
        private readonly SkeletonAnimation skeletonAnimation;
        private readonly AudioCharacter audioCharacter;

        public AnimationAudioFacade(SkeletonAnimation skeletonAnimation, AudioCharacter audioCharacter = null)
        {
            if (skeletonAnimation == null)
            {
                throw new System.ArgumentNullException(nameof(skeletonAnimation), "SkeletonAnimation is required.");
            }

            this.skeletonAnimation = skeletonAnimation;
            this.audioCharacter = audioCharacter; // AudioCharacter là tùy chọn
        }

        /// <summary>
        /// Phát animation và âm thanh (nếu có AudioCharacter).
        /// </summary>
        /// <param name="animationName">Tên animation.</param>
        /// <param name="audioKey">Key của AudioClip trong AudioCharacter.</param>
        public void PlayAnimationWithSound(string animationName, string audioKey = null)
        {
            skeletonAnimation.AnimationName = animationName;

            if (audioCharacter != null && !string.IsNullOrEmpty(audioKey))
            {
                audioCharacter.PlaySound(audioKey);
            }
        }

        public void StopSound()
        {
            if (audioCharacter != null)
            {
                audioCharacter.StopSound();
            }
        }
    }
}