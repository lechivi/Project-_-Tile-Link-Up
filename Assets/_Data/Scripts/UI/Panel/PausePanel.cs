using UnityEngine;
using UnityEngine.UI;

public class PausePanel : BaseUIElement
{
    [Header("PAUSE PANEL")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgmSlider;

    private float bgmValue;
    private float sfxValue;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.sfxSlider == null)
            this.sfxSlider = transform.Find("Container/AudioList/SFX").GetComponentInChildren<Slider>();

        if (this.bgmSlider == null)
            this.bgmSlider = transform.Find("Container/AudioList/BGM").GetComponentInChildren<Slider>();
    }

    public override void Show(object data)
    {
        base.Show(data);
        this.SetupValueAudio();
    }

    private void SetupValueAudio()
    {
        this.bgmValue = PlayerPrefs.GetFloat(CONST.BGM_VOLUME_KEY, CONST.BGM_VOLUME_DEFAULT);
        this.sfxValue = PlayerPrefs.GetFloat(CONST.SFX_VOLUME_KEY, CONST.SFX_VOLUME_DEFAULT);
        this.bgmSlider.value = this.bgmValue;
        this.sfxSlider.value = this.sfxValue;
    }

    public void OnSliderChangeBGMValue(float value)
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.ChangeBGMVolume(value);
        }
    }

    public void OnSliderChangeSFXValue(float value)
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.ChangeSFXVolume(value);
        }
    }

    public void OnClickCloseButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON_BUTTON11);
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
        }

        this.Hide();
    }

    public void OnClickMainMenuButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON_BUTTON11);
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.SceneMainMenu();
        }

        this.Hide();
    }

}
