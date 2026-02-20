using System;

using DG.Tweening;

using TestTaskMadMax.ScriptableObjects;

using UnityEngine;

namespace TestTaskMadMax
{
    public class MainCamera : IDisposable
    {
        private Camera _camera;
        private GameSettings _gameSettings;

        public float ScreenHeight { get => _camera.orthographicSize * 2f; }
        public float ScreenBottom { get => _camera.transform.position.y - _camera.orthographicSize; }
        public float ScreenTop { get => _camera.transform.position.y + _camera.orthographicSize; }

        public MainCamera(GameSettings gameSettings, Camera camera)
        {
            _gameSettings = gameSettings;
            _camera = camera;

            float aspect = (float)Screen.width / Screen.height;
            _camera.orthographicSize = _gameSettings.SceneWidthUnits / (2f * aspect);

            _camera.transform.position = new Vector3(0, _camera.orthographicSize, -200);
        }

        public void Restart()
        {
            _camera.transform.DOKill();
            _camera.transform.position = new Vector3(0, _camera.orthographicSize, -200);
        }

        public void MoveUp(float yPos)
        {
            float targetY = _camera.transform.position.y + yPos;
            _camera.transform.DOKill();
            _camera.transform.DOMoveY(targetY, _gameSettings.JumpBigUpDuration + _gameSettings.JumpBigFallDuration)
                .SetEase(Ease.OutCubic);
        }

        public void Dispose()
        {
            _camera.DOKill();
        }
    }
}