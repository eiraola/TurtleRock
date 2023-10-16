using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAim : PlayerMovementBase
{
    public override void OnEnter()
    {
      
    }

    public override void OnExit()
    {
       
    }

    public override void OnUpdate()
    {
        _context.SetOwnerAimingVelocity();
        ChecktateChange();
    }

    protected override void ChecktateChange()
    {
        if (!_context.IsAiming)
        {
            _context.SwapMovementType(EMovementType.Walk);
        }
    }
}
