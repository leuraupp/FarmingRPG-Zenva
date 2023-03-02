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

    public CropData selectedCropToPlant;
    public TextMeshProUGUI stats;

    public static GameManager instance;

    public event UnityAction onNewDay;

    private void OnEnable() {
        Crop.onPlantCrop += OnPlantCrop;
        Crop.onHarvestCrop += OnHarvestCrop;
    }

    private void OnDisable() {
        Crop.onPlantCrop -= OnPlantCrop;
        Crop.onHarvestCrop -= OnHarvestCrop;
    }

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
        UpdateStatsText();
    }

    public void SetNextDay() {
        curDay++;
        onNewDay?.Invoke();
        UpdateStatsText();
    }

    public void OnPlantCrop(CropData crop) {
        cropInventory--;
        UpdateStatsText();
    }

    public void OnHarvestCrop(CropData crop) {
        money += crop.sellPrice;
        UpdateStatsText();
    }

    public void PurchaseCrop(CropData crop) {
        money -= crop.purchasePrice;
        cropInventory++;
        UpdateStatsText();
    }

    public bool CanPlantCrop() {
        return cropInventory > 0;
    }

    public void OnBuyCropButton(CropData crop) {
        if (money >= crop.purchasePrice) {
            PurchaseCrop(crop);
        }
    }

    private void UpdateStatsText() {
        stats.text = $"Day: {curDay}\nMoney: ${money}\nCrop Inventory: {cropInventory}";
    }
}
