using UnityEngine;
using UnityEngine.InputSystem;
namespace EternalFlame
{
    public class JoystickInput : MonoBehaviour
    {
        [SerializeField] private Joystick joystick; // Gắn Joystick UI

        private InputSystem_Actions playerInput; // Input System PlayerInput
        private Vector2 moveInput;
      

        private void Awake()
        {
            playerInput = new InputSystem_Actions();
            playerInput.Enable(); // Bật Input Actions
        }

        private void Update()
        {
            // Đọc giá trị từ joystick
            moveInput = joystick.InputVector;
            // Override giá trị Input System bằng giá trị từ joystick       
        }

     
        public Vector2 GetMoveInput()
        {
            return moveInput; // Để sử dụng trong các script khác
        }
    }
}
