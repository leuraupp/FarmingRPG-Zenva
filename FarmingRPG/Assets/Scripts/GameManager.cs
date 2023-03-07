using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int curDay;
    public int money;
    public int wheatInventory;
    public int potatoInventory;

    public CropData selectedCropToPlant;
    public TextMeshProUGUI stats;

    public Buttons[] buttons;

    private Buttons selectedButton; // Botão atualmente selecionado

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

    private void Start() {
        // Adiciona o listener para o evento de desselecionar o botão atual
        foreach (Buttons button in buttons) {
            button.button.onClick.AddListener(DeselectButton);
        }
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
        Debug.Log(crop.name);
        if (crop.name == "Wheat") {
            wheatInventory--;
        } else if (crop.name == "Potato"){
            potatoInventory--;
        }
        UpdateStatsText();
    }

    public void OnHarvestCrop(CropData crop) {
        money += crop.sellPrice;
        UpdateStatsText();
    }

    public void PurchaseCrop(CropData crop) {
        money -= crop.purchasePrice;
        if (crop.name == "Wheat") {
            wheatInventory++;
        } else if (crop.name == "Potato") {
            potatoInventory++;
        }
        UpdateStatsText();
    }

    public bool CanPlantCrop() {
        if (selectedCropToPlant == null) {
            return false;
        }
        if (selectedCropToPlant.name == "Wheat") {
            return wheatInventory > 0;
        } else if (selectedCropToPlant.name == "Potato") {
            return potatoInventory > 0;
        }
        return false;
    }

    public void OnBuyCropButton(CropData crop) {
        if (money >= crop.purchasePrice) {
            PurchaseCrop(crop);
        }
    }

    public void OnSelectCropToPlant(CropData crop) {
        selectedCropToPlant = crop;
    }

    private void UpdateStatsText() {
        stats.text = $"Day: {curDay}\nMoney: ${money}\nWheat Inventory: {wheatInventory}\nPotato Inventory: {potatoInventory}";
    }

    private void DeselectButton() {
        foreach (Buttons button in buttons) {
            button.DeselectButton();
        }
    }
}
