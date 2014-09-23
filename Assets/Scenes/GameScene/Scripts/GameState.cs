using UnityEngine;
using System.Collections;

public class GameState
{
    public const int INITIAL_LIVES = 3;
    public const int INITIAL_MULTIPLIER = 1;

/*
    1x  level : Black background : Score up to 999
    2x  level : Blue background : Score 1,000-4,999
    3x  level : Purple background : Score 5,000-19,999
    4x  level : Turquoise background : Score 20,000-49,999
    5x  level : Gray background : Score 50,000-99,999
    6x  level : Black background : Score 100,000 and over
*/
    private const int THRESH_LEVEL_1 = 1000;
    private const int THRESH_LEVEL_2 = 5000;
    private const int THRESH_LEVEL_3 = 20000;
    private const int THRESH_LEVEL_4 = 50000;
    private const int THRESH_LEVEL_5 = 100000;

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
            AdjustMultplier();
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
            OnMultChange();
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
    public void AdjustScore( int score_offset )
    {
        if (Mode == GameMode.GAMEOVER) return;

        Score += score_offset * Multiplier;

        if (Score > _peak_score) {
            _peak_score = Score;
        }
    }

    private void AdjustMultplier()
    {
        if (Score < THRESH_LEVEL_1) {
            Multiplier = 1;
        } 
        else if (Score < THRESH_LEVEL_2) {
            Multiplier = 2;
        }
        else if (Score < THRESH_LEVEL_3) {
            Multiplier = 3;
        }
        else if (Score < THRESH_LEVEL_4) {
            Multiplier = 4;
        }
        else if (Score < THRESH_LEVEL_5) {
            Multiplier = 5;
        }
        else {
            Multiplier = 6;
        }
    }


    private void OnMultChange()
    {

    }
 }