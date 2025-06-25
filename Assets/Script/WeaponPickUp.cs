using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    public WeaponData weaponData;

    // Add saved ammo fields
    public int savedMag = -1;
    public int savedReserve = -1;

    private bool playerInRange = false;
    private WeaponHolder playerWeaponHolder;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (playerWeaponHolder != null)
            {
                playerWeaponHolder.TryPickupWeapon(weaponData, savedMag, savedReserve);
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerWeaponHolder = other.GetComponentInChildren<WeaponHolder>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerWeaponHolder = null;
        }
    }
}
