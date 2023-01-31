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

    void Start()
    {
        // Set the default grass sprite
        sr.sprite = grassSprite;
    }
    public void Interact()
    {
        if (!tilled)
        {
            Till();
        }
        else if (!HasCrop() && GameManager.instance.CanPlantCrop())
        {
            PlantNewCrop(GameManager.instance.seletedCropToPlant);
        }
        else if (HasCrop() && curCrop.CanHarvest())
        {
            curCrop.Harvest();
            //Debug.Log("harvest?");
        }
        else
        {
            Water();
        }


        //gameObject.SetActive(false);
        //Debug.Log("Interacted!");   
    }

    private void PlantNewCrop(CropData crop)
    {
        if (!tilled)
        {
            return;
        }
        curCrop = Instantiate(cropPrefab, transform).GetComponent<Crop>();
        curCrop.Plant(crop);

        GameManager.instance.onNewDay += OnNewDay;
    }
    private void Till()
    {
        tilled = true;
        sr.sprite = tilledSprite;
    }
    private void Water()
    {
        sr.sprite = wateredTilledSprite;

        if (HasCrop())
        {
            curCrop.Water();
        }
    }
    private void OnNewDay()
    {
        // check on new day if crop was harvest or destroyed on previous day and reset if so and unsub from event
        if(curCrop == null)
        {
            tilled = false;
            sr.sprite = grassSprite;

            GameManager.instance.onNewDay -= OnNewDay;
        } 
        else if(curCrop != null) // remove water and make tilled and do check for new day
        {
            sr.sprite = tilledSprite;
            curCrop.NewDayCheck();
        }
    }
    bool HasCrop()
    {
        return curCrop != null;
    }
}
