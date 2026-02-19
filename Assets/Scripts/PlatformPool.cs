using System.Collections.Generic;

using TestTaskMadMax.ScriptableObjects;

using UnityEngine;

namespace TestTaskMadMax
{
    public class PlatformPool
    {
        private GameSettings _gameSettings;

        private GameObject _platformTemplate;

        private Queue<GameObject> _platforms = new();
        private Transform _lastPlatform;
        private Camera _playerCamera;

        public PlatformPool(GameSettings gameSettings, GameObject platformTemplate, Camera playerCamera)
        {
            _gameSettings = gameSettings;
            _platformTemplate = platformTemplate;
            _playerCamera = playerCamera;

            FillScreen();
        }

        private void FillScreen()
        {
            float screenHeight = _playerCamera.orthographicSize * 2f;
            int platformsOnScreen = Mathf.CeilToInt(screenHeight / _gameSettings.PlatformHeight) + 1;

            for (int i = 0; i < platformsOnScreen; i++)
            {
                GameObject newPlatform = GameObject.Instantiate(_platformTemplate, _platformTemplate.transform.parent);
                newPlatform.transform.localPosition = new Vector3(0, _gameSettings.PlatformHeight * (i + 1), 0);
                _platforms.Enqueue(newPlatform);
                _lastPlatform = newPlatform.transform;
                newPlatform.SetActive(true);
            }
        }
    }
}