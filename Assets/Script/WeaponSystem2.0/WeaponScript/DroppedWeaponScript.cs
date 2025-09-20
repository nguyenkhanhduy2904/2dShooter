using UnityEngine;
using System.Collections;

public class DroppedWeaponScript : MonoBehaviour
{
    public Transform spriteHolder;
    public float pickUpTimer = 2f;
    private float pickUpTimerLeft;
    private Collider2D collider2D;

    public WeaponData2 weaponData;
    public LayerMask layerMask;

    private void Start()
    {
        collider2D = GetComponent<Collider2D>();
        collider2D.enabled = false;
        pickUpTimerLeft = pickUpTimer;

        // Pick final drop pos
        Vector3 targetPos = FindValidDropPosition(transform.position);
        StartCoroutine(MoveToTarget(targetPos));

        // Random flip
        if (spriteHolder != null && Random.Range(0, 2) == 0)
        {
            var rot = spriteHolder.localEulerAngles;
            rot.y += 180f;
            spriteHolder.localEulerAngles = rot;
        }
    }

    private void Update()
    {
        if (pickUpTimerLeft > 0f)
            pickUpTimerLeft -= Time.deltaTime;
        else
            collider2D.enabled = true;
    }

    private Vector3 FindValidDropPosition(Vector3 origin)
    {
        for (int i = 0; i < 20; i++)
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float dist = Random.Range(0.5f, 2f);
            Vector3 candidate = origin + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f) * dist;

            var nnInfo = AstarPath.active.GetNearest(candidate);
            if (nnInfo.node != null && nnInfo.node.Walkable)
            {
                return (Vector3)nnInfo.position; // snap to walkable node
            }
        }
        return origin;
    }


    private IEnumerator MoveToTarget(Vector3 targetPos)
    {
        float duration = 0.3f; // how long to move
        float t = 0f;
        Vector3 startPos = transform.position;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;

            // Arc movement (jump-like)
            float height = Mathf.Sin(progress * Mathf.PI) * 0.5f;
            transform.position = Vector3.Lerp(startPos, targetPos, progress) + new Vector3(0, height, 0);

            yield return null;
        }

        transform.position = targetPos;
    }

    private bool IsWalkable(Vector3 pos)
    {
        // For now just assume always walkable.
        // If using Aron Granberg A*:
        return AstarPath.active.GetNearest(pos).node.Walkable;
        //return true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if ((layerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            var weaponHolder = collision.GetComponent<WeaponHolder2>();
            if (weaponHolder.WeaponData != null) return;
            if (weaponHolder != null)
            {
                weaponHolder.EquipWeapon(weaponData);
                Destroy(gameObject);
            }
        }
    }
}
