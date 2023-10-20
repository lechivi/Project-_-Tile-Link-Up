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

    private bool isAtSlot = false;
    private Coroutine addTorqueRoutine;

    #region PUBLIC PROPERTIES
    public Rigidbody Rb { get => this.rb; }
    public Collider Col { get => this.col; }
    public Outline Outline { get => this.outline; }
    public SpriteRenderer SpriteRenderer { get => this.spriteRenderer; }
    public TileBehaviour TileBehaviour { get => this.tileBehaviour;}
    public TileInputHandle TileInputHandle { get => this.tileInputHandle;}
    public TileMovement TileMovement { get => this.tileMovement;}
    public TileSO TileSO { get => this.tileSO; }
    #endregion

    protected override void LoadCompoent()
    {
        base.LoadCompoent();
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

    private void Start()
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


    //private void OnMouseEnter()
    //{
    //    Vector3 targetPosition = new Vector3(transform.position.x, 0.5f, transform.position.z);
    //    this.tileMovement.Move(targetPosition, 0.25f);

    //    this.SetStatic(true);
    //    this.outline.enabled = true;
    //}

    //private void OnMouseExit()
    //{
    //    if (this.isAtSlot) return;
    //    if (this.outline.enabled)
    //        this.outline.enabled = false;

    //    this.SetStatic(false);
    //    this.tileMovement.IsMoving = false;
    //}

    //private void OnMouseUp()
    //{
    //    bool canAdd = TilesPanel.instance.AddTileToSlot(this);
    //    if (canAdd)
    //    {
    //        this.outline.enabled = false;
    //        this.isAtSlot = true;
    //    }
    //}

    public void SetTile(TileSO tileSO)
    {
        this.tileSO = tileSO;
        if (tileSO.Sprite != null)
            this.spriteRenderer.sprite = tileSO.Sprite;
    }

    public void SetStatic(bool isStatic)
    {
        this.rb.useGravity = !isStatic;
        this.rb.isKinematic = isStatic;
    }
}
