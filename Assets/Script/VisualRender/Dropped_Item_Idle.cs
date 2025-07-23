using UnityEngine;

public class Dropped_Item_Idle : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Transform sprite;

    [Header("Hover Settings")]
    [Range(0f, 2f)] public float maxHeight = 0.5f;
    [Range(0f, 5f)] public float hoverSpeed = 2f;

    [Header("Color Pulse Settings")]
    [Range(0f, 1f)] public float darkenAmount = 0.6f; // How dark to go (0 = no change, 1 = full black)
    [Range(0f, 1f)] public float fadeAlpha = 0.8f;    // Alpha when darkened
    public float colorSpeed = 2f;

    private Vector3 initialLocalPosition;
    private Color originalColor;

    private void Start()
    {
        if(sprite != null)
        {
            initialLocalPosition = sprite.localPosition;
        }
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
           
    }

    // Update is called once per frame
    void Update()
    {
        if (sprite == null || spriteRenderer == null) return;
        float offset = Mathf.Sin(Time.time * hoverSpeed) * maxHeight;
        sprite.localPosition = initialLocalPosition + new Vector3(0f, offset, 0f);

        float t = (Mathf.Sin(Time.time * colorSpeed) + 1f) / 2f;

        // Create a target color that's a darker version of the original
        Color darker = Color.Lerp(originalColor, Color.black, darkenAmount);
        darker.a = Mathf.Lerp(originalColor.a, fadeAlpha, darkenAmount);

        spriteRenderer.color = Color.Lerp(originalColor, darker, t);
    }
}
