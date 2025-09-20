using Assets.Script;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] float _bulletSpeed = 10f;
    public Vector2 direction = Vector2.right;
    private Rigidbody2D _rb;
    public int _damage = 10; // default value, overwritten later
    public bool _isCrit;
    public AudioClip[] arrowInAirSounds;
    public AudioClip[] arrowHitSounds;
    public LayerMask layerMask;
    public bool isDropAtImpact = false;
    public bool isFlyForever = false;
    public GameObject droppedGameObjPrefab;
    public void SetDamage(int damage, bool isCrit = false)
    {
        _damage = damage;
        _isCrit = isCrit;
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.linearVelocity = direction.normalized * _bulletSpeed;
        SoundFXManager.Instance.PlaySoundFXClip(arrowInAirSounds, transform, 1f);
        if (!isFlyForever)
        {
            //Invoke(nameof(DropOrDestroy), 0.85f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((layerMask.value & (1 << collision.gameObject.layer)) != 0)
        {
            Debug.Log("Hit " + collision.gameObject.name);

            IDamageable target = collision.GetComponent<IDamageable>();
            if (target != null)
            {
                target.TakeDmg(_damage, _isCrit);
            }

            SoundFXManager.Instance.PlaySoundFXClip(arrowHitSounds, transform, 1f);
           
            if (isDropAtImpact)
            {
                if (droppedGameObjPrefab != null)
                {
                    Drop();
                }
               
              
            }
            Destroy(gameObject);

        }


    }


    private void DropOrDestroy()
    {
        if (droppedGameObjPrefab != null)
        {
            Drop();
        }
        Destroy(gameObject);
    }

    private void Drop()
    {
        Instantiate(droppedGameObjPrefab, transform.position, Quaternion.identity);
    }
}
