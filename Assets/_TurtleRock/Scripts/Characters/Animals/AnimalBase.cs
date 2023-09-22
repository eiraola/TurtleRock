using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public abstract class AnimalBase : MonoBehaviour
{
    private NavMeshAgent _agent;
    protected virtual void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    /// <summary>
    /// Orders the agent to move to de designed position.
    /// </summary>
    /// <param name="destiny"></param>
    public virtual void GoTo(Vector3 destiny)
    {
        _agent.SetDestination(destiny);
    }
    
}
