using UnityEngine;
using EternalFlame;

public class HitState : IState
{
    private AnimationAudioFacade animationAudioFacade;

    public HitState(AnimationAudioFacade animationAudioFacade)
    {
        this.animationAudioFacade = animationAudioFacade;
    }

    public void Enter()
    {
        animationAudioFacade.PlayAnimationWithSound("behit");
    }

    public void Exit() { }

    public void Update() { }
}