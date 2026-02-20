using UnityEngine;

namespace TestTaskMadMax.Player
{
    public class PlayerView : MonoBehaviour
    {
        private PlayerPresenter _playerPresenter;

        public void Initialize(PlayerPresenter playerPresenter)
        {
            _playerPresenter = playerPresenter;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _playerPresenter.OnHit?.Invoke();
            Debug.Log("Player hit enemy");
        }
    }
}