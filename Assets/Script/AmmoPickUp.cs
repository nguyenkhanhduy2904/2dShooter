using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    //public int ammoAmount = 10;
    [SerializeField] Loot loot;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Try to get the WeaponHolder component (or whatever handles ammo)
            var weaponHolder = collision.GetComponentInChildren<WeaponHolder>();

            if (weaponHolder != null)
            {
                switch (loot.lootName)
                {
                    case "Ammo":
                        int ammoAmount = 10;
                        weaponHolder.AddAmmoToCurrentWeapon(ammoAmount);
                        break;
                    case "Heath":
                        int amount = 20;
                        var Player = collision.GetComponent<PlayerController>();
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
