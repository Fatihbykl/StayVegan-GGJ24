using System.Collections;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject player;
    public BoxCollider gameBounds;
    public Wave[] waves;

    [HideInInspector] public int currentWaveIndex = 0;
    private bool readyToCountDown;
    private float countdown;
    
    private void Start()
    {
        readyToCountDown = true;

        for (int i = 0; i < waves.Length; i++)
        {
            waves[i].enemiesLeft = waves[i].enemies.Length;
        }
    }
    private void Update()
    {
        if (currentWaveIndex >= waves.Length)
        {
            GameManager.instance.OpenYouWonScreen();
            return;
        }

        if (readyToCountDown)
        {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0)
        {
            readyToCountDown = false;

            countdown = waves[currentWaveIndex].timeToNextWave;

            StartCoroutine(SpawnWave());
        }

        if (waves[currentWaveIndex].enemiesLeft == 0)
        {
            readyToCountDown = true;
            GameManager.instance.OpenEndWavePage();
            currentWaveIndex++;
        }
    }
    private IEnumerator SpawnWave()
    {
        Bounds bounds = gameBounds.bounds;
        if (currentWaveIndex < waves.Length)
        {
            for (int i = 0; i < waves.ElementAtOrDefault(currentWaveIndex)?.enemies.Length; i++)
            {
                float offsetX = Random.Range(-bounds.extents.x, bounds.extents.x);
                float offsetY = Random.Range(-bounds.extents.y, bounds.extents.y);
                float offsetZ = Random.Range(-bounds.extents.z, bounds.extents.z);
                //var randomPosition = new Vector3(Random.Range(-19, 20), 1f, Random.Range(-45, 46));
                var enemy = Instantiate(waves[currentWaveIndex].enemies[i]);
                enemy.transform.position = bounds.center + new Vector3(offsetX, offsetY, offsetZ);
                enemy.transform.parent = this.gameObject.transform;
                
                yield return new WaitForSeconds(waves[currentWaveIndex].timeToNextEnemy);
            }
        }
    }
}

[System.Serializable]
public class Wave
{
    public GameObject[] enemies;
    public float timeToNextEnemy;
    public float timeToNextWave;

    [HideInInspector] public int enemiesLeft;
}