using UnityEngine;
using EternalFlame;

public class RunningState : IState
{
    private AnimationAudioFacade animationAudioFacade;
    private float runTimer = 0f;
    private const float runInterval = 10f;

    public RunningState(AnimationAudioFacade animationAudioFacade)
    {
        this.animationAudioFacade = animationAudioFacade;
    }

    public void Enter()
    {
        runTimer = 0f;
        animationAudioFacade.PlayAnimationWithSound("move");
    }

    public void Exit() { }

    public void Update()
    {
        runTimer += Time.deltaTime;
        if (runTimer >= runInterval)
        {
            animationAudioFacade.PlayAnimationWithSound("move", "Run");
            runTimer = 0f;
        }
    }
}