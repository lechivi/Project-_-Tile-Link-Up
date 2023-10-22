using Unity.VisualScripting;
using UnityEngine;

public class GameplayManager : BaseManager<GameplayManager>
{
    [Header("PLAYTIME")]
    [SerializeField] private float playtime = 120f;
    [SerializeField] private bool playtimeCountingDown = false;

    [Header("COMBO")]
    [SerializeField] private int comboCount = 0;
    [SerializeField] private float comboCDTime = 10.0f;
    [SerializeField] private bool comboCountingDown = false;
    private float comboTimer;

    [Header("POINT")]
    [SerializeField] private int pointEachMatch = 3;
    private int collectedPoint;

    [Header("SETUP LEVEL")]
    [SerializeField] private PoolTile poolTile;
    [SerializeField] private LevelSO level;
    [SerializeField] private int matchCount = 3;

    [Header("SKILL")]
    [SerializeField] private ReturnLastSkill returnLastSkill;
    [SerializeField] private BounceUpSkill bounceUpSkill;
    [SerializeField] private FreezeTimeSkill freezeTimeSkill;

    public bool PlaytimeCountingDown { get => this.playtimeCountingDown; set => this.playtimeCountingDown = value; }
    public int CollectedPoint { get => this.collectedPoint; }
    public PoolTile PoolTile { get => this.poolTile; }
    public ReturnLastSkill ReturnLastSkill { get => this.returnLastSkill; }
    public BounceUpSkill BounceUpSkill { get => this.bounceUpSkill; }
    public FreezeTimeSkill FreezeTimeSkill { get => this.freezeTimeSkill; }

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.poolTile == null)
            this.poolTile = GetComponentInChildren<PoolTile>();

        if (this.returnLastSkill == null)
            this.returnLastSkill = GetComponentInChildren<ReturnLastSkill>();

        if (this.bounceUpSkill == null)
            this.bounceUpSkill = GetComponentInChildren<BounceUpSkill>();

        if (this.freezeTimeSkill == null)
            this.freezeTimeSkill = GetComponentInChildren<FreezeTimeSkill>();
    }

    private void Update()
    {
        if (this.playtime > 0.01f && this.playtimeCountingDown)
        {
            this.PlaytimeCountdownTimer();
        }

        if (this.comboCountingDown)
        {
            this.ComboCountdownTimer();
        }

    }

    public void SetupLevel()
    {
        this.ResetStat();
        this.Setup();
    }

    private void ResetStat()
    {
        if (GameManager.HasInstance)
        {
            this.SetPlaytime(GameManager.Instance.Levels[GameManager.Instance.CurrentLevel - 1].PlayTime);
        }

        this.comboCount = 0;
        this.collectedPoint = 0;
    }

    private void Setup()
    {
        if (GameManager.HasInstance)
        {
            this.level = GameManager.Instance.Levels[GameManager.Instance.CurrentLevel - 1];
        }

        if (this.level == null) return;

        TileSO[] tileSOs = this.level.GetTiles();
        for (int i = 0; i < tileSOs.Length; i++)
        {
            for (int j = 0; j < this.matchCount; j++)
            {
                TileCtrl tile = this.poolTile.SpawnTile();
                this.poolTile.PoolingTile.Add(tile);
                this.poolTile.TilesGame.Add(tile);
                tile.SetTile(tileSOs[i]);
            }
        }
    }

    private void SetPlaytime(float time)
    {
        this.playtime = time;
        this.playtimeCountingDown = true;
    }

    private void PlaytimeCountdownTimer()
    {
        this.playtime -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(this.playtime / 60);
        int seconds = Mathf.FloorToInt(this.playtime % 60);

        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.TimerText.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
        }

        if (this.playtime < 0.01f)
        {
            this.playtimeCountingDown = false;
        }
    }


    public void Combo()
    {
        this.comboCount++;
        this.comboTimer = this.AdjustTime(comboCount);

        if (UIManager.HasInstance)
        {
            ComboPanel comboPanel = UIManager.Instance.GamePanel.ComboPanel;
            comboPanel.Show(null);
            comboPanel.Text.SetText("Combo x " + this.comboCount);
            comboPanel.Slider.maxValue = this.comboTimer;
            comboPanel.Slider.value = this.comboTimer;
        }

        this.comboCountingDown = true;
    }

    private void ComboCountdownTimer()
    {
        this.comboTimer -= Time.deltaTime;

        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.ComboPanel.Slider.value = comboTimer;
        }

        if (comboTimer < 0.01f)
        {
            if (UIManager.HasInstance)
            {
                UIManager.Instance.GamePanel.ComboPanel.Hide();
            }

            this.comboCountingDown = false;
            this.comboCount = 0;
        }
    }

    private float AdjustTime(int count)
    {
        float adjustedTime = this.comboCDTime;

        for (int i = 0; i < count; i++)
            adjustedTime *= 0.9f;

        return adjustedTime;
    }


    public void ScorePoint()
    {
        this.collectedPoint += this.pointEachMatch;

        if (UIManager.HasInstance)
        {
            UIManager.Instance.GamePanel.PointText.SetText(this.collectedPoint.ToString());
        }
    }
}
