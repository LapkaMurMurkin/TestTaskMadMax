using TestTaskMadMax.Player;
using TestTaskMadMax.ScriptableObjects;

using UnityEngine;

namespace TestTaskMadMax
{
    public class GameEntryPoint : MonoBehaviour
    {
        [SerializeField] private GameSettings _gameSettings;

        [SerializeField] private Camera _playerCamera;
        [SerializeField] private Transform _playerCar;
        [SerializeField] private GameObject _platformTemplate;

        private ActionMap _actionMap;
        private PlayerPresenter _player;
        private PlatformPool _platformPool;

        private void Awake()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            _actionMap = new ActionMap();
            _actionMap.Enable();
            _player = new PlayerPresenter(_gameSettings, _actionMap.PlayeInput, _playerCar);
            _platformPool = new PlatformPool(_gameSettings, _platformTemplate, _playerCamera);
            Debug.Log("GameEntryPoint");
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            _player.Update(dt);
        }
    }
}