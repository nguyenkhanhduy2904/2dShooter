using UnityEngine;


[CreateAssetMenu(menuName = "Item Effects/Speed")]

public class SpeedEffect : ItemEffect
{
    public float speedBoost = 20f;
    public float duration = 3f;
    public override void ApplyEffect(GameObject user)
    {
        var speed = user.GetComponent<PlayerController>();
        if (speed != null)
        {
           
            speed.SpeedModify(speedBoost,duration);
            SoundFXManager.Instance.PlaySoundFXClip(FXSound, user.transform, 1f);
            ParticleFXManager.Instance.PlayParticleFX(ParticalPrefab, user.transform, 2.5f);


        }
    }
}
