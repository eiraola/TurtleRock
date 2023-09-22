using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
/// Structure to define a piece of UI corresponding to a type of food
public struct FoodUI
{
    public EFoodType foodType;
    public TextMeshProUGUI textUI;
    public Image pointer;
    public void UpdateText(string text)
    {
        if (!textUI)
        {
            return;
        }
        textUI.text = text;
    }
    public void ActivatePointer()
    {
        if (!pointer)
        {
            return;
        }
        pointer.gameObject.SetActive(true);
    }
    public void DeactivatePointer()
    {
        if (!pointer)
        {
            return;
        }
        pointer.gameObject.SetActive(false);
    }
}
public class FoodCounterUI : MonoBehaviour
{
    [SerializeField]
    private List<FoodUI> _foodUIList = new List<FoodUI>();
    [SerializeField]
    private TextMeshProUGUI turtleFoodText;
    GameObject playerGO;
    private FoodUI UIToUpdate;
    void Start()
    {
        playerGO = GameObject.FindGameObjectWithTag(Constants.TAG_PLAYER);
        if (!playerGO) { return; }
        FoodWallet wallet = playerGO.GetComponent<FoodWallet>();
        if (!wallet) { return; }
        wallet.OnUpdateFood.AddListener(UpdateFoodUI);
        FoodEquiper launcher = playerGO.GetComponent<FoodEquiper>();
        if (!launcher) { return; }
        launcher.OnEquippedFoodChange.AddListener(UpdateEquipUI);
        
    }
    private void UpdateFoodUI(EFoodType foodType, Dictionary<EFoodType, int> foodInfo)
    {
        UIToUpdate = _foodUIList.Find(x => x.foodType == foodType);
        UIToUpdate.UpdateText($"X {foodInfo[foodType]}");
    }
    private void UpdateEquipUI(EFoodType foodType)
    {
        Debug.LogError(foodType);
        foreach (FoodUI foodUI in _foodUIList)
        {
            if (foodUI.foodType == foodType)
            {
                foodUI.ActivatePointer();
                continue;
            }
            foodUI.DeactivatePointer();
        }
    }

}
