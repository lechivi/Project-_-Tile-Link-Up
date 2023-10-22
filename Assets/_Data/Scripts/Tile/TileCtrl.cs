using UnityEngine;

public class TileCtrl : SaiMonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;
    [SerializeField] private Outline outline;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private TileBehaviour tileBehaviour;
    [SerializeField] private TileInputHandle tileInputHandle;
    [SerializeField] private TileMovement tileMovement;

    [Header("DATA")]
    [SerializeField] private TileSO tileSO;

    private Coroutine addTorqueRoutine;

    #region PUBLIC PROPERTIES
    public Rigidbody Rb { get => this.rb; }
    public Collider Col { get => this.col; }
    public Outline Outline { get => this.outline; }
    public SpriteRenderer SpriteRenderer { get => this.spriteRenderer; }
    public TileBehaviour TileBehaviour { get => this.tileBehaviour; }
    public TileInputHandle TileInputHandle { get => this.tileInputHandle; }
    public TileMovement TileMovement { get => this.tileMovement; }
    public TileSO TileSO { get => this.tileSO; }
    #endregion

    protected override void LoadComponent()
    {
        base.LoadComponent();
        if (this.rb == null)
            this.rb = GetComponent<Rigidbody>();

        if (this.col == null)
            this.col = GetComponent<Collider>();

        if (this.outline == null)
            this.outline = GetComponent<Outline>();

        if (this.spriteRenderer == null)
            this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (this.tileBehaviour == null)
            this.tileBehaviour = GetComponentInChildren<TileBehaviour>();

        if (this.tileInputHandle == null)
            this.tileInputHandle = GetComponentInChildren<TileInputHandle>();

        if (this.tileMovement == null)
            this.tileMovement = GetComponentInChildren<TileMovement>();
    }

    private void OnEnable()
    {
        this.outline.enabled = false;
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Bound")) return;

        if (!this.tileBehaviour.IsLandedOnBackFace() && this.addTorqueRoutine == null)
        {
            this.addTorqueRoutine = StartCoroutine(this.tileBehaviour.StartFlipRoutine());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (this.addTorqueRoutine != null)
        {
            StopCoroutine(this.addTorqueRoutine);
            this.addTorqueRoutine = null;
        }
    }

    public void SetTile(TileSO tileSO)
    {
        if (tileSO == null)
        {
            Debug.Log("Null paramenter tileSO");
        }
        this.tileSO = tileSO;
        if (this.spriteRenderer == null)
            Debug.Log("Null spriteRrr");
        if (tileSO.Sprite != null)
            this.spriteRenderer.sprite = tileSO.Sprite;
    }

    public void SetStatic(bool isStatic)
    {
        this.rb.useGravity = !isStatic;
        this.rb.isKinematic = isStatic;
    }

    public void SetState(bool isPhysic)
    {
        if (isPhysic)
        {
            UI_Slot slot = GetComponentInParent<UI_Slot>();
            if (slot != null)
            {
                slot.RemoveTile();
            }

            this.SetStatic(false);
            this.col.isTrigger = false;
            this.TileInputHandle.IsSelected = false;
        }

        else
        {
            this.SetStatic(true);
            this.col.isTrigger = true;
        }
    }
}
