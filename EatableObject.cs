using UnityEngine;
using UnityEngine.AI;

public class EatableObject : MonoBehaviour
{
    [SerializeField] private float maxHP = 10f;
    [SerializeField] private float fleeSpeed = 5f;
    [SerializeField] private float fleeDistance = 10f;

    public float speed { get; set; } 

    private float currentHP;
    private NavMeshAgent agent;
    private Transform blackHoleTransform;

    private void Start()
    {
        currentHP = maxHP;
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        }

        BlackHole blackHole = FindObjectOfType<BlackHole>();
        if (blackHole != null)
        {
            blackHoleTransform = blackHole.transform;
        }
        else
        {
            Debug.LogError("BlackHole not found in the scene");
        }

        speed = fleeSpeed; 
        if (agent != null)
        {
            agent.speed = speed;
            Debug.Log(gameObject.name + " initial speed: " + speed);
        }
    }

    private void Update()
    {
        Debug.Log(gameObject.name + " current speed: " + agent.speed); 

        if (agent != null && agent.isActiveAndEnabled && blackHoleTransform != null)
        {
            Vector3 fleeDirection = transform.position - blackHoleTransform.position;
            fleeDirection.y = 0;
            fleeDirection.Normalize();

            Vector3 targetPosition = transform.position + fleeDirection * fleeDistance;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, fleeDistance, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        Debug.Log(gameObject.name + " took damage: " + damage + ", Current HP: " + currentHP); 

        if (currentHP <= 0)
        {
            Debug.Log(gameObject.name + " died!");
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return currentHP <= 0;
    }

    public float GetCurrentHP()
    {
        return currentHP;
    }
}