using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpMovement : MonoBehaviour
{
    [Tooltip("Defines the rotation speed of the pickup")]
    [SerializeField]
    private float rotationSpeed = 1.0f;
    [Tooltip("Defines heigth of the vertical movement")]
    [SerializeField]
    private float waveLength = 1.0f;
    [Tooltip("Defines the speed of the vertical movement")]
    [SerializeField]
    private float amplitude = 1.0f;
    void Update()
    {
        Rotate();
        Move();
    }
    private void Rotate()
    {
        transform.rotation = transform.rotation * Quaternion.Euler(0.0f, rotationSpeed * Time.deltaTime, 0.0f);
    }
    private void Move()
    {
        transform.localPosition = Vector3.up * Mathf.Sin(Time.time * waveLength) * amplitude;
    }
}
