using Assets.Script;
using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private string _playerName = "Default";
    [SerializeField] private int _playerHealth;
    [SerializeField] private float _playerSpeed;

    public static int PlayerMaxHealth = 100;
    public static float PlayerMaxSpeed = 10f;

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private LayerMask _layerMask;
    private Rigidbody2D _rb;
    private Vector2 _boxSize = new Vector2(0.5f, 0.5f);

    [Header("Weapon")]
    [SerializeField] private WeaponHolder weaponHolder; // Reference to new WeaponHolder component

    [Header("Audio")]
    [SerializeField] private AudioClip[] _hurtedSounds;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerHealth = PlayerMaxHealth;
        _playerSpeed = PlayerMaxSpeed;

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

    private void HandleMovement()// run in fix update
    {
        float moveX = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
        float moveY = Input.GetKey(KeyCode.W) ? 1f : Input.GetKey(KeyCode.S) ? -1f : 0f;

        Vector2 moveDir = new Vector2(moveX, moveY).normalized;
        Vector2 targetPos = _rb.position + moveDir * _moveSpeed * Time.deltaTime;

        // Only move if no wall ahead
        if (!Physics2D.OverlapBox(targetPos, _boxSize, 0f, _layerMask))
        {
            _rb.MovePosition(targetPos);
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
    }

    #endregion

    #region Damage Handling

    public void TakeDmg(int dmg, bool _isCrit)
    {
        _playerHealth -= dmg;
        _playerHealth = Mathf.Clamp(_playerHealth, 0, PlayerMaxHealth);
        Debug.Log($"{_playerName} took {dmg} damage. Health: {_playerHealth}");

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
