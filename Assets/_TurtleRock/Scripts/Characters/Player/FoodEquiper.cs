using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnEquippedFoodChange : UnityEvent<EFoodType> { }
[System.Serializable]
/// A simple structure to handle each of the food types we can equip.
public struct FoodEquipment
{
    public EFoodType foodType;
    public Food foodReference;
    private bool isActive;
    /// <summary>
    /// Activates this equipment
    /// </summary>
    public void Activate()
    {
        isActive = true;
        foodReference.gameObject.SetActive(true);
    }
    /// <summary>
    /// Deactivates current equipment
    /// </summary>
    public void Deactivate()
    {
        isActive = false;
        foodReference.gameObject.SetActive(false);
    }
    /// <summary>
    /// Asks if the current equipment is active
    /// </summary>
    /// <returns></returns>
    public bool IsActive()
    {
        return isActive;
    }
}

public class FoodEquiper : MonoBehaviour
{
    [Header("Equipments")]
    [Tooltip("A list of the different Equipments that the player will have")]
    [SerializeField]
    private List<FoodEquipment> _foodReferences = new List<FoodEquipment>();
    [Header("Input")]
    [Tooltip("Reference to a player input prefab")]
    [SerializeField]
    private PlayerInput _playerInput;
    private FoodWallet _foodWallet;
    private EFoodType _currentEquipedFoodType = EFoodType.Turtle;
    private Food _currentEquippedFoodInstance = null;
    private FoodEquipment _currentFoodEquipment;
    public OnEquippedFoodChange OnEquippedFoodChange = new OnEquippedFoodChange();

    public EFoodType CurrentEquipedFoodType {
        get => _currentEquipedFoodType;
        private set
        { 
            _currentEquipedFoodType = value;
            OnEquippedFoodChange.Invoke(value);
        }
    }

    private void Start()
    {
        _foodWallet = GetComponent<FoodWallet>();
        _foodWallet.OnUpdateFood.AddListener(CheckEquippedFoodUpdate);
    }

    private void OnEnable()
    {
        _playerInput.SwapEquipEvent += SwapFood;
        _playerInput.UseEvent += UseFood;
        _playerInput.UnuseEvent += UnuseFood;
    }
    private void OnDisable()
    {
        _playerInput.SwapEquipEvent -= SwapFood;
        _playerInput.UseEvent -= UseFood;
        _playerInput.UnuseEvent -= UnuseFood;
    }
    /// <summary>
    /// It equipes or unequipes the food if the amount that is in foodInfo requieres it.
    /// </summary>
    /// <param name="foodType"></param>
    /// <param name="foodInfo"></param>
    private void CheckEquippedFoodUpdate(EFoodType foodType, Dictionary<EFoodType, int> foodInfo)
    {
        if (CurrentEquipedFoodType != foodType) { return; }
        if (foodInfo[foodType] > 0 && !_currentFoodEquipment.IsActive())
        {
            EquipFood();
            return;
        }
        if (foodInfo[foodType] <= 0 && _currentFoodEquipment.IsActive())
        {
            UnequipFood();
            return;
        }
    }
    /// <summary>
    /// Stops using the current equiped food.
    /// </summary>
    private void UnuseFood()
    {
        if (!_currentEquippedFoodInstance) { return; }
        _currentEquippedFoodInstance.StopUsing();
    }
    /// <summary>
    /// Use the current equiped food.
    /// </summary>
    private void UseFood()
    {
        if (!_currentEquippedFoodInstance) { return; }
        _currentEquippedFoodInstance.Use();
    }
    /// <summary>
    /// Swaps the current equiped food
    /// </summary>
    /// <param name="valueToSum"></param>
    private void SwapFood(int valueToSum)
    {
        CurrentEquipedFoodType = SelectNewFood(valueToSum);
        EquipFood();
    }
    /// <summary>
    /// Selects a food type depending the current equiped food and a value to go up or down in the values.
    /// </summary>
    /// <param name="valueToSum"></param>
    /// <returns></returns>
    private EFoodType SelectNewFood(int valueToSum)
    {
        if ((CurrentEquipedFoodType + valueToSum) >= EFoodType.Size)
        {
            return EFoodType.Turtle;
        }
        if ((CurrentEquipedFoodType + valueToSum) <= 0)
        {
            return EFoodType.Size - 1;
        }
        return CurrentEquipedFoodType + valueToSum;
    }
    /// <summary>
    /// Unequips the previous equiped food and equips the current equiped food.
    /// </summary>
    private void EquipFood()
    {
        if (!_foodWallet) { return; }
        UnequipFood();
        if (!_foodWallet.HasFood(CurrentEquipedFoodType)) { return; }
        _currentFoodEquipment = _foodReferences.Find(x => x.foodType == CurrentEquipedFoodType);
        _currentFoodEquipment.Activate();
        _currentEquippedFoodInstance = _currentFoodEquipment.foodReference;
        if (!_currentEquippedFoodInstance)
        {
            return;
        }
        _currentEquippedFoodInstance.Equip();
    }
    /// <summary>
    /// Unequips the current equiped food.
    /// </summary>
    private void UnequipFood()
    {
        if (!_currentEquippedFoodInstance) { return; }
        _currentEquippedFoodInstance.Unequip();
        _currentFoodEquipment.Deactivate();
        _currentEquippedFoodInstance = null;

    }

}
