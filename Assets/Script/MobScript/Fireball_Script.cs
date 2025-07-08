using Assets.Script;
using UnityEngine;

public class Fireball_Script : MonoBehaviour
{
    [SerializeField] float _bulletSpeed = 10f;
    [SerializeField] GameObject _explosionPrefab;
    public Vector2 direction = Vector2.right;
    private Rigidbody2D _rb;
    public int _damage = 10; // default value, overwritten later
    public bool _isCrit;
    [SerializeField] AudioClip[] _explosionSounds;
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
        if (collision.CompareTag("Player") || collision.CompareTag("ExplosionBarrel") || collision.CompareTag("Wall"))
        {
            Debug.Log("Hit " + collision.gameObject.name);

            // Instantiate the explosion prefab
            var explo = Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

            // Get the Explosion script component
            Explosion explosionScript = explo.GetComponent<Explosion>();

            // Initialize custom values
            if (explosionScript != null)
            {
                explosionScript.Init(
                    radius: 1.5f,     
                    damage: this._damage,     // set by the wizard stat
                    lifetime: 1f    
                );
            }

            // Play sound
            SoundFXManager.Instance.PlaySoundFXClip(_explosionSounds, transform, 1f);

            // Destroy this projectile or whatever
            Destroy(gameObject);
        }
    }

}
