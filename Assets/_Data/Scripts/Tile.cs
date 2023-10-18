using System.Collections;
using UnityEngine;

public class Tile : SaiMonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Outline outline;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private float flipTorque = 100;
    [SerializeField] private float flipForce = 200f;
    [SerializeField] private float flipWaitTime = 2f;
    [SerializeField] private TileSO tileSO;

    private bool isMoving = false;
    private Vector3 targetPosition;
    private Coroutine addTorqueRoutine;

    public SpriteRenderer SpriteRenderer { get => this.spriteRenderer; set => this.spriteRenderer = value; }

    protected override void LoadCompoent()
    {
        base.LoadCompoent();
        if (this.rb == null)
            this.rb = GetComponent<Rigidbody>();

        if (this.outline == null)
            this.outline = GetComponent<Outline>();

        if (this.spriteRenderer == null)
            this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start()
    {
        this.outline.enabled = false;
    }

    private void Update()
    {
        if (this.isMoving)
        {
            float speed = 10f;

            transform.position = Vector3.Lerp(transform.position, this.targetPosition, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, this.targetPosition) < 0.01f)
            {
                transform.position = this.targetPosition;
                this.isMoving = false;
            }
        }
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


    private bool isMouseDown;
    //private void OnMouseDown()
    //{
    //    this.isMouseDown = true;
    //    Debug.Log("Mouse Down");
    //}

    //private void OnMouseUp()
    //{
    //    this.isMouseDown = false;
    //    Debug.Log("Mouse Up: " + gameObject.name,gameObject);
    //}

    private void OnMouseEnter()
    {
        //if (this.isMouseDown)
        if (!isMoving)
        {
            targetPosition = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
            isMoving = true;
        }
        this.outline.enabled = true;
        //this.transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        this.rb.useGravity = false;
        this.rb.isKinematic = true;
    }

    private void OnMouseExit()
    {
        if (this.outline.enabled)
            this.outline.enabled = false;
        this.rb.useGravity = true;
        this.rb.isKinematic = false;
        this.isMoving = false;
    }


    public void SetTile(TileSO tileSO)
    {
        this.tileSO = tileSO;
        if (tileSO.Sprite != null)
            this.spriteRenderer.sprite = tileSO.Sprite;
    }
}
