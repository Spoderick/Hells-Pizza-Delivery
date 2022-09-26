using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] BoxCollider boxCollider;

    [SerializeField]Missions_Manager missionManager;
    [SerializeField]GameManager manager;

    [SerializeField] private GameObject[] enemies;
    [SerializeField] private ParticleSystem spawnEffect;
    [SerializeField] private float[] coinsForLevel;
    [SerializeField] private AudioSource spawnerSound;

    [SerializeField] private float[] maxEnemiesPerSpawner;
    private int spawnerLevel;
    private int enemiesTypes;
    
    [SerializeField] private float[] timePerEnemieSpawn;

    private int timedifficulty;

    //Controll
    private bool hasSpawned;
   


    void Start()
    {
        hasSpawned = false;
    }

  
    void Update()
    {
        if (!missionManager.startMissions) { hasSpawned = false; }
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
            //int f = Random.Range(0, (3 + 1));
           // print(f);
        //}
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && missionManager.startMissions && !hasSpawned)
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
            hasSpawned = true;
        }
        if (manager.TotalMoneyFromMissions >= coinsForLevel[1] && manager.TotalMoneyFromMissions < coinsForLevel[2])//Nivel 2
        {
            timedifficulty = 1;
            enemiesTypes = 3;
            spawnerLevel = 1;
            StartCoroutine(SpawnEnemies());
            hasSpawned = true;
        }
        if (manager.TotalMoneyFromMissions >= coinsForLevel[2] && manager.TotalMoneyFromMissions < coinsForLevel[3])//Nivel 3
        {
            timedifficulty = 2;

            enemiesTypes = 3;
            spawnerLevel = 2;
            StartCoroutine(SpawnEnemies());
            hasSpawned = true;
        }
        if (manager.TotalMoneyFromMissions >= coinsForLevel[3]) //Nicel 4
        {
            timedifficulty=3;
            enemiesTypes = 3;
            spawnerLevel = 3;
            StartCoroutine(SpawnEnemies());
            hasSpawned = true;
        }

    }

    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < maxEnemiesPerSpawner[spawnerLevel]; i++)
        {
            Bounds b = boxCollider.bounds;
            float randX = Random.Range(b.min.x, b.max.x);
            float randZ = Random.Range(b.min.z,b.max.z);
            Vector3 randPos = new Vector3(randX,0,randZ);
            int enemiesClass = Random.Range(0, (enemiesTypes));
            print(enemies[enemiesClass].name);
            Instantiate(spawnEffect, randPos, Quaternion.Euler(-90,0,0));
            var newEnemy = Instantiate(enemies[enemiesClass], randPos, Quaternion.identity);
            newEnemy.gameObject.tag = "Enemy";
            spawnerSound.Play();
            yield return new WaitForSeconds(timePerEnemieSpawn[timedifficulty]);
        }
    }   

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawCube(transform.position, new Vector3(boxCollider.size.x, boxCollider.size.y, boxCollider.size.z));
    }

}
