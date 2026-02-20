using System;
using System.Collections.Generic;
using System.Linq;

using TestTaskMadMax.ScriptableObjects;

using UnityEngine;

namespace TestTaskMadMax
{
    public class EnemyPool : IDisposable
    {
        private GameSettings _gameSettings;
        private PlatformPool _platformPool;
        private Enemy _enemyTemplate;

        private Dictionary<Transform, Enemy> _enemies = new();
        private int _platformCounter;

        public EnemyPool(GameSettings gameSettings, PlatformPool platformPool, Enemy enemyTemplate)
        {
            _gameSettings = gameSettings;
            _platformPool = platformPool;
            _enemyTemplate = enemyTemplate;

            FillPlatforms();
            Subscribe();
        }

        public void Restart()
        {
            _platformCounter = 0;

            foreach (Transform platform in _platformPool.Platforms)
                InitializeEnemy(platform);

            Transform firstPlatform = _platformPool.Platforms.First();
            _enemies[firstPlatform].gameObject.SetActive(false); // skip first platform
        }

        private void FillPlatforms()
        {
            foreach (Transform platform in _platformPool.Platforms)
            {
                Enemy newEnemy = SpawnEnemy(platform);
                InitializeEnemy(platform);
            }

            _enemies.First().Value.gameObject.SetActive(false); // skip first platform
        }

        private void Subscribe()
        {
            _platformPool.PlatformRecycled += InitializeEnemy;
            //_platformPool.FreePlatform += AddEnemyToPlatform;
        }

        private void Unsubscribe()
        {
            _platformPool.PlatformRecycled -= InitializeEnemy;
        }

        public void Dispose()
        {
            Unsubscribe();
            foreach (Enemy enemy in _enemies.Values)
                GameObject.Destroy(enemy.gameObject);

            _enemies.Clear();
        }

        private Enemy SpawnEnemy(Transform platform)
        {
            Enemy enemy = MonoBehaviour.Instantiate(_enemyTemplate, _enemyTemplate.transform.parent);
            enemy.transform.position = platform.position;
            _enemies.Add(platform, enemy);
            return enemy;
        }

        private void InitializeEnemy(Transform platform)
        {
            _platformCounter++;

            Enemy enemy = _enemies[platform];

            if (UnityEngine.Random.value > _gameSettings.EnemySpawnChance)
            {
                enemy.gameObject.SetActive(false);
                return;
            }

            enemy.gameObject.SetActive(true);

            float speed = CalculateEnemySpeed();

            enemy.transform.position = platform.position;
            enemy.Initialize(_gameSettings.HorizontalMovementBounds, speed);
        }

        private float CalculateEnemySpeed()
        {
            float t = Mathf.Clamp01((float)_platformCounter / _gameSettings.PlatformToReachMaxSpeed);

            return Mathf.Lerp(
                _gameSettings.EnemyStartSpeed,
                _gameSettings.EnemyMaxSpeed,
                t);
        }
    }
}