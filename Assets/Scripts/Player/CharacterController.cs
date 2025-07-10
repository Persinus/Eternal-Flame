using UnityEngine;
using Spine.Unity;
using System.Collections;
using LoM.Super;

namespace EternalFlame
{
    [SuperIcon(SuperBehaviourIcon.PlayerController)]
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] InputSystem_Actions inputActions;
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float jumpForce = 15f;
        [SerializeField] FiniteStatmachine finite_Statmachine;
        [SerializeField] Rigidbody2D rb;
        [SerializeField] Transform FlipTransform;
        private bool isGrounded = false;
        private bool isAttacking = false;
        private bool isSkilling = false;
        private bool isUltimateSkill = false;
        [SerializeField] bool isInputEnabled = false;

        private static CharacterController instance;
        public static CharacterController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<CharacterController>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject("CharacterController");
                        instance = obj.AddComponent<CharacterController>();
                        DontDestroyOnLoad(obj);
                        Debug.LogWarning("CharacterController instance was not found in the scene. A new instance has been created.");
                    }
                }
                return instance;
            }
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }

            // Khởi tạo inputActions ở đây
            inputActions = new InputSystem_Actions();

            // Đăng ký sự kiện từ ButtonSkill
            if (buttonSkill != null)
            {
                buttonSkill.OnSkillSelected += OnSkillButton;
                buttonSkill.OnUltimateSkillSelected += OnUltimateSkillButton;
                buttonSkill.AttackButton.onClick.AddListener(OnAttackButton); // Hoặc tạo event riêng nếu muốn
            }
        }

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("Rigidbody2D component is missing from the GameObject.");
            }
        }
        private void OnEnable()
        {
            if (!isInputEnabled)
            {
                inputActions.Player.Enable();
                inputActions.Player.Jump.performed += OnJumpPerformed;
                inputActions.Player.Move.performed += OnMovePerformed;
                isInputEnabled = true;
            }
        }

        private void OnDisable()
        {
            if (isInputEnabled)
            {
                inputActions.Player.Jump.performed -= OnJumpPerformed;
                inputActions.Player.Move.performed -= OnMovePerformed;
                inputActions.Player.Disable();
                isInputEnabled = false;
            }
        }

        [SerializeField] private JoystickInput joystickInput; // Kéo thả JoystickInput vào Inspector
        [SerializeField] private ButtonSkill buttonSkill; // Kéo thả ButtonSkill vào Inspector

        void Update()
        {
            Vector2 movementInput = Vector2.zero;

            // Ưu tiên lấy input từ Joystick nếu có
            if (joystickInput != null)
            {
                movementInput = joystickInput.GetMoveInput();

                // Nhảy nếu đẩy joystick lên trên và đang đứng đất
                if (movementInput.y > 0.5f && isGrounded)
                {
                    Jump();
                    finite_Statmachine.OnJump(isGrounded, rb.linearVelocity.y);
                }
            }
            else
            {
                movementInput = inputActions.Player.Move.ReadValue<Vector2>();

                if (inputActions.Player.Jump.triggered && isGrounded)
                {
                    Jump();
                    finite_Statmachine.OnJump(isGrounded, rb.linearVelocity.y);
                }
            }

            HandleMovement(movementInput);

          
        }

        private void HandleMovement(Vector2 movementInput)
        {
            // Di chuyển ngang
            rb.linearVelocity = new Vector2(movementInput.x * moveSpeed, rb.linearVelocity.y);

            // Xử lý trạng thái chạy/đứng yên
            if (Mathf.Abs(movementInput.x) > 0.01f)
            {
                finite_Statmachine.OnMove(movementInput, isGrounded);
                FlipCharacter(movementInput.x);
            }
            else
            {
                finite_Statmachine.OnMove(Vector2.zero, isGrounded);
            }
        }

        private void Jump(Vector2 movementInput = default)
        {

            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        }

        private void FlipCharacter(float direction)
        {
            float scaleX = Mathf.Abs(FlipTransform.localScale.x);
            FlipTransform.localScale = new Vector3(direction > 0 ? scaleX : -scaleX, FlipTransform.localScale.y, FlipTransform.localScale.z);
        }

        private IEnumerator HandleAttack(float duration)
        {
            isAttacking = true;
            finite_Statmachine.SetState(State.Attacking);
            yield return new WaitForSeconds(duration);

            isAttacking = false;
            finite_Statmachine.SetState(State.Idle);
        }

        private IEnumerator HandleSkill(float duration)
        {
            isSkilling = true;
            finite_Statmachine.SetState(State.Skill);
            yield return new WaitForSeconds(duration);
            isSkilling = false;
            finite_Statmachine.SetState(State.Idle);
        }
        private IEnumerator HandleUltimateSkill(float duration)
        {
            isUltimateSkill = true;
            finite_Statmachine.SetState(State.UltimateSkill);
            yield return new WaitForSeconds(duration);
            isUltimateSkill = false;
            finite_Statmachine.SetState(State.Idle);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
                isGrounded = true;
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
                isGrounded = false;
        }


  
        private void OnJumpPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (isGrounded)
            {
                Jump();
            }
        }

        private void OnMovePerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            Vector2 movementInput = context.ReadValue<Vector2>();
            HandleMovement(movementInput);
        }

      
        private void OnAttackButton()
        {
            if (!isAttacking)
                StartCoroutine(HandleAttack(buttonSkill.attackDuration));
        }

        private void OnSkillButton()
        {
            if (!isSkilling)
                StartCoroutine(HandleSkill(buttonSkill.skillDuration));
        }

        private void OnUltimateSkillButton()
        {
            if (!isUltimateSkill)
                StartCoroutine(HandleUltimateSkill(buttonSkill.ultimateSkillDuration));
        }
        void Destroy()
        {
            if (instance == this)
            {
                instance = null;
            }
            inputActions.Player.Disable();
            inputActions.Dispose();
        }
    }
}
