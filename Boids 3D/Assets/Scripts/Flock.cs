using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockAgent agentPrefab; 
    List<FlockAgent> agents = new List<FlockAgent>();

    public FlockBehaviour behaviour;

    [Range(10, 2000)] public int startingCount = 250;
    const float agentDensity = 0.08f;

    [Range(1f, 100f)] public float driveFactor = 10f;
    [Range(1f, 100f)] public float maxSpeed = 5f;
    
    [Range(1f, 10f)] public float neighbourRadius = 1.5f;
    [Range(0f, 1f)] public float avoidanceRadiusMultiplier = 0.5f;

    private float squareMaxSpeed;
    private float sqaureNeighbourRadius;
    private float squareAvoidanceRadius;

    public float squareAvoidanceRadiusF
    {
        get { return squareAvoidanceRadius; }
    }


   
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        sqaureNeighbourRadius = neighbourRadius * neighbourRadius;
        squareAvoidanceRadius = sqaureNeighbourRadius * avoidanceRadiusMultiplier;

        for (int i = 0; i < startingCount; i++)
        {
            FlockAgent newAgent = Instantiate(
                agentPrefab,
                Random.insideUnitSphere * startingCount * agentDensity,
                Quaternion.Euler(Vector3.one * Random.Range(0f, 360f)),
                transform
            );
                newAgent.name = "Agent" + i;
                newAgent.Initialise(this);
                agents.Add(newAgent);
        }
    }
    
    void Update()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = getNearbyObjects(agent);
           // agent.GetComponentInChildren<Renderer>().material.color = Color.Lerp(Color.blue, Color.red, context.Count / 6f);

           Vector3 move = behaviour.CalculateMove(agent, context, this);
              move *= driveFactor;
             if (move.sqrMagnitude > squareMaxSpeed)
              {
                  move = move.normalized * maxSpeed;
             }
              agent.Move(move);
        }
    }

    List<Transform> getNearbyObjects(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();
        Collider[] contextCollider = Physics.OverlapSphere(agent.transform.position, neighbourRadius);
        foreach (Collider c in contextCollider)
        {
            if (c != agent.AgentCollider)
            {
                context.Add(c.transform);
            }
        }
        return context; 
    }
}
