using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    public List<WeaponAmmoState> weaponStates = new(); // stores ammo for each unlocked weapon
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private float orbitDistance = 0.5f; // Max distance from player
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform weaponSlot; // This is where the weapon prefab is attached

    [SerializeField] private List<WeaponData> carriedWeapons = new();
    private int maxWeaponCount = 2;
    private int currentWeaponIndex = 0;
    //[SerializeField] private GameObject[] testWeaponPrefab; // Drag your Glock18 prefab here in Inspector
    //[SerializeField] private WeaponData[] testWeaponData;

    //int _currentWeaponIndex = 0;
    private void Update()
    {
        //// Equip weapon on key press (e.g., cycling through test weapons)
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    if (testWeaponData.Length == 0) return;

        //    Debug.Log("Pressed E: Equipping test weapon");

        //    // Cycle weapon index
        //    _currentWeaponIndex = (_currentWeaponIndex + 1) % testWeaponData.Length;

        //    EquipWeapon(testWeaponData[_currentWeaponIndex]);
        //}

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CycleWeapon();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropCurrentWeapon();
        }


        // Null checks
        if (currentWeapon == null || playerTransform == null)
            return;

        RotateToMouse();
        HandleShooting();
        HandleReload(); // Call reload here to handle input consistently
    }

    //public void TryPickupWeapon(WeaponData newWeapon)
    //{ 

    //    // Already carrying this weapon?
    //    if (carriedWeapons.Exists(w => w == newWeapon))
    //    {
    //        Debug.Log("Already have this weapon");
    //        return;
    //    }

    //    if (carriedWeapons.Count < maxWeaponCount)
    //    {
    //        carriedWeapons.Add(newWeapon);
    //        EquipWeapon(newWeapon);
    //        Debug.Log("Picked up new weapon: " + newWeapon.weaponName);
    //    }
    //    else
    //    {
    //        // Swap logic - replace current weapon
    //        int currentIndex = carriedWeapons.FindIndex(w => w == currentWeapon.weaponData);
    //        carriedWeapons[currentIndex] = newWeapon;
    //        EquipWeapon(newWeapon);
    //        Debug.Log("Swapped weapon to: " + newWeapon.weaponName);
    //    }
    //}
    public void TryPickupWeapon(WeaponData newWeapon, int savedMag = -1, int savedReserve = -1)
    {
        if (carriedWeapons.Exists(w => w == newWeapon))
        {
            Debug.Log("Already have this weapon");
            return;
        }

        if (carriedWeapons.Count < maxWeaponCount)
        {
            carriedWeapons.Add(newWeapon);
            EquipWeapon(newWeapon, savedMag, savedReserve);
            Debug.Log("Picked up new weapon: " + newWeapon.weaponName);
        }
        else
        {
            // Drop old weapon
            var oldWeaponData = currentWeapon.weaponData;
            int oldMag = currentWeapon.CurrentMag;
            int oldReserve = currentWeapon.CurrentReserve;
            SpawnDroppedWeapon(oldWeaponData, oldMag, oldReserve);

            // Replace
            int currentIndex = carriedWeapons.FindIndex(w => w == oldWeaponData);
            carriedWeapons[currentIndex] = newWeapon;
            EquipWeapon(newWeapon, savedMag, savedReserve);
            Debug.Log("Swapped weapon to: " + newWeapon.weaponName);
        }
    }

    private void SpawnDroppedWeapon(WeaponData weaponData, int mag, int reserve)
    {
        if (weaponData.pickupPrefab == null)
        {
            Debug.LogWarning("No pickup prefab assigned for: " + weaponData.weaponName);
            return;
        }

        // Random drop position around the player (between 0.5 and 1.5 units away)
        Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(0.5f, 1.5f);
        Vector3 dropPos = transform.position + new Vector3(randomOffset.x, randomOffset.y, 0);

        GameObject drop = Instantiate(weaponData.pickupPrefab, dropPos, Quaternion.identity);
        SoundFXManager.Instance.PlaySoundFXClip(weaponData.dropSounds, transform, 1f);

        var pickup = drop.GetComponent<WeaponPickUp>();
        if (pickup != null)
        {
            pickup.weaponData = weaponData;
            pickup.savedMag = mag;
            pickup.savedReserve = reserve;
        }
    }

    public void DropCurrentWeapon()
    {
        if (currentWeapon == null) return;

        var data = currentWeapon.weaponData;
        var mag = currentWeapon.CurrentMag;
        var reserve = currentWeapon.CurrentReserve;

        // Spawn drop
        SpawnDroppedWeapon(data, mag, reserve);

        // Remove from inventory
        carriedWeapons.Remove(data);
        weaponStates.RemoveAll(w => w.weaponData == data);

        Destroy(currentWeapon.gameObject);
        currentWeapon = null;

        Debug.Log("Dropped weapon: " + data.weaponName);

        // Optionally auto-equip another weapon if any left
        if (carriedWeapons.Count > 0)
        {
            EquipWeapon(carriedWeapons[0]);
        }
    }






    public void CycleWeapon()
    {
        if (carriedWeapons.Count == 0) return;

        currentWeaponIndex = (currentWeaponIndex + 1) % carriedWeapons.Count;
        EquipWeapon(carriedWeapons[currentWeaponIndex]);
    }

    public void HandleShooting()
    {
        if (currentWeapon == null) return;

        bool fireInput = currentWeapon.IsAutomatic
            ? Input.GetMouseButton(0)
            : Input.GetMouseButtonDown(0);

        if (fireInput)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = (mousePos - (Vector2)playerTransform.position).normalized;

            currentWeapon.TryShoot(direction);
        }
        //SyncAmmoState();
    }

    public void HandleReload()
    {
        if( currentWeapon == null) return;

        if(Input.GetKeyDown(KeyCode.R))
        {
            currentWeapon.Reload();
        }
        //SyncAmmoState();
    }

    public void RotateToMouse()
    {
        if(currentWeapon == null)
        {
            return;
        }
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = 0f;

        Vector3 dirFromPlayer = mouseWorld - playerTransform.position;

        // Limit weapon distance from player
        float distance = Mathf.Min(dirFromPlayer.magnitude, orbitDistance);
        Vector3 clampedOffset = dirFromPlayer.normalized * distance;

        // Move weapon to orbit position
        transform.position = playerTransform.position + clampedOffset;

        // Rotate weapon to face the direction
        float angle = Mathf.Atan2(clampedOffset.y, clampedOffset.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Flip visual sprite inside the weapon prefab
        currentWeapon.FlipSprite(clampedOffset);
    }

    //public void EquipWeapon(WeaponData weaponData)
    //{
    //    // Save old weapon state before switching
    //    if (currentWeapon != null)
    //    {
    //        var oldState = weaponStates.Find(w => w.weaponData == currentWeapon.weaponData);
    //        if (oldState != null)
    //        {
    //            oldState.currentMag = currentWeapon.CurrentMag;
    //            oldState.currentReserve = currentWeapon.CurrentReserve;
    //        }

    //        Destroy(currentWeapon.gameObject); // or disable if pooled
    //    }

    //    // Spawn new weapon prefab
    //    GameObject weaponGO = Instantiate(weaponData.weaponPrefab, transform);
    //    currentWeapon = weaponGO.GetComponent<Weapon>();
    //    currentWeapon.InitHolder(this); //  Pass self to the weapon

    //    // Load stored ammo if exists
    //    var newState = weaponStates.Find(w => w.weaponData == weaponData);
    //    if (newState != null)
    //    {
    //        currentWeapon.LoadAmmo(newState.currentMag, newState.currentReserve);
    //        Debug.Log($"[LOAD] {weaponData.weaponName} => Loaded Mag: {newState.currentMag}, Reserve: {newState.currentReserve}");
    //    }
    //    else
    //    {
    //        // If first time using weapon, add it to tracking
    //        currentWeapon.InitAmmoFromData(); // only here
    //        weaponStates.Add(new WeaponAmmoState(weaponData, weaponData.magSize, weaponData.reserveAmmo));
    //        currentWeapon.LoadAmmo(weaponData.magSize, weaponData.reserveAmmo);
    //    }
    //}


    public void EquipWeapon(WeaponData weaponData, int mag = -1, int reserve = -1)
    {
        if (currentWeapon != null)
        {
            var oldState = weaponStates.Find(w => w.weaponData == currentWeapon.weaponData);
            if (oldState != null)
            {
                oldState.currentMag = currentWeapon.CurrentMag;
                oldState.currentReserve = currentWeapon.CurrentReserve;
            }

            Destroy(currentWeapon.gameObject);
        }

        GameObject weaponGO = Instantiate(weaponData.weaponPrefab, transform);
        currentWeapon = weaponGO.GetComponent<Weapon>();
        currentWeapon.InitHolder(this);
        SoundFXManager.Instance.PlaySoundFXClip(weaponData.pickUpSounds, transform, 1f);

        var state = weaponStates.Find(w => w.weaponData == weaponData);
        if (state != null)
        {
            // Override ammo if specified
            state.currentMag = mag >= 0 ? mag : state.currentMag;
            state.currentReserve = reserve >= 0 ? reserve : state.currentReserve;

            currentWeapon.LoadAmmo(state.currentMag, state.currentReserve);
        }
        else
        {
            int finalMag = mag >= 0 ? mag : weaponData.magSize;
            int finalReserve = reserve >= 0 ? reserve : weaponData.reserveAmmo;

            weaponStates.Add(new WeaponAmmoState(weaponData, finalMag, finalReserve));
            currentWeapon.InitAmmoFromData();
            currentWeapon.LoadAmmo(finalMag, finalReserve);
        }
    }



    public void SyncAmmoState()
    {
        if (currentWeapon == null) return;

        var state = weaponStates.Find(w => w.weaponData == currentWeapon.weaponData);
        if (state != null)
        {
            state.currentMag = currentWeapon.CurrentMag;
            state.currentReserve = currentWeapon.CurrentReserve;

            Debug.Log($"[SYNC] Saved: {state.weaponData.weaponName}, Mag: {state.currentMag}, Reserve: {state.currentReserve}");
        }
        else
        {
            Debug.LogWarning("No weapon state found to sync!");
        }
    }

    public void AddAmmoToCurrentWeapon(int amount)
    {
        if (currentWeapon != null)
        {
            currentWeapon.AddReserveAmmo(amount);
            Debug.Log($"Added {amount} ammo to {currentWeapon.weaponData.weaponName}");
            SyncAmmoState();
        }
    }


    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
