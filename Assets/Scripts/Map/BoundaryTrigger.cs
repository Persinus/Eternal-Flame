using UnityEngine;
using TMPro;
using LoM.Super;
namespace EternalFlame
{
    [SuperIcon(SuperBehaviourIcon.Trigger)]
    public class BoundaryTrigger : MonoBehaviour
    {
        [SerializeField] string mapName; // Tên bản đồ mới
        [SerializeField] Canvas mapNameText; // Tham chiếu đến UI Canvas để hiển thị tên
        [SerializeField] TextMeshProUGUI mapNameTextComponent; // Tham chiếu đến TextMeshProUGUI để hiển thị tên bản đồ
        private void OnTriggerEnter2D(Collider2D other)
        {
            // Kiểm tra nếu đối tượng là người chơi
            if (other.CompareTag("Player"))
            {
                ShowMapName();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                HideMapName();
            }
        }

        void ShowMapName()
        {
            mapNameText.gameObject.SetActive(true); // Hiển thị Canvas
            mapNameTextComponent.text = mapName; // Cập nhật tên bản đồ
        }

        void HideMapName()
        {
            mapNameText.gameObject.SetActive(false); // Ẩn Canvas
        }
    }
}
