using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUpBase : MonoBehaviour
{
    public abstract void Collect(FoodWallet foodWallet);
}
