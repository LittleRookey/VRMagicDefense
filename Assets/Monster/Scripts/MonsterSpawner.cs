using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Pool Settings")]
    [SerializeField]
    private bool _resetAllPoolOnStart;
    [SerializeField] private Transform enemyContainer;
    
    [Header("Spawner Settings")]
    [SerializeField] private bool waveStartWhenAllEnemiesDead = true;
    [SerializeField] private float timeBetweenWaves;
    [SerializeField] private float spawnRandDist = 5f;
    [SerializeField] private Wave[] waves;

    

    [System.Serializable]
    struct Wave
    {
        public EnemyInfo[] enemies;
        public float spawnTimeBetweenEnemies;
        public SpawnMethod spawnMethod;
    }

    [System.Serializable]
    struct EnemyInfo
    {
        public string enemyName;
        public int enemyNumber;
        public SpawnPoint spawnPoint;
    }

    [System.Serializable]
    public enum SpawnPoint
    {
        SpawnPoint1,
        SpawnPoint2,
        SpawnPoint3,
        Random
    };

    public enum SpawnMethod
    {
        Sequentially,
        Syncrhonouslly
    };

    [SerializeField] private Transform[] spawnPositions; // should match the number of enums

    private bool waveFinished => waveEnemyNumber == 0;
    private int waveEnemyNumber; 

    private int waveIndex = 0;

    private MonsterContainer monsterContainer;

    public Transform initialTarget;

    private void Awake()
    {
        
        //tokenSource = tokenSource.Token.ThrowIfCancellationRequested();
        monsterContainer = GetComponent<MonsterContainer>();
    }
    private void Start()
    {
        waveIndex = 0;

        //enemyPool.Get();
        //for (int i = 0; i < 5; i++)
        //    Spawn("Square");
        //StartSpawnEnemies(waveIndex);
    }

    private void Update()
    {
        Debug.Log(waveFinished);
        if (waveFinished)
            StartSpawnEnemies(ThreadingUtility.QuitToken, waveIndex);
    }

    private Vector3 GetSmallError()
    {
        Vector3 smallError = ((Vector3.right + Vector3.forward));
        smallError.x *= Random.Range(-1 * Mathf.Abs(spawnRandDist), Mathf.Abs(spawnRandDist));
        smallError.z *= Random.Range(-1 * Mathf.Abs(spawnRandDist), Mathf.Abs(spawnRandDist));
        return smallError;
    }

    private Vector3 GetFinalSpawnPosition(Vector3 pos)
    {
        Vector3 finalPos = pos;
        finalPos += GetSmallError();
        finalPos.y = 0f;
        return finalPos;
    }

    private async void StartSpawnEnemies(CancellationToken tok, int index)
    {
        tok.ThrowIfCancellationRequested();
        await Task.Delay((int)(timeBetweenWaves * 1000));
        // spawn enemies
        if (waveStartWhenAllEnemiesDead)
        {
            if (waveFinished)
            {
                tok.ThrowIfCancellationRequested();
                await SpawnWave(tok, waves[waveIndex]);
            }
        }


    }

    private async Task SpawnWave(CancellationToken tok, Wave wave)
    {
        Debug.Log($"Wave {waveIndex} started");
        var enemies = wave.enemies;
        int spawnTime = (int)(wave.spawnTimeBetweenEnemies * 1000);
        waveEnemyNumber = GetTotalEnemyNumberOf(wave);
        waveIndex = (waveIndex + 1) % waves.Length;

        switch (wave.spawnMethod)
        {
            case SpawnMethod.Sequentially:
                for (int i = 0; i < enemies.Length; i++)
                {
                    for (int k = 0; k < enemies[i].enemyNumber; k++)
                    {
                        tok.ThrowIfCancellationRequested();
                        await Task.Delay(spawnTime);
                        Spawn(enemies[i].enemyName, enemies[i].spawnPoint);
                    }
                }
                break;
            case SpawnMethod.Syncrhonouslly:
                foreach (EnemyInfo enemInfo in enemies)
                {
                    tok.ThrowIfCancellationRequested();
                    Spawn(enemInfo.enemyName, enemInfo.spawnPoint, wave.spawnTimeBetweenEnemies, enemInfo.enemyNumber);
                }
                break;
        }

    }


    // Spawns each enemeis
    public void Spawn(string name, SpawnPoint spawnPoint)
    {
        // loads the info of monster
        Monster monster = monsterContainer.GetMonster(name);

        Monster spawnedMonster;
        // spawns the loaded monster
        if (spawnPoint != SpawnPoint.Random)
        {
            // if spawnpoint is not random
            Vector3 pos = spawnPositions[(int)spawnPoint].position;
            
            spawnedMonster = Instantiate(monster, GetFinalSpawnPosition(pos), monster.transform.rotation);
        } else
        {
            // if spawn point is random
            int randPos = Random.Range(0, spawnPositions.Length);
            Vector3 pos = spawnPositions[randPos].position;
            spawnedMonster = Instantiate(monster, GetFinalSpawnPosition(pos), monster.transform.rotation);
        }
        spawnedMonster.SetTarget(initialTarget);
        spawnedMonster.GetComponent<Health>().OnDeath += OnEnemyDeath;
        if (enemyContainer)
            spawnedMonster.transform.SetParent(enemyContainer);

    }

    public async void Spawn(string name, SpawnPoint spawnPoint, float spawnTime, int enemyNumber)
    {
        for (int i = 0; i < enemyNumber; i++)
        {
            ThreadingUtility.QuitToken.ThrowIfCancellationRequested();
            await Task.Delay((int)(1000 * spawnTime));
            Spawn(name, spawnPoint);
        }

    }

    // Gets the total number of enemy of the given wave
    private int GetTotalEnemyNumberOf(Wave wave)
    {
        int num = 0;
        for (int i = 0; i < wave.enemies.Length; i++)
        {
            num += wave.enemies[i].enemyNumber;
        }
        return num;
    }

    private void OnEnemyDeath(GameObject go)
    {
        waveEnemyNumber--;

        go.GetComponent<Health>().OnDeath -= OnEnemyDeath;
    }

}
