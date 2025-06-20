using UnityEngine;
using System;
using System.Collections;

namespace EternalFlame
{
    public class CountDown : MonoBehaviour
    {
        public static float CountDownTime { get; private set; }

        /// <summary>
        /// Bắt đầu đếm ngược, gọi callback khi kết thúc.
        /// </summary>
        public void StartCountDown(float time, Action onComplete = null)
        {
            StopAllCoroutines();
            StartCoroutine(TimeCountDownCoroutine(time, onComplete));
        }

        private IEnumerator TimeCountDownCoroutine(float time, Action onComplete)
        {
            CountDownTime = time;
            while (CountDownTime > 0)
            {
                CountDownTime -= Time.deltaTime;
                yield return null;
            }
            CountDownTime = 0;
            onComplete?.Invoke();
        }
    }
}
