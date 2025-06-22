using UnityEngine;
using DG.Tweening; // Thư viện DOTween để hỗ trợ tweening
namespace EternalFlame
{
    public class BirdHelpPlayer : MonoBehaviour
    {
        [SerializeField] private float duration = 2f; // Thời gian di chuyển
        [SerializeField] private Transform startPoint; // Điểm bắt đầu
        [SerializeField] private Transform MidPoint; // Điểm giữa
        [SerializeField] private Transform endPoint; // Điểm đích

        void Start()
        {
            // Đặt đối tượng vào điểm bắt đầu
            transform.position = startPoint.position;

            // Di chuyển từ điểm bắt đầu tới điểm giữa
            transform.DOMove(MidPoint.position, duration).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                // Di chuyển từ điểm giữa tới điểm đích
                transform.DOMove(endPoint.position, duration).SetEase(Ease.InOutSine);
            });
        }
    }
}