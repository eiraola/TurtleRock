using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class FoodWallet : MonoBehaviour
{
    private Dictionary<EFoodType, int> _foodCount = new Dictionary<EFoodType, int>();
    public  UnityEvent<Dictionary<EFoodType, int>> OnUpdateFood = new UnityEvent<Dictionary<EFoodType, int>>();
    public void AddFoodToWallet(EFoodType type)
    {
        if (!_foodCount.ContainsKey(type))
        {
            _foodCount[type] = 0;
        }
        _foodCount[type]++;
        OnUpdateFood.Invoke(_foodCount);
    }
    public bool SpendFood(EFoodType type)
    {
        if (_foodCount.GetValueOrDefault(type) > 0)
        {
            _foodCount[type]--;
            OnUpdateFood.Invoke(_foodCount);
            return true;
        }
        return false;
    }
}
