using UnityEngine;
using System.Collections;

public class GameState
{
    public const int INITIAL_LIVES = 3;

    public int Lives {
        get {
            return _lives;
        }
        set {
            // Only adjust lives when the game is running; once it's
            // at zero, only a Reset() will unlock it
            if ( !GameOver ) {
                _lives = value;
                ValidateLives();
            }

        }
    }

    public int Score;
    public bool IsRunning;
    public bool GameOver;  // FIXME: is this and IsRunning redundant?
            
    int _lives = 0;

    public GameState()
    {
        Reset();
    }
        
    public void Reset()
    {
        Lives = INITIAL_LIVES;
        Score = 0;      
        IsRunning = false;
        GameOver = false;
    }

    public void ValidateLives()
    {
        if ( Lives == 0 ) {
            GameController.instance.OnGameOver();
        }
    }
}