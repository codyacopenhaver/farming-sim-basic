using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int curDay;
    public int money;
    public int cropInventory;

    public CropData seletedCropToPlant;
    public TextMeshProUGUI statsText;

    public event UnityAction onNewDay;

    private void OnEnable()
    {
        Crop.OnPlantCrop += OnPlantCrop;
        Crop.OnHarvestCrop+= OnHarvestCrop;
    }

    private void OnDisable()
    {
        Crop.OnPlantCrop -= OnPlantCrop;
        Crop.OnHarvestCrop -= OnHarvestCrop;
    }

    // Singleton
    public static GameManager instance;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(instance);
        }
        else
        {
            instance = this;
        }
    }
    public void SetNextDay()
    {
        curDay++;
        onNewDay?.Invoke();
        UpdateStatsText();
    }
    public void OnPlantCrop(CropData crop)
    {
        cropInventory--;
        UpdateStatsText();
    }
    public void OnHarvestCrop(CropData crop)
    {
        money += crop.sellPrice;
        UpdateStatsText();
    }
    public void PurchaseCrop(CropData crop)
    {
        money -= crop.purchasePrice;
        cropInventory++;
        UpdateStatsText();
    }
    public bool CanPlantCrop()
    {
        return cropInventory > 0;
    }
    public void OnBuyCropButton(CropData crop)
    {
        if(money >= crop.purchasePrice)
        {
            PurchaseCrop(crop);
        }
    }

    void UpdateStatsText()
    {
        statsText.text = $"Day: {curDay}\nMoney: ${money}\nCrop Inventory: {cropInventory}";
    }
}
