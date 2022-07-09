using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPanelScript : MonoBehaviour
{
    public RawImage img;
    public string color;
    private DroneBuilderManager dbManager;
    private ColorBoardScript dbBoardScript;
    public string category;

    public void Start()
    {
        dbManager = GameObject.FindGameObjectWithTag("DroneBuilderManager").GetComponent<DroneBuilderManager>();
        category = this.transform.parent.GetComponent<ColorBoardScript>().category;
    }

    public void setColor(string color)
    {
        this.color = color;

        Color newCol;
        ColorUtility.TryParseHtmlString(color, out newCol);
        img.color = newCol;
    }

    public void setDroneColor()
    {
        Preset currentPreset = dbManager.currentPreset;
        if (category.Equals("Propeller Color (Primary)"))
        {
            currentPreset.propCol1 = color;
            dbManager.destroyAndVisualize(currentPreset);
        }
        else if (category.Equals("Propeller Color (Secondary)"))
        {
            currentPreset.propCol2 = color;
            dbManager.destroyAndVisualize(currentPreset);
        }
        else if (category.Equals("Body Color (Primary)"))
        {
            currentPreset.bodyCol1 = color;
            dbManager.destroyAndVisualize(currentPreset);
        }
        else if (category.Equals("Body Color (Secondary)"))
        {
            currentPreset.bodyCol2 = color;
            dbManager.destroyAndVisualize(currentPreset);
        }
    }

}
