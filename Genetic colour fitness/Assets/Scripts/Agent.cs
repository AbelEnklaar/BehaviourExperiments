using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Agent : MonoBehaviour
{

    public float fitnessScore = 1.0f;
    public Color colour = new Color(1.0f, 1.0f, 1.0f);
    public Material material;
    public Transform agentScale; 

    // Start is called before the first frame update
    void Awake()
    {
        material = gameObject.GetComponent<Renderer>().material;
        agentScale = transform; 
    }

    public void SetColour(Color colour)
    {
        this.colour = colour;
        material.color = colour; 
    }

    public void Growth()
    {
        gameObject.transform.localScale *= 1.2f; 
    }
}
