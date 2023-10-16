using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementWalk : PlayerMovementBase
{
    public override void Init(PlayerMovement context)
    {
        base.Init(context);
        _type = EMovementType.Walk;
    }
    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
       
    }

    public override void OnUpdate()
    {
        _context.SetOwnerVelocity();
        ChecktateChange();
    }

    protected override void ChecktateChange()
    {
        if (_context.IsAiming)
        {
            _context.SwapMovementType(EMovementType.Aim);
        }
    }
}
