using UnityEngine;
using Spine.Unity;
using System.Collections;

namespace EternalFlame
{
    public class CharacterController : MonoBehaviour
    {
        [SerializeField] InputSystem_Actions inputActions;
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float jumpForce = 15f;
        [SerializeField] FiniteStatmachine finite_Statmachine;
        [SerializeField] Rigidbody2D rb;
        [SerializeField] Transform FlipTransform;

        [SerializeField] float check;
        [SerializeField] bool isGrounded = false;
        private bool isAttacking = false;

        void Start()
        {
            check = rb.linearVelocityY;
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
            // Kiểm tra xem nhân vật có đang tấn công không, nếu có thì không cho phép chuyển trạng thái khác
            if (isAttacking)
            {
                return;
            }

            Vector2 movementInput = inputActions.Player.Move.ReadValue<Vector2>();
            if (movementInput.x != 0)
            {
                finite_Statmachine.SetState(State.Running);
                if (movementInput.x > 0 && FlipTransform.localScale.x < 0)
                {
                    FlipTransform.localScale = new Vector3(-FlipTransform.localScale.x, FlipTransform.localScale.y, FlipTransform.localScale.z);
                }
                else if (movementInput.x < 0 && FlipTransform.localScale.x > 0)
                {
                    FlipTransform.localScale = new Vector3(-FlipTransform.localScale.x, FlipTransform.localScale.y, FlipTransform.localScale.z);
                }
            }
            else
            {
                if (isGrounded)
                    finite_Statmachine.SetState(State.Idle);
                else if (rb.linearVelocity.y < -0.1f && !isGrounded)
                {
                    finite_Statmachine.SetState(State.Jumping_Up);
                   
                }
                else
                {
                     finite_Statmachine.SetState(State.Jumping_Down);
                }
            }

            rb.linearVelocity = new Vector2(movementInput.x * moveSpeed, rb.linearVelocity.y);

            if (inputActions.Player.Jump.triggered && Mathf.Abs(rb.linearVelocity.y) < 0.1f)
            {
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
               
            }

            if (inputActions.Player.Attack.triggered)
            {
                StartCoroutine(HandleAttack());
            }
        }

        private IEnumerator HandleAttack()
        {
            isAttacking = true;
            finite_Statmachine.SetState(State.Attacking);

            // Giả lập thời gian thực hiện hành động tấn công (thời gian này nên khớp với thời lượng animation tấn công)
            yield return new WaitForSeconds(0.5f);

            isAttacking = false;
            finite_Statmachine.SetState(State.Idle); // Quay lại trạng thái Idle sau khi tấn công xong
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {

                isGrounded = true;
            }

        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
            }
        }
    }
}
