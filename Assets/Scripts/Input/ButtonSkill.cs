using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace EternalFlame
{
    public enum SkillType
    {
        None,
        Attack,
        Skill,
        UltimateSkill
    }

    public class ButtonSkill : MonoBehaviour
    {
        [Header("Skill Buttons")]
        public Button AttackButton;
        [SerializeField] public Button skillButton;
        [SerializeField] public Button ultimateSkillButton;

        [Header("Skill Durations")]
        public float attackDuration = 0.7f;
        public float skillDuration = 1.4f;
        public float ultimateSkillDuration = 2.9f;

        [Header("Cooldown Images")]
        [SerializeField] private Image attackFillImage;
        [SerializeField] private Image skillFillImage;
        [SerializeField] private Image ultimateFillImage;

        private SkillType activeSkill = SkillType.None;

        public event Action OnSkillSelected;
        public event Action OnUltimateSkillSelected;
        public event Action OnSkillDeselected;
        

        private void Start()
        {
            skillButton.onClick.AddListener(() => TrySelectSkill(SkillType.Skill));
            ultimateSkillButton.onClick.AddListener(() => TrySelectSkill(SkillType.UltimateSkill));
            AttackButton.onClick.AddListener(() => TrySelectSkill(SkillType.Attack));
        }

        private void TrySelectSkill(SkillType skillType)
        {
            if (!IsSkillReady(skillType))
            {
                // Đổi màu fill thành đỏ để báo hiệu chưa hồi xong
                SetFillColor(skillType, Color.red);
                // Có thể thêm hiệu ứng rung hoặc âm báo ở đây
                return;
            }
            SetFillColor(skillType, Color.white); // Đặt lại màu fill về bình thường
            SelectSkill(skillType);
        }

        private bool IsSkillReady(SkillType skillType)
        {
            switch (skillType)
            {
                case SkillType.Attack: return attackFillImage != null && attackFillImage.fillAmount >= 1f;
                case SkillType.Skill: return skillFillImage != null && skillFillImage.fillAmount >= 1f;
                case SkillType.UltimateSkill: return ultimateFillImage != null && ultimateFillImage.fillAmount >= 1f;
                default: return false;
            }
        }

        private void SetFillColor(SkillType skillType, Color color)
        {
            switch (skillType)
            {
                case SkillType.Attack: if (attackFillImage != null) attackFillImage.color = color; break;
                case SkillType.Skill: if (skillFillImage != null) skillFillImage.color = color; break;
                case SkillType.UltimateSkill: if (ultimateFillImage != null) ultimateFillImage.color = color; break;
            }
        }

        private void SelectSkill(SkillType skillType)
        {
            if (activeSkill == skillType)
            {
                DeselectSkill();
                return;
            }

            if (activeSkill != SkillType.None)
            {
                DeselectSkill();
            }

            activeSkill = skillType;

            if (skillType == SkillType.Skill)
            {
                OnSkillSelected?.Invoke();
                Debug.Log("Kỹ năng thông thường được chọn!");
            }
            else if (skillType == SkillType.UltimateSkill)
            {
                OnUltimateSkillSelected?.Invoke();
                Debug.Log("Kỹ năng tối thượng được chọn!");
            }
            else if (skillType == SkillType.Attack)
            {
                Debug.Log("Tấn công thường được chọn!");
            }
        }

        private void DeselectSkill()
        {
            if (activeSkill == SkillType.Skill)
            {
                Debug.Log("Hủy kỹ năng thông thường!");
            }
            else if (activeSkill == SkillType.UltimateSkill)
            {
                Debug.Log("Hủy kỹ năng tối thượng!");
            }
            else if (activeSkill == SkillType.Attack)
            {
                Debug.Log("Hủy tấn công thường!");
            }

            OnSkillDeselected?.Invoke();
            activeSkill = SkillType.None;
        }

        public void StartCooldown(SkillType skillType, float duration)
        {
            Image img = null;
            switch (skillType)
            {
                case SkillType.Attack: img = attackFillImage; break;
                case SkillType.Skill: img = skillFillImage; break;
                case SkillType.UltimateSkill: img = ultimateFillImage; break;
            }
            if (img != null)
                StartCoroutine(FillAmountOverTime(duration, img));
        }

        private IEnumerator FillAmountOverTime(float t, Image fillImage)
        {
            float elapsed = 0f;
            fillImage.fillAmount = 0f;
            fillImage.color = Color.white;
            while (elapsed < t)
            {
                elapsed += Time.deltaTime;
                fillImage.fillAmount = Mathf.Clamp01(elapsed / t);
                Debug.Log($"Fill: {fillImage.fillAmount}"); // Thêm dòng này để kiểm tra
                yield return null;
            }
            fillImage.fillAmount = 1f;
            fillImage.color = Color.white;
        }
    }
}
