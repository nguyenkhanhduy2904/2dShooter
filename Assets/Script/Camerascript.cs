using UnityEngine;

public class Camerascript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] Transform target;
    [SerializeField] Vector3 _offset;
    float _smoothSpeed = 10f;

    private void LateUpdate()
    {
        if (target == null) 
        {
            return;
        }
        Vector3 desiredPosition = target.position + _offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, _smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;


    }
}
