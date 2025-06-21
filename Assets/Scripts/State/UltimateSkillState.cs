using UnityEngine;
using EternalFlame;
using System.Collections;

public class UltimateSkillState : IState
{
    private AnimationAudioFacade animationAudioFacade;
    private MonoBehaviour coroutineRunner;

    public UltimateSkillState(AnimationAudioFacade animationAudioFacade, MonoBehaviour coroutineRunner)
    {
        this.animationAudioFacade = animationAudioFacade;
        this.coroutineRunner = coroutineRunner;
    }

    public void Enter()
    {
        animationAudioFacade.PlayAnimationWithSound("ultimateskill","UltimateSkill");
        if (coroutineRunner != null)
            coroutineRunner.StartCoroutine(UltimateSkillEffectCoroutine());
    }

    private IEnumerator UltimateSkillEffectCoroutine()
    {
        yield return new WaitForSeconds(1.2f); // Chờ 1.2 giây trước khi tạo hiệu ứng
        if (coroutineRunner == null)
        {
            Debug.LogError("Coroutine runner is null. Cannot instantiate effect.");
            yield break;
        }
        // Đặt hiệu ứng là con của GameObject chứa coroutineRunner
        GameObject effect = Object.Instantiate(
            Resources.Load<GameObject>("UltimateSkillEffect"),
            coroutineRunner.transform // parent
        );
        yield return new WaitForSeconds(1f);
        Object.Destroy(effect);
    }

    public void Exit() { }
    public void Update() { }
}