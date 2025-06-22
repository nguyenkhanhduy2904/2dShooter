using Assets.Script;
using UnityEngine;

public class Enemy
{
    [SerializeField] string _enemyName;
    int _enemyHealth;
    float _enemySpeed;
    int _enemyAtk;
    static int _enemyMaxAtk =30;
    static int _enemyMaxHealth = 100;
    static float _enemyMaxSpeed = 5f;

    

    public Enemy(string enemyName, int enemyHealth, float enemySpeed, int enemyAtk)
    {
        _enemyName = enemyName;
        _enemyHealth = enemyHealth;
        _enemySpeed = enemySpeed;
        _enemyAtk = enemyAtk;
    }

    public Enemy()
    {
        _enemyName = "Creep";
        _enemyHealth =_enemyMaxHealth;
        _enemySpeed =_enemyMaxSpeed;
        _enemyAtk =_enemyMaxAtk;
    }

    public string EnemyName { get => _enemyName; set => _enemyName = value; }
    public int EnemyHealth 
    { 
        get => _enemyHealth;
        set
        {
            if (_enemyHealth > _enemyMaxHealth)
            {
                _enemyHealth = _enemyMaxHealth;
            }
        }
    }
    public float EnemySpeed 
    { 
        get => _enemySpeed;
        set
        {
            if(_enemySpeed > _enemyMaxSpeed)
            {
                _enemySpeed = _enemyMaxSpeed;
            }
        }
    }
    public int EnemyAtk
    {
        get => _enemyAtk;
        set
        {
            if(_enemyAtk > _enemyMaxAtk)
            {
                _enemyAtk= _enemyMaxAtk;
            }
        }
    }
    public static int EnemyMaxHealth { get => _enemyMaxHealth; set => _enemyMaxHealth = value; }
    public static float EnemyMaxSpeed { get => _enemyMaxSpeed; set => _enemyMaxSpeed = value; }

    public static int EnemyMaxAtk { get => _enemyMaxAtk; set => _enemyMaxAtk = value; }

    public void DealDmg(int dmg, object target)
    {
        if(target is IDamageable damageable)
        {
            damageable.TakeDmg(_enemyAtk , false);
        }
    }

}
