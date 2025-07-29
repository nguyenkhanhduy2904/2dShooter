using UnityEngine;

[CreateAssetMenu(menuName = "Item Effects/TimeScaleEffect")]
public class TimeScaleEffect : ItemEffect
{
    public float duration = 3f;
    public float bufferTime = 0.25f;
    public float scale = 0.2f;
    public override void ApplyEffect(GameObject user)
    {
        var _player = user.GetComponent<PlayerController>();
        if (_player != null) 
        {
            _player.TimeScaleModify(scale, duration);
            var length = FXSound[2].length;
            SoundFXManager.Instance.PlaySoundFXInSequence(FXSound, user.transform, 1f, duration, bufferTime,duration - length);
        }
    }
}
