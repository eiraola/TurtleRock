using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public abstract class AnimalBase : MonoBehaviour
{
    private NavMeshAgent _agent;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    public void GoTo(Vector3 destiny)
    {
        _agent.SetDestination(destiny);
    }
    
}
