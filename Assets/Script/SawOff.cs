using UnityEngine;
using System.Collections;

public class SawOff: Weapon
{
    public override bool IsAutomatic => false; // semi-auto
    public override bool CanShootWhileReLoad => true;

    [SerializeField] private int pelletPerShot = 5;
    [SerializeField] float spreadAngle = 15f;
    // Optional: override TryShoot if it behaves differently
    public override void TryShoot(Vector2 direction)
    {

        if (Time.time >= _nextFireTime && _currentMagSize == 0)
        {
            SoundFXManager.Instance.PlaySoundFXClip(weaponData.clipEmptySound, transform, 0.15f);
            _nextFireTime = Time.time + 1f / weaponData.fireRate;
            return;
        }
        if (Time.time >= _nextFireTime && _currentMagSize>0)
        {
            float baseAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            for (int i = 0; i < pelletPerShot; i++)
            {
                // Distribute pellets evenly in the spread angle
                float offset = Mathf.Lerp(-spreadAngle / 2f, spreadAngle / 2f, i / (pelletPerShot - 1f));
                float angleWithSpread = baseAngle + offset;

                // Convert back to Vector2
                float rad = angleWithSpread * Mathf.Deg2Rad;
                Vector2 spreadDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;

                Shoot(spreadDir);
                
                                  // Use base class Shoot()
                                  // Sound
                                  //if (weaponData.shootSounds != null && weaponData.shootSounds.Length > 0)
                                  //{
                                  //    var clip = weaponData.shootSounds[Random.Range(0, weaponData.shootSounds.Length)];
                                  //    AudioSource.PlayClipAtPoint(clip, transform.position);
                                  //}


            }
            SoundFXManager.Instance.PlaySoundFXClip(weaponData.shootSounds, transform, 0.15f);
            _nextFireTime = Time.time + 1f / weaponData.fireRate;
            _currentMagSize--;
        }



        
    }

    public override void Shoot(Vector2 direction)
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


        //// Sound
        //if (weaponData.shootSounds != null && weaponData.shootSounds.Length > 0)
        //{
        //    var clip = weaponData.shootSounds[Random.Range(0, weaponData.shootSounds.Length)];
        //    AudioSource.PlayClipAtPoint(clip, transform.position);
        //}

        // Muzzle Flash
        //if (muzzleFlash != null)
        //{
        //    StartCoroutine(ShowMuzzleFlash());
        //}
    }
    public override void Reload()
    {
        if (!_isReloading) // Optional: prevent double reload
        {
            _isReloading = true;
            StartCoroutine(ReloadTimer());
            
           
            
        }
    }

    public override IEnumerator ReloadTimer()
    {
        float reloadStepTime = 0.5f;

        while (_currentMagSize < _magSize)
        {
            // Interrupt anytime during wait
            float elapsed = 0f;
            while (elapsed < reloadStepTime)
            {
                if (Input.GetMouseButtonDown(0) && _currentMagSize > 0)
                {
                    Debug.Log("Reload interrupted during wait.");
                    _isReloading = false;
                    yield break; // Exit coroutine immediately
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Add 1 bullet after successful wait
            int randomIndex = Random.Range(0, weaponData.reloadProcedure.Length - 3);
            SoundFXManager.Instance.PlaySoundFXClipAt(weaponData.reloadProcedure, transform, 0.5f, randomIndex);
            _currentMagSize++;
            

            if (_currentMagSize == _magSize)
            {
                yield return new WaitForSeconds(0.5f);
                // Play final reload sound
                SoundFXManager.Instance.PlaySoundFXClipAt(
                    weaponData.reloadProcedure, transform, 0.5f, weaponData.reloadProcedure.Length - 2
                );
            }
        }

        _isReloading = false;
    }




}
