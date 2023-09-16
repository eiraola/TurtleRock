using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PondSight : MonoBehaviour
{
    [SerializeField]
    private AnimalBase _turtle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_FOOD))
        {
            if (_turtle)
            {
                _turtle.GoTo(other.transform.position);
            }
        }
    }

}
