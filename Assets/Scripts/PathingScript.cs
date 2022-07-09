using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathingScript : MonoBehaviour
{
    public List<Transform> Checkpoints;
    public List<Transform> Anchors;
    public List<GameObject> movers;
    public GameObject moverPrefab;

    public float interpolateAmount = 2f;
    public float speedOfPathers = 0.5f;

    public float offset;
    private bool startOffset = false;
    private GameObject tracers;

    private void Start()
    {
        tracers = GameObject.FindGameObjectWithTag("Tracers");
        foreach(Transform child in this.transform)
        {
            Checkpoints.Add(child);
        }
        

        for(int i = 0; i<Checkpoints.Count; i++)
        {
            GameObject newMover = Instantiate(moverPrefab);
            movers.Add(newMover);
            newMover.transform.parent = tracers.transform;

            Anchors.Add(Checkpoints[i].GetChild(1));
            if (i == Checkpoints.Count - 1)//last checkpoint
            {
                Anchors.Add(Checkpoints[0].GetChild(2));
            }
            else
            {
                Anchors.Add(Checkpoints[i+1].GetChild(2));
            }

        }
        
    }

    private void Update()
    {
        if (startOffset)
        {
            interpolateAmount = (interpolateAmount + Time.deltaTime * speedOfPathers) % 1f;
        }
        else
        {
            interpolateAmount = (interpolateAmount + Time.deltaTime * speedOfPathers) % 1f;
            if (interpolateAmount > offset)
            {
                startOffset = true;
                interpolateAmount = 0f;
            }
            else
            {
                return;
            }
        }
        
        for (int i = 0; i < Checkpoints.Count; i++)
        {
            movers[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = Checkpoints[i].transform.GetComponent<MeshRenderer>().material;
            if (i == Checkpoints.Count - 1)//last checkpoint
            {
                movers[i].transform.position = CubicLerp(Checkpoints[i].GetChild(0).position, Anchors[i * 2].position, Anchors[i * 2 + 1].position, Checkpoints[0].GetChild(0).position, interpolateAmount);
            }
            else
            {
                movers[i].transform.position = CubicLerp(Checkpoints[i].GetChild(0).position, Anchors[i * 2].position, Anchors[i * 2 + 1].position, Checkpoints[i + 1].GetChild(0).position, interpolateAmount);
            }
        }

    }

    private Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 ab = Vector3.Lerp(a, b, t);
        Vector3 bc = Vector3.Lerp(b, c, t);

        return Vector3.Lerp(ab, bc, interpolateAmount);
    }
    private Vector3 CubicLerp(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 ab_bc = QuadraticLerp(a, b, c, t);
        Vector3 bc_cd = QuadraticLerp(b, c, d, t);

        return Vector3.Lerp(ab_bc, bc_cd, interpolateAmount);
    }

}
