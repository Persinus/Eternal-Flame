using UnityEngine;
using EternalFlame;

public class DeadState : IState
{
    private AnimationAudioFacade animationAudioFacade;

    public DeadState(AnimationAudioFacade animationAudioFacade)
    {
        this.animationAudioFacade = animationAudioFacade;
    }

    public void Enter()
    {
        // Chỉ phát animation, không phát audio
        animationAudioFacade.PlayAnimationWithSound("death");
    }

    public void Exit() { }

    public void Update() { }
}