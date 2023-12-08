using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private List<GameObject> _spawners = new List<GameObject>();
    private GameObject _currentSpawner;

    [SerializeField] private float _spawnSwitchTime;

    private void Start()
    {
        EnemySpawner[] spawners = FindObjectsOfType<EnemySpawner>();
        for (int i = 0; i < spawners.Length; i++)
        {
            _spawners.Add(spawners[i].gameObject);
            _spawners[i].SetActive(false);
        }

        _currentSpawner = _spawners[0];
        _spawners[0].SetActive(true);
    }

    private void Update()
    {
        _spawnSwitchTime -= Time.deltaTime;

        if (_spawnSwitchTime <= 0f)
        {
            _currentSpawner.SetActive(false);

            int i = Random.Range(0, _spawners.Count);

            _currentSpawner = _spawners[i];
        }
    }
}
