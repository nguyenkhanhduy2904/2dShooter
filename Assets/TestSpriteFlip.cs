using UnityEngine;

public class TestSpriteFlip : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    void Start()
    {
        if (spriteRenderer != null)
        {
            Debug.Log("Assigned SpriteRenderer on: " + spriteRenderer.gameObject.name);
        }
        else
        {
            Debug.LogError("SpriteRenderer not assigned!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
            Debug.Log("Flipped sprite. flipX now: " + spriteRenderer.flipX);
        }
    }
}
