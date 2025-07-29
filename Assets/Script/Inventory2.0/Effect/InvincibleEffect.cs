using UnityEngine;

[CreateAssetMenu(menuName = "Item Effects/Invincible")]
public class InvincibleEffect : ItemEffect
{
    public float duration = 2.5f;
    public float bufferTime = 0.25f;

    public override void ApplyEffect(GameObject user)
    {
        var player = user.GetComponent<PlayerController>();
        var playerSprite = user.GetComponentInChildren<SpriteRenderer>();
        if (player != null) 
        {
            player.InvincibleActivate(duration);
            player.ChangeSpriteColor(duration + bufferTime, Color.yellow);
            var length = FXSound[2].length;
            SoundFXManager.Instance.PlaySoundFXInSequence(FXSound, user.transform, 1f, duration, bufferTime, duration );
            ParticleFXManager.Instance.PlayParticleFX(ParticalPrefab, user.transform, 2.5f);
        }



    }
}
