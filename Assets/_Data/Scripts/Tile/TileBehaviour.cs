using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class TileBehaviour : TileAbstract
{
    [Header("BEHAVIOUR")]
    [SerializeField] private float flipTorque = 100;
    [SerializeField] private float flipForce = 200f;
    [SerializeField] private float flipWaitTime = 2f;
    [SerializeField] private float rotationSpeed = 360;
    //[SerializeField] private float maxRotationAngle = 45;

    private bool isRotating = false;

    public IEnumerator StartFlipRoutine()
    {
        int count = 0;
        while (!this.IsLandedOnBackFace())
        {
            if (count > 20)
                yield break;

            yield return new WaitForSeconds(this.flipWaitTime);
            this.tileCtrl.Rb.AddForce(-this.tileCtrl.transform.up * this.flipForce);

            this.FlipTileWithTorque();

            count++;
        }
    }

    public bool IsLandedOnBackFace()
    {
        return this.tileCtrl.transform.up.y > 0.02f;
    }

    private void FlipTileWithTorque()
    {
        int dir = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;
        this.tileCtrl.Rb.AddTorque(this.tileCtrl.transform.forward * dir * this.flipTorque);

        this.tileCtrl.Rb.AddForce(-this.tileCtrl.transform.up * this.flipForce);
        //this.tileCtrl.Rb.AddForce((-this.tileCtrl.transform.up + (this.tileCtrl.transform.right * dir)) * this.flipForce);
        //this.tileCtrl.Rb.AddForce(Vector3.up * this.flipForce);
    }

    //private void FlipTileWithRotate()
    //{
    //    int dir = UnityEngine.Random.Range(0, 2) == 0 ? 1 : -1;

    //    this.currentRotation += dir * this.maxRotationAngle;
    //    this.currentRotation = Mathf.Clamp(this.currentRotation, -this.maxRotationAngle, this.maxRotationAngle);
    //    this.tileCtrl.transform.rotation = Quaternion.Euler(0, this.currentRotation, 0);

    //    this.tileCtrl.Rb.AddForce(-this.tileCtrl.transform.up * this.flipForce);
    //}



    private void FixedUpdate()
    {
        if (isRotating)
        {
            RotateFaceUp();
        }
    }

    private Quaternion targetRotation;

    public void StartRotation()
    {
        targetRotation = Quaternion.LookRotation(Vector3.up, this.tileCtrl.transform.up);
        //targetRotation = Quaternion.Euler(180, 0, 0) * this.tileCtrl.transform.rotation;
        isRotating = true;
    }

    private void RotateFaceUp()
    {
        float step = rotationSpeed * Time.fixedDeltaTime;
        this.tileCtrl.transform.rotation = Quaternion.RotateTowards(this.tileCtrl.transform.rotation, targetRotation, step);

        float rotationThreshold = 0.1f; // Ngưỡng kiểm tra sự gần bằng nhau
        if (Quaternion.Angle(this.tileCtrl.transform.rotation, targetRotation) < rotationThreshold)
        {
            isRotating = false;
        }
    }
}
