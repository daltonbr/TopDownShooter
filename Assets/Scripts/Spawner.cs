using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

//[RequireComponent (typeof(MapGenerator))]
public class Spawner : MonoBehaviour {

    public bool devMode;

    public Wave[] waves;
    public Enemy enemy;
    public Color initialTileColor = Color.white;

	LivingEntity playerEntity;
	Transform playerT;

	[Header("Health Pack")]
	public HealthPack hp;
	public float hpSpawnTime = 3;	// in seconds
	public int hpCapAmmount = 3;

	private int hpAccumulated = 0;
	private float hpNextSpawnTime;

    [Header("Guns & Ammo")]
    public GunPickup[] gunsPickupToRespawn;
    public Vector2 gunsSpawnTime = new Vector2(3f, 6f);   // in seconds - x for min, y for max
    public int gunsCapAmmount = 3;

    private int gunsAccumulated = 0;
    private float gunsNextSpawnTime;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
	float enemyNextSpawnTime;

    MapGenerator map;

    // Variables to check if the player is camping
    float timeBetweenCampingChecks = 2f;
    float campThresholdDistance = 1.5f;
    float nextCampCheckTime;
    Vector3 campPositionOld;
    bool isCamping;

    bool isDisbled;

    public event System.Action<int> OnNewWave;

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
            if ((enemiesRemainingToSpawn > 0 || currentWave.infinite) && Time.time > enemyNextSpawnTime)
            {
                enemiesRemainingToSpawn--;
                enemyNextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

                StartCoroutine("SpawnEnemy");
            }

			// Spawning HP's
			if ((hpAccumulated < hpCapAmmount) && (hpNextSpawnTime < Time.time))
			{
				// Instantiate a HP
				hpAccumulated++;
				hpNextSpawnTime = Time.time + hpSpawnTime;
				SpawnHP();
			}

            // Spawning Guns
            if ((gunsAccumulated < gunsCapAmmount) && (gunsNextSpawnTime < Time.time))
            {
                gunsAccumulated++;
                gunsNextSpawnTime = Time.time + Random.Range(gunsSpawnTime.x, gunsSpawnTime.y);
                SpawnRandomGun();
            }

        }

        if(devMode)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StopCoroutine("SpawnEnemy");
                foreach(Enemy enemy in FindObjectsOfType<Enemy>())
                {
                    GameObject.Destroy(enemy.gameObject);
                }
                NextWave();
            }
        }

    }

    void SpawnRandomGun()
    {
        Transform spawnTile = map.GetRandomOpenTile();
        int index = Random.Range(0, gunsPickupToRespawn.Length);
        GunPickup spawnedGun = Instantiate(gunsPickupToRespawn[index], spawnTile.position + Vector3.up, Quaternion.identity) as GunPickup;
        spawnedGun.OnCollected += this.OnGunCollected;
    }

    void SpawnHP()
	{
		Transform spawnTile = map.GetRandomOpenTile();
		HealthPack spawnedHP = Instantiate(hp, spawnTile.position + Vector3.up, Quaternion.identity) as HealthPack;
		spawnedHP.OnCollected += this.OnHPCollected;
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
        Color initialColor = initialTileColor;
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
        spawnedEnemy.SetCharacteristics(currentWave.moveSpeed, currentWave.hitsToKillPlayer, currentWave.enemyHealth, currentWave.skinColor);

    }

    void OnGunCollected()
    {
        if (gunsAccumulated > 0)
        {
            gunsAccumulated--;
        }
    }

    void OnHPCollected()
	{
		if(hpAccumulated>0)	
		{	
			hpAccumulated--;
		}
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

    void ResetPlayerPosition()
    {
        playerT.position = map.GetTileFromPosition(Vector3.zero).position + Vector3.up * 3;
    }

    void NextWave()
    {
        if (currentWaveNumber > 0)
        {
            AudioManager.instance.PlaySound2D("LevelComplete");
        }
        currentWaveNumber++;
        if (currentWaveNumber - 1 < waves.Length)
        {
            //print("Wave " + currentWaveNumber);
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;

            if (OnNewWave != null)
            {
                OnNewWave(currentWaveNumber);
            }
        ResetPlayerPosition();
        }
    }

    [System.Serializable]
    public class Wave
    {
        public bool infinite;
        public int enemyCount;
        public float timeBetweenSpawns;

        public float moveSpeed;
        public int hitsToKillPlayer;
        public float enemyHealth;
        public Color skinColor;
    }
    
}
