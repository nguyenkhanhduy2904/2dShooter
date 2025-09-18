using UnityEngine;

public class SceneManager : MonoBehaviour
{

    [SerializeField] public PlayerController _player;
    


    //private void Update()
    //{
    //    if (_player == null) return;

    //    if ( !_player.isAlive)
    //    {
    //        Debug.LogError("You alive?" + _player.isAlive);
    //        _player.Die();
    //        foreach (EnemyBehaviour enemy in FindObjectsOfType<EnemyBehaviour>())
    //        {
    //            enemy.StopAllCoroutines();
    //            enemy.ForceIdle();
                
    //        }

    //    }
    //}

    public void SpawnAOEEffect (Vector3 startPos, Vector3 endPos, GameObject AOEeffect, Vector3 scale)
    {
        GameObject instance = Instantiate(AOEeffect, endPos, Quaternion.identity);
        instance.transform.localScale = scale;
    }





}
