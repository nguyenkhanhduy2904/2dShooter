using Assets.Script;
using System.Collections;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private HealthBarScript healthBar;
    [Header("Stats")]
    [SerializeField] private string _playerName = "Default";
    [SerializeField] private int _playerHealth;
    [SerializeField] private float _playerSpeed;

    public static int PlayerMaxHealth = 100;
    public static float PlayerMaxSpeed = 10f;

    int _playerCoins;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private LayerMask _layerMask;
    private Rigidbody2D _rb;
    private Vector2 _boxSize = new Vector2(0.5f, 0.5f);

    [Header("Weapon")]
    [SerializeField] private WeaponHolder weaponHolder; // Reference to new WeaponHolder component

    [Header("Audio")]
    [SerializeField] private AudioClip[] _hurtedSounds;
    [SerializeField] private AudioClip[] _healingSounds;
    [SerializeField] private AudioClip[] _getCoinSounds;


    [SerializeField] private GameObject explosionPrefab;

    [SerializeField] private GameObject _floatingTextPreFab;

    private void Start()
    {
        _playerCoins = 0;
        _rb = GetComponent<Rigidbody2D>();
        _playerHealth = PlayerMaxHealth;
        _playerSpeed = PlayerMaxSpeed;
        healthBar.SetMaxHealth(_playerHealth);

        if (weaponHolder == null)
            Debug.LogWarning("PlayerController: WeaponHolder is not assigned!");
    }

    private void Update()
    {
        //HandleMovement();
        if (weaponHolder != null)
        {
            HandleInput();
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                Debug.Log("You need to find a weapon");
                
            }
        }
       
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    #region Movement

    private void HandleMovement()
    {
        float moveX = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
        float moveY = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;

        Vector2 moveDir = new Vector2(moveX, moveY).normalized;

        Vector2 targetPos = _rb.position + moveDir * _moveSpeed * Time.fixedDeltaTime;

        // Check if wall is ahead
        if (!Physics2D.OverlapBox(targetPos, _boxSize, 0f, _layerMask))
        {
            _rb.linearVelocity = moveDir * _moveSpeed;
        }
        else
        {
            _rb.linearVelocity = Vector2.zero; // Block movement if wall ahead
        }
    }




    //private void HandleMovement()
    //{
    //    float moveX = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
    //    float moveY = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;

    //    Vector2 moveDir = new Vector2(moveX, moveY).normalized;
    //    Vector3 targetPos = transform.position + (Vector3)(moveDir * _moveSpeed * Time.deltaTime);

    //    if (!Physics2D.OverlapBox(targetPos, _boxSize, 0f, _layerMask))
    //    {
    //        transform.position = targetPos;
    //    }
    //}

    #endregion

    #region Input Handling

    private void HandleInput()
    {
        
        weaponHolder.RotateToMouse();
        weaponHolder.HandleShooting();
        weaponHolder.HandleReload();
        HandleExplosion();
    }

    private void HandleExplosion()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            // Instantiate explosion at player position
            GameObject explosionObj = Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity
            );

            // (Optional) Configure explosion
            //Explosion explosion = explosionObj.GetComponent<Explosion>();
            //explosion.Init(5f, 80, 2f); // radius, damage, lifetime
        }
    }

    #endregion

    #region Damage Handling

    public void TakeDmg(int dmg, bool _isCrit)
    {
        _playerHealth -= dmg;
        _playerHealth = Mathf.Clamp(_playerHealth, 0, PlayerMaxHealth);
        Debug.Log($"{_playerName} took {dmg} damage. Health: {_playerHealth}");
        healthBar.SetHealth(_playerHealth);

        SoundFXManager.Instance?.PlaySoundFXClip(_hurtedSounds, transform, 1f);

        if (_playerHealth <= 0)
        {
            Die();
        }
    }

    public void DealDmg(IDamageable target)
    {
        // Future expansion
    }

    public void ShowDamage(string text, bool isCrit = false)
    {
        GameObject prefab = Instantiate(_floatingTextPreFab, transform.position, Quaternion.identity);
        TextMeshPro textMesh = prefab.GetComponentInChildren<TextMeshPro>();

        textMesh.text = text;

        // Color and style based on crit
        if (isCrit)
        {
            textMesh.color = Color.red;
            textMesh.fontSize = 10f;
            // Enable Crit Icon
            Transform critIcon = prefab.transform.Find("FloatingText/CritIcon");


            if (critIcon != null)
            {
                Debug.Log("found it");
                critIcon.gameObject.SetActive(true);
            }


        }
        else
        {
            textMesh.color = Color.white;
            textMesh.fontSize = 5f;
        }

        Destroy(prefab, 1f);
    }

    public void Heal(int amount)
    {
        _playerHealth += amount;
        _playerHealth = Mathf.Clamp(_playerHealth, 0, PlayerMaxHealth);
        healthBar.SetHealth(_playerHealth);
        SoundFXManager.Instance.PlaySoundFXClip(_healingSounds, transform, 1f);
        Debug.Log($"{_playerName} heal {amount}. Health: {_playerHealth}");
    }

    public void GetCoin()
    {
        _playerCoins++;
        SoundFXManager.Instance.PlaySoundFXClip(_getCoinSounds, transform, 1f);
    }

    private void Die()
    {
        Debug.Log($"{_playerName} died.");
        // TODO: Add death animation, respawn, etc.
    }

    #endregion

    #region Properties
    
    public int PlayerHealth => _playerHealth;
    public float PlayerSpeed => _playerSpeed;
    public string PlayerName => _playerName;

    #endregion
}
