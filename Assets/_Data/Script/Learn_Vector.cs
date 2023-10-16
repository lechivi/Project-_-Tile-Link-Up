using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Learn_Vector : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            //transform.position = Vector3.zero;
            Vector3 upDirection = transform.rotation * Vector3.up;
            Debug.Log(transform.up + transform.right);
            //Debug.Log(transform.right);
            //Debug.Log(transform.forward);
        }
    }
}
