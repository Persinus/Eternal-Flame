using UnityEngine;
using Spine.Unity;
using System.Collections.Generic;
using LoM.Super;
using AYellowpaper.SerializedCollections;
using System.Collections;

namespace EternalFlame 
{
    /// <summary>
    /// FiniteStatmachine is a base class for implementing a finite state machine in Unity.
    /// /// It can be used to manage different states of a character or game object, allowing for
    /// /// transitions between states based on game logic.
    /// /// </summary>
    /// /// <remarks>
    /// /// This class is intended to be extended by other classes that implement specific states and
    /// /// state transitions. It provides a structure for managing states and can be used in conjunction
    /// /// with Unity's MonoBehaviour lifecycle methods.
    /// /// </remarks>
    public enum State
    {
        Idle,
        UltimateSkill,
        Skill,
        Running,
        Jumping_Up,
        Jumping_Down,
    
        Attacking,

        Hit,
        Dead,
    }
    [SuperIcon(SuperBehaviourIcon.Animation)]
    [System.Serializable]
    public class StateDictionary : SerializedDictionary<State, IState> { }
    public class FiniteStatmachine : MonoBehaviour
    {
        [SerializeField] private State currentState = State.Idle;
        [SerializeField] private SkeletonAnimation skeletonAnimation;
        [SerializeField] private AudioCharacter audioCharacter; // Thêm dòng này
      

        private AnimationAudioFacade animationAudioFacade; // Thêm dòng này
        private Coroutine stateCoroutine;

        private Dictionary<State, IState> stateMap;
        private IState currentStateObj;

        void Awake()
        {
            animationAudioFacade = new AnimationAudioFacade(skeletonAnimation, audioCharacter);

            stateMap = new Dictionary<State, IState>
            {
                { State.Idle, new IdleState(animationAudioFacade) },
                { State.UltimateSkill, new UltimateSkillState(animationAudioFacade, this) },
                { State.Skill, new SkillState(animationAudioFacade) },
                { State.Running, new RunningState(animationAudioFacade) },
                { State.Jumping_Up, new JumpingUpState(animationAudioFacade) },
                { State.Jumping_Down, new JumpingDownState(animationAudioFacade) },
                { State.Attacking, new AttackingState(animationAudioFacade,this ) },
                { State.Hit, new HitState(animationAudioFacade) },
                { State.Dead, new DeadState(animationAudioFacade) }
            };

            currentStateObj = stateMap[currentState];
            currentStateObj.Enter();
        }

        // Update is called once per frame
        void Update()
        {
            currentStateObj?.Update();
        }

        public void SetState(State newState)
        {
            if (currentState != newState)
            {
                currentStateObj?.Exit();
                currentState = newState;
                currentStateObj = stateMap[currentState];
                currentStateObj.Enter();
            }
        }

        public void OnMove(Vector2 movementInput, bool isGrounded)
        {
            if (currentState == State.Attacking || currentState == State.Skill || currentState == State.UltimateSkill) return;

            if (Mathf.Abs(movementInput.x) > 0.1f)
            {
                SetState(State.Running);
                // Gọi FlipCharacter nếu cần
            }
            else
            {
                // Chỉ chuyển về Idle nếu đang ở trên mặt đất
                if (isGrounded)
                    SetState(State.Idle);
            }
        }

        public void OnJump(bool isGrounded, float velocityY)
        {
            if (isGrounded && currentState != State.Attacking && currentState != State.Skill)
            {
                if (velocityY > 0.1f)
                {
                    SetState(State.Jumping_Up);
                }
                else if (velocityY < -0.1f)
                {
                    SetState(State.Jumping_Down);
                }
            }
        }

        public void OnAttack(float duration)
        {
            if (currentState != State.Attacking && currentState != State.Skill)
            {
                SetState(State.Attacking);
                if (stateCoroutine != null) StopCoroutine(stateCoroutine);
                stateCoroutine = StartCoroutine(ReturnToIdleAfterDelay(duration));
            }
        }

        public void OnSkill(float duration)
        {
            if (currentState != State.Attacking && currentState != State.Skill)
            {
                SetState(State.Skill);
                if (stateCoroutine != null) StopCoroutine(stateCoroutine);
                stateCoroutine = StartCoroutine(ReturnToIdleAfterDelay(duration));
            }
        }
        public void OnUltimateSkill(float duration)
        {
            if (currentState != State.Attacking && currentState != State.Skill && currentState != State.UltimateSkill)
            {
                SetState(State.UltimateSkill);
                if (stateCoroutine != null) StopCoroutine(stateCoroutine);
                stateCoroutine = StartCoroutine(ReturnToIdleAfterDelay(duration));
            }
        }

        private IEnumerator ReturnToIdleAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            SetState(State.Idle);
            stateCoroutine = null;
        }
    }
}
