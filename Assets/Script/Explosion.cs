using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Assets.Script;
using Pathfinding;

public class Explosion : MonoBehaviour
{
    [SerializeField] private float radius = 3f;
    [SerializeField] private int damage = 50;
    [SerializeField] private LayerMask damageableLayers;
    [SerializeField] private float lifetime = 1f;
    //[SerializeField] Transform _explosionPoint;

    private List<IDamageable> targets = new List<IDamageable>();

    void Start()
    {
        Explode();
        Debug.Log("Kaboom!!!");
    }

    public void Init(float radius, int damage, float lifetime)
    {
        this.radius = radius;
        this.damage = damage;
        this.lifetime = lifetime;
    }

    public void Explode()
    {
        // Scan for targets in radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);//, damageableLayers);

        foreach (Collider2D col in colliders)
        {
            IDamageable _object = col.GetComponent<IDamageable>();
            if (_object != null && !targets.Contains(_object))
            {
                targets.Add(_object);
            }
        }

        // Apply damage
        foreach (IDamageable target in targets)
        {
            target.TakeDmg(damage, false);
            Transform targetTransform = ((MonoBehaviour)target).transform;
            Vector3 direction = (targetTransform.position - transform.position).normalized;
            //targetTransform.position += direction * 1f;
            // After applying damage:
            EnemyBehaviour enemy = targetTransform.GetComponent<EnemyBehaviour>();
            if (enemy != null)
            {
                enemy.InterruptAttack();
            }
            StartCoroutine(Knockback(targetTransform, direction, 2f, 0.2f));



        }

        // Optionally play animation or sound here

        // Destroy after lifetime
        //Destroy(gameObject, lifetime);
    }

    private IEnumerator Knockback(Transform target, Vector3 direction, float distance, float duration)
    {
        AILerp ai = target.GetComponent<AILerp>();
        EnemyBehaviour enemy = target.GetComponent<EnemyBehaviour>();

        // Cancel any attack or recover
        if (enemy != null)
        {
            enemy.InterruptAttack();
        }

        if (ai != null)
            ai.enabled = false;

        float elapsed = 0f;
        Vector3 movePerFrame = direction * (distance / duration);

        while (elapsed < duration)
        {
            float deltaMove = Time.deltaTime;
            target.Translate(movePerFrame * deltaMove, Space.World);
            elapsed += deltaMove;
            yield return null;
        }

        if (ai != null)
        {
            ai.enabled = true;
            ai.SearchPath();
        }

        // Always resume chase
        if (enemy != null)
        {
            enemy.ChangeStateToChase();
        }
    }




    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
