using UnityEngine;

public class SlowEffect : MonoBehaviour
{
    public float slowDuration = 5f; // Duration of the slow effect
    public float slowFactor = 0.5f; // Percentage of speed reduction (0.5 = 50% slower)

    private float originalSpeed;
    private float slowEndTime;

    void Start()
    {
        EatableObject eatable = GetComponent<EatableObject>();
        if (eatable != null)
        {
            originalSpeed = eatable.speed;
        }
    }

    public void ApplySlow()
    {
        EatableObject eatable = GetComponent<EatableObject>();
        if (eatable != null)
        {
            eatable.speed = originalSpeed * slowFactor;
            slowEndTime = Time.time + slowDuration;
        }
    }

    void Update()
    {
        if (Time.time > slowEndTime)
        {
            RemoveSlow();
        }
    }

    void RemoveSlow()
    {
        EatableObject eatable = GetComponent<EatableObject>();
        if (eatable != null)
        {
            eatable.speed = originalSpeed;
        }
        Destroy(this); // Remove the SlowEffect component
    }
}