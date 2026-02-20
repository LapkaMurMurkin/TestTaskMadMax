using System;

using DG.Tweening;

using UnityEngine;

namespace TestTaskMadMax
{
    public class Enemy : MonoBehaviour
    {
        public void Initialize(Vector2 horizontalMovementBounds, float loopTime)
        {
            this.transform.DOKill();

            float startPosition = horizontalMovementBounds.x;
            float endPosition = horizontalMovementBounds.y;

            this.transform.position = new Vector3(startPosition, this.transform.position.y, this.transform.position.z);
            this.transform.DOLocalMoveX(endPosition, loopTime / 2)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }
}