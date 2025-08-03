using UnityEngine;
using System.Collections;

public class WeaponHolder2 : MonoBehaviour
{

    [SerializeField]public WeaponData2 WeaponData;
    PlayerController _player;

    [SerializeField] public Transform weaponPivot;      // "WeaponHolder2" object
    [SerializeField] public Transform weaponSprite;     // "WeaponSprite" object
    public SpriteRenderer weaponSpriteRenderer;
    public SpriteRenderer slashEffect;
    [SerializeField] private float bobSpeed = 60f;
    [SerializeField] private float bobAmount = 0.1f;
    private Vector3 initialSpriteLocalPos;

    private void Start()
    {
        _player = GetComponent<PlayerController>();


        weaponSpriteRenderer = weaponSprite.GetComponent<SpriteRenderer>();
        weaponSpriteRenderer.sprite = WeaponData.weaponSprite;
        initialSpriteLocalPos = weaponSprite.localPosition;
        slashEffect.sprite = WeaponData.sliceEffect;
        slashEffect.enabled = false;

        if (WeaponData != null)
        {
          UpdateSpritePos();
        }

       
    }
    public Vector3 GetMousePositionInWorld()
    {
        Vector3 mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mousePosInWorld;
    }

    public void TryAttack()
    {
        if (WeaponData != null)
        {
            StartCoroutine(AttackWrapper());
            Debug.Log("TryAttack called from: " + gameObject.name);
        }
    }

    private IEnumerator AttackWrapper()
    {
        // Before the coroutine: activate effect
        slashEffect.enabled = true;

        // Run the original coroutine
        yield return StartCoroutine(WeaponData.weaponBehaviour.LightAttack(
            WeaponData.weaponBaseDmg,
            isCrit(WeaponData.critChance),
            _player));

        // After the coroutine: disable effect
        slashEffect.enabled = false;
    }

    public bool isCrit(int chance)
    {
        chance = Mathf.Clamp(chance, 0, 100);
        return Random.Range(1, 101) <= chance;
    }


    public void UpdateSpritePos()
    {
        // Set the sprite visuals


        weaponSprite.localPosition = WeaponData.spriteLocalPosition;
        weaponSprite.localEulerAngles = WeaponData.spriteLocalRotation;
        weaponSprite.localScale = WeaponData.spriteLocalScale;

        // Optionally, offset the whole weapon pivot
        weaponPivot.localPosition = WeaponData.weaponPivotOffset;
    }


    private void Update()
    {
        UpdateSpritePos();
    }


    public void DynamicWeaponRenderOrder()
    {
        if(_player._currentDirectionState == PlayerController.directionState.front)
        {
            weaponSpriteRenderer.sortingOrder = _player._spriteRenderer.sortingOrder - 1;
        }
        else if (_player._currentDirectionState == PlayerController.directionState.back)
        {
            weaponSpriteRenderer.sortingOrder = _player._spriteRenderer.sortingOrder + 1;
        }
    }

    public void AnimateWeaponBob()
    {
        if (_player.isMoving) // Or whatever your movement check is
        {
            float offsetY = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
            weaponSprite.localPosition = initialSpriteLocalPos + new Vector3(0, offsetY, 0);
        }
        else
        {
            weaponSprite.localPosition = initialSpriteLocalPos;
        }
    }


    public void SwingWeapon(float angle)
    {
        weaponPivot.localEulerAngles = new Vector3(0, 0, angle);
    }


}
