using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WinPanel : BaseUIElement
{
    [Header("WIN PANEL")]
    [SerializeField] private TMP_Text collectScoreText;
    [SerializeField] private Button nextButton;
    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.collectScoreText == null)
            this.collectScoreText = transform.Find("Container/MIDDLE").GetComponentInChildren<TMP_Text>();

        if (this.nextButton == null)
            this.nextButton = transform.Find("Container/ButtonList/Next_Button").GetComponent<Button>();
    }

    public override void Show(object data)
    {
        if (GameplayManager.HasInstance)
        {
            this.collectScoreText.SetText("+" + GameplayManager.Instance.CollectedPoint);
        }

        if (GameManager.HasInstance)
        {
            if (GameManager.Instance.CurrentLevel - 1 == GameManager.Instance.Levels.Length)
            {
                this.nextButton.gameObject.SetActive(false);
            }
        }

        base.Show(data);
    }

    public void OnClickNextButton()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlaySFX(AUDIO.SFX_BUTTON_BUTTON11);
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.ResumeGame();
        }

        if (GameplayManager.HasInstance)
        {
            GameplayManager.Instance.PoolTile.SetInactiveTile();
            GameplayManager.Instance.SetupLevel();
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.HideAllPanel();
            UIManager.Instance.GamePanel.Show(null);
        }

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
    }
}
