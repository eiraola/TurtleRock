using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FoodCounterUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI turtleFoodText;
    void Start()
    {
        GameObject walletGO = GameObject.FindGameObjectWithTag(Constants.TAG_PLAYER);
        if (!walletGO) { return; }
        FoodWallet wallet = walletGO.GetComponent<FoodWallet>();
        if (!wallet) { return; }
        wallet.OnUpdateFood.AddListener(UpdateFoodUI);
    }
    private void UpdateFoodUI(Dictionary<EFoodType, int> foodInfo)
    {
        turtleFoodText.text = $"X {foodInfo[EFoodType.Turtle]}";
    }
    
}
