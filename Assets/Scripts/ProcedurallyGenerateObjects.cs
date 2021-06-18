using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedurallyGenerateObjects : MonoBehaviour
{
    public GameObject objectToSpread;
    public int numToSpread = 100;
    public float minDist;
    public float maxDist;
    public float yOffset;

    public float minScale;
    public float maxScale;

    public bool giveRandomMat = true;
    public Material basicPlanetMaterial;

    void Start()
    {
        for (int i = 0; i < numToSpread; i++)
        {
            SpreadObject();
        }
    }

    void SpreadObject()
    {
        Vector3 position = new Vector3(Random.Range(minDist, maxDist), Random.Range(minDist, maxDist) + yOffset, Random.Range(minDist, maxDist));
        GameObject clone = Instantiate(objectToSpread, position, Quaternion.identity);
        float size = Random.Range(minScale, maxScale);
        clone.transform.localScale = new Vector3(size, size, size);
        clone.transform.rotation = Quaternion.Euler(new Vector3(Random.Range(0, 180), Random.Range(0, 180), Random.Range(0, 180)));
        clone.transform.SetParent(this.transform);
        if (giveRandomMat)
        {
            Material mat = Instantiate(basicPlanetMaterial);
            mat.SetColor("_Color", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
            mat.SetFloat("_Metallic", Random.Range(0, 1));
            mat.SetFloat("_Glossiness", Random.Range(0, 1));
            clone.GetComponent<MeshRenderer>().material = mat;
        }
        
        
    }

}
