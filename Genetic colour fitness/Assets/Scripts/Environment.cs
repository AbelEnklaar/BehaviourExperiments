using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    private Material _environmentMaterial;
    public float colourTransitionTime = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        _environmentMaterial = gameObject.GetComponent<Renderer>().material;
        StartCoroutine("CycleColours"); 
    }

    IEnumerator CycleColours()
    {
        Vector3 previousColour = new Vector3(_environmentMaterial.color.r, _environmentMaterial.color.g,_environmentMaterial.color.b);
        Vector3 currentColour = previousColour;

        while (true)
        {
            Vector3 newColour = new Vector3(Random.Range(0.0f, 1.0f ), Random.Range(0.0f, 1.0f ),Random.Range(0.0f, 1.0f ));
            Debug.Log("new colour value = " + newColour);

            Vector3 deltaColour = (newColour - previousColour) * (1.0f / colourTransitionTime);

            while ((newColour - currentColour).magnitude > 0.1f)
            {
                currentColour = currentColour + deltaColour * Time.deltaTime;
                _environmentMaterial.color = new Color(currentColour.x, currentColour.y, currentColour.z);
               
                yield return null; 
            }
            previousColour = newColour;
        }
        
    }
}
