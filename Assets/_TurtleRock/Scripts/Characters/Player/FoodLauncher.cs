using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodLauncher : MonoBehaviour
{
    [SerializeField]
    private GameObject foodPrefab;
    [SerializeField]
    private Transform launchPoint;
    [SerializeField]
    private PlayerInput _playerInput;
    [SerializeField]
    private float cooldown = 0.5f;
    [SerializeField]
    private AnimationCurve launchCurve;
    [SerializeField]
    private float maxLaunchDistance = 10.0f;
    [SerializeField]
    private float launchSpeed = 10.0f;
    private float lastLaunchTime = 0.0f;
    private FoodWallet _foodWallet;
    private void Start()
    {
        _foodWallet = GetComponent<FoodWallet>();
    }
    private void OnEnable()
    {
        _playerInput.LaunchEvent += LaunchFood;
    }
    private void OnDisable()
    {
        _playerInput.LaunchEvent -= LaunchFood;
    }
    private void LaunchFood()
    {
        if (!_foodWallet) { return; }
        if (Time.time < lastLaunchTime + cooldown)
        {
            return;
        }
        //As we only have one food type, we are going to use a raw value
        if (!_foodWallet.SpendFood(EFoodType.Turtle)) { return; }
        lastLaunchTime = Time.time;
        GameObject foodIntance = Instantiate(foodPrefab, launchPoint.position, launchPoint.rotation);
        Food xd = foodIntance.GetComponent<Food>();
        xd.Launch(launchCurve, maxLaunchDistance, launchSpeed);
    }
}
