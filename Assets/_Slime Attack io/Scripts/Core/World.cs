using UnityEngine;

public class World : MonoBehaviour
{
    [field: SerializeField]
    public DecorContainer DecorContainer { get; private set; }

    [field: SerializeField]
    public BoostContainer BoostContainer { get; private set; }

    [field: SerializeField]
    public Transform PlayerSpawn { get; private set; }

    [field: SerializeField]
    public EnemySpawner EnemySpawner { get; private set; }

    private Transform[] _playerSpawnPoints;

    public void Init(GameFeaturesModule gameFeatures)
    {
        DecorContainer.Init(gameFeatures);
        BoostContainer.Init(gameFeatures);

        _playerSpawnPoints = new Transform[PlayerSpawn.childCount];
        for (int i = 0; i < PlayerSpawn.childCount; i++)
            _playerSpawnPoints[i] = PlayerSpawn.GetChild(i);
    }

    public Vector3 GetPlayerSpawnPoint()
    {
        int random = Random.Range(0, _playerSpawnPoints.Length);

        return _playerSpawnPoints[random].position;
    }
}
