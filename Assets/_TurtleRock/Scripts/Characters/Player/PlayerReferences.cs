using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReferences : MonoBehaviour
{
    public static PlayerReferences Instance { get; private set; }
    public PlayerMovement PlayerMovementRef { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        PlayerMovementRef = GetComponentInChildren<PlayerMovement>();
    }
}