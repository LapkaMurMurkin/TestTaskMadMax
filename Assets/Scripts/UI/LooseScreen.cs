using System;

using TestTaskMadMax.GameState;
using TestTaskMadMax.Player;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace TestTaskMadMax.UI
{
    public class LooseScreen : MonoBehaviour, IDisposable
    {
        [SerializeField] private TextMeshProUGUI _recordText;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Button _restartButton;

        private GameModel _gameModel;
        private ActionMap _actionMap;
        private PlayerPresenter _playerPresenter;

        private Action _restartGameAction;

        public void Initialize(GameModel gameModel, ActionMap actionMap, Action restartGameAction, PlayerPresenter playerPresenter)
        {
            _gameModel = gameModel;
            _actionMap = actionMap;
            _restartGameAction = restartGameAction;
            _playerPresenter = playerPresenter;

            Subscribe();
        }

        private void Subscribe()
        {
            _restartButton.onClick.AddListener(RestartGame);

            _playerPresenter.OnHit += Show;
        }

        private void Unsubsribe()
        {
            _restartButton.onClick.RemoveAllListeners();

            _playerPresenter.OnHit -= Show;
        }

        public void Dispose()
        {
            Unsubsribe();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void RestartGame()
        {
            Hide();
            _restartGameAction.Invoke();
        }

        private void Show()
        {
            _actionMap.Disable();
            _recordText.text = _gameModel.Record.ToString();
            _scoreText.text = _gameModel.CurrentScore.ToString();
            this.gameObject.SetActive(true);
        }

        private void Hide()
        {
            _actionMap.Enable();
            this.gameObject.SetActive(false);
        }
    }
}