using UnityEngine;
using Spine.Unity;
using LoM.Super;

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
        Ultimate_Skill,
        Skill,
        Running,
        Jumping_Up,
        Jumping_Down,
    
        Attacking,

        Hit,
        Dead,
    }
    [SuperIcon(SuperBehaviourIcon.Animation)]
    public class FiniteStatmachine : MonoBehaviour
    {
        [SerializeField] private State currentState = State.Idle;
        [SerializeField] SkeletonAnimation skeletonAnimation;

        [SerializeField] Spine.AnimationState spineAnimationState;


        // Update is called once per frame
        void Update()
        {
            // Handle state transitions and logic here
            switch (currentState)
            {
                case State.Idle:
                    skeletonAnimation.AnimationName = "holdon";

                    break;
                case State.Ultimate_Skill:
                    skeletonAnimation.AnimationName = "ultimate_skill";
                    break;
                case State.Skill:
                    skeletonAnimation.AnimationName = "skill";
                    break;
                case State.Running:
                    skeletonAnimation.AnimationName = "move";
                    break;
                case State.Jumping_Up:
                    skeletonAnimation.AnimationName = "go";
                    break;
                case State.Jumping_Down:
                    skeletonAnimation.AnimationName = "back";
                    break;
                case State.Attacking:
                    skeletonAnimation.AnimationName = "att";
                    break;
                case State.Hit:
                    skeletonAnimation.AnimationName = "behit";
                    break;
                case State.Dead:
                    skeletonAnimation.AnimationName = "death";
                    break;
            }

        }
        public void SetState(State newState)
        {
            if (currentState != newState)
            {
                currentState = newState;
                // Optionally, you can add logic here to handle state entry/exit
                // For example, you might want to reset certain variables or trigger events
            }
        }
    }
}
