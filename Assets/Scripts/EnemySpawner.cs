using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private float _spawnTimer;

    [SerializeField] private float _minSpawnTime;
    [SerializeField] private float _maxSpawnTime;
    [SerializeField] private GameObject _enemyPrefab;

    private void Update()
    {
        _spawnTimer -= Time.deltaTime;

        if (_spawnTimer <= 0f)
        {
            Instantiate(_enemyPrefab, transform.position, transform.rotation);
            _spawnTimer = Random.Range(_minSpawnTime, _maxSpawnTime);
        }
    }
}
