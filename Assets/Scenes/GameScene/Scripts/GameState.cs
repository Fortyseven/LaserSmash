using UnityEngine;
using System.Collections;

public class GameState
{
    public const int INITIAL_LIVES = 3;
    public const int INITIAL_MULTIPLIER = 1;


    private int _score;
    private int _peak_score;
    private int _mult;

    public enum GameMode {
        RUNNING,
        PAUSED,
        POSTDEATH,
        GAMEOVER
    }


    //public bool IsRunning;
//    public bool PostDeath;
//    public bool GameOver;  // FIXME: is this and IsRunning redundant?
        
    public GameMode _mode;

    int _lives = 0;
//    bool _is_paused = false;



#region properties
    public int Lives {
        get {
            return _lives;
        }
        set {
            // Only adjust lives when the game is running; once it's
            // at zero, only a Reset() will unlock it
            if (Mode == GameMode.GAMEOVER) return;

            _lives = value;
            GameController.instance.SetLivesValue(value);
        }
    }

    public GameMode Mode {
        get {
            return _mode;
        }
        set {
            _mode = value;
        }
    }

    public int Score {
        get {
            return _score;
        }
        set {
            _score = value;
            GameController.instance.SetScoreValue(value);
        }
    }

    public int PeakScore {
        get {
            return _peak_score;
        }
    }

    public int Multiplier {
        get {
            return _mult;
        }
        set {
            _mult = value;
            GameController.instance.SetMultValue(_mult);
        }
    }

    GameMode _paused_previous_mode = GameMode.RUNNING;

    public bool Paused {
        get {
            return Mode == GameMode.PAUSED;
        }
        set {
            if (value == true) {
                _paused_previous_mode = Mode;
                Mode = GameMode.PAUSED;
            } else {
                Mode = _paused_previous_mode;
            }

            Time.timeScale = Paused ? 0 : 1.0f;
//            IsRunning = !value;
        }
    }
#endregion

    /***************************************************************************/
    public GameState()
    {
//        Reset();
    }

    /***************************************************************************/
    public void Reset()
    {
        Paused = false;
        Mode = GameMode.RUNNING;
        Lives = INITIAL_LIVES;
        Score = 0;
        Multiplier = INITIAL_MULTIPLIER;
    }

    /***************************************************************************/
//    public void ValidateLives()
//    {
//        if ( Lives == 0 ) {
//            GameController.instance.OnGameOver();
//        }
//    }

    /***************************************************************************/
    public void AdjustScore( int score_offset )
    {
        if (Mode == GameMode.GAMEOVER) return;

        Score += score_offset * Multiplier;

        if (Score > _peak_score) {
            _peak_score = Score;
        }
        //CurrentDifficulty = UpdateDifficultyValues();
    }
 }