using UnityEngine;

public class BouncyWalls : MonoBehaviour
{
    [SerializeField] private float bounciness = 1f; // Adjust bounciness in the Inspector

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Eatable")) // Check for EatableObject collisions
        {
            // Calculate bounce direction
            Vector3 normal = collision.contacts[0].normal;
            Vector3 bounceDirection = Vector3.Reflect(collision.relativeVelocity, normal);

            // Apply bounce force
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = bounceDirection * bounciness;
            }
        }
    }
}