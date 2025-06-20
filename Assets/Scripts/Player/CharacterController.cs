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
        [SerializeField] float skillDuration;
        [SerializeField] float attackDuration;

        private bool isGrounded = false;
        private bool isAttacking = false;
        private bool isSkilling = false;

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
            if (isAttacking || isSkilling) return;

            Vector2 movementInput = inputActions.Player.Move.ReadValue<Vector2>();
            HandleMovement(movementInput);

            if (inputActions.Player.Jump.triggered && isGrounded)
            {
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }

            if (inputActions.Player.Attack.triggered)
            {
                StartCoroutine(HandleAttack());
            }
            else if (inputActions.Player.Skill.triggered)
            {
                StartCoroutine(HandleSkill());
            }
        }

        private void HandleMovement(Vector2 movementInput)
        {
            rb.linearVelocity = new Vector2(movementInput.x * moveSpeed, rb.linearVelocity.y);

            if (movementInput.x != 0)
            {
                finite_Statmachine.SetState(State.Running);
                FlipCharacter(movementInput.x);
            }
            else
            {
                if (isGrounded)
                    finite_Statmachine.SetState(State.Idle);
                else
                    finite_Statmachine.SetState(rb.linearVelocity.y < -0.1f ? State.Jumping_Up : State.Jumping_Down);
            }
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
