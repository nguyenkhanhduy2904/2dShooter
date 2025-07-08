using UnityEngine;
using System.Collections;
using Pathfinding;

public class Wizard_Default_Script : EnemyBehaviour
{
    [SerializeField] GameObject bulletPrefab;

    
    [SerializeField] float teleportRadius = 5f;
    [SerializeField] int maxAttempts = 8;
    [SerializeField] float teleportCoolDown = 5f;
    bool _canTeleport = true;


    public override void Update()
    {
        base.Update();
        if (_canTeleport == false) return;
        float dist = Vector2.Distance(transform.position, _player.transform.position);
        if (dist < 2f) // Example trigger
        {
            InterruptAttack();
            TryRandomTeleport();
            //StartCoroutine(TeleportCooldown());
            //_canTeleport = false; // Maybe you add a cooldown flag
            ChangeState(EnemyState.Idle); //  THIS is essential
        }
    }
    public void TryRandomTeleport()
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            // Random angle in radians
            float angle = Random.Range(0f, Mathf.PI * 2f);

            // Random point in a circle
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * teleportRadius;

            Vector3 candidatePos = _player.transform.position + (Vector3)offset;

            // Optional: clamp inside map bounds
            candidatePos.x = Mathf.Clamp(candidatePos.x, -7.5f, 7.5f);
            candidatePos.y = Mathf.Clamp(candidatePos.y, -4f, 4f);

            // Check A* walkability
            GraphNode node = AstarPath.active.GetNearest(candidatePos).node;
            if (node != null && node.Walkable)
            {
                Debug.Log("Teleporting to: " + candidatePos);
                aiLerp.Teleport(candidatePos); // <-- ONLY THIS
                _canTeleport = false;
                return;
            }
        }

        // All attempts failed
        Debug.Log("Teleport failed: no walkable spot found.");
    }

    IEnumerator TeleportCooldown()// use this if the wizard can teleport multiple times
    {
        yield return new WaitForSeconds(teleportCoolDown);
        _canTeleport = true;
    }


    public override IEnumerator ChargeAndAttack(float animLenght)
    {
        Debug.Log("Charging...");
        yield return new WaitForSeconds(animLenght);

        float dist = Vector2.Distance(transform.position, _player.transform.position);

        if (dist > _enemyRange)
        {
            ChangeState(EnemyState.Chase);
            Debug.Log("Player too run away while charging");
            yield break;
        }

        Debug.Log("Swing!");
        if (dist <= _enemyRange)
        {

            //_playerTarget.TakeDmg(_enemyAtk, false);
            FireProjectile();
            ChangeState(EnemyState.Recover);
        }
       

    }

    private void FireProjectile()
    {
        Vector3 dir = (_player.transform.position - transform.position).normalized;

        GameObject bulletObj = Instantiate(
            bulletPrefab,
            transform.position,
            Quaternion.identity
        );

        Fireball_Script bullet = bulletObj.GetComponent<Fireball_Script>();
        bullet.direction = dir;
        bullet._damage = _enemyAtk; // Or any custom damage
        bullet._isCrit = false;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bulletObj.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
