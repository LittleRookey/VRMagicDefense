using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFloating : MonoBehaviour
{
    public bool floating = true;

    public float amplitude = 0.5f;
    public float frequency = 1f;

    public bool rotate = false;
    public float rotationDegree = 20f;

    Vector3 originPos;
    void Start()
    {
        originPos = transform.position;
    }

    void Update()
    {
        if (rotate)
        {
            transform.Rotate(new Vector3(0f, rotationDegree * Time.deltaTime, 0f), Space.World);
        }
        if (floating)
        {
            Vector3 tempPos = originPos;
            tempPos.y += Mathf.Sin(Time.time * Mathf.PI * frequency) * amplitude;
            transform.position = tempPos;
        }
    }

    public void ToggleFloating(bool value)
    {
        floating = value;
    }

    public void ToggleRotation(bool value)
    {
        rotate = value;
    }

    public void UpdatePosition()
    {
        originPos = transform.position;
    }
}
