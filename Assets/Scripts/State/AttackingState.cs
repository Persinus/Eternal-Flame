using UnityEngine;
using EternalFlame;

public class AttackingState : IState
{
    private AnimationAudioFacade animationAudioFacade;
    
    public AttackingState(AnimationAudioFacade animationAudioFacade, MonoBehaviour coroutineRunner)
    {
    
        this.animationAudioFacade = animationAudioFacade;
    }

    public void Enter()
    {
        animationAudioFacade.PlayAnimationWithSound("att", "Attack");
        
         
        
    }

    public void Exit() { }

    public void Update() { }
}