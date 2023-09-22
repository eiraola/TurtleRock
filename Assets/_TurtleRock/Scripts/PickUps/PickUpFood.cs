using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFood : PickUpBase
{
    [Tooltip("The type of food that will be added to the wallet")]
    [SerializeField]
    private EFoodType _foodType = EFoodType.Turtle;
    [Tooltip("The quantity of the food that will be added to the wallet")]
    [SerializeField]
    private int _quantity = 1;
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.TAG_PLAYER))
        {
            Collect(other.GetComponent<FoodWallet>());
        }
    }
    /// <summary>
    /// Adds a value to the foodWalled defined by the current foodType and quantity
    /// </summary>
    /// <param name="foodWallet"></param>
    public override void Collect(FoodWallet foodWallet)
    {
        if (!foodWallet) { return; }
        foodWallet.AddFoodToWallet(_foodType, _quantity);
        Destroy(this.gameObject);
    }
}
