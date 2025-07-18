using UnityEngine;

public class LootDrop : MonoBehaviour
{
    public Transform spriteHolder;  // assign this in Inspector (child holding SpriteRenderer)

    private Vector3 groundPosition;
    private float verticalVelocity;
    private float gravity = -10f;
    private float currentHeight;
    private Vector3 horizontalVelocity;

    private TrailRenderer trailRenderer;

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
        // Gravity simulation
        verticalVelocity += gravity * Time.deltaTime;
        currentHeight += verticalVelocity * Time.deltaTime;

        if (currentHeight <= 0f)
        {
            currentHeight = 0f;
            verticalVelocity = 0f;
            horizontalVelocity = Vector3.zero;

            if (trailRenderer != null)
            {
                trailRenderer.emitting = false;
            }
        }

        // Move the root object horizontally (ground level)
        groundPosition += horizontalVelocity * Time.deltaTime;
        transform.position = groundPosition;

        // Move the sprite up/down visually (fake jump)
        if (spriteHolder != null)
        {
            spriteHolder.localPosition = new Vector3(0f, currentHeight, 0f);
        }
    }
}
