using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace EternalFlame
{
    /// <summary>
    /// Joystick điều khiển di chuyển cho mobile hoặc PC.
    /// Kéo thả vào Canvas, gán handle và background trên Inspector.
    /// </summary>
    public class Joystick : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
    {
        [Header("Joystick UI")]
        [Tooltip("Nút di chuyển của joystick")]
        [SerializeField] private RectTransform handle; // Nút di chuyển của joystick

        [Tooltip("Nền joystick")]
        [SerializeField] private RectTransform background; // Nền joystick

        [Header("Debug")]
        [Tooltip("Vector input hiện tại (chỉ đọc)")]
        [SerializeField] private Vector2 debugInputVector; // Để xem giá trị input trên Inspector

        [SerializeField] private float deadZone = 0.1f;

        private Vector2 inputVector = Vector2.zero;

        /// <summary>
        /// Đọc giá trị input từ joystick (chuẩn hóa từ -1 đến 1).
        /// </summary>
        public Vector2 InputVector
        {
            get
            {
                return inputVector.magnitude < deadZone ? Vector2.zero : inputVector;
            }
        }

        public event Action<Vector2> OnInputChanged;

        public void OnBeginDrag(PointerEventData eventData)
        {
            UpdateJoystickPosition(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            UpdateJoystickPosition(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ResetJoystick();
        }

        /// <summary>
        /// Cập nhật vị trí joystick và giá trị input khi kéo.
        /// </summary>
        private void UpdateJoystickPosition(PointerEventData eventData)
        {
            Vector2 pos;
            // Chuyển đổi vị trí chuột/touch sang tọa độ local của background
            RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pos);

            float radius = background.sizeDelta.x / 2;
            inputVector = pos / radius;

            // Giới hạn inputVector trong phạm vi unit circle (-1 đến 1)
            inputVector = Vector2.ClampMagnitude(inputVector, 1);

            // Di chuyển handle theo input
            handle.anchoredPosition = inputVector * radius;

            // Gán giá trị debug để xem trên Inspector
            debugInputVector = inputVector;
            OnInputChanged?.Invoke(inputVector);
        }

        /// <summary>
        /// Reset joystick về vị trí gốc khi thả ra.
        /// </summary>
        private void ResetJoystick()
        {
            inputVector = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            debugInputVector = Vector2.zero;
        }
    }
}