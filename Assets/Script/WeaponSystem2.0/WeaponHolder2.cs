using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponHolder2 : MonoBehaviour
{
    public Coroutine currentAttack = null;

    public float attackCooldownTimer = 0f;
    public float comboTimer = 0f;
    public int comboStep = 0;
    public bool isAttacking = false;
    public enum AttackType
    {
        Light,
        Heavy
    }


    //public GameObject weaponObject;
    //public GameObject weaponEffect;


    [SerializeField]public WeaponData2 WeaponData;
    //PlayerController _player;
    public GameObject player;
    public PlayerStats playerStats;
    public PlayerInputControl playerInputControl;
    public PlayerAnimation playerAnimation;
    public Transform weaponPivot;      // "WeaponHolder2" object
    //public Transform weaponSprite;     // "WeaponSprite" object
    //public Transform slashSprite;

    public GameObject weaponGameObjectHolder;
  
    public GameObject weaponSlash_light_FXGameObjectHolder;
    public GameObject weaponSlash_heavy_FXGameObjectHolder;

    public GameObject droppedGOPrefab;

    public SpriteRenderer weaponSpriteRenderer;
    public SpriteRenderer weaponSlash_light_FXSpriteRenderer;
    public SpriteRenderer weaponSlash_heavy_FXSpriteRenderer;


    public Vector3 weaponStandTransformRight;
    public Vector3 weaponStandTransformLeft;



    [SerializeField] private float bobSpeed = 60f;
    [SerializeField] private float bobAmount = 0.1f;
    private Vector3 initialSpriteLocalPos;


    public WeaponBehaviour weaponBehaviour;//cache this so the discard wont break thing;



    private void Start()
    {
        
        player = GameObject.FindWithTag("Player");
        playerStats = player.GetComponent<PlayerStats>();

        GetWeaponInfo();
        
        //initialSpriteLocalPos = weaponSprite.localPosition;
        //slashEffect.sprite = WeaponData.sliceEffect;
        //slashEffect.enabled = false;

        if (WeaponData != null)
        {
          //UpdateSpritePos();
        }

       
    }
    public void EquipWeapon(WeaponData2 weapon)
    {
        WeaponData = weapon;
        SoundFXManager.Instance.PlaySoundFXClip(weapon.pickUpSFX, transform, 1f);
        GetWeaponInfo();
        resetWeaponStand();
    }
    public void DropWeapon(Vector2 dropPosition)
    {
        SoundFXManager.Instance.PlaySoundFXClip(WeaponData.droppedSFX, transform, 1f);
        DiscardWeapon();
        GameObject droppedWeaponObj = Instantiate(droppedGOPrefab, dropPosition, Quaternion.identity);

        

       
    }

    public void DiscardWeapon()
    {
        if (WeaponData == null) return;
        WeaponData = null;
        weaponSpriteRenderer.sprite = null;
    }


    public void GetWeaponInfo()
    {

        weaponBehaviour = WeaponData.weaponBehaviour;
        weaponSpriteRenderer.sprite = WeaponData.weaponSprite;
        weaponSlash_light_FXSpriteRenderer.sprite = WeaponData.sliceEffect_light;
        weaponSlash_heavy_FXSpriteRenderer.sprite = WeaponData.sliceEffect_heavy;


        weaponSlash_light_FXSpriteRenderer.enabled = false;
        weaponSlash_heavy_FXSpriteRenderer.enabled = false;


        weaponStandTransformRight = WeaponData.spriteLocalRotationRight;
        //Debug.Log("weaponStandTransformRight is: " + weaponStandTransformRight);
        weaponStandTransformLeft = WeaponData.spriteLocalRotationLeft;
        //Debug.Log("weaponStandTransformLeft is: " + weaponStandTransformLeft);    
        droppedGOPrefab = WeaponData.droppedWeaponPrefab;
       
      
        
    }



    void Update()
    {
       
        if (attackCooldownTimer > 0)
        {
            attackCooldownTimer -= Time.deltaTime;

            if (attackCooldownTimer <= 0)
            {
                // Attack finished
                attackCooldownTimer = 0f;
                isAttacking = false;
                

            }
        }
        if(comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
            if(comboTimer <= 0)
            {
                comboTimer = 0f;
                comboStep = 0;
                //resetWeaponStand();
            }
        }

        if(!isAttacking && comboTimer <= 0)
        {
            resetWeaponStand();
        }
    }



    public Vector3 GetMousePositionInWorld()
    {
        Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePosInWorld;
    }

    public void TryAttack(AttackType attackType)
    {
        if (WeaponData == null) return;
        if (isAttacking) return;

        if (attackType == AttackType.Light)
        {
            if (comboStep == 0)
            {
                ExecuteLightAttack1();
                comboStep = 1;
                comboTimer = weaponBehaviour.comboTimeWindow;
                attackCooldownTimer = weaponBehaviour.lightAttackCoolDown;
            }
            else if (comboStep == 1 && comboTimer > 0f)
            {
                ExecuteLightAttack2();
                comboStep = 0;
                attackCooldownTimer = weaponBehaviour.lightAttackCoolDown;
            }

           
            isAttacking = true;
        }
        else if (attackType == AttackType.Heavy)
        {
            // Heavy attack logic
            ExecuteHeavyAttack();
            comboStep = 0;
            attackCooldownTimer = weaponBehaviour.heavyAttackCoolDown;
            isAttacking = true;
            StartCoroutine(StartCountDown(0.25f, () => resetWeaponStand()));
        }
    }


    void ExecuteLightAttack1()
    {
        Vector3 mouseWorld = GetMousePositionInWorld();
        Vector3 direction = (mouseWorld - player.transform.position).normalized;
        playerInputControl.SetFacingDirection(direction);
        StartCoroutine(weaponBehaviour.ModifyMovement1(weaponBehaviour.chargeDistance, weaponBehaviour.chargeTime, direction, player.transform, playerInputControl));
        weaponBehaviour.LightAttack(WeaponData.weaponBaseDmg, isCrit(WeaponData.critChance), WeaponData.critMultiplier, player.transform.position, GetMousePositionInWorld(), this, playerStats);
        
        StartCoroutine(weaponBehaviour.PlayLightAttackAnimation1(player.transform.position, GetMousePositionInWorld(), weaponGameObjectHolder.transform, weaponSlash_light_FXGameObjectHolder.transform));
        ShowSlashEffect(weaponSlash_light_FXSpriteRenderer);
        FadeOutEffect(weaponSlash_light_FXSpriteRenderer, 0.25f);
       
    }

    void ExecuteLightAttack2()
    {
        Vector3 mouseWorld = GetMousePositionInWorld();
        Vector3 direction = (mouseWorld - player.transform.position).normalized;
        playerInputControl.SetFacingDirection(direction);
        StartCoroutine(weaponBehaviour.ModifyMovement1(weaponBehaviour.chargeDistance, weaponBehaviour.chargeTime, direction, player.transform, playerInputControl));
        weaponBehaviour.LightAttack(WeaponData.weaponBaseDmg, isCrit(WeaponData.critChance),WeaponData.critMultiplier, player.transform.position, GetMousePositionInWorld(), this, playerStats);
        StartCoroutine(weaponBehaviour.PlayLightAttackAnimation2(player.transform.position, GetMousePositionInWorld(), weaponGameObjectHolder.transform, weaponSlash_light_FXGameObjectHolder.transform));
        ShowSlashEffect(weaponSlash_light_FXSpriteRenderer);
        FadeOutEffect(weaponSlash_light_FXSpriteRenderer, 0.25f);
    }

    void ExecuteHeavyAttack()
    {
        Vector3 mouseWorld = GetMousePositionInWorld();
        Vector3 direction = (mouseWorld - player.transform.position).normalized;
        playerInputControl.SetFacingDirection(direction);
        StartCoroutine(weaponBehaviour.ModifyMovement2(weaponBehaviour.chargeDistance, weaponBehaviour.chargeTime, direction, player.transform, playerInputControl));
        weaponBehaviour.HeavyAttack(WeaponData.weaponBaseDmg, isCrit(WeaponData.critChance), WeaponData.critMultiplier, player.transform.position, GetMousePositionInWorld(), this, playerStats);
        StartCoroutine(weaponBehaviour.PlayHeavyAttackAnimation(player.transform.position, GetMousePositionInWorld(), weaponGameObjectHolder.transform, weaponSlash_heavy_FXGameObjectHolder.transform));
        ShowSlashEffect(weaponSlash_heavy_FXSpriteRenderer);
        FadeOutEffect(weaponSlash_heavy_FXSpriteRenderer, 0.25f);


    }




    void ShowSlashEffect(SpriteRenderer sr)
    {
        sr.enabled = true;
        // Make it visible instantly
        Color c = sr.color;
        c.a = 1f;
        sr.color = c;

        // Start fading out
        StartCoroutine(FadeOutEffect(sr, 0.5f)); // 0.5 seconds fade
    }


    IEnumerator FadeOutEffect(SpriteRenderer sr, float duration)
    {
        float elapsed = 0f;
        Color originalColor = sr.color;

        while (elapsed < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure it's fully transparent at the end
        sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
    }



    public void resetWeaponStand()
    {
        if (WeaponData == null) return;
        if(playerAnimation.currentHorizontalDirection == PlayerAnimation.HorizontalDirection.Right)
        {
            weaponGameObjectHolder.transform.localRotation = Quaternion.Euler(weaponStandTransformRight);
            //Debug.Log("weaponStandTransformRight is: " + weaponStandTransformRight);
        }

        else
        {
            weaponGameObjectHolder.transform.localRotation = Quaternion.Euler(weaponStandTransformLeft);
            //Debug.Log("weaponStandTransformLeft is: " + weaponStandTransformLeft);
        }
           

    }



    public bool isCrit(int chance)
    {
        chance = Mathf.Clamp(chance, 0, 100);
        return UnityEngine.Random.Range(1, 101) <= chance;
    }


    //public void UpdateSpritePos()
    //{
    //    // Set the sprite visuals


    //    weaponSprite.localPosition = WeaponData.spriteLocalPosition;
    //    weaponSprite.localEulerAngles = WeaponData.spriteLocalRotation;
    //    weaponSprite.localScale = WeaponData.spriteLocalScale;

    //    // Optionally, offset the whole weapon pivot
    //    weaponPivot.localPosition = WeaponData.weaponPivotOffset;
    //}


    


    //public void DynamicWeaponRenderOrder()
    //{
    //    if(_player._currentDirectionState == PlayerController.directionState.front)
    //    {
    //        weaponSpriteRenderer.sortingOrder = _player._spriteRenderer.sortingOrder - 1;
    //    }
    //    else if (_player._currentDirectionState == PlayerController.directionState.back)
    //    {
    //        weaponSpriteRenderer.sortingOrder = _player._spriteRenderer.sortingOrder + 1;
    //    }
    //}

    //public void AnimateWeaponBob()
    //{
    //    if (_player.isMoving) // Or whatever your movement check is
    //    {
    //        float offsetY = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
    //        weaponSprite.localPosition = initialSpriteLocalPos + new Vector3(0, offsetY, 0);
    //    }
    //    else
    //    {
    //        weaponSprite.localPosition = initialSpriteLocalPos;
    //    }
    //}




    public void SwingWeapon(float angle)
    {
        weaponPivot.localEulerAngles = new Vector3(0, 0, angle);
    }

    public IEnumerator StartCountDown(float duration, Action onFinished)
    {
        yield return new WaitForSeconds(duration);
        onFinished?.Invoke();
    }


   

}
