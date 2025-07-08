using UnityEngine;

[CreateAssetMenu(fileName = "Mob_SO", menuName = "Scriptable Objects/Mob_SO")]
public class Mob_SO : ScriptableObject
{
    [SerializeField] public string _enemyName;
    [SerializeField] public int _enemyMaxHealth;
    [SerializeField] public int _enemyAtk;
    [SerializeField] public float _enemySpeed;
    [SerializeField] public float _enemyAtkSpeed;
    [SerializeField] public float _enemyRange;
    [SerializeField] public float _enemyAggroRange;

    [SerializeField] public AudioClip[] _enemyHurtedSounds;
    [SerializeField] public AudioClip[] _enemyCritHurtedSounds;


    [SerializeField] public GameObject _floatingTextPreFab;
    ////type in the string name of the animation of each character. 
    //[SerializeField] public string idleAnim;
    //[SerializeField] public string moveAnim;
    //[SerializeField] public string AttackAnim;
    //[SerializeField] public string deathAnim;

}
