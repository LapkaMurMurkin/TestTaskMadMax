using System;

using TestTaskMadMax.GameState;
using TestTaskMadMax.Player;

namespace TestTaskMadMax
{
    public class ScoreService : IDisposable
    {
        private readonly GameModel _model;
        private readonly PlayerPresenter _player;

        public ScoreService(GameModel model, PlayerPresenter player)
        {
            _model = model;
            _player = player;

            _player.OnJumpLand += OnSuccessfulLand;
        }

        public void Restart()
        {
            _model.CurrentScore = 0;
        }

        private void OnSuccessfulLand()
        {
            _model.CurrentScore++;

            if (_model.CurrentScore > _model.Record)
                _model.Record = _model.CurrentScore;
        }

        public void Dispose()
        {
            _player.OnJumpLand -= OnSuccessfulLand;
        }
    }
}