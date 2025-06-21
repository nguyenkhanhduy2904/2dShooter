using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [Header("Weapon Config")]
    public WeaponData weaponData;

    [Header("References")]
    public Transform firePoint;
    [SerializeField] private Transform firePointSprite;
    [SerializeField] private GameObject muzzleFlash;
    
    protected float _nextFireTime;
    [SerializeField] protected int _magSize;
    protected int _currentMagSize;

    public virtual bool IsAutomatic => true;
    public virtual bool CanShootWhileReLoad => false;


    protected bool _isReloading = false;
    private void Start()
    {
        _magSize = weaponData.magSize;
        _currentMagSize = _magSize;
    }


    public virtual void TryShoot(Vector2 direction)
    {

        if(_isReloading && CanShootWhileReLoad == false) return;
        Debug.Log($"TryShoot called on {gameObject.name}, activeSelf: {gameObject.activeSelf}, enabled: {enabled}");

        if (!gameObject.activeInHierarchy)
        {
            Debug.LogWarning("Weapon GameObject is inactive!");
            return;
        }



        if(Time.time >= _nextFireTime && _currentMagSize == 0 && _isReloading == false)
        {
            SoundFXManager.Instance.PlaySoundFXClip(weaponData.clipEmptySound, transform, 0.15f);
            _nextFireTime = Time.time + 1f / weaponData.fireRate;
            return;
        }



        if (Time.time >= _nextFireTime && _currentMagSize >0 && _isReloading == false)
        {
            Shoot(direction);
            _currentMagSize--;
            _nextFireTime = Time.time + 1f / weaponData.fireRate;
        }
    }

    public virtual void Shoot(Vector2 direction)
    {
        if (weaponData.bulletPrefab == null)
        {
            Debug.LogError("Missing bullet prefab!");
            return;
        }
        Debug.Log($"Shoot direction: {direction}, weapon: {gameObject.name}");
        // Angle for rotation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        // Spawn bullet
        GameObject bullet = Instantiate(weaponData.bulletPrefab, firePoint.position, rotation);
        //bullet.GetComponent<BulletBehaviour>().direction = direction;

        // Pass damage to bullet
        BulletBehaviour bulletScript = bullet.GetComponent<BulletBehaviour>();
        bulletScript.direction = direction;
        bulletScript.SetDamage(weaponData.damage); // <- pass damage from SO


        // Sound
        if (weaponData.shootSounds != null && weaponData.shootSounds.Length > 0)
        {
            var clip = weaponData.shootSounds[Random.Range(0, weaponData.shootSounds.Length)];
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }

        // Muzzle Flash
        if (muzzleFlash != null)
        {
            StartCoroutine(ShowMuzzleFlash());
        }
    }

    private IEnumerator ShowMuzzleFlash()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        muzzleFlash.SetActive(false);
    }

    public virtual void FlipSprite(Vector2 lookDirection)
    {
        if (firePointSprite == null) return;

        if (lookDirection.x < 0)
        {
            firePointSprite.localScale = new Vector3(1, -1, 1);
            firePointSprite.localPosition = new Vector3(-1.5f, 1.5f, 0);
        }
        else
        {
            firePointSprite.localScale = new Vector3(1, 1, 1);
            firePointSprite.localPosition = new Vector3(-1.5f, -0.5f, 0);
        }
    }

    public virtual void Reload()
    {
        if (!_isReloading) // Optional: prevent double reload
        {
            _isReloading = true;
            StartCoroutine(ReloadTimer());
        }
    }

    public virtual IEnumerator ReloadTimer()
    {
        Debug.Log("Reloading...");

        int reloadProcedureLength = weaponData.reloadProcedure.Length;

        for (int i = 0; i < reloadProcedureLength -1 ; i++) 
        {
            SoundFXManager.Instance.PlaySoundFXClipAt(weaponData.reloadProcedure, transform, 0.5f, i);
            yield return new WaitForSeconds(0.5f);

        }
        //SoundFXManager.Instance.PlaySoundFXClipAt(weaponData.reloadProcedure, transform, 0.5f, reloadProcedureLength - reloadProcedureLength );
        //yield return new WaitForSeconds(0.5f);
        //SoundFXManager.Instance.PlaySoundFXClipAt(weaponData.reloadProcedure, transform, 0.5f, 1);
        //yield return new WaitForSeconds(0.5f);
        //SoundFXManager.Instance.PlaySoundFXClipAt(weaponData.reloadProcedure, transform, 0.5f, 2);
        //yield return new WaitForSeconds(0.5f);
        //SoundFXManager.Instance.PlaySoundFXClipAt(weaponData.reloadProcedure, transform, 0.5f, 3);
        //yield return new WaitForSeconds(0.5f);



        _currentMagSize = _magSize;
        _isReloading = false; // Only unset after reload is complete

        Debug.Log("Reload complete.");
    }

}
