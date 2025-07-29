using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName = "Item Effects/Heal")]
public class HealEffect : ItemEffect
{
    public int healAmount = 50;

    public override void ApplyEffect(GameObject user)
    {
        var health = user.GetComponent<PlayerController>();
        if (health != null)
        {
            health.Heal(healAmount);
            SoundFXManager.Instance.PlaySoundFXClip(FXSound, user.transform, 1f);
            ParticleFXManager.Instance.PlayParticleFX(ParticalPrefab, user.transform, 2.5f);
        }
    }

    
}
