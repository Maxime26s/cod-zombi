using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixRotation : MonoBehaviour
{
    Quaternion rotation;
    void OnEnable()
    {
        rotation = transform.rotation;
    }
    void LateUpdate()
    {
        transform.rotation = rotation;
    }
}
