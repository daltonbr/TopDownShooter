using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//[RequireComponent (typeof(MapGenerator))]
public class Spawner : MonoBehaviour {

    public Wave[] waves;
    public Enemy enemy;

    LivingEntity playerEntity;
    Transform playerT;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    MapGenerator map;

    // Variables to check if the player is camping
    float timeBetweenCampingChecks = 2f;
    float campThresholdDistance = 1.5f;
    float nextCampCheckTime;
    Vector3 campPositionOld;
    bool isCamping;

    bool isDisbled;

    private void Start()
    {
        playerEntity = FindObjectOfType<Player>();
        playerT = playerEntity.transform;

        nextCampCheckTime = timeBetweenCampingChecks + Time.time;
        campPositionOld = playerT.position;
        playerEntity.OnDeath += OnPlayerDeath;

        map = FindObjectOfType<MapGenerator>();
        Assert.IsNotNull(map, "Spawner::Start() - Error: Can't find a MapGenerator!");
        NextWave();
    }

    void Update()
    {
        if (!isDisbled)
        {
            // Camping check
            if (Time.time > nextCampCheckTime)
            {
                nextCampCheckTime = Time.time + timeBetweenCampingChecks;

                isCamping = (Vector3.Distance(playerT.position, campPositionOld) < campThresholdDistance);
                campPositionOld = playerT.position;
            }

            // Spawning enemies
            if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

                StartCoroutine(SpawnEnemy());
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        float spawnDelay = 1;
        float tileFlashSpeed = 4f;

        Transform spawnTile = map.GetRandomOpenTile();
        if(isCamping)
        {
            spawnTile = map.GetTileFromPosition(playerT.position);
        }
        Material tileMat = spawnTile.GetComponent<Renderer>().material;
        Color initialColor = tileMat.color;
        Color flashColor = Color.red;
        float spawnTimer = 0f;

        while (spawnTimer < spawnDelay)
        {
            // Flashing tiles (PingPong creates an oscilating pattern)
            tileMat.color = Color.Lerp(initialColor, flashColor, Mathf.PingPong(spawnTimer * tileFlashSpeed, 1));
            spawnTimer += Time.deltaTime;
            yield return null;                      // just skip a frame
        }
        Enemy spawnedEnemy = Instantiate(enemy, spawnTile.position + Vector3.up, Quaternion.identity) as Enemy;
        spawnedEnemy.OnDeath += OnEnemyDeath;

    }

    void OnPlayerDeath()
    {
        isDisbled = true;
    }

    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;

        if (enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    void NextWave()
    {
        currentWaveNumber++;
        if (currentWaveNumber - 1 < waves.Length)
        {
            print("Wave " + currentWaveNumber);
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
        }
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }

}
