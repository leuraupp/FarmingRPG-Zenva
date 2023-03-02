using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldTile : MonoBehaviour
{
    private Crop curCrop;
    public GameObject cropPrefab;

    public SpriteRenderer sr;
    private bool tilled;

    [Header("Sprites")]
    public Sprite grassSprite;
    public Sprite tilledSprite;
    public Sprite wateredTilledSprite;

    private void Start() {
        sr.sprite = grassSprite;
    }

    public void Interact() {
        Debug.Log("alo você");
        if (!tilled) {
            Till();
        } else if (!HasCrop() && GameManager.instance.CanPlantCrop()) {
            plantNewCrop(GameManager.instance.selectedCropToPlant);
        } else if (HasCrop() && curCrop.CanHarvest()) {
            curCrop.Harvest();
        } else {
            Water();
        }
    }

    private void plantNewCrop(CropData crop) {
        if (!tilled) {
            return;
        }

        curCrop = Instantiate(cropPrefab, transform).GetComponent<Crop>();
        curCrop.Plant(crop);

        GameManager.instance.onNewDay += OnNewDay;
    }

    private void Till() {
        tilled = true;
        sr.sprite = tilledSprite;
    }

    private void Water() {
        sr.sprite = wateredTilledSprite;

        if (HasCrop()) {
            curCrop.Water();
        }
    }

    private void OnNewDay() {
        if (curCrop == null) {
            tilled = false;
            sr.sprite = grassSprite;

            GameManager.instance.onNewDay -= OnNewDay;
        } else {
            sr.sprite = tilledSprite;
            curCrop.NewDayCheck();
        }
    }

    private bool HasCrop() {
        return curCrop != null;
    }
}
