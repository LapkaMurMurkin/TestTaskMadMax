using TestTaskMadMax.Player;

using UnityEngine;

namespace TestTaskMadMax
{
    public class Sound : MonoBehaviour
    {
        public AudioClip GasSound;

        [SerializeField] private AudioSource _audioSource;


        private PlayerPresenter _playerPresenter;

        public void Initialize(PlayerPresenter playerPresenter)
        {
            _playerPresenter = playerPresenter;
            _playerPresenter.OnJump += PlayGas;
        }

        private void PlayGas()
        {
            _audioSource.PlayOneShot(GasSound);
        }

        private void OnDestroy()
        {
            _playerPresenter.OnJump -= PlayGas;
        }
    }
}