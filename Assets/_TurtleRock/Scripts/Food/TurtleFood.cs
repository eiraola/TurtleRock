using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleFood : Food
{
    [SerializeField]
    private FoodWallet _foodWallet;
    [Tooltip("The prefab of the projectile to be launched")]
    [SerializeField]
    private FoodProjectile _projectilePrefab;
    [Tooltip("A reference transform to copy the rotation to the projectile")]
    [SerializeField]
    private Transform _launchingReference;
    [Tooltip("Cooldown in seconds that will take to launch another projectile")]
    [SerializeField]
    private float _cooldown = 0.5f;
    [Tooltip("The curve that will define the vertical movement of the projectile")]
    [SerializeField]
    private AnimationCurve _launchCurve;
    [Tooltip("How far can travel a projectile horizontally")]
    [SerializeField]
    private float _maxLaunchDistance = 10.0f;
    [Tooltip("The speed of the projectile")]
    [SerializeField]
    private float _launchSpeed = 10.0f;
    private float _lastLaunchTime = 0.0f;
    public override void Equip()
    {
        
    }

    public override void Unequip()
    {
        
    }
    /// <summary>
    /// It launches a projectile in the defined direction
    /// </summary>
    public override void Use()
    {
        if (!_foodWallet){ return; }
        if (!_foodWallet.HasFood(_foodType)) { return; }
        if (Time.time < _lastLaunchTime + _cooldown) { return; }
        _lastLaunchTime = Time.time;
        FoodProjectile projectileInstance = Instantiate(_projectilePrefab, transform.position, _launchingReference.rotation).GetComponent<FoodProjectile>();
        projectileInstance.Launch(_launchCurve, _maxLaunchDistance, _launchSpeed);
        _foodWallet.SpendFood(_foodType);
    }
    public override void StopUsing()
    {
        
    }
}
