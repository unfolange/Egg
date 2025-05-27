using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;
    public GameObject manager;
    private float spawnManage = 9.0f;
    public int enemyCount;
    public int waveNumber = 1;

    void Start()
    {
        SpawnEnemyWave(waveNumber);
    }
    // Update is called once per frame
    void Update()
    {
        if (!manager.GetComponent<GameManager>().isGameActive)
            return;
        enemyCount = FindObjectsByType<Enemy>(FindObjectsSortMode.None).Length;
        if (enemyCount == 0)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            Instantiate(powerupPrefab, GenerateSpawanPosition(), powerupPrefab.transform.rotation);
        }
    }
    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawanPosition(), enemyPrefab.transform.rotation);
        }
    }
    private Vector3 GenerateSpawanPosition()
    {
        float spawnPosX = Random.Range(-spawnManage, spawnManage);
        float spawnPosZ = Random.Range(-spawnManage, spawnManage);
        Vector3 randomPos = new Vector3(spawnPosX, 10, spawnPosZ);
        return randomPos;
    }
}
