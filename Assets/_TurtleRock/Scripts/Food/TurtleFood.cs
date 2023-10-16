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
    [Tooltip("In how many chuncks will be the line renderer indicating the projectile trajectory divide")]
    [SerializeField]
    private int _lineChunkCount = 10;
    [Tooltip("How far can travel a projectile horizontally")]
    [SerializeField]
    private float _maxLaunchDistance = 10.0f;
    [Tooltip("The speed of the projectile")]
    [SerializeField]
    private float _launchSpeed = 10.0f;
    private float _lastLaunchTime = 0.0f;
    private LineRenderer _launchLineRenderer;
    private bool isAiming = false;
    private void Start()
    {
        _launchLineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        DefindeLaunchPath();
    }
    public override void Equip()
    {
       
    }

    public override void Unequip()
    {
        StopAiming();
    }
    /// <summary>
    /// It launches a projectile in the defined direction
    /// </summary>
    public override void Use()
    {
        StartAiming();
    }
    public override void StopUsing()
    {
        StopAiming();
    }
    public void DefindeLaunchPath()
    {
        float chunckSize = _maxLaunchDistance / (float)_lineChunkCount;
        Vector3 currentPoint = Vector3.zero;
        Vector3 previousPoint = Vector3.zero;
        _launchLineRenderer.positionCount = _lineChunkCount;
        _launchLineRenderer.SetPosition(0, transform.position);
        for (int i = 1; i < _lineChunkCount; i++)
        {
            previousPoint = currentPoint;
            currentPoint = Vector3.zero;
            currentPoint.y = _launchCurve.Evaluate(chunckSize *i / _maxLaunchDistance);
            currentPoint = Vector3.up  * (0.5f - currentPoint.y);
            currentPoint += previousPoint + transform.forward * chunckSize;
            _launchLineRenderer.SetPosition(i, currentPoint + transform.position);
        }
    }
    private void StartAiming()
    {
        PlayerReferences.Instance.PlayerMovementRef.IsAiming = true;
        isAiming = true;
    }
    private void StopAiming()
    {
        PlayerReferences.Instance.PlayerMovementRef.IsAiming = false;
        isAiming = false;
    }

    public void Shoot()
    {
        if (!isAiming) { return; }
        if (!_foodWallet) { return; }
        if (!_foodWallet.HasFood(_foodType)) { return; }
        if (Time.time < _lastLaunchTime + _cooldown) { return; }
        _lastLaunchTime = Time.time;
        FoodProjectile projectileInstance = Instantiate(_projectilePrefab, transform.position, _launchingReference.rotation).GetComponent<FoodProjectile>();
        projectileInstance.Launch(_launchCurve, _maxLaunchDistance, _launchSpeed);
        _foodWallet.SpendFood(_foodType);
    }
}
