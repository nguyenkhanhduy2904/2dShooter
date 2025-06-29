using UnityEngine;

public class PickUpLoot : MonoBehaviour
{
    //public int ammoAmount = 10;
    public Loot loot;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            var weaponHolder = collision.GetComponentInChildren<WeaponHolder>();

            if (weaponHolder != null)
            {
                switch (loot.lootName)
                {
                    case "Ammo":
                        var currentWeapon = weaponHolder.GetCurrentWeapon();
                        int magSize = currentWeapon._magSize;
                        int ammoAmount = Mathf.Max(Mathf.FloorToInt(magSize * 0.2f), 1);//convert to int, round down, but min is 1

                        weaponHolder.AddAmmoToCurrentWeapon(ammoAmount);
                        break;
                    case "Health":
                        
                        var Player = collision.GetComponent<PlayerController>();
                        int health = Player.PlayerHealth;
                        int maxhealth = PlayerController.PlayerMaxHealth;
                        int lossHealth = maxhealth - health;
                        int amount = Mathf.Max(Mathf.FloorToInt(lossHealth * 0.3f), 10);
                        if (Player != null)
                        {
                            Debug.Log("Player is not null");
                        }
                        else
                        {
                            Debug.Log("Player is null");
                        }
                        Player.Heal(amount);
                        break;
                    default:
                        break;

                }

            }



            Destroy(gameObject); // Remove the pickup
        }
    }
}
