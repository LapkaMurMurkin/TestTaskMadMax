using System;

using DG.Tweening;

using TestTaskMadMax.ScriptableObjects;

using UnityEngine;
using UnityEngine.InputSystem;

using static ActionMap;

namespace TestTaskMadMax.Player
{
    public class PlayerPresenter : IDisposable
    {
        private GameSettings _gameSettings;

        private PlayeInputActions _playerInput;
        private Transform _playerCar;

        private float _holdTime;

        private bool _isJumping;

        public PlayerPresenter(GameSettings gameSettings, PlayeInputActions playerInput, Transform playerCar)
        {
            _gameSettings = gameSettings;
            _playerInput = playerInput;
            _playerCar = playerCar;

            _playerCar.transform.position = Vector3.up * gameSettings.PlatformHeight;

            StartHorizontalMovementLoop();
            Subscribe();
        }

        private void Subscribe()
        {
            _playerInput.Jump.started += SaveClickTime;
            _playerInput.Jump.canceled += Jump;
        }

        private void Unsubscribe()
        {
            _playerInput.Jump.started += SaveClickTime;
            _playerInput.Jump.canceled -= Jump;
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        public void Update(float dt)
        {

        }

        private void SaveClickTime(InputAction.CallbackContext context)
        {
            _holdTime = Time.time;
        }

        private void StartHorizontalMovementLoop()
        {
            float startPosition = _gameSettings.HorizontalMovementBounds.x;
            float endPosition = _gameSettings.HorizontalMovementBounds.y;

            _playerCar.transform.position = new Vector3(startPosition, _playerCar.transform.position.y, _playerCar.transform.position.z);
            _playerCar.DOLocalMoveX(endPosition, _gameSettings.HorizontalMovementDuration / 2)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (_isJumping)
                return;

            if (Time.time - _holdTime > _gameSettings.JumpHoldThreshold)
                BigJump();
            else
                SmallJump();
        }

        private void SmallJump()
        {
            DOJump(_playerCar, _gameSettings.PlatformHeight + _gameSettings.JumpSmallPeak);
            Debug.Log("SmallJump");
        }

        private void BigJump()
        {
            DOJump(_playerCar, _gameSettings.PlatformHeight + _gameSettings.JumpBigPeak, _gameSettings.PlatformHeight);
            Debug.Log("BigJump");
        }

        private void DOJump(Transform transform, float peak, float land = 0)
        {
            peak += transform.localPosition.y;
            land += transform.localPosition.y;

            _isJumping = true;

            DOTween.Sequence()
            .Append(transform.DOLocalMoveY(peak, _gameSettings.JumpUpDuration).SetEase(Ease.OutCubic))
            .Append(transform.DOLocalMoveY(land, _gameSettings.JumpFallDuration).SetEase(Ease.OutBounce))
            .OnComplete(() => _isJumping = false)
            .Play();
        }
    }
}