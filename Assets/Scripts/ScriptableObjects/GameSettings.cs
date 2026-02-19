using UnityEngine;

namespace TestTaskMadMax.ScriptableObjects
{
    [CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects")]
    public class GameSettings : ScriptableObject
    {
        [field: SerializeField] public int PlatformHeight { get; private set; } = 200;
        [field: SerializeField] public Vector2 HorizontalMovementBounds { get; private set; } = new Vector2(-200, 200);
        [field: SerializeField] public float HorizontalMovementDuration { get; private set; } = 3;
        [field: SerializeField] public float JumpBigPeak { get; private set; } = 50F;
        [field: SerializeField] public float JumpSmallPeak { get; private set; } = -50f;
        [field: SerializeField] public float JumpUpDuration { get; private set; } = 0.3f;
        [field: SerializeField] public float JumpFallDuration { get; private set; } = 0.4f;
        [field: SerializeField] public float JumpHoldThreshold { get; private set; } = 0.5f;

    }
}