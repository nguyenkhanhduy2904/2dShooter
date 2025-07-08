using Assets.Script;
using UnityEngine;

public class PierceableBullet : MonoBehaviour
{
    [SerializeField] float _bulletSpeed = 10f;
    [SerializeField] int _pierceableUnit = 1; // how many enemies it can pierce

    public Vector2 direction = Vector2.right;
    private Rigidbody2D _rb;

    private int _damage = 10; // default value, set later
    private bool _isCrit;
    private int _unitHit = 0; // <-- now it's tracked across hits


    public void SetPierceCount(int count)
    {
        _pierceableUnit = count;
    }
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
                target.TakeDmg(_damage, _isCrit);
                _unitHit++; // <-- increase pierced unit count
            }

            if (_unitHit >= _pierceableUnit)
            {
                Destroy(gameObject); // <-- destroy only after enough hits
            }
        }
        //else if (collision.CompareTag("Obstacle")) // Optional: bullet stops on wall
        //{
        //    Destroy(gameObject);
        //}
    }
}
