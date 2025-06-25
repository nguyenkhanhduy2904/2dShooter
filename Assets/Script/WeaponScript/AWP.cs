using System.Collections;
using UnityEngine;

public class AWP: Weapon
{
    public override bool IsAutomatic => false;

    private bool isAccurate = true;
    private Rigidbody2D playerRb;

    private void Start()
    {
        //_magSize = weaponData.magSize;
        //_currentMagSize = _magSize;
        playerRb = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Debug.Log("Velocity magnitude: " + playerRb.linearVelocity.magnitude);

        if (playerRb != null)
        {
            // If moving above a small threshold, consider inaccurate
            isAccurate = playerRb.linearVelocity.magnitude < 0.1f;
        }
        if (!isAccurate) 
        {
            currentSpread = spreadAngleMax;
            Debug.Log("current spread:" + currentSpread);
        }
        else
        {
            currentSpread = 0;
        }

    }

    public override void TryShoot(Vector2 direction)
    {
        if (_isReloading && !CanShootWhileReLoad) return;
        if (!gameObject.activeInHierarchy) return;

        if (Time.time >= _nextFireTime && _currentMagSize == 0 && !_isReloading)
        {
            SoundFXManager.Instance.PlaySoundFXClip(weaponData.clipEmptySound, transform, 0.15f);
            _nextFireTime = Time.time + 1f / weaponData.fireRate;
            return;
        }

        if (Time.time >= _nextFireTime && _currentMagSize > 0 && !_isReloading)
        {
            Vector2 finalDir = isAccurate ? direction : ApplyDynamicSpread(direction);
            Shoot(finalDir);
            //SoundFXManager.Instance.PlaySoundFXClipAt(weaponData.reloadProcedure, transform, 0.5f, weaponData.reloadProcedure.Length - 3);
            //SoundFXManager.Instance.PlaySoundFXClipAt(weaponData.reloadProcedure, transform, 0.5f, weaponData.reloadProcedure.Length - 2);
            StartCoroutine(PlayChamberingSound());
            _currentMagSize--;
            _nextFireTime = Time.time + 1f / weaponData.fireRate;
            _holder?.SyncAmmoState();
        }
    }

    public override void Shoot(Vector2 direction)
    {
        if (weaponData.bulletPrefab == null)
        {
            Debug.LogError("Missing bullet prefab!");
            return;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject bullet = Instantiate(weaponData.bulletPrefab, firePoint.position, rotation);

        PierceableBullet bulletScript = bullet.GetComponent<PierceableBullet>();
        if (bulletScript != null)
        {
            bulletScript.direction = direction;

            int rng = Random.Range(0, 100);
            bool isCrit = rng < weaponCritChance;
            float rawDmg = isCrit ? weaponData.damage * weaponData.weaponCritMultiplier : weaponData.damage;
            int finalDmg = Mathf.RoundToInt(rawDmg);

            bulletScript.SetDamage(finalDmg, isCrit);
            bulletScript.SetPierceCount(weaponData.pierceAmount);
        }

        if (weaponData.shootSounds != null && weaponData.shootSounds.Length > 0)
        {
            var clip = weaponData.shootSounds[Random.Range(0, weaponData.shootSounds.Length)];
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }

        if (muzzleFlash != null)
        {
            StartCoroutine(ShowMuzzleFlash());
        }
       
    }

    IEnumerator PlayChamberingSound()
    {
        yield return new WaitForSeconds(0.75f);
        SoundFXManager.Instance.PlaySoundFXClipAt(weaponData.reloadProcedure, transform, 0.5f, weaponData.reloadProcedure.Length - 3);
        yield return new WaitForSeconds(0.25f);
        SoundFXManager.Instance.PlaySoundFXClipAt(weaponData.reloadProcedure, transform, 0.5f, weaponData.reloadProcedure.Length - 2);
    }

    public override IEnumerator ReloadTimer()
    {
        //Debug.Log("Reloading...");

        int reloadProcedureLength = weaponData.reloadProcedure.Length;


        SoundFXManager.Instance.PlaySoundFXClipAt(weaponData.reloadProcedure, transform, 0.5f, reloadProcedureLength - reloadProcedureLength);
        yield return new WaitForSeconds(0.5f);
        SoundFXManager.Instance.PlaySoundFXClipAt(weaponData.reloadProcedure, transform, 0.5f, 1);
        yield return new WaitForSeconds(0.5f);
        SoundFXManager.Instance.PlaySoundFXClipAt(weaponData.reloadProcedure, transform, 0.5f, 2);
        yield return new WaitForSeconds(0.5f);
        SoundFXManager.Instance.PlaySoundFXClipAt(weaponData.reloadProcedure, transform, 0.5f, 3);
        yield return new WaitForSeconds(0.25f);
        SoundFXManager.Instance.PlaySoundFXClipAt(weaponData.reloadProcedure, transform, 0.5f, 4);
        yield return new WaitForSeconds(0.25f);


        int bulletsNeeded = Mathf.Max(0, _magSize - _currentMagSize);
        int bulletsToReload = Mathf.Min(bulletsNeeded, _currentReserveAmmo);


        _currentMagSize += bulletsToReload;
        _currentReserveAmmo -= bulletsToReload;

        Debug.Log("in mag: " + _currentMagSize + ", in reserve: " + _currentReserveAmmo);

        _isReloading = false;

        // Only unset after reload is complete

        //Debug.Log("Reload complete.");
        _holder?.SyncAmmoState();

        //Debug.Log("Reload complete.");
    }

}

