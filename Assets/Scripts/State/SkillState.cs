using UnityEngine;
using EternalFlame;

public class SkillState : IState
{
    private AnimationAudioFacade animationAudioFacade;

    public SkillState(AnimationAudioFacade animationAudioFacade)
    {
        this.animationAudioFacade = animationAudioFacade;
    }

    public void Enter()
    {
        animationAudioFacade.PlayAnimationWithSound("skill", "Skill");
    }

    public void Exit() { }

    public void Update() { }
}