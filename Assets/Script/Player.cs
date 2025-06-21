using UnityEngine;

public class Player
{
    [SerializeField] string _playerName;
    int _playerHealth;
    float _playerSpeed;
    static int _playerMaxHealth =100;
    static float _playerMaxSpeed =10f;

   

    public Player(string playerName, int playerHealth, float playerSpeed)
    {
        _playerName = playerName;
        _playerHealth = playerHealth;
        _playerSpeed = playerSpeed;
    }

    public Player()
    {
        _playerName = "Default";
        _playerHealth = _playerMaxHealth;
        _playerSpeed = _playerMaxSpeed;
    }

    public string PlayerName { get => _playerName; set => _playerName = value; }
    public int PlayerHealth 
    { 
        get => _playerHealth;
        set
        {
            if(_playerHealth > _playerMaxHealth)
            {
                _playerHealth = _playerMaxHealth;
            }
        }
    }



    public float PlayerSpeed 
    {
        get => _playerSpeed;
        set
        {
            if( _playerSpeed > _playerMaxSpeed)
            {
                _playerSpeed = _playerMaxSpeed;
            }
        }
    }
    public static int PlayerMaxHealth { get => _playerMaxHealth; set => _playerMaxHealth = value; }
    public static float PlayerMaxSpeed { get => _playerMaxSpeed; set => _playerMaxSpeed = value; }


    public void TakeDmg(int dmg)
    {
        _playerHealth += dmg;
        Debug.Log("take " + dmg + ", " + _playerHealth + " remain");
    }

}
   
