using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StingRayFood : Food
{
    [SerializeField]
    private FoodWallet _foodWallet;
    [Tooltip("The amount of food consumed per second")]
    [SerializeField]
    private int costPerSecond = 10;
    [Tooltip("The point where the sting ray is going to be called")]
    [SerializeField]
    private Transform _baitPoint;
    private bool inUse = false;
    private float timeToSpendFood = 0.0f;
    private float currentPassedTime = 0.0f;
    private List<StingRay> _stingRayList = new List<StingRay>();
    private void Start()
    {
        timeToSpendFood = (1.0f / (float)costPerSecond);
    }
    private void Update()
    {
        SpendFood();
    }
    /// <summary>
    /// It will spend food if the wallet is not empty
    /// </summary>
    private void SpendFood()
    {
        if (!inUse) { return; }
        currentPassedTime += Time.deltaTime;
        CallStingRay();
        if (currentPassedTime < timeToSpendFood) { return; }
        _foodWallet.SpendFood(_foodType);
        currentPassedTime = 0.0f;
    }
    /// <summary>
    /// Calls closest sting ray to the bait position
    /// </summary>
    private void CallStingRay()
    {
        float closestDistance = float.MaxValue;
        StingRay closestStingRay = null;
        foreach (StingRay stingRay in _stingRayList)
        {
            if (Vector3.Distance(_baitPoint.position, stingRay.transform.position) < closestDistance)
            {
                closestDistance = Vector3.Distance(_baitPoint.position, stingRay.transform.position);
                closestStingRay = stingRay;
            }
        }
        if (closestStingRay)
        {
            closestStingRay.GoTo(_baitPoint.position);
        }
    }
    /// <summary>
    /// Finds all StingRays ( will change in a future )
    /// </summary>
    public override void Equip()
    {
        _stingRayList = new List<StingRay>(GameObject.FindObjectsOfType<StingRay>());
    }
    /// <summary>
    /// Activates the bait
    /// </summary>
    public override void Use()
    {
        inUse = true;
    }
    /// <summary>
    /// Deactivates the bait
    /// </summary>
    public override void StopUsing()
    {
        inUse = false;
    }

    public override void Unequip()
    {
        inUse = false;
    }


}
