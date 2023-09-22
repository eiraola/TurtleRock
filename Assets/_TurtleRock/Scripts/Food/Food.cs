using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EFoodType
{
    None,
    Turtle,
    StingRay,
    Octopus,
    Size
}
public abstract class Food: MonoBehaviour
{
    [Header("Food type")]
    [Tooltip("The type of the food")]
    [SerializeField]
    protected EFoodType _foodType = EFoodType.Turtle;
    public abstract void Use();
    public abstract void StopUsing();
    public abstract void Equip();
    public abstract void Unequip();
    
}
