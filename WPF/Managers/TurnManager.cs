using System;
using System.Timers;
using SeaBattle.Services;

namespace SeaBattle.Managers;

public class TurnManager : IDisposable
{
    public event EventHandler TurnChanged;

    private readonly TurnService _turnService;
    private readonly Action _onTurnUpdated;
    private readonly int _gameId;
    private bool _lastTurnState;
    private System.Timers.Timer _turnCheckerTimer;

    public TurnManager(TurnService turnService, Action onTurnUpdated, int gameId)
    {
        _turnService = turnService;
        _onTurnUpdated = onTurnUpdated;
        _gameId = gameId;

        SetupTurnChecker();
    }


    private void SetupTurnChecker()
    {
        _turnCheckerTimer = new System.Timers.Timer
        {
            Interval = 1000, // 1 second interval
            AutoReset = true,
            Enabled = true
        };

        _turnCheckerTimer.Elapsed += TurnCheckerElapsed;
    }

    private void TurnCheckerElapsed(object sender, ElapsedEventArgs e)
    {
        bool isPlayerTurn = _turnService.IsPlayerTurn(_gameId);
        if (isPlayerTurn != _lastTurnState)
        {
            _lastTurnState = isPlayerTurn;
            OnTurnChanged();
        }
    }
    protected virtual void OnTurnChanged()
    {
        TurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool CheckTurnState() 
    {
        return _turnService.IsPlayerTurn(_gameId);
    }

    public void SwitchTurn()
    {
        _turnService.SwitchTurn(_gameId);
        _onTurnUpdated?.Invoke(); 
    }

    public void Dispose()
    {
        if (_turnCheckerTimer != null)
        {
            _turnCheckerTimer.Stop();
            _turnCheckerTimer.Elapsed -= TurnCheckerElapsed;
            _turnCheckerTimer.Dispose();
            _turnCheckerTimer = null;
        }
    }
}
