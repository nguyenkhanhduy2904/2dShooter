using UnityEngine;

public class SceneManager : MonoBehaviour
{
   
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField]SpawnPointScript[] _spawnPoints; 

    

    [SerializeField] static int _maxEnemy = 1;
    public static int _currentEnemy=0;

    


    private void Update()
    {
        if(_currentEnemy < _maxEnemy)
        {
            SpawnAtRandom();
            _currentEnemy++;
        }
    }

    void SpawnAtRandom()
    {
        int randomIndex = Random.Range(0, _spawnPoints.Length);
        _spawnPoints[randomIndex].SpawnEnemy();
    }

    public static void NotifyEnemyDied()
    {
        _currentEnemy = Mathf.Max(0, _currentEnemy - 1); // avoid going below 0
    }





}
