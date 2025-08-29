using UnityEngine;
using System.Collections;
using Assets.Script;
public class AIStats : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private string _name;
    [SerializeField] private int _maxHp;
    [SerializeField] private int _currentHp;
    [SerializeField] private int _attackDamage;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _aggroRange;
    [SerializeField] private float _critChance;
    [SerializeField] private float _critMultiplier;


    [Header("Utils")]
    [SerializeField] Mob_SO mob_SO;

    public string Name { get => _name; set => _name = value; }
    public int MaxHP { get => _maxHp; set => _maxHp = value; }
    public int CurrentHP { get => _currentHp; set => _currentHp = value; }
    public int AttackDamage { get => _attackDamage; set => _attackDamage = value; }
    public float AttackSpeed { get => _attackSpeed; set => _attackSpeed = value; }
    public float MoveSpeed { get => _moveSpeed; set => _moveSpeed = value; }
    public float AttackRange { get => _attackRange; set => _attackRange = value; }
    public float AggroRange { get => _aggroRange; set => _aggroRange = value; }
    public float CritChance { get => _critChance; set => _critChance = value; }
    public float CritMultiplier { get => _critMultiplier; set => _critMultiplier = value; }

    private void Awake()
    {
        Name = mob_SO._enemyName;
        MaxHP = mob_SO._enemyMaxHealth;
        CurrentHP = MaxHP;
        AttackDamage = mob_SO._enemyAtk;
        AttackSpeed = mob_SO._enemyAtkSpeed;
        MoveSpeed = mob_SO._enemySpeed;
        AttackRange = mob_SO._enemyRange;
        AggroRange = mob_SO._enemyAggroRange;
        CritChance = 0f;
        CritMultiplier = 1f;
    }




    public void TakeDmg(int amount, bool isCrit)
    {
        CurrentHP -= amount;
        Debug.Log( Name +" took " +  amount + "damage, have " + CurrentHP +"left");
    }

    public void DealDmg(IDamageable target, int amount, bool isCrit)
    {
        target.TakeDmg(amount, isCrit);
        Debug.Log("Deal " + amount + " damage");
    }

    public void ShowDamage(string text, bool isCrit)
    {
      
    }

    public IEnumerator ChangeColor(Color color, float time)
    {
        yield return new WaitForSeconds(time);
    }

    public float GetAttackDelay()
    {
        float atkDelay = 1f / AttackSpeed;
        return atkDelay;
    }
}
