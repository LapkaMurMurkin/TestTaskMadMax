using System;

using TestTaskMadMax.GameState;

using TMPro;

using UnityEngine;

namespace TestTaskMadMax.UI
{
    public class MainScreen : MonoBehaviour, IDisposable
    {
        [SerializeField] private TextMeshProUGUI _currentScore;

        private GameModel _gameModel;

        public void Initialize(GameModel gameModel)
        {
            _gameModel = gameModel;
            Subscribe();
        }

        public void Restart()
        {
            _currentScore.text = $"Score: 0";
        }

        private void Subscribe()
        {

        }

        private void Unsubsribe()
        {

        }

        public void Dispose()
        {
            Unsubsribe();
        }

        public void UpdateUI(float dt)
        {
            _currentScore.text = $"Score: {_gameModel.CurrentScore}";
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}