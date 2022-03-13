using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] bool fixedCamera = true;

    void Start()
    {
        transform.LookAt(Camera.main.transform.position);
        if (fixedCamera)
            enabled = false;
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform.position);
    }
}
