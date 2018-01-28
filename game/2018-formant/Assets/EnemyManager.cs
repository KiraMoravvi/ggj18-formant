using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyTypes
{
    EnemyFaller,
    EnemySideways,
    EnemyFollower,
    EnemyCount
};

public class EnemyManager : MonoBehaviour {

    public float nextEnemySpawnTime;
    public float spawnDecreaseRate;
    public float minSpawnTime;

    public int enemyDecreaseCount;
    public int maxEnemies;

    float spawnTimer = 0.0f;
    int spawnCount = 0;

    public List<Mesh> meshList;

    public GameObject Instance;
    List<GameObject> enemies = new List<GameObject>();

    public List<AudioClip> EnemyAmbienceAudioClips = new List<AudioClip>();
    public List<AudioClip> EnemyDeathAudioClips = new List<AudioClip>();

	// Update is called once per frame
	void FixedUpdate ()
    {
        spawnTimer += Time.fixedDeltaTime;
        if (spawnTimer > nextEnemySpawnTime)
        {            
            if (enemies.Count < maxEnemies)
            {
                spawnEnemy();

                spawnCount++;
                if (spawnCount >= enemyDecreaseCount)
                {
                    nextEnemySpawnTime -= spawnDecreaseRate;
                    if (nextEnemySpawnTime < minSpawnTime)
                        nextEnemySpawnTime = minSpawnTime;
                    spawnCount = 0;
                }
            }

            spawnTimer = 0;
        }

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] == null)
            {
                enemies.RemoveAt(i);
                i--;
            }
        }
    }

    void spawnEnemy()
    {
        GameObject go = Instantiate(Instance);
        
        int type = Random.Range(0, (int)EnemyTypes.EnemyCount);
        switch ((EnemyTypes)type)
        {
            case EnemyTypes.EnemyFaller:
                Faller fall = go.AddComponent<Faller>();
                break;
            case EnemyTypes.EnemyFollower:
                Follower follow = go.AddComponent<Follower>();
                break;
            case EnemyTypes.EnemySideways:
                Sideways side = go.AddComponent<Sideways>();
                break;
        }

        MeshFilter mf = go.GetComponent<MeshFilter>();
        mf.mesh = Instantiate(meshList[type]);

        Bounds b = mf.mesh.bounds;

        BoxCollider bc = go.GetComponent<BoxCollider>();
        bc.size = b.size;
        bc.center = b.center;

        var audioSource = go.GetComponent<AudioSource>();
        audioSource.clip = EnemyAmbienceAudioClips[type];
        audioSource.Play();

        go.GetComponent<Enemy>().DeathAudioClip = EnemyDeathAudioClips[type];

        enemies.Add(go);
    }
}