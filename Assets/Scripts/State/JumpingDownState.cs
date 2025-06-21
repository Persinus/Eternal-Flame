using UnityEngine;
using EternalFlame;

public class JumpingDownState : IState
{
    private AnimationAudioFacade animationAudioFacade;

    public JumpingDownState(AnimationAudioFacade animationAudioFacade)
    {
        this.animationAudioFacade = animationAudioFacade;
    }

    public void Enter()
    {
        animationAudioFacade.PlayAnimationWithSound("back");
    }

    public void Exit() { }

    public void Update() { }
}