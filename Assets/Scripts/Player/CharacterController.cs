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
        [SerializeField] private float attackDuration = 0.5f;
        [SerializeField] private float skillDuration = 1.0f;
        [SerializeField] private float ultimateSkillDuration = 2.0f;
        private bool isGrounded = false;
        private bool isAttacking = false;
        private bool isSkilling = false;
        private bool isUltimateSkill = false;

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
        }

        void Start()
        {
            inputActions = new InputSystem_Actions();
            inputActions.Enable();
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("Rigidbody2D component is missing from the GameObject.");
            }
        }

        void Update()
        {
            Vector2 movementInput = inputActions.Player.Move.ReadValue<Vector2>();
            HandleMovement(movementInput);

            if (inputActions.Player.Jump.triggered && isGrounded)
            {
                Jump();
                finite_Statmachine.OnJump(isGrounded, rb.linearVelocity.y);
            }

            if (inputActions.Player.Attack.triggered)
            {
                finite_Statmachine.OnAttack(attackDuration);
            }
            else if (inputActions.Player.Skill.triggered)
            {
                finite_Statmachine.OnSkill(skillDuration);
            }
            else if (inputActions.Player.UltimateSkill.triggered)
            {
                finite_Statmachine.OnUltimateSkill(ultimateSkillDuration);
            }
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

        private void Jump()
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
          
        }

        private void FlipCharacter(float direction)
        {
            float scaleX = Mathf.Abs(FlipTransform.localScale.x);
            FlipTransform.localScale = new Vector3(direction > 0 ? scaleX : -scaleX, FlipTransform.localScale.y, FlipTransform.localScale.z);
        }

        private IEnumerator HandleAttack()
        {
            isAttacking = true;
            finite_Statmachine.SetState(State.Attacking);
            yield return new WaitForSeconds(attackDuration);
           
            isAttacking = false;
            finite_Statmachine.SetState(State.Idle);
        }

        private IEnumerator HandleSkill()
        {
            isSkilling = true;
            finite_Statmachine.SetState(State.Skill);
            yield return new WaitForSeconds(skillDuration);
            isSkilling = false;
            finite_Statmachine.SetState(State.Idle);
        }
        private IEnumerator HandleUltimateSkill()
        {
            isUltimateSkill = true;
            finite_Statmachine.SetState(State.UltimateSkill);
            yield return new WaitForSeconds(skillDuration);
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 0.1f);
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 0.1f);
        }
    }
}
