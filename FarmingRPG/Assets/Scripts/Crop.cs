using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Crop : MonoBehaviour
{
    private CropData curCrop;
    private int plantday;
    private int dayssinceLastWatered;

    public SpriteRenderer sr;

    public static event UnityAction<CropData> onPlantCrop;
    public static event UnityAction<CropData> onHarvestCrop;

    public void Plant(CropData crop) {
        curCrop = crop;
        plantday = GameManager.instance.curDay;
        dayssinceLastWatered = 1;
        UpdateCropSprite();

        onPlantCrop?.Invoke(crop);
    }

    public void NewDayCheck() {
        if (dayssinceLastWatered > 3) {
            Destroy(gameObject);
        }

        UpdateCropSprite();
    }

    private void UpdateCropSprite() {
        int cropProg = CropProgress();

        if (cropProg < curCrop.daysToGrow) {
            sr.sprite = curCrop.growProgressSprites[cropProg];
        } else {
            sr.sprite = curCrop.readyToHavestSprite;
        }
    }

    public void Water() {
        dayssinceLastWatered = 0;
    }

    public void Harvest() {
        if (CanHarvest()) {
            onHarvestCrop?.Invoke(curCrop);
            Destroy(gameObject);
        }
    }

    private int CropProgress() {
        return GameManager.instance.curDay - plantday;
    }

    public bool CanHarvest() {
        return CropProgress() > curCrop.daysToGrow;
    }
}
