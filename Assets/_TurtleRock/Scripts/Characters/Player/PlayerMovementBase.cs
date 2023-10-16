using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EMovementType
{
    None,
    Walk,
    Jump,
    Aim
}
public abstract class PlayerMovementBase 
{
    protected PlayerMovement _context;
    protected EMovementType _type;
    public EMovementType Type { get => _type; }

    public virtual void Init(PlayerMovement context) {
        _context = context;
    }
    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();
    protected abstract void ChecktateChange();
}
