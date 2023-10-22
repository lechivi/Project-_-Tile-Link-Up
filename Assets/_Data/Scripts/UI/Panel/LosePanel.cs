using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : BaseUIElement
{
    public void OnClickRetryButton()
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

        if (GameplayManager.HasInstance)
        {
            GameplayManager.Instance.PoolTile.SetInactiveTile();
        }

        if (GameManager.HasInstance)
        {
            GameManager.Instance.SceneMainMenu();
        }
    }
}
