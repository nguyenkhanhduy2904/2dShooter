using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [SerializeField] private Weapon currentWeapon;
    [SerializeField] private float orbitDistance = 0.5f; // Max distance from player
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform weaponSlot; // This is where the weapon prefab is attached

    [SerializeField] private GameObject[] testWeaponPrefab; // Drag your Glock18 prefab here in Inspector
    int _currentWeaponIndex = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Pressed E: Equipping test weapon");
            EquipWeapon(testWeaponPrefab[_currentWeaponIndex]);
            if(_currentWeaponIndex < testWeaponPrefab.Length - 1)
            {
                _currentWeaponIndex++;
            }
            else
            {
                _currentWeaponIndex = 0;
            }
           
            
            
        }


        if (currentWeapon == null || playerTransform == null)
        {
            Debug.LogWarning("WeaponHolder: Missing weapon or playerTransform!");
            return;
        }

       

        RotateToMouse();
        HandleShooting();
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
    }

    public void HandleReload()
    {
        if( currentWeapon == null) return;

        if(Input.GetKeyDown(KeyCode.R))
        {
            currentWeapon.Reload();
        }
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

    public void EquipWeapon(GameObject weaponPrefab)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon.gameObject);
        }

        GameObject weaponInstance = Instantiate(weaponPrefab, weaponSlot);
        weaponInstance.transform.localPosition = Vector3.zero;
        currentWeapon = weaponInstance.GetComponent<Weapon>();

        if (currentWeapon == null)
        {
            Debug.LogError("Equipped weapon prefab does not have a Weapon script!");
        }
    }
    

    public Weapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
}
