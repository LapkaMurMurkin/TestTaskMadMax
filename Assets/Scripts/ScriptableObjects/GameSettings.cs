using UnityEngine;

namespace TestTaskMadMax.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects")]
    public class GameSettings : ScriptableObject
    {
        [Header("Сфьукф")]
        [SerializeField] private float _sceneWidthUnits = 10f;

        [Header("Platform")]
        [SerializeField] private int _platformHeight = 200;

        [Header("Enemies")]
        [SerializeField] private float _enemyStartSpeed = 2f;
        [SerializeField] private float _enemyMaxSpeed = 6f;
        [SerializeField] private int _platformToReachMaxSpeed = 20;
        [Range(0f, 1f)]
        [SerializeField] private float _enemySpawnChance = 0.5f;

        [Header("Player Horizontal Movement")]
        [SerializeField] private Vector2 _horizontalMovementBounds = new(-500, 500);
        [SerializeField] private float _horizontalMovementDuration = 3f;

        [Header("Big Jump")]
        [SerializeField] private float _jumpBigPeak = 50f;
        [SerializeField] private float _jumpBigUpDuration = 0.3f;
        [SerializeField] private float _jumpBigFallDuration = 0.4f;

        [Header("Small Jump")]
        [SerializeField] private float _jumpSmallPeak = -75f;
        [SerializeField] private float _jumpSmallUpDuration = 0.2f;
        [SerializeField] private float _jumpSmallFallDuration = 0.2f;

        [Header("Input")]
        [SerializeField] private float _jumpHoldThreshold = 0.2f;

        public float SceneWidthUnits => _sceneWidthUnits;

        public int PlatformHeight => _platformHeight;

        public float EnemyStartSpeed => _enemyStartSpeed;
        public float EnemyMaxSpeed => _enemyMaxSpeed;
        public int PlatformToReachMaxSpeed => _platformToReachMaxSpeed;
        public float EnemySpawnChance => _enemySpawnChance;

        public Vector2 HorizontalMovementBounds => _horizontalMovementBounds;
        public float HorizontalMovementDuration => _horizontalMovementDuration;

        public float JumpBigPeak => _jumpBigPeak;
        public float JumpBigUpDuration => _jumpBigUpDuration;
        public float JumpBigFallDuration => _jumpBigFallDuration;

        public float JumpSmallPeak => _jumpSmallPeak;
        public float JumpSmallUpDuration => _jumpSmallUpDuration;
        public float JumpSmallFallDuration => _jumpSmallFallDuration;

        public float JumpHoldThreshold => _jumpHoldThreshold;

    }
}