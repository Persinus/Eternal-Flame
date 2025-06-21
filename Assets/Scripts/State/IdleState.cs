using UnityEngine;
using Spine.Unity;
using EternalFlame;

public class IdleState : IState
{
    private AnimationAudioFacade animationAudioFacade;
    private float idleTimer = 0f;
    private const float idleInterval = 10f;

    public IdleState(AnimationAudioFacade animationAudioFacade)
    {
        this.animationAudioFacade = animationAudioFacade;
    }

    public void Enter()
    {
        idleTimer = 0f;
        animationAudioFacade.PlayAnimationWithSound("holdon");
    }

    public void Exit() { }

    public void Update()
    {
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleInterval)
        {
            animationAudioFacade.PlayAnimationWithSound("holdon", "Idle");
            idleTimer = 0f;
        }
    }
}