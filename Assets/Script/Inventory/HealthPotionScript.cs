using UnityEngine;

[CreateAssetMenu(menuName = "Items/Health Potion")]
public class HealthPotionScript : UsableItem
{
    public override void Use(GameObject user)
    {
        PlayerController _playerController = user.GetComponent<PlayerController>();
        int totalHealAmount = Mathf.CeilToInt((PlayerController.PlayerMaxHealth - _playerController.PlayerHealth) * 0.3f);
        _playerController.Heal(totalHealAmount);
        var spawner = user.GetComponent<ParticleSpawner>();
        if (spawner != null)
        {
            spawner.StartCoroutine(spawner.SpawnHealEffect(user.transform));

        }

        SoundFXManager.Instance.PlaySoundFXClip(activeSounds, user.transform , 1f);
    }
}
