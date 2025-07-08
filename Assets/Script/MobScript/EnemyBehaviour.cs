using UnityEngine;
using System.Collections;
using Assets.Script;
using TMPro;
using Pathfinding;

public class EnemyBehaviour : MonoBehaviour, IDamageable
{
    //[SerializeField] Orc_Default_SO mobSO;
    [SerializeField] Mob_SO _mobData;
    [SerializeField] LayerMask obstacleMask;

    protected enum EnemyState { Idle, Chase, Charge, Attack, Recover, Dead }

    [Header("Stats")]
    [SerializeField] string _enemyName;
    [SerializeField] int _enemyMaxHealth;
    [SerializeField] protected int _enemyAtk;
    [SerializeField] float _enemySpeed;
    [SerializeField] float _enemyAtkSpeed;
    [SerializeField] protected float _enemyRange;
    [SerializeField] protected float _enemyAggroRange;
    [SerializeField] int _segments = 60;
    

    bool _canAttack = true;

    int _enemyHealth;
    protected EnemyState _currentState;

    [Header("Targeting")]
    protected GameObject _player;
    protected IDamageable _playerTarget;
    //public AIPath aiPath;
    public AILerp aiLerp;
    bool wasHit = false;

    [Header("Components")]
    Rigidbody2D _rb;
    LineRenderer _rangeCircle;
    Animator _animator;

    Coroutine _currentStateRoutine;
    Coroutine _recoveryRoutine;

    [Header("Audio")]
    [SerializeField] private AudioClip[] _enemyHurtedSounds;
    [SerializeField] private AudioClip[] _enemyCritHurtedSounds;


    [SerializeField] private GameObject _floatingTextPreFab;

    public SpriteRenderer spriteRenderer;
    //Transform _spriteTransform;

    void Awake()
    {
        _enemyName = _mobData._enemyName;
        _enemyMaxHealth = _mobData._enemyMaxHealth;
        _enemyAtk = _mobData._enemyAtk;
        _enemySpeed = _mobData._enemySpeed;
        _enemyAtkSpeed = _mobData._enemyAtkSpeed;
        _enemyRange = _mobData._enemyRange;
        _enemyAggroRange = _mobData._enemyAggroRange;

        _enemyHurtedSounds = _mobData._enemyHurtedSounds;
        _enemyCritHurtedSounds = _mobData._enemyCritHurtedSounds;

        _floatingTextPreFab = _mobData._floatingTextPreFab;//init stat and resource





        _enemyHealth = _enemyMaxHealth;
    }

    void Start()
    {
        //aiPath = GetComponent<AIPath>();
        aiLerp = GetComponent<AILerp>();
        _player = GameObject.FindWithTag("Player");
        _playerTarget = _player.GetComponent<IDamageable>();
        _rb = GetComponent<Rigidbody2D>();
        _rangeCircle = GetComponent<LineRenderer>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        //_spriteTransform = transform.GetChild(0);
        
        //Debug.Log("Using child transform: " + _spriteTransform.name);


        //Debug.Log(spriteRenderer.gameObject.name);
        _animator = GetComponentInChildren<Animator>();
        DrawRangeCircle();

        ChangeState(EnemyState.Idle);


    }

    virtual public void Update()
    {
        changeSpriteDirection();
        if (_player == null || _currentState == EnemyState.Dead) return;

        float dist = Vector2.Distance(transform.position, _player.transform.position);

        switch (_currentState)
        {
            case EnemyState.Idle:
                _animator.Play("idle_anim");
                if (dist <= _enemyAggroRange && HasLineOfSight() || wasHit ) // detect player
                    ChangeState(EnemyState.Chase);
                break;

            case EnemyState.Chase:
                //MoveToPlayer();
                _animator.Play("move_anim");
                UpdateDestination();
                if (dist <= _enemyRange && _canAttack && HasLineOfSight())
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

    void changeSpriteDirection()
    {
        //Debug.Log("changeSpriteDirection CALLED!");
        if (_player != null)
        {
            Vector3 dir = _player.transform.position - transform.position;

            //Vector3 scale = _spriteTransform.localScale;

            if (dir.x < 0)
            {
                // Face left
               spriteRenderer.flipX = true;
            }
            else
            {
                // Face right
               spriteRenderer.flipX= false;
            }

            //_spriteTransform.localScale = scale;
        }
    }



    //void MoveToPlayer()
    //{
    //    //Vector2 direction = (_player.transform.position - transform.position).normalized;
    //    //_rb.linearVelocity = direction * _enemySpeed;

    //    //aiPath.destination = _player.transform.position;
    //    aiLerp.destination = _player.transform.position;
    //    float dist = Vector2.Distance(transform.position, _player.transform.position);
    //    changeSpriteDirection();
    //    if (dist <= _enemyRange)
    //    {
    //        ChangeState(EnemyState.Idle);
    //    }
    //}
    void UpdateDestination()
    {
        if (_currentState == EnemyState.Chase)
        {
            aiLerp.destination = _player.transform.position;
        }
    }


    protected void ChangeState(EnemyState newState)
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
                //_rb.linearVelocity = Vector2.zero; //this is now obsolete cause AIpath using tranform movement. using tranform instead
                aiLerp.canMove = false;
                break;

            case EnemyState.Chase:
               
                aiLerp.canMove = true;
                break;

            case EnemyState.Charge:
                aiLerp.canMove = false;
                _rb.linearVelocity = Vector2.zero;
                _animator.Play("attack_anim");
                float animLenght = _animator.GetCurrentAnimatorStateInfo(0).length;
                _currentStateRoutine = StartCoroutine(ChargeAndAttack(animLenght));
                break;

            case EnemyState.Recover:
                float dist = Vector2.Distance(transform.position, _player.transform.position);
                if (dist <= _enemyRange)
                {
                    aiLerp.canMove = false;
                    
                }
                else
                {
                    aiLerp.canMove = true;
                   
                }
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


    public virtual IEnumerator ChargeAndAttack(float animLenght)
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
         
            _playerTarget.TakeDmg(_enemyAtk, false);
            ChangeState(EnemyState.Recover);
        }
        else
        {
            Debug.Log("Player dogded!!");
            ChangeState(EnemyState.Chase);
        }
        //yield return new WaitForSeconds(0.25f);

    }


    private bool HasLineOfSight()
    {
        Vector2 origin = transform.position;
        Vector2 targetPos = _player.transform.position;
        Vector2 dir = (targetPos - origin).normalized;
        float dist = Vector2.Distance(origin, targetPos);

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, dist, obstacleMask);

        if (hit.collider == null)
        {
            // No obstacle
            return true;
        }
        else
        {
            //Debug.Log("Line of sight blocked by: " + hit.collider.name);
            return false;
        }
    }

    IEnumerator Recover()
    {
        _animator.Play("idle_anim");
        _canAttack = false;
        Debug.Log("Recovering...");

        float timer = 0f;
        while (timer < _enemyAtkSpeed)
        {
           
            timer += Time.deltaTime;
            yield return null;
        }

        _canAttack = true;
        float dist = Vector2.Distance(transform.position, _player.transform.position);
        if (dist <= _enemyRange)
        {
           ChangeState(EnemyState.Charge);
        }
        else
        {
           ChangeState(EnemyState.Chase);

        }
        
    }

    public void InterruptAttack()
    {
        if (_currentStateRoutine != null)
        {
            StopCoroutine(_currentStateRoutine);
            _currentStateRoutine = null;
        }
        if (_recoveryRoutine != null)
        {
            StopCoroutine(_recoveryRoutine);
            _canAttack = true;
            _recoveryRoutine = null;
        }
    }

    public void ChangeStateToChase()
    {
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
        //Debug.Log("TakeDmg() called");
        if (_currentState == EnemyState.Dead) return;
        wasHit = true;
        _enemyHealth -= amount;
        _enemyHealth = Mathf.Clamp(_enemyHealth, 0, _enemyMaxHealth);
        //_animator.Play("Orc_hurt_anim");

        if (isCrit)
        {
            Debug.Log($"{_enemyName} took a CRITICAL hit: {amount} damage!");
            SoundFXManager.Instance.PlaySoundFXClip(_enemyCritHurtedSounds, transform, 0.5f);
            ShowDamage(amount.ToString(), isCrit);

        }

        else
        {
            Debug.Log($"{_enemyName} took {amount} damage.");
            SoundFXManager.Instance.PlaySoundFXClip(_enemyHurtedSounds, transform, 0.5f);
            ShowDamage(amount.ToString(), isCrit);

        }



        if (_enemyHealth <= 0)
            ChangeState(EnemyState.Dead);
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
        StartCoroutine(PlayDeathAndCleanup());
    }

    IEnumerator PlayDeathAndCleanup()
    {
        // Stop movement, disable AI etc. if needed
        aiLerp.canMove = false;

        _animator.Play("death_anim");

        // Optionally: ensure the Animator updates before querying length
        yield return null;

        float animLength = _animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(animLength + 0.5f);

        // Drop loot AFTER animation
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        Debug.Log($"{_enemyName} has died.");

        SceneManager.NotifyEnemyDied();
        

        // Now destroy
        Destroy(gameObject);
    }
}
