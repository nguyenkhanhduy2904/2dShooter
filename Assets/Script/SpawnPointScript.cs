using UnityEngine;

public class SpawnPointScript : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
    }
    
}
