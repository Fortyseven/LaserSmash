using UnityEngine;

// ReSharper disable InconsistentNaming

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
    public const int SCORE_THRESH_1X = 0;
    public const int SCORE_THRESH_2X = 1000;
    public const int SCORE_THRESH_3X = 5000;
    public const int SCORE_THRESH_4X = 20000;
    public const int SCORE_THRESH_5X = 50000;
    public const int SCORE_THRESH_6X = 100000;

    private int _score;
    private int _peak_score;
    private int _last_life_peak_score;
    private int _mult;

    public enum GameMode
    {
        RUNNING,
        PAUSED,
        POSTDEATH,
        GAMEOVER
    }

    public GameMode _mode;

    int _lives;

    #region properties
    /***********************************/
    public int Lives
    {
        get
        {
            return _lives;
        }
        set
        {
            // Only adjust lives when the game is running; once it's
            // at zero, only a Reset() will unlock it
            if ( Mode == GameMode.GAMEOVER )
                return;

            _lives = value;
            GameController.instance.SetLivesValue( value );
        }
    }

    /***********************************/
    public GameMode Mode
    {
        get
        {
            return _mode;
        }
        set
        {
            _mode = value;
        }
    }

    /***********************************/
    public int Score
    {
        get
        {
            return _score;
        }
        set
        {
            _score = value;
            GameController.instance.SetScoreValue( value );
            ValidateMultplier();
        }
    }

    /***********************************/
    public int PeakScore
    {
        get
        {
            return _peak_score;
        }
    }

    /***********************************/
    public int Multiplier
    {
        get
        {
            return _mult;
        }
        set
        {
            _mult = value;
            GameController.instance.SetMultValue( _mult );
        }
    }

    /***********************************/
    GameMode _paused_previous_mode = GameMode.RUNNING;

    public bool Paused
    {
        get
        {
            return Mode == GameMode.PAUSED;
        }
        set
        {
            if ( value == true ) {
                _paused_previous_mode = Mode;
                Mode = GameMode.PAUSED;
            }
            else {
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
        if ( Mode == GameMode.GAMEOVER )
            return;

        Score += score_offset * Multiplier;

        if ( Score > _peak_score ) {
            _peak_score = Score;
            if ( _peak_score - _last_life_peak_score > 1000 ) {
                _last_life_peak_score = _peak_score;
                Lives++;
                //TODO: Add 1UP noise
            }
        }
    }

    /***************************************************************************/
    private void ValidateMultplier()
    {
        int cur_mult = Multiplier;

        if ( Score < SCORE_THRESH_2X ) {
            Multiplier = 1;
        }
        else if ( Score < SCORE_THRESH_3X ) {
            Multiplier = 2;
        }
        else if ( Score < SCORE_THRESH_4X ) {
            Multiplier = 3;
        }
        else if ( Score < SCORE_THRESH_5X ) {
            Multiplier = 4;
        }
        else if ( Score < SCORE_THRESH_6X ) {
            Multiplier = 5;
        }
        else {
            Multiplier = 6;
        }

        if ( cur_mult != Multiplier )
            OnMultChange();
    }

    /***************************************************************************/
    private void OnMultChange()
    {
        //switch ( Multiplier ) {
        //    case 2: // Blue
        //        GameController.instance.DoLevelTransition( new Color( 0, 0, 1.0f, 1.0f ) );
        //        break;
        //    case 3: // Purple
        //        GameController.instance.DoLevelTransition( new Color( 1.0f, 0, 1.0f, 1.0f ) );
        //        break;
        //    case 4: // Cyan
        //        GameController.instance.DoLevelTransition( new Color( 0, 1.0f, 1.0f, 1.0f ) );
        //        break;
        //    case 5: // Gray
        //        GameController.instance.DoLevelTransition( new Color( 1.0f, 0.75f, 0, 1.0f ) );
        //        break;
        //    default: // "Black"
        //        GameController.instance.DoLevelTransition( new Color( 0.5f, 0.5f, 0.5f, 1.0f ) );
        //        break;
        //}
    }
}