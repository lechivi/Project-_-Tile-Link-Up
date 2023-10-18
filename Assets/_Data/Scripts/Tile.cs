using System.Collections;
using UnityEngine;

public class Tile : SaiMonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider col;
    [SerializeField] private Outline outline;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private float flipTorque = 100;
    [SerializeField] private float flipForce = 200f;
    [SerializeField] private float flipWaitTime = 2f;
    [SerializeField] private TileSO tileSO;

    [SerializeField] private InterpType Interpolation = InterpType.SmootherStep;

    private bool isMoving = false;
    private bool isAtSlot = false;
    private Coroutine addTorqueRoutine;

    public Collider Col { get => this.col; set => this.col = value; }
    public SpriteRenderer SpriteRenderer { get => this.spriteRenderer; set => this.spriteRenderer = value; }
    public TileSO TileSO
    {
        get => this.tileSO;
        //set
        //{
        //    this.tileSO = value;
        //}
    }

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
    }

    private void Start()
    {
        this.outline.enabled = false;
    }


    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.CompareTag("Bound")) return;

        if (!this.IsLandedOnBackFace() && this.addTorqueRoutine == null)
        {
            this.addTorqueRoutine = StartCoroutine(this.StartFlipRoutine());
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


    private void FlipTile()
    {
        int dir = Random.Range(0, 2) == 0 ? 1 : -1;
        rb.AddTorque(transform.forward * dir * this.flipTorque);
        //rb.AddForce((-transform.up + (transform.right * dir)) * this.flipForce);
        rb.AddForce(Vector3.up * this.flipForce);
    }

    private IEnumerator StartFlipRoutine()
    {
        int maxCount = 20;
        int count = 0;
        while (!this.IsLandedOnBackFace())
        {
            if (count > maxCount)
                yield break;

            yield return new WaitForSeconds(this.flipWaitTime);

            this.FlipTile();
            count++;
        }
    }

    private bool IsLandedOnBackFace()
    {
        //if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 5f))
        //{
        //    return Vector3.Dot(hit.normal, Vector3.up) > 0.9f;
        //}
        //return false;
        return (transform.up.y > 0.05);
    }


    private void OnMouseEnter()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, 0.5f, transform.position.z);
        this.Move(targetPosition, 0.25f);

        this.SetStatic(true);
        this.outline.enabled = true;
    }

    private void OnMouseExit()
    {
        if (this.isAtSlot) return;
        if (this.outline.enabled)
            this.outline.enabled = false;

        this.SetStatic(false);
        this.isMoving = false;
    }

    private void OnMouseUp()
    {
        bool canAdd = TilesPanel.instance.AddTileToSlot(this);
        if (canAdd)
        {
            this.outline.enabled = false;
            this.isAtSlot = true;
        }
    }

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


    public void Move(Transform destTransform, float timeToMove) //destination
    {
        if (!this.isMoving)
        {
            StartCoroutine(this.MoveRoutine(destTransform, timeToMove));
        }
    }

    public void Move(Vector3 destination, float timeToMove) //destination
    {
        if (!this.isMoving)
        {
            StartCoroutine(this.MoveRoutine(destination, timeToMove));
        }
    }

    private IEnumerator MoveRoutine(Transform destTransform, float timeToMove)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        bool reachedDestination = false;
        float elapsedTime = 0;

        this.isMoving = true;

        while (!reachedDestination)
        {
            if (Vector3.Distance(transform.position, destTransform.position) < 0.01f)
            {
                reachedDestination = true;
                TilesPanel.instance.PlaceTile(this, destTransform);
                break;
            }

            elapsedTime += Time.deltaTime;

            float t = this.InterpTime(elapsedTime, timeToMove);

            transform.position = Vector3.Lerp(startPosition, destTransform.position, t);
            transform.rotation = Quaternion.Slerp(startRotation, Quaternion.identity, t);
            //Wait until next frame
            yield return null;
        }

        this.isMoving = false;

    }

    private IEnumerator MoveRoutine(Vector3 destination, float timeToMove)
    {
        Vector3 startPosition = transform.position;
        bool reachedDestination = false;
        float elapsedTime = 0;

        this.isMoving = true;

        while (!reachedDestination)
        {
            if (Vector3.Distance(transform.position, destination) < 0.01f)
            {
                reachedDestination = true;
                transform.position = destination;
                break;
            }

            elapsedTime += Time.deltaTime;

            float t = this.InterpTime(elapsedTime, timeToMove);

            transform.position = Vector3.Lerp(startPosition, destination, t);

            //Wait until next frame
            yield return null;
        }

        this.isMoving = false;

    }

    private float InterpTime(float elapsedTime, float timeToMove)
    {
        float t = Mathf.Clamp01(elapsedTime / timeToMove);
        switch (this.Interpolation)
        {
            case InterpType.Linear:
                break;

            case InterpType.EaseIn:
                t = Mathf.Sin(t * Mathf.PI * 0.5f);
                break;

            case InterpType.EaseOut:
                t = 1 - Mathf.Cos(t * Mathf.PI * 0.5f);
                break;

            case InterpType.SmoothStep:
                t = t * t * (3 - 2 * t);
                break;

            case InterpType.SmootherStep:
                t = t * t * t * (t * (t * 6 - 15) + 10);
                break;
        }

        return t;
    }
}
