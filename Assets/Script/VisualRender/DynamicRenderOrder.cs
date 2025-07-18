using UnityEngine;

public class DynamicRenderOrder : MonoBehaviour
{
    Vector3 _lastPosition;
    SpriteRenderer _spriteRenderer;
    const int BASE_RENDER_ORDER = 5000;
    void Start()
    {
        _lastPosition = transform.position;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        int order = BASE_RENDER_ORDER + Mathf.RoundToInt(-transform.position.y * 100);
        _spriteRenderer.sortingOrder = Mathf.Clamp(order, -32768, 32767);
        

    }


    void LateUpdate()
    {
        if (_lastPosition != transform.position)
        {
            int order = BASE_RENDER_ORDER + Mathf.RoundToInt(-transform.position.y * 100);
            _spriteRenderer.sortingOrder = Mathf.Clamp(order, -32768, 32767);

            _lastPosition = transform.position;
        }
    }

}
