using UnityEngine;
using TMPro; // Thêm thư viện TextMeshPro để sử dụng TextMeshProUGUI
namespace EternalFlame 
{
    public class CountdownTimer : MonoBehaviour
    {
     [SerializeField] private TextMeshProUGUI countdownText; // Kéo thả TextMeshPro vào đây
    [SerializeField] private int startMinutes = 5; // Phút bắt đầu

    private float currentTime; // Thời gian hiện tại (tính bằng giây)
    private bool isCountingDown = true;

    void Start()
    {
        // Chuyển đổi phút sang giây
        currentTime = startMinutes * 60;
        UpdateCountdownDisplay();
    }

    void Update()
    {
        if (isCountingDown)
        {
            // Giảm thời gian theo thời gian thực
            currentTime -= Time.deltaTime;

            // Nếu hết thời gian, dừng lại ở 0
            if (currentTime <= 0)
            {
                currentTime = 0;
                isCountingDown = false;
            }

            UpdateCountdownDisplay();
        }
    }

    void UpdateCountdownDisplay()
    {
        // Tính số phút và giây còn lại
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // Cập nhật TextMeshPro
        countdownText.text = $"{minutes:D2}:{seconds:D2}";
    }
    }
}
