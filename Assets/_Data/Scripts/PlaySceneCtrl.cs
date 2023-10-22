using System.Collections;
using UnityEngine;

public class PlaySceneCtrl : MonoBehaviour
{
    public static PlaySceneCtrl instance;

    private bool isLoseGame = false;
    private bool isWinGame = false;

    private void Awake()
    {
        PlaySceneCtrl.instance = this;
    }

    private void Start()
    {
        if (AudioManager.HasInstance)
        {
            AudioManager.Instance.PlayBGM(AUDIO.BGM_ANVILLETOWN);
        }

        this.StartLevel();
    }

    private void Update()
    {
        if (this.CheckLoseGame() && !this.isLoseGame)
        {
            this.isLoseGame = true;
            Debug.Log("Lose");
            Invoke("DoubleCheckLoseGame", 1.2f);


        }

        if (this.CheckWinGame() && !this.isWinGame)
        {
            this.isWinGame = true;
            Invoke("WinGame", 0.75f);
        }
    }

    private bool CheckLoseGame()
    {
        if (TilesPanel.instance != null)
        {
            if (!TilesPanel.instance.IsHaveEmptySlot() && TilesPanel.instance.Matches().Count == 0)
            {
                return true;
            }
        }

        return false;
    }

    private void DoubleCheckLoseGame()
    {
        if (this.CheckLoseGame())
        {
            this.LoseGame();
        }
        else
        {
            this.isLoseGame = false;
        }
    }

    private void LoseGame()
    {

        if (GameManager.HasInstance)
        {
            GameManager.Instance.PauseGame();
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.LosePanel.Show(null);
        }
    }

    private bool CheckWinGame()
    {
        if (GameplayManager.HasInstance)
        {
            if (GameplayManager.Instance.PoolTile.TilesGame.Count == 0)
                return true;
        }

        return false;
    }

    private void WinGame()
    {
        if (GameManager.HasInstance)
        {
            GameManager.Instance.PauseGame();

            if (GameplayManager.HasInstance)
            {
                GameManager.Instance.CompleteLevel(GameplayManager.Instance.CollectedPoint);
            }
        }


        if (UIManager.HasInstance)
        {
            UIManager.Instance.WinPanel.Show(null);
        }
    }

    public void StartLevel()
    {
        if (GameplayManager.HasInstance)
        {
            GameplayManager.Instance.SetupLevel();
        }

        if (UIManager.HasInstance)
        {
            UIManager.Instance.HideAllPanel();
            UIManager.Instance.GamePanel.Show(null);
        }
    }
}
