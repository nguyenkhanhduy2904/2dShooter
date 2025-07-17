using UnityEngine;

public class LootDrop : MonoBehaviour
{
    private Vector3 groundPosition;
    private float verticalVelocity;
    private float gravity = -10f;
    private float currentHeight;
    private TrailRenderer trailRenderer;

    private Vector3 horizontalVelocity;

    void Start()
    {
        groundPosition = transform.position;

        // Pop up
        verticalVelocity = Random.Range(3f, 5f);

        // Drift outward a bit
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float speed = Random.Range(1f, 2f); // adjust to taste
        horizontalVelocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * speed;
    }

    void Update()
    {
        verticalVelocity += gravity * Time.deltaTime;
        currentHeight += verticalVelocity * Time.deltaTime;

        if (currentHeight <= 0f)
        {
            currentHeight = 0f;
            verticalVelocity = 0f;
            horizontalVelocity = Vector3.zero;

            if (trailRenderer != null)
            {
                // Stop emitting new trail
                trailRenderer.emitting = false;

                //// Immediately clear any remaining trail
                //trailRenderer.Clear();
            }
        }

        groundPosition += horizontalVelocity * Time.deltaTime;
        transform.position = groundPosition + new Vector3(0f, currentHeight, 0f);
    }

}
