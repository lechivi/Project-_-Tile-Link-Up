using UnityEngine;
using TMPro;

public class MainMenuPanel : BaseUIElement
{
    [Header("MAIN MENU PANEL")]
    [SerializeField] private TMP_Text pointText;
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text currentLevelText;

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.pointText == null)
            this.pointText = transform.Find("TOP/ListLabel/PointLabel").GetComponentInChildren<TMP_Text>();   
        
        if (this.coinText == null)
            this.coinText = transform.Find("TOP/ListLabel/CoinLabel").GetComponentInChildren<TMP_Text>();   
        
        if (this.currentLevelText == null)
            this.currentLevelText = transform.Find("CurrentLevelPanel/CurrentLevel_Text").GetComponent<TMP_Text>();
    }

    public override void Show(object data)
    {
        base.Show(data);
        if (GameManager.HasInstance)
        {
            this.pointText.SetText(GameManager.Instance.Point.ToString());
            this.coinText.SetText(GameManager.Instance.Coin.ToString());
            this.currentLevelText.SetText(GameManager.Instance.CurrentLevel.ToString());
        }
    }

    public void OnClickPlayButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON_BUTTON11);
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.ScenePlayGame();
        }
    }

    public void OnClickSettingButton()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON_BUTTON11);
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.SettingPanel.Show(null);
        }
    }
}
