using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTester : MonoBehaviour
{
    // Simple Camera Follow Script
    public Transform target;

    public float smoothSpeed;
    public Vector3 offSet;
    
    void LateUpdate()
    {
        transform.position = target.position + offSet;
    }
}
