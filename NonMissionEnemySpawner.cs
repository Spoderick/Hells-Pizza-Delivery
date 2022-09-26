using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonMissionEnemySpawner : MonoBehaviour
{
    [SerializeField] BoxCollider boxCollider;

    [SerializeField] Missions_Manager missionManager;
    [SerializeField] GameManager manager;

    [SerializeField] private GameObject[] enemies;
    [SerializeField] private float[] coinsForLevel;
    [SerializeField] private AudioSource spawnerSound;

    [SerializeField] private float[] maxEnemiesPerSpawner;
    [SerializeField] private ParticleSystem spawnEffect;
    private int spawnerLevel;
    private int enemiesTypes;

    [SerializeField] private float[] timePerEnemieSpawn;
    private int timedifficulty;


    //cronometro
    private float timeforReActivate;
    [SerializeField] private float timeForReload;


    //Controll
    private bool HasSpawned;



    void Start()
    {
       timeforReActivate = timeForReload;
       HasSpawned = false;
    }


    void Update()
    {
        timeCounter();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !missionManager.startMissions && !HasSpawned)
        {
            ActivateHorde();
        }

    }

    void ActivateHorde()
    {
        if (manager.TotalMoneyFromMissions >= coinsForLevel[0] && manager.TotalMoneyFromMissions < coinsForLevel[1]) //Nivel 1
        {
            timedifficulty = 0;
            enemiesTypes = 3;
            spawnerLevel = 0;
            StartCoroutine(SpawnEnemies());
            HasSpawned = true;

        }
        if (manager.TotalMoneyFromMissions >= coinsForLevel[1] && manager.TotalMoneyFromMissions < coinsForLevel[2])//Nivel 2
        {
            timedifficulty = 1;
            enemiesTypes = 3;
            spawnerLevel = 1;
            StartCoroutine(SpawnEnemies());
            HasSpawned = true;

        }
        if (manager.TotalMoneyFromMissions >= coinsForLevel[2] && manager.TotalMoneyFromMissions < coinsForLevel[3])//Nivel 3
        {
            timedifficulty = 2;

            enemiesTypes = 3;
            spawnerLevel = 2;
            StartCoroutine(SpawnEnemies());
            HasSpawned = true;
        }
        if (manager.TotalMoneyFromMissions >= coinsForLevel[3]) //Nicel 4
        {
            timedifficulty = 3;
            enemiesTypes = 3;
            spawnerLevel = 3;
            StartCoroutine(SpawnEnemies());
            HasSpawned = true;
        }

    }

    IEnumerator SpawnEnemies()
    {
        
            for (int i = 0; i < maxEnemiesPerSpawner[spawnerLevel]; i++)
            {
                int probablility = Random.Range(0, 4);
                if (probablility == 2)
                {
                    Bounds b = boxCollider.bounds;
                    float randX = Random.Range(b.min.x, b.max.x);
                    float randZ = Random.Range(b.min.z, b.max.z);
                    Vector3 randPos = new Vector3(randX, 0, randZ);
                    int enemiesClass = Random.Range(0, (enemiesTypes+1));
                    var newEnemy = Instantiate(enemies[enemiesClass], randPos, Quaternion.identity);
                    newEnemy.gameObject.tag = "Enemy";
                    Instantiate(spawnEffect, randPos, Quaternion.identity);
                    spawnerSound.Play();
            }
                yield return new WaitForSeconds(timePerEnemieSpawn[timedifficulty]);
                
            }

        
    }

    void timeCounter()
    {
        if (HasSpawned)
        {
            timeforReActivate -= Time.deltaTime;
        }
        if (timeforReActivate <= 0)
        {
            HasSpawned = false;
            timeforReActivate = timeForReload;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(.65f, 0, .45f, 0.4f);
        Gizmos.DrawCube(transform.position, new Vector3(boxCollider.size.x, boxCollider.size.y, boxCollider.size.z));
    }
}
