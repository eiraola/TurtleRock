using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class FoodWallet : MonoBehaviour
{
    private Dictionary<EFoodType, int> _foodCount = new Dictionary<EFoodType, int>();
    public  UnityEvent<EFoodType, Dictionary<EFoodType, int>> OnUpdateFood = new UnityEvent<EFoodType, Dictionary<EFoodType, int>>();
    /// <summary>
    /// Adds the quantity of the foodType to the wallet.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="quantity"></param>
    public void AddFoodToWallet(EFoodType type, int quantity)
    {
        if (!_foodCount.ContainsKey(type))
        {
            _foodCount[type] = 0;
        }
        _foodCount[type] += quantity;
        OnUpdateFood.Invoke(type, _foodCount);
    }
    /// <summary>
    /// Spends ONE PIECE of the type of food.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool SpendFood(EFoodType type)
    {
        if (_foodCount.GetValueOrDefault(type) > 0)
        {
            _foodCount[type]--;
            OnUpdateFood.Invoke(type, _foodCount);
            return true;
        }
        return false;
    }
    /// <summary>
    /// Asks if the value of foodType in the wallet.
    /// </summary>
    /// <param name="foodType"></param>
    /// <returns></returns>
    public bool HasFood(EFoodType foodType)
    {
        return _foodCount.GetValueOrDefault(foodType) > 0;
    }
}
