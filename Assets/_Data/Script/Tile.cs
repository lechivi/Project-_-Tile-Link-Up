using System.Collections;
using UnityEngine;

public class Tile : SaiMonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private float flipTorque = 100;
    [SerializeField] private float flipForce = 200f;
    [SerializeField] private float flipWaitTime = 2f;

    private Coroutine addTorqueRoutine;

    protected override void LoadCompoent()
    {
        base.LoadCompoent();
        if (this.rb == null)
            this.rb = GetComponent<Rigidbody>();

        if (this.spriteRenderer == null)
            this.spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        this.addTorqueRoutine = StartCoroutine(this.StartFlipRoutine());
    }

    private void OnCollisionExit(Collision collision)
    {
        if (this.addTorqueRoutine != null)
            StopCoroutine(this.addTorqueRoutine);
    }


    private void FlipTile()
    {
        int dir = Random.Range(0, 2) == 0 ? 1 : -1;
        rb.AddTorque(transform.forward * dir * this.flipTorque);
        rb.AddForce((-transform.up + (transform.right * dir)) * this.flipForce);
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
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, 2f))
        {
            return Vector3.Dot(hit.normal, Vector3.up) > 0.9f;
        }
        return false;
    }
}
