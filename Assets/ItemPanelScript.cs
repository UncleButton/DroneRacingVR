using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemPanelScript : MonoBehaviour
{
    public Text itemName;
    public GameObject imageOnPanel;
    private RawImage img;
    private DroneBuilderManager dbManager;
    private DroneBuilderBoardScript dbBoardScript;
    public string category;

    public void Start()
    {
        dbManager = GameObject.FindGameObjectWithTag("DroneBuilderManager").GetComponent<DroneBuilderManager>();
        category = this.transform.parent.GetComponent<DroneBuilderBoardScript>().category;
    }
    public void setThumbnail(string path)
    {
        Texture thumbnail = Resources.Load<Texture2D>(path);
        img = (RawImage)imageOnPanel.GetComponent<RawImage>();
        img.texture = (Texture)thumbnail;
    }

    public void setItemName(string name)
    {
        itemName.text = name;
        
    }

    public void equipItem()
    {
        Preset currentPreset = dbManager.currentPreset;
        if (category.Equals("Bodies"))
        {
            currentPreset.body = itemName.text;
            dbManager.destroyAndVisualize(currentPreset);
        }
        else if (category.Equals("Propellers"))
        {
            currentPreset.prop = itemName.text;
            dbManager.destroyAndVisualize(currentPreset);
        }
        else if (category.Equals("Propeller Materials (Primary)"))
        {
            currentPreset.propMat1 = itemName.text;
            dbManager.destroyAndVisualize(currentPreset);
        }
        else if (category.Equals("Propeller Materials (Secondary)"))
        {
            currentPreset.propMat2 = itemName.text;
            dbManager.destroyAndVisualize(currentPreset);
        }
        else if (category.Equals("Body Materials (Primary)"))
        {
            currentPreset.bodyMat1 = itemName.text;
            dbManager.destroyAndVisualize(currentPreset);
        }
        else if (category.Equals("Body Materials (Secondary)"))
        {
            currentPreset.bodyMat2 = itemName.text;
            dbManager.destroyAndVisualize(currentPreset);
        }

    }
}
