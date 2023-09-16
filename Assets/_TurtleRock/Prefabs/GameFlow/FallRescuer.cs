using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallRescuer : MonoBehaviour
{
    [SerializeField]
    private Transform _startPoint;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_PLAYER))
        {
            other.transform.position = _startPoint.position;
        }
    }
}
