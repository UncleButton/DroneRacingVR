using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeMyColor : MonoBehaviour
{
    public Material basicMaterial;
    void Start()
    {
        Material mat = Instantiate(basicMaterial);
        mat.SetColor("_Color", Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f));
        mat.SetFloat("_Metallic", Random.Range(0, 1));
        mat.SetFloat("_Glossiness", Random.Range(0, 1));
        this.GetComponent<MeshRenderer>().material = mat;
    }

    
}
