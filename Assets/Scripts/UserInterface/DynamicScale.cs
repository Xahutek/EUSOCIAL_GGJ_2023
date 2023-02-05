using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicScale : MonoBehaviour
{
    Vector3 initialScale;
    private void Awake()
    {
        initialScale = transform.localScale;
    }
    private void FixedUpdate()
    {
        transform.localScale = initialScale * Mathf.Clamp(Camera.main.orthographicSize / 5,1,2f);
    }
}
