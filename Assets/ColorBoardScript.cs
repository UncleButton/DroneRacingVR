using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorBoardScript : MonoBehaviour
{
    public GameObject parentPanel;
    public GameObject colorPanel;
    public float itemPanelScale = 1;

    private Vector3 startPos = new Vector3(0, 0, 0);
    private float slideRightIncrement = 250f;
    private float rowIncrement = 250f;
    public int numPerRow;

    public string category;

    private List<string> colors = new List<string>(){"#ffb3b3","#ff8080","#ff4d4d","#ff0000","#e60000","#b30000","#800000","#4d0000",//reds
                                                    "#ffd1b3","#ffb380","#ff944d","#ff751a","#e65c00","#b34700","#803300","#4d1f00",//oranges
                                                    "#ffffb3","#ffff80","#ffff4d","#ffff1a","#e6e600","#b3b300","#808000","#4d4d00",//yellows
                                                    "#b3ffb3","#80ff80","#4dff4d","#1aff1a","#00e600","#00b300","#008000","#004d00",//greens
                                                    "#b3fff0","#80ffe5","#4dffdb","#1affd1","#00e6b8","#00b38f","#008066","#004d3d",//aquas
                                                    "#b3e0ff","#80ccff","#4db8ff","#1aa3ff","#008ae6","#006bb3","#004d80","#002e4d",//light blues
                                                    "#b3b3ff","#8080ff","#4d4dff","#1a1aff","#0000e6","#0000b3","#000080","#00004d",//blues
                                                    "#d1b3ff","#b380ff","#944dff","#751aff","#5c00e6","#4700b3","#330080","#1f004d",//purpBlue
                                                    "#f0b3ff","#e580ff","#db4dff","#d11aff","#b800e6","#8f00b3","#660080","#3d004d",//purple
                                                    "#ffb3d1","#ff80b3","#ff4d94","#ff1a75","#e6005c","#b30047","#800033","#4d001f",//pinks 
                                                    "#ffffff","#e6e6e6","#bfbfbf","#999999","#666666","#404040","#1a1a1a","#000000"};//greyScale
    void Start()
    {
        getPanels();
    }

    void getPanels()
    {

        startPos = -(slideRightIncrement * parentPanel.transform.right * (numPerRow / 2)) / 0.8f - (rowIncrement * -parentPanel.transform.up * Mathf.Floor(numPerRow / 2) / 1.2f);
        for (int i = 0; i < colors.Count; i++)
        {
            GameObject newColor = Instantiate(colorPanel);
            newColor.transform.SetParent(parentPanel.transform);
            newColor.transform.rotation = parentPanel.transform.rotation;
            newColor.transform.localScale *= itemPanelScale;
            newColor.transform.position = parentPanel.transform.position + (startPos + (slideRightIncrement * -newColor.transform.up * (i % numPerRow)) + (rowIncrement * newColor.transform.right * Mathf.Floor(i / numPerRow))) * itemPanelScale;

            newColor.GetComponent<ColorPanelScript>().setColor(colors[i]);
        }
    }
}









