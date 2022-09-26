using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    [SerializeField] BoxCollider boxCollider;

    [SerializeField] Missions_Manager missionManager;
    [SerializeField] GameManager manager;
    [SerializeField] AttackCombosController a;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private ParticleSystem spawnEffect;
    [SerializeField] private AudioSource spawnerSound;
    [SerializeField] private AudioSource FinalMusic;
    [SerializeField] private AudioSource NormalMusic;
    [SerializeField] private float maxEnemiesPerSpawner;
    [SerializeField]EnemyChecker enemyChecker;
    
    private bool startfight;
    private int enemiesTypes;

    [SerializeField] private float timePerEnemieSpawn;
    
    private bool hasSpawned;

    [SerializeField] private float numTrys;
    public float NumTrys
    {
        get { return numTrys; }
        set { numTrys = value; }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (a.Playerhealth <= 0 && startfight)
        {
            enemyChecker.nonstart = true;
            StopCoroutine(SpawnEnemies());
            startfight = false;
            FinalMusic.Stop();
            NormalMusic.Play();
            hasSpawned = false;
            var clones = GameObject.FindGameObjectsWithTag ("Enemy");
            foreach (var clone in clones)
            {
                Destroy(clone);
            }
    }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !hasSpawned)
        {
            enemyChecker.nonstart = false;
            FinalMusic.Play();
            enemiesTypes = 3;
            startfight = true;
            StartCoroutine(SpawnEnemies());
            hasSpawned = true;
        }

    }
   
    IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < maxEnemiesPerSpawner; i++)
        {
            if (startfight)
            {
                Bounds b = boxCollider.bounds;
                float randX = Random.Range(b.min.x, b.max.x);
                float randZ = Random.Range(b.min.z, b.max.z);
                Vector3 randPos = new Vector3(randX, 0, randZ);
                int enemiesClass = Random.Range(0, (enemiesTypes));
                print(enemies[enemiesClass].name);
                Instantiate(spawnEffect, randPos, Quaternion.Euler(-90, 0, 0));
                var newEnemy = Instantiate(enemies[enemiesClass], randPos, Quaternion.identity);
                newEnemy.gameObject.tag = "Enemy";
                spawnerSound.Play();
                yield return new WaitForSeconds(timePerEnemieSpawn);
            }
            
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.4f);
        Gizmos.DrawCube(transform.position, new Vector3(boxCollider.size.x, boxCollider.size.y, boxCollider.size.z));
    }
}
