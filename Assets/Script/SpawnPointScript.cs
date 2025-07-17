using UnityEngine;

public class SpawnPointScript : MonoBehaviour
{
    [SerializeField] GameObject[] _enemyPrefab;
    public void SpawnEnemy()
    {
        int rand = Random.Range(0, _enemyPrefab.Length);
        GameObject enemy = Instantiate(_enemyPrefab[rand], transform.position, Quaternion.identity);
    }
    
}
