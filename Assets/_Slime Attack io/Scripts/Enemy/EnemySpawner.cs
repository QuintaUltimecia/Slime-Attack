using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Enemy _enemyPrefab;

    private DecorContainer _decorContainer;
    private GameFeaturesModule _gameFeatures;
    private Camera _camera;
    private MainCanvas _canvas;

    private List<Transform> _points = new List<Transform>();
    private List<Enemy> _enemies = new List<Enemy>();

    private int _enemyCount;

    public System.Action OnRemoveEnemies;
    public System.Action<int> OnRemoveEnemy;

    public IEnumerable<Enemy> Enemies { get { return _enemies; } }

    public void Init(DecorContainer decorContainer, Camera camera, MainCanvas canvas, GameFeaturesModule gameFeatures)
    {
        _decorContainer = decorContainer;
        _camera = camera;
        _canvas = canvas;
        _gameFeatures = gameFeatures;

        for (int i = 0; i < transform.childCount; i++)
            _points.Add(transform.GetChild(i));
    }

    public void SpawnEnemies()
    {
        for (int i = 0; i < _points.Count; i++)
        {
            Enemy enemy = Instantiate(_enemyPrefab, _points[i]);
            _enemies.Add(enemy);
            enemy.Init(_decorContainer, _camera, _canvas, _gameFeatures);
            enemy.OnRemove += () => DisableEnemy();
            enemy.transform.localPosition = Vector3.zero;
            enemy.Colorized.RandomRecolor();

            enemy.EnemyAI.Disable();

            _decorContainer.AddDecor(enemy.Decor);
        }
    }

    public void EnableEnemies()
    {
        foreach (Enemy enemy in _enemies)
        {
            enemy.Enable();
            _enemyCount++;
        }
    }

    public void Restart()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.IsActive == true)
                enemy.Disable();
        }
    }

    public void StopEnemies()
    {
        foreach (var enemy in _enemies)
        {
            if (enemy.IsActive == true)
                enemy.EnemyAI.Disable();
        }
    }

    public void DisableEnemy()
    {
        _enemyCount--;

        OnRemoveEnemy?.Invoke(_enemyCount);

        if (_enemyCount == 0)
            OnRemoveEnemies?.Invoke();
    }
}
