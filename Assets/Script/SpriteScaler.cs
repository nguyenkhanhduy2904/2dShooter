using UnityEngine;

public class SpriteScaler : MonoBehaviour
{
    public float targetUnitSize = 1f;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Start()
    {
        Rescale();
    }

    public void Rescale()
    {
        if (spriteRenderer == null || spriteRenderer.sprite == null)
        {
            Debug.LogWarning("No sprite to scale!");
            return;
        }

        // Instead of bounds, get rect size in pixels
        var rect = spriteRenderer.sprite.rect;
        float pixelsPerUnit = spriteRenderer.sprite.pixelsPerUnit;
        Vector2 sizeInUnits = new Vector2(rect.width / pixelsPerUnit, rect.height / pixelsPerUnit);

        float spriteSize = Mathf.Max(sizeInUnits.x, sizeInUnits.y);

        float scale = targetUnitSize / spriteSize;

        spriteRenderer.transform.localScale = Vector3.one * scale;
    }
}
