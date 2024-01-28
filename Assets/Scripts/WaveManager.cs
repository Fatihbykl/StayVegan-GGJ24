using System.Collections;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject player;
    public bool readyToCountDown;
    public Wave[] waves;
    public int currentWaveIndex = 0;

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
        if (currentWaveIndex < waves.Length)
        {
            for (int i = 0; i < waves.ElementAtOrDefault(currentWaveIndex)?.enemies.Length; i++)
            {
                var randomPosition = new Vector3(Random.Range(-19, 20), 1f, Random.Range(-45, 46));
                var enemy = Instantiate(waves[currentWaveIndex].enemies[i], randomPosition, Quaternion.identity);
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