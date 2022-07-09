using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DroneBuilderBoardScript : MonoBehaviour
{
    public GameObject parentPanel;
    public GameObject itemPanel;
    public string partsFolderPath;
    public float itemPanelScale = 1;

    private Vector3 startPos = new Vector3(0, 0, 0);
    private float slideRightIncrement = 0.5f;
    private float rowIncrement = 0.6f;
    public int numPerRow;

    public string category = "Bodies";

    void Start()
    {
        getPanels();
    }

    void getPanels()
    {
        partsFolderPath = Application.dataPath + "/Resources/DroneParts/"+category;

        string[] dirs = Directory.GetFiles(partsFolderPath, "*.meta", SearchOption.TopDirectoryOnly);

        startPos = -(slideRightIncrement * parentPanel.transform.right * (numPerRow/2))/1.2f - (rowIncrement * -parentPanel.transform.up * Mathf.Floor(numPerRow / 2) /2.5f);

        for (int i = 0; i < dirs.Length; i++)
        {
            GameObject newItem = Instantiate(itemPanel);
            newItem.transform.SetParent(parentPanel.transform);
            newItem.transform.rotation = parentPanel.transform.rotation;
            newItem.transform.localScale *= itemPanelScale;
            newItem.transform.position = parentPanel.transform.position + (startPos + (slideRightIncrement * newItem.transform.right * (i % numPerRow)) + (rowIncrement * -newItem.transform.up * Mathf.Floor(i / numPerRow))) *itemPanelScale;

            string itemName = dirs[i].Replace(".meta", "").Replace(partsFolderPath + "\\", "").Replace(".prefab", "").Replace(".mat","");
            string thumbnail = "DroneParts/" + category + "/" + itemName + "/" + itemName;

            newItem.GetComponent<ItemPanelScript>().setItemName(itemName);
            newItem.GetComponent<ItemPanelScript>().setThumbnail(thumbnail);

        }
    }
}
