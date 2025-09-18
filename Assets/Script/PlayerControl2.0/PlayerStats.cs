using UnityEngine;
using Assets.Script;
using System.Collections;

public class PlayerStats : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private string _name;
    [SerializeField] private int _maxHP;
    [SerializeField] private int _currentHP;
    [SerializeField] private float _maxMoveSpeed;
    [SerializeField] private float _currentMoveSpeed;
    [SerializeField] private float _maxAttackSpeed;
    [SerializeField] private float _currentAttackSpeed;
    [SerializeField] private float _critChance;
    [SerializeField] private float _critMultiplier;
    [SerializeField] private int _attackDamage;

    public string Name { get => _name; set => _name = value; }
    public int MaxHP { get => _maxHP; set => _maxHP = value; }
    public int CurrentHP { get => _currentHP; set => _currentHP = value; }
    public float MaxMoveSpeed { get => _maxMoveSpeed; set => _maxMoveSpeed = value; }
    public float CurrentMoveSpeed { get => _currentMoveSpeed; set => _currentMoveSpeed = value; }
    public float MaxAttackSpeed { get => _maxAttackSpeed; set => _maxAttackSpeed = value; }
    public float CurrentAttackSpeed { get => _currentAttackSpeed; set => _currentAttackSpeed = value; }
    public float CritChance { get => _critChance; set => _critChance = value; }
    public float CritMultiplier { get => _critMultiplier; set => _critMultiplier = value; }
    public int AttackDamage { get => _attackDamage; set => _attackDamage = value; }



    private void Awake()
    {
        Name = _name;
        MaxHP = _maxHP;
        CurrentHP = MaxHP;
        MaxMoveSpeed = _maxMoveSpeed;
        CurrentMoveSpeed = MaxMoveSpeed;
        MaxAttackSpeed = _maxAttackSpeed;
        CurrentAttackSpeed = MaxAttackSpeed;
        CritChance = _critChance;
        CritMultiplier = _critMultiplier;

    }
    public void TakeDmg(int amount, bool isCrit)
    {
        CurrentHP -= amount;
        Debug.Log(Name + " took " + amount + "damage, have " + CurrentHP + "left");
    }

    public void DealDmg(IDamageable target, int amount, bool isCrit)
    {
        target.TakeDmg(amount, isCrit);
        Debug.Log(Name +"Deal " + amount + " damage");
    }

    public void ShowDamage(string text, bool isCrit)
    {

    }

    public IEnumerator ChangeColor(Color color, float time)
    {
        yield return new WaitForSeconds(time);
    }

}
