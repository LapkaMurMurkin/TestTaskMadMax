using TestTaskMadMax.GameState;
using TestTaskMadMax.Player;
using TestTaskMadMax.ScriptableObjects;
using TestTaskMadMax.UI;

using UnityEngine;

namespace TestTaskMadMax
{
    public class GameEntryPoint : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GameSettings _gameSettings;

        [Header("Refs")]
        [SerializeField] private Camera _mainCameraRef;
        [SerializeField] private Transform _playerObject;
        [SerializeField] private GameObject _platformTemplate;
        [SerializeField] private Enemy _enemyTemplate;

        [Header("View")]
        [SerializeField] private PlayerView _playerView;

        [Header("UI")]
        [SerializeField] private MainScreen _mainScreen;
        [SerializeField] private LooseScreen _looseScreen;


        private GameModel _gameModel;
        private ActionMap _actionMap;
        private PlayerPresenter _player;
        private PlatformPool _platformPool;
        private MainCamera _playerCamera;
        private EnemyPool _enemyPool;
        private ScoreService _scoreService;

        private void Awake()
        {
            InitializeGame();
            InitializeView();
            InitializeUI();
        }

        private void RestartGame()
        {
            _playerCamera.Restart();
            _player.Restart();
            _platformPool.Restart();
            _enemyPool.Restart();
            _scoreService.Restart();

            _mainScreen.Restart();
        }

        private void InitializeGame()
        {
            _gameModel = new GameModel();
            _actionMap = new ActionMap();
            _actionMap.Enable();
            _playerCamera = new MainCamera(_gameSettings, _mainCameraRef);
            _player = new PlayerPresenter(_gameModel, _gameSettings, _actionMap.PlayeInput, _playerObject, _playerCamera);
            _platformPool = new PlatformPool(_gameSettings, _playerCamera, _platformTemplate);
            _enemyPool = new EnemyPool(_gameSettings, _platformPool, _enemyTemplate);
            _scoreService = new ScoreService(_gameModel, _player);
            Debug.Log("GameEntryPoint");
        }

        private void InitializeView()
        {
            _playerView.Initialize(_player);
        }

        private void InitializeUI()
        {
            _mainScreen.Initialize(_gameModel);
            _looseScreen.Initialize(_gameModel, _actionMap, RestartGame, _player);
        }

        private void Update()
        {
            float dt = Time.deltaTime;
            _player.Update(dt);
            _platformPool.Update(dt);

            _mainScreen.UpdateUI(dt);
        }

        private void DisposeGame()
        {
            _actionMap.Disable();
            _playerCamera.Dispose();
            _player.Dispose();
            _platformPool.Dispose();
            _enemyPool.Dispose();
        }

        private void DisposeUI()
        {
            _looseScreen.Dispose();
        }

        private void OnDestroy()
        {
            DisposeUI();
            DisposeGame();
        }
    }
}