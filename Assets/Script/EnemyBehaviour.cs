using UnityEngine;
using System.Collections;
using Assets.Script;

public class EnemyBehaviour : MonoBehaviour, IDamageable
{
    enum EnemyState { Idle, Chase, Charge, Attack, Recover, Dead }

    [Header("Stats")]
    [SerializeField] string _enemyName = "Creep";
    [SerializeField] int _enemyMaxHealth = 100;
    [SerializeField] int _enemyAtk = 30;
    [SerializeField] float _enemySpeed = 5f;
    [SerializeField] float _enemyAtkSpeed = 1.5f;
    [SerializeField] float _enemyRange = 1.5f;
    [SerializeField] int _segments = 60;
    

    bool _canAttack = true;

    int _enemyHealth;
    EnemyState _currentState;

    [Header("Targeting")]
    GameObject _player;
    IDamageable _playerTarget;

    [Header("Components")]
    Rigidbody2D _rb;
    LineRenderer _rangeCircle;

    Coroutine _currentStateRoutine;
    Coroutine _recoveryRoutine;

    [Header("Audio")]
    [SerializeField] private AudioClip[] _enemyHurtedSounds;
    [SerializeField] private AudioClip[] _enemyCritHurtedSounds;




    void Awake()
    {
        _enemyHealth = _enemyMaxHealth;
    }

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _playerTarget = _player.GetComponent<IDamageable>();
        _rb = GetComponent<Rigidbody2D>();
        _rangeCircle = GetComponent<LineRenderer>();
        DrawRangeCircle();

        ChangeState(EnemyState.Idle);
    }

    void Update()
    {
        if (_player == null || _currentState == EnemyState.Dead) return;

        float dist = Vector2.Distance(transform.position, _player.transform.position);

        switch (_currentState)
        {
            case EnemyState.Idle:
                if (dist <= _enemyRange * 3f) // detect player
                    ChangeState(EnemyState.Chase);
                break;

            case EnemyState.Chase:
                MoveToPlayer();
                if (dist <= _enemyRange && _canAttack)
                    ChangeState(EnemyState.Charge);
                break;

            case EnemyState.Charge:
            case EnemyState.Attack:
            case EnemyState.Recover:
                //MoveToPlayer();
                // Handled by coroutine
                break;
        }
    }

    void MoveToPlayer()
    {
        Vector2 direction = (_player.transform.position - transform.position).normalized;
        _rb.linearVelocity = direction * _enemySpeed;
        float dist = Vector2.Distance(transform.position, _player.transform.position);
        if (dist <= _enemyRange)
        {
            ChangeState(EnemyState.Idle);
        }
    }

    void ChangeState(EnemyState newState)
    {
        // If changing to the same state, skip (except maybe Charge to force restart)
        if (_currentState == newState && newState != EnemyState.Charge) return;

        _currentState = newState;

        // Optionally stop only the main coroutine if needed
        if (_currentStateRoutine != null)
        {
            StopCoroutine(_currentStateRoutine);
            _currentStateRoutine = null;
        }

        switch (_currentState)
        {
            case EnemyState.Idle:
                _rb.linearVelocity = Vector2.zero;
                break;

            case EnemyState.Chase:
                break;

            case EnemyState.Charge:
                _rb.linearVelocity = Vector2.zero;
                _currentStateRoutine = StartCoroutine(ChargeAndAttack());
                break;

            case EnemyState.Recover:
                if (_recoveryRoutine != null)
                    StopCoroutine(_recoveryRoutine);
                _recoveryRoutine = StartCoroutine(Recover());
                break;

            case EnemyState.Dead:
                _rb.linearVelocity = Vector2.zero;
                Die();
                break;
        }
    }


    IEnumerator ChargeAndAttack()
    {
        Debug.Log("Charging...");
        yield return new WaitForSeconds(0.25f);

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
            _playerTarget.TakeDmg(_enemyAtk, false);
            ChangeState(EnemyState.Recover);
        }
        else
        {
            Debug.Log("Player dogded!!");
            ChangeState(EnemyState.Chase);
        }
       
    }

    IEnumerator Recover()
    {
        _canAttack = false;
        Debug.Log("Recovering...");

        float timer = 0f;
        while (timer < _enemyAtkSpeed)
        {
            MoveToPlayer();
            timer += Time.deltaTime;
            yield return null;
        }

        _canAttack = true;

        if (_currentState == EnemyState.Recover)
            ChangeState(EnemyState.Chase);
    }


    void DrawRangeCircle()
    {
        _rangeCircle.positionCount = _segments + 1;
        _rangeCircle.widthMultiplier = 0.05f;

        for (int i = 0; i <= _segments; i++)
        {
            float angle = i * 2 * Mathf.PI / _segments;
            float x = Mathf.Cos(angle) * _enemyRange;
            float y = Mathf.Sin(angle) * _enemyRange;
            _rangeCircle.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    public void TakeDmg(int amount, bool isCrit = false)
    {
        if (_currentState == EnemyState.Dead) return;

        _enemyHealth -= amount;
        _enemyHealth = Mathf.Clamp(_enemyHealth, 0, _enemyMaxHealth);

        if (isCrit)
        {
            Debug.Log($"{_enemyName} took a CRITICAL hit: {amount} damage!");
            SoundFXManager.Instance.PlaySoundFXClip(_enemyCritHurtedSounds, transform, 0.5f);
        }
            
        else
        {
            Debug.Log($"{_enemyName} took {amount} damage.");
            SoundFXManager.Instance.PlaySoundFXClip(_enemyHurtedSounds, transform, 0.5f);
        }
            


        if (_enemyHealth <= 0)
            ChangeState(EnemyState.Dead);
    }


    public void Heal(int amount)
    {
        _enemyHealth += amount;
        _enemyHealth = Mathf.Clamp(_enemyHealth, 0, _enemyMaxHealth);
        Debug.Log($"{_enemyName} healed {amount}. HP now: {_enemyHealth}");
    }

    public void DealDmg(IDamageable target)
    {
        target.TakeDmg(_enemyAtk , false);
    }

    void Die()
    {
        Debug.Log($"{_enemyName} has died.");
        Destroy(gameObject);
        SceneManager.NotifyEnemyDied();
        
    }
}
