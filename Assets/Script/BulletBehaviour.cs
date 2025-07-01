using Assets.Script;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] float _bulletSpeed = 10f;
    public Vector2 direction = Vector2.right;
    private Rigidbody2D _rb;
    private int _damage = 10; // default value, overwritten later
    private bool _isCrit;
    public void SetDamage(int damage, bool isCrit = false)
    {
        _damage = damage;
        _isCrit = isCrit;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.linearVelocity = direction.normalized * _bulletSpeed;
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("ExplosionBarrel"))
        {
            Debug.Log("Hit " + collision.gameObject.name);

            IDamageable target = collision.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDmg(_damage, _isCrit); // use damage from weapon
            }

            Destroy(gameObject);
        }
    }
}
