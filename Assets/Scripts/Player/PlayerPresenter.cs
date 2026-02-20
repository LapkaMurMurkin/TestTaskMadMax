using System;

using DG.Tweening;

using TestTaskMadMax.GameState;
using TestTaskMadMax.ScriptableObjects;

using UnityEngine;
using UnityEngine.InputSystem;

using static ActionMap;

namespace TestTaskMadMax.Player
{
    public class PlayerPresenter : IDisposable
    {
        private GameModel _gameModel;
        private GameSettings _gameSettings;

        private PlayeInputActions _playerInput;
        private Transform _playerCar;
        private MainCamera _mainCamera;

        private float _holdTime;

        private bool _isJumping;

        private Tween _moveXAnim;
        private Sequence _jumpAnim;

        public Action OnHit;
        public event Action OnJump;
        public event Action OnJumpLand;

        public PlayerPresenter(GameModel gameModel, GameSettings gameSettings, PlayeInputActions playerInput, Transform playerCar, MainCamera mainCamera)
        {
            _gameModel = gameModel;
            _gameSettings = gameSettings;
            _playerInput = playerInput;
            _playerCar = playerCar;
            _mainCamera = mainCamera;

            _playerCar.transform.position = Vector3.up * _gameSettings.PlatformHeight;

            StartHorizontalMovementLoop();
            Subscribe();
        }

        public void Restart()
        {
            _jumpAnim?.Kill();
            _moveXAnim.Kill();

            _playerCar.transform.position = Vector3.up * _gameSettings.PlatformHeight;
            _isJumping = false;
            StartHorizontalMovementLoop();
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

            _jumpAnim?.Kill();
            _moveXAnim.Kill();
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
            float left = _gameSettings.HorizontalMovementBounds.x;
            float right = _gameSettings.HorizontalMovementBounds.y;
            float duration = _gameSettings.HorizontalMovementDuration / 2f;
            float rotateDuration = 0.4f; // время поворота Y
            float zTilt = -30f; // наклон при повороте

            // Начальная позиция и поворот
            _playerCar.position = new Vector3(left, _playerCar.position.y, _playerCar.position.z);
            _playerCar.rotation = Quaternion.Euler(0, 0, 0);

            _moveXAnim?.Kill();

            Sequence _moveSequence = DOTween.Sequence();

            // --- движение вправо ---
            var moveRight = _playerCar.DOMoveX(right, duration).SetEase(Ease.InOutSine);
            var rotateRight = _playerCar.DORotate(new Vector3(0, 180, 0), rotateDuration);
            var tiltRightUp = _playerCar.DOLocalRotate(new Vector3(0, 180, zTilt), rotateDuration);
            var tiltRightDown = _playerCar.DOLocalRotate(new Vector3(0, 180, 0), duration - rotateDuration).SetEase(Ease.OutSine);

            _moveSequence.Append(moveRight);
            _moveSequence.Join(rotateRight);
            _moveSequence.Join(tiltRightUp);
            _moveSequence.Insert(rotateDuration, tiltRightDown);

            // --- движение влево ---
            var moveLeft = _playerCar.DOMoveX(left, duration).SetEase(Ease.InOutSine);
            var rotateLeft = _playerCar.DORotate(new Vector3(0, 0, 0), rotateDuration);
            var tiltLeftUp = _playerCar.DOLocalRotate(new Vector3(0, 0, zTilt), rotateDuration);
            var tiltLeftDown = _playerCar.DOLocalRotate(new Vector3(0, 0, 0), duration - rotateDuration).SetEase(Ease.OutSine);

            _moveSequence.Append(moveLeft);
            _moveSequence.Join(rotateLeft);
            _moveSequence.Join(tiltLeftUp);
            _moveSequence.Insert(duration + rotateDuration, tiltLeftDown);

            _moveSequence.SetLoops(-1, LoopType.Restart);
            _moveXAnim = _moveSequence;

            /*             float startPosition = _gameSettings.HorizontalMovementBounds.x;
                        float endPosition = _gameSettings.HorizontalMovementBounds.y;

                        _playerCar.position = new Vector3(startPosition, _playerCar.position.y, _playerCar.position.z);

                        _moveXAnim?.Kill();
                        _moveXAnim = _playerCar.DOMoveX(endPosition, _gameSettings.HorizontalMovementDuration / 2)
                            .SetEase(Ease.InOutSine)
                            .SetLoops(-1, LoopType.Yoyo); */
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
            float peak = _gameSettings.PlatformHeight + _gameSettings.JumpSmallPeak;
            float land = 0;
            float upDuration = _gameSettings.JumpSmallUpDuration;
            float fallDuration = _gameSettings.JumpSmallFallDuration;

            DOJump(_playerCar, peak, land, upDuration, fallDuration);
            Debug.Log("SmallJump");
        }

        private void BigJump()
        {
            float peak = _gameSettings.PlatformHeight + _gameSettings.JumpBigPeak;
            float land = _gameSettings.PlatformHeight;
            float upDuration = _gameSettings.JumpSmallUpDuration;
            float fallDuration = _gameSettings.JumpSmallFallDuration;

            DOJump(_playerCar, peak, land, upDuration, fallDuration, true);
            _mainCamera.MoveUp(_gameSettings.PlatformHeight);
            Debug.Log("BigJump");
        }

        private void DOJump(Transform transform, float peak, float land = 0, float upDuration = 0.3f, float fallDuration = 0.3f, bool invokeLand = false)
        {
            peak += transform.position.y;
            land += transform.position.y;

            _isJumping = true;

            _jumpAnim?.Kill();
            _jumpAnim = DOTween.Sequence()
            .Append(transform.DOMoveY(peak, upDuration).SetEase(Ease.OutCubic))
            .Append(transform.DOMoveY(land, fallDuration).SetEase(Ease.OutBounce))
            .OnComplete(() =>
            {
                _isJumping = false;
                if (invokeLand) OnJumpLand?.Invoke();
            })
            .Play();

            OnJump?.Invoke();
        }
    }
}