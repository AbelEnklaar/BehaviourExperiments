using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class FlockPopulation : MonoBehaviour
{
    public int populationSize = 100; 
    public GameObject environment;
    public GameObject agentPrefab;
    protected List<Agent> population = new List<Agent>();
    
    
    void Start()
    {
        Bounds boundaries = environment.GetComponent<Renderer>().bounds;

        for (int i = 0; i < populationSize; i++)
        {
            Agent agent = CreateAgent(boundaries, i);
            population.Add(agent);
        }

        StartCoroutine("EvaluationLoop"); 
    }

    public Agent CreateAgent(Bounds bounds, int i)
    {
        Vector3 randomPosition = new Vector3((Random.Range(-0.5f, 0.5f) * bounds.size.x),Random.Range(-0.5f, 0.5f)* bounds.size.y, Random.Range(-0.5f, 0.5f) * bounds.size.z);
        Vector3 worldPosition = environment.transform.position + randomPosition;

        GameObject temp = Instantiate(agentPrefab);
        Agent agent = temp.GetComponent<Agent>();
        float height = temp.GetComponent<MeshFilter>().mesh.bounds.size.y;
        worldPosition.y += height / 2.0f;
        temp.transform.position = worldPosition; 
        temp.name = "agent" + i; 
        AssignRandomColour(temp);
      

        return agent ; 

    }

    public void AssignRandomColour(GameObject agent)
    {
        agent.GetComponent<Agent>()
            .SetColour(new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f))); 
    }

    float EvaluateFitness(Color environment, Color agent)
    {
        float fitness = (new Vector3(environment.r,environment.g,environment.b) - new Vector3(agent.r,agent.g,agent.b)).magnitude;
        return fitness; 
    }

    void EvaluatePopulation()
    {
        //fitness 
        for (int i = 0; i < population.Count; i++)
        {
            float fitness = EvaluateFitness(environment.GetComponent<MeshRenderer>().material.color,
                population[i].GetComponent<MeshRenderer>().material.color);
            population[i].fitnessScore = fitness;
            population[i].Growth();
        }

        // sort 
        population.Sort(
            delegate(Agent agent, Agent agent1)
            {
                if (agent.fitnessScore > agent1.fitnessScore)
                    return 1;
                else if (agent.fitnessScore == agent1.fitnessScore)
                    return 0;
                else
                    return -1;
            });

        //death 
        int halfPoint = (int) (population.Count / 2f);

        if (halfPoint % 2 != 0)
            halfPoint++;

        for (int i = halfPoint; i < population.Count; i++)
        {
            Destroy(population[i].gameObject);
            population[i] = null;
           
            
            
        }

        population.RemoveRange(halfPoint, population.Count - halfPoint);
       
        
    // breed
    
    Breed();
    }

    void Breed()
    {
        //list of offspring 
        List<Agent> tempList = new List<Agent>();

        for (int i = 1; i < population.Count; i += 2)
        {
            int parent1Index = i - 1;
            int parent2Index = i;

            float geneSplit = Random.Range(0.0f, 1.0f);

            Bounds bounds = environment.GetComponent<MeshRenderer>().bounds;

            Agent agentchild1 = CreateAgent(bounds, 0 );
            Agent agentchild2 = CreateAgent(bounds, 0 );

            tempList.Add(agentchild1);
            tempList.Add(agentchild2);

            if (geneSplit <= 0.16f)
            {
                Color tempColour = new Color(population[parent1Index].colour.r, population[parent1Index].colour.g,
                    population[parent2Index].colour.b);
                
                tempColour = EvaluateMutation(tempColour);
                agentchild1.SetColour(tempColour);

                tempColour = new Color(population[parent2Index].colour.r, population[parent2Index].colour.g,
                    population[parent1Index].colour.b);
                
                tempColour = EvaluateMutation(tempColour);
                agentchild2.SetColour(tempColour);
            }
            else if (geneSplit <= 0.32f)
            {
                Color tempColour = new Color(population[parent1Index].colour.r, population[parent2Index].colour.g,
                    population[parent1Index].colour.b);
                
                tempColour = EvaluateMutation(tempColour);
                agentchild1.SetColour(tempColour);

                tempColour = new Color(population[parent2Index].colour.r, population[parent1Index].colour.g,
                    population[parent2Index].colour.b);
                
                tempColour = EvaluateMutation(tempColour);
                agentchild2.SetColour(tempColour);
            }
            else if (geneSplit <= 0.48f)
            {
                Color tempColour = new Color(population[parent1Index].colour.r, population[parent2Index].colour.g,
                    population[parent2Index].colour.b);
                
                tempColour = EvaluateMutation(tempColour);
                agentchild1.SetColour(tempColour);

                tempColour = new Color(population[parent2Index].colour.r, population[parent1Index].colour.g,
                    population[parent1Index].colour.b);
                
                tempColour = EvaluateMutation(tempColour);
                agentchild2.SetColour(tempColour);
            }
            else if (geneSplit <= 0.64f)
            {
                Color tempColour = new Color(population[parent2Index].colour.r, population[parent1Index].colour.g,
                    population[parent1Index].colour.b);
                
                tempColour = EvaluateMutation(tempColour);
                agentchild1.SetColour(tempColour);

                tempColour = new Color(population[parent1Index].colour.r, population[parent1Index].colour.g,
                    population[parent2Index].colour.b);
                
                tempColour = EvaluateMutation(tempColour);
                agentchild2.SetColour(tempColour);
            }
            else if (geneSplit <= 0.80f)
            {
                Color tempColour = new Color(population[parent2Index].colour.r, population[parent2Index].colour.g,
                    population[parent1Index].colour.b);
                
                tempColour = EvaluateMutation(tempColour);
                agentchild1.SetColour(tempColour);

                tempColour = new Color(population[parent1Index].colour.r, population[parent1Index].colour.g,
                    population[parent2Index].colour.b);
                
                tempColour = EvaluateMutation(tempColour);
                agentchild2.SetColour(tempColour);
            }
            else
            {
                Color tempColour = new Color(population[parent2Index].colour.r, population[parent1Index].colour.g,
                    population[parent2Index].colour.b);
                
                tempColour = EvaluateMutation(tempColour);
                agentchild1.SetColour(tempColour);

                tempColour = new Color(population[parent1Index].colour.r, population[parent2Index].colour.g,
                    population[parent1Index].colour.b);
                
                tempColour = EvaluateMutation(tempColour);
                agentchild2.SetColour(tempColour);
            }
        }

        population.AddRange(tempList);
    }

    public Color EvaluateMutation(Color agent)
    {
        float rateOfMutation = 0.1f; 
        
        Vector3 mutatedColour = new Vector3(agent.r, agent.g, agent.b);

        for (int i = 0; i < 3; i++)
        {
            if (Random.Range(0.0f, 1.0f) <= rateOfMutation)
            {
                mutatedColour[i] = Random.Range(0.0f, 1.0f); 
                
            }
        }
        
        return new Color(mutatedColour.x, mutatedColour.y, mutatedColour.z );
    }
    
    IEnumerator EvaluationLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f); 
            EvaluatePopulation();
        }
    }
}
