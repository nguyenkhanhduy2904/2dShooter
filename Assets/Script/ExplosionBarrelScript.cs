using Assets.Script;
using TMPro;
using UnityEngine;
using System.Collections;

public class ExplosionBarrelScript : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject _explosionPrefab;
    [SerializeField] private int _barrelMaxHealth = 50;
    public SpriteRenderer _explosionRenderer;
    [SerializeField] private GameObject _floatingTextPreFab;
    [SerializeField] private AudioClip[] _metalHitSounds;
    [SerializeField] private AudioClip[] _explosionSounds;
    private int _barrelHealth;

    private void Start()
    {
        _barrelHealth = _barrelMaxHealth;
        _explosionRenderer = GetComponentInChildren<SpriteRenderer>();
    }


    public void TakeDmg(int amount, bool _isCrit)
    {
        _barrelHealth -= amount;
        _barrelHealth = Mathf.Clamp(_barrelHealth, 0, _barrelMaxHealth);
        ShowDamage(amount.ToString(), _isCrit);
        SoundFXManager.Instance.PlaySoundFXClip(_metalHitSounds, transform, 1f);// do you got the audio clip yet?

        if(_barrelHealth <= 0)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            SoundFXManager.Instance.PlaySoundFXClip(_explosionSounds, transform, 1f);
            Destroy(gameObject);
        }


    }


    public void ShowDamage(string text, bool isCrit = false)
    {
        GameObject prefab = Instantiate(_floatingTextPreFab, transform.position, Quaternion.identity);
        TextMeshPro textMesh = prefab.GetComponentInChildren<TextMeshPro>();

        textMesh.text = text;

        // Color and style based on crit
        if (isCrit)
        {
            textMesh.color = Color.red;
            textMesh.fontSize = 10f;
            // Enable Crit Icon
            Transform critIcon = prefab.transform.Find("FloatingText/CritIcon");


            if (critIcon != null)
            {
                Debug.Log("found it");
                critIcon.gameObject.SetActive(true);
            }


        }
        else
        {
            textMesh.color = Color.white;
            textMesh.fontSize = 5f;
        }

        Destroy(prefab, 1f);
    }
    public IEnumerator ChangeColor(Color color, float time)
    {
        _explosionRenderer.color = color;
        yield return new WaitForSeconds(time);
    }

    public void DealDmg(IDamageable target, int dmg, bool isCrit)
    {
        //this object cannot deal dmg by itself, the dmg will come from the explosion instead
    }


}
