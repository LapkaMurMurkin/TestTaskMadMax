using System;
using System.Collections.Generic;

using TestTaskMadMax.ScriptableObjects;

using UnityEngine;

namespace TestTaskMadMax
{
    public class PlatformPool : IDisposable
    {
        private GameSettings _gameSettings;
        private MainCamera _mainCamera;
        private GameObject _platformTemplate;


        private LinkedList<Transform> _platforms = new();
        public IReadOnlyCollection<Transform> Platforms => _platforms;

        public Action<Transform> PlatformRecycled;

        public PlatformPool(GameSettings gameSettings, MainCamera mainCamera, GameObject platformTemplate)
        {
            _gameSettings = gameSettings;
            _mainCamera = mainCamera;
            _platformTemplate = platformTemplate;

            FillScreen();
        }

        public void Restart()
        {
            float position = _gameSettings.PlatformHeight;
            foreach (Transform platform in _platforms)
            {
                platform.localPosition = new Vector3(0, position, 0);
                position += _gameSettings.PlatformHeight;
            }
        }

        private void FillScreen()
        {
            int platformsOnScreen = Mathf.CeilToInt(_mainCamera.ScreenHeight / _gameSettings.PlatformHeight) + 1;

            for (int i = 0; i < platformsOnScreen; i++)
            {
                GameObject newPlatform = GameObject.Instantiate(_platformTemplate, _platformTemplate.transform.parent);
                newPlatform.transform.localPosition = new Vector3(0, _gameSettings.PlatformHeight * (i + 1), 0);
                _platforms.AddLast(newPlatform.transform);
                newPlatform.SetActive(true);
            }
        }

        public void Update(float dt)
        {
            Transform bottomPlatform = _platforms.First.Value;
            if (bottomPlatform.position.y <= _mainCamera.ScreenBottom)
                BotPlatformToTop();
        }

        private void BotPlatformToTop()
        {
            Transform bottomPlatform = _platforms.First.Value;
            Transform topPlatform = _platforms.Last.Value;

            bottomPlatform.position = topPlatform.position + Vector3.up * _gameSettings.PlatformHeight;
            _platforms.RemoveFirst();
            _platforms.AddLast(bottomPlatform);

            PlatformRecycled?.Invoke(bottomPlatform);
        }

        public void Dispose()
        {
            for (int i = 0; i < _platforms.Count; i++)
            {
                if (_platforms.Last.Value.gameObject)
                    GameObject.Destroy(_platforms.Last.Value.gameObject);
                _platforms.RemoveLast();
            }
        }
    }
}