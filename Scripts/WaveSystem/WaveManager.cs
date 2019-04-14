using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Wave[] waves;
    private static Wave[] Waves { get; set; }
    private static int CurrentWaveIndex { get; set; }
    public static float? Timer { get; private set; }
    public static Status? WaveStatus { get; private set; }

    private void Start()
    {
        Waves = waves;
        WaveStatus = Status.Incomplete;
        CurrentWaveIndex = 0;
        Timer = null;

        //EventManager.TriggerEvent("OnLevelReady");
    }

    private void OnEnable()
    {
        EventManager.StartListening("OnLevelReady", LoadWave);
        EventManager.StartListening("OnEnemyDestroyed", OnEnemyDestroyed);
    }

    private void OnDisable()
    {
        EventManager.StopListening("OnLevelReady", LoadWave);
        EventManager.StopListening("OnEnemyDestroyed", OnEnemyDestroyed);
    }

    private void Update()
    {
        if (Timer != null)
        {
            Timer -= Time.deltaTime;

            if (Timer <= 0)
            {
                EndWave();

                switch (WaveStatus)
                {
                    case Status.Incomplete:
                        Debug.Log("Failed");
                        LoadWave(CurrentWaveIndex);
                        break;
                    case Status.Complete:
                        Debug.Log("Success");
                        LoadWave(++CurrentWaveIndex);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void OnEnemyDestroyed()
    {
        if (Enemy.Count <= 0)
        {
            WaveStatus = Status.Complete;
            Debug.Log("Next wave in 3 seconds!");
            Invoke("LoadNextWave", 3.0f);
        }
    }

    private void EndWave()
    {
        EventManager.StopListening("OnEnemyDestroyed", OnEnemyDestroyed);
        EventManager.TriggerEvent("OnSilentDestroyAllEnemies");
    }

    private void LoadWave(int index)
    {
        EventManager.StartListening("OnEnemyDestroyed", OnEnemyDestroyed);
        Timer = null;
        WaveStatus = Status.Incomplete;
        Wave wave = Waves[CurrentWaveIndex];

        if (wave.IsTimed)
        {
            Timer = wave.TimeLimit;
        }

        int i = 0;
        foreach (Spawner spawner in wave.Spawners)
        {
            GameObject go = Instantiate(spawner.gameObject);
            go.transform.position = wave.SpawnPoints[i++].position;
            go.GetComponent<Spawner>().Play();
        }
    }

    private void LoadWave()
    {
        LoadWave(CurrentWaveIndex);
    }

    private void LoadNextWave()
    {
        LoadWave(++CurrentWaveIndex);
    }
}