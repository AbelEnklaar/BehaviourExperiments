using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{

    public float fitnessScore = 1.0f;
    public Color colour = new Color(1.0f, 1.0f, 1.0f);
    public Material material;

    // Start is called before the first frame update
    void Awake()
    {
        material = gameObject.GetComponent<Renderer>().material;
    }

    public void SetColour(Color colour)
    {
        this.colour = colour;
        material.color = colour; 
    }
}
