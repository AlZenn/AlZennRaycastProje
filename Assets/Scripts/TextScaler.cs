using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScaler : MonoBehaviour
{
    public float scaleSpeed = 1f;

    void Update()
    {
        float scale = 1f + Mathf.Sin(Time.time * scaleSpeed) * 0.5f;
        this.transform.localScale = new Vector3(scale, scale, 1f);
    }
}
