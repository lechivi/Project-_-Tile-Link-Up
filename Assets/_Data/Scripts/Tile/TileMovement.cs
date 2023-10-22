using System.Collections;
using UnityEngine;

public class TileMovement : TileAbstract
{
    [Header("MOVEMENT")]
    [SerializeField] private InterpType Interpolation = InterpType.SmootherStep;

    private bool isMovingTransform;
    private bool isMovingPosition;

    public bool IsMovingTransform { get => this.isMovingTransform; set => this.isMovingTransform = value; }
    public bool IsMovingPosition { get => this.isMovingPosition; set => this.isMovingPosition = value; }

    public void Move(Transform destTransform, float timeToMove)
    {
        if (!this.isMovingTransform)
        {
            StartCoroutine(this.MoveRoutine(destTransform, timeToMove));
        }
    }

    private IEnumerator MoveRoutine(Transform destTransform, float timeToMove)
    {
        Transform tileTransform = this.tileCtrl.transform;
        Vector3 startPosition = tileTransform.position;
        Quaternion startRotation = tileTransform.rotation;

        bool reachedDestination = false;
        float elapsedTime = 0;

        this.isMovingTransform = true;

        while (!reachedDestination)
        {
            if (Vector3.Distance(tileTransform.position, destTransform.position) < 0.01f)
            {
                reachedDestination = true;
                TilesPanel.instance.PlaceTile(this.tileCtrl, destTransform);
                break;
            }

            elapsedTime += Time.deltaTime;

            float t = this.InterpTime(elapsedTime, timeToMove);

            tileTransform.position = Vector3.Lerp(startPosition, destTransform.position, t);
            tileTransform.rotation = Quaternion.Slerp(startRotation, Quaternion.identity, t);

            yield return null;
        }

        this.isMovingTransform = false;

    }


    public void Move(Vector3 destination, float timeToMove)
    {
        if (!this.isMovingPosition)
        {
            StartCoroutine(this.MoveRoutine(destination, timeToMove));
        }
    }

    private IEnumerator MoveRoutine(Vector3 destination, float timeToMove)
    {
        Transform tileTransform = this.tileCtrl.transform;
        Vector3 startPosition = tileTransform.position;
        bool reachedDestination = false;
        float elapsedTime = 0;

        this.isMovingPosition = true;

        while (!reachedDestination && this.isMovingPosition && !this.isMovingTransform)
        {
            if (Vector3.Distance(tileTransform.position, destination) < 0.01f)
            {
                reachedDestination = true;
                tileTransform.position = destination;
                break;
            }

            elapsedTime += Time.deltaTime;

            float t = this.InterpTime(elapsedTime, timeToMove);

            tileTransform.position = Vector3.Lerp(startPosition, destination, t);

            //Wait until next frame
            yield return null;
        }

        this.isMovingPosition = false;

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
