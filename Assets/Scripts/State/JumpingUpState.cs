using UnityEngine;
using EternalFlame;

public class JumpingUpState : IState
{
    private AnimationAudioFacade animationAudioFacade;

    public JumpingUpState(AnimationAudioFacade animationAudioFacade)
    {
        this.animationAudioFacade = animationAudioFacade;
    }

    public void Enter()
    {
        animationAudioFacade.PlayAnimationWithSound("go");
    }

    public void Exit() { }

    public void Update() { }
}