using UnityEngine;
using TMPro;

public class GamePanel : BaseUIElement
{
    [Header("GAME PANEL")]
    [SerializeField] private TMP_Text pointText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private ComboPanel comboSlider;

    public TMP_Text PointText { get => this.pointText; set => this.pointText = value; }
    public TMP_Text TimerText { get => this.timerText; set => this.timerText = value; }
    public TMP_Text LevelText { get => this.levelText; set => this.levelText = value; }
    public ComboPanel ComboPanel { get => this.comboSlider; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.pointText == null)
            this.pointText = transform.Find("TOP/ListLabel/PointLabel").GetComponentInChildren<TMP_Text>();

        if (this.timerText == null)
            this.timerText = transform.Find("TOP/ListLabel/TimerLabel").GetComponentInChildren<TMP_Text>();

        if (this.levelText == null)
            this.levelText = transform.Find("TOP/ListLabel/LevelLabel").GetComponentInChildren<TMP_Text>();

        if (this.comboSlider == null)
            this.comboSlider = GetComponentInChildren<ComboPanel>();
    }

    public override void Show(object data)
    {
        base.Show(data);

        this.comboSlider.Hide();

        if (GameManager.HasInstance)
        {
            this.levelText.SetText("Lv." + GameManager.Instance.CurrentLevel);
        }

        if (GameplayManager.HasInstance)
        {
            this.pointText.SetText(GameplayManager.Instance.CollectedPoint.ToString());
        }

    }

    #region BUTTON ON CLICK
    public void OnClickPauseButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON_BUTTON11);
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.PausePanel.Show(null);
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.PauseGame();
        }
    }

    public void OnClick_ReturnLastSkillButton()
    {
        if (GameplayManager.HasInstance)
        {
            if (!GameplayManager.Instance.ReturnLastSkill.CheckCanPlay()) return;

            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySFX(AUDIO.SFX_SKILL_RETURNLAST_DOWN_1);
            }

            GameplayManager.Instance.ReturnLastSkill.PlaySkill();
        }
    }

    public void OnClick_TipSkillButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_SKILL_TIP_HEAL_01);
        }
    }

    public void OnClick_BounceUpSkillButton()
    {
        if (GameplayManager.HasInstance)
        {
            if (!GameplayManager.Instance.BounceUpSkill.CheckCanPlay()) return;

            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySFX(AUDIO.SFX_SKILL_BOUNCEUP_UP_10);
            }
            GameplayManager.Instance.BounceUpSkill.PlaySkill();
        }
    }

    public void OnClick_FreezeTimeSkillButton()
    {
        if (GameplayManager.HasInstance)
        {
            if (!GameplayManager.Instance.BounceUpSkill.CheckCanPlay()) return;

            if (AudioManager.HasInstance)
            {
                AudioManager.Instance.PlaySFX(AUDIO.SFX_SKILL_FREEZETIME_ICE_BUFF_05);
            }

            GameplayManager.Instance.FreezeTimeSkill.PlaySkill();
        }
    }
    #endregion
}
