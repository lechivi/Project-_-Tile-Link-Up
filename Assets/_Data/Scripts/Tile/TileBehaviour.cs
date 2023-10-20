using System.Collections;
using UnityEngine;

public class TileBehaviour : TileAbstract
{
    [Header("BEHAVIOUR")]
    [SerializeField] private float flipTorque = 100;
    [SerializeField] private float flipForce = 200f;
    [SerializeField] private float flipWaitTime = 2f;

    public IEnumerator StartFlipRoutine()
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

    public bool IsLandedOnBackFace()
    {
        //if (Physics.Raycast(this.tileCtrl.transform.position, -this.tileCtrl.transform.up, out RaycastHit hit, 5f))
        //{
        //    return Vector3.Dot(hit.normal, Vector3.up) > 0.9f;
        //}
        //return false;
        return this.tileCtrl.transform.up.y > 0.05f;
    }

    private void FlipTile()
    {
        int dir = Random.Range(0, 2) == 0 ? 1 : -1;
        this.tileCtrl.Rb.AddTorque(this.tileCtrl.transform.forward * dir * this.flipTorque);

        //this.tileCtrl.Rb.AddForce((-this.tileCtrl.transform.up + (this.tileCtrl.transform.right * dir)) * this.flipForce);
        this.tileCtrl.Rb.AddForce(Vector3.up * this.flipForce);
    }

}
