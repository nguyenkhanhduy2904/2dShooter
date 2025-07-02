using UnityEngine;

public class PickUpLoot : MonoBehaviour
{
    //public int ammoAmount = 10;
    public Loot loot;
    [SerializeField] AudioClip[] _pickUpSounds;

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
                        if (currentWeapon != null)
                        {
                            int magSize = currentWeapon._magSize;
                            int ammoAmount = Mathf.Max(Mathf.FloorToInt(magSize * 0.2f), 1);//convert to int, round down, but min is 1

                            weaponHolder.AddAmmoToCurrentWeapon(ammoAmount);
                            SoundFXManager.Instance.PlaySoundFXClip(_pickUpSounds, transform, 1f);

                            Destroy(gameObject); // Remove the pickup
                        }
                        
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
                        SoundFXManager.Instance.PlaySoundFXClip(_pickUpSounds, transform, 1f);
                        Destroy(gameObject); // Remove the pickup
                        break;
                    default:
                        break;

                }
                

            }



            
        }
    }
}
