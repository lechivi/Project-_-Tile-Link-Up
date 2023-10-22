using UnityEngine;

public class UIManager : BaseManager<UIManager>
{
    [SerializeField] private MainMenuPanel mainMenuPanel;
    [SerializeField] private GamePanel gamePanel;
    [SerializeField] private SettingPanel settingPanel;
    [SerializeField] private PausePanel pausePanel;
    [SerializeField] private WinPanel winPanel;
    [SerializeField] private LosePanel losePanel;
    [SerializeField] private LoadingPanel loadingPanel;

    public MainMenuPanel MainMenuPanel { get => this.mainMenuPanel; }
    public GamePanel GamePanel { get => this.gamePanel; }
    public SettingPanel SettingPanel { get => this.settingPanel; }
    public PausePanel PausePanel { get => this.pausePanel; }
    public WinPanel WinPanel { get => this.winPanel; }
    public LosePanel LosePanel { get => this.losePanel; }
    public LoadingPanel LoadingPanel { get => this.loadingPanel; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.mainMenuPanel == null)
            this.mainMenuPanel = GetComponentInChildren<MainMenuPanel>();

        if (this.gamePanel == null)
            this.gamePanel = GetComponentInChildren<GamePanel>();

        if (this.settingPanel == null)
            this.settingPanel = GetComponentInChildren<SettingPanel>();

        if (this.pausePanel == null)
            this.pausePanel = GetComponentInChildren<PausePanel>();

        if (this.winPanel == null)
            this.winPanel = GetComponentInChildren<WinPanel>();

        if (this.losePanel == null)
            this.losePanel = GetComponentInChildren<LosePanel>();

        if (this.loadingPanel == null)
            this.loadingPanel = GetComponentInChildren<LoadingPanel>();
    }

    public void HideAllPanel()
    {
        this.mainMenuPanel.Hide();
        this.gamePanel.Hide();
        this.settingPanel.Hide();
        this.pausePanel.Hide();
        this.winPanel.Hide();
        this.losePanel.Hide();
        this.loadingPanel.Hide();
    }

}
