using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : BaseManager<GameManager>
{
    private int currentLevel = 3;
    private int point = 0;
    private int coin = 1000;
    private LevelSO[] levels;

    private bool isPlaying;

    public int CurrentLevel { get => this.currentLevel; set => this.currentLevel = value; }
    public int Point
    {
        get => this.point;
        set
        {
            this.point = value;
            PlayerPrefs.SetInt("POINT", this.point);
        }
    }

    public int Coin { get => this.coin; set => this.coin = value; }
    public LevelSO[] Levels { get => this.levels; }
    public bool IsPlaying { get => this.isPlaying; }

    protected override void Awake()
    {
        base.Awake();
        this.point = PlayerPrefs.GetInt("POINT", 0);
        this.currentLevel = PlayerPrefs.GetInt("CURRENT LEVEL", 1);
        this.levels = Resources.LoadAll<LevelSO>(@"SO/LevelSO");
    }

    private void Start()
    {
        this.StartGame();
    }

    public void StartGame()
    {
        this.isPlaying = true;
        Time.timeScale = 1.0f;
    }

    public void PauseGame()
    {
        if (this.isPlaying)
        {
            this.isPlaying = false;
            Time.timeScale = 0.0f;
        }
    }

    public void ResumeGame()
    {
        this.isPlaying = true;
        Time.timeScale = 1.0f;
    }

    public void SceneMainMenu()
    {
        this.ResumeGame();

        this.LoadChangeScene(0);
    }

    public void ScenePlayGame()
    {
        this.StartGame();

        this.LoadChangeScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void LoadChangeScene(int sceneIndex)
    {
        StartCoroutine(UIManager.Instance.LoadingPanel.LoadScene(sceneIndex));
    }

    public void CompleteLevel(int collectPoint)
    {
        this.point += collectPoint;
        this.currentLevel += 1;

        if (this.currentLevel > this.levels.Length)
        {
            this.currentLevel = this.levels.Length;
        }
    }

}
