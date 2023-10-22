using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BaseUIElement
{
    [Header("SETTING PANEL")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;

    private float bgmValue;
    private float sfxValue;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.bgmSlider == null )
            this.bgmSlider = transform.Find("Container/AudioList/BGM").GetComponentInChildren<Slider>();
        
        if (this.sfxSlider == null )
            this.sfxSlider = transform.Find("Container/AudioList/SFX").GetComponentInChildren<Slider>();    
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

        this.Hide();
    }

    public void OnClickQuitButton()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.QuitGame();
        }
    }
}
