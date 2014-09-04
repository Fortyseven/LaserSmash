using UnityEngine;
using System.Collections;

public class GameState
{
    public const int INITIAL_LIVES = 3;


    private int _score;
    private int _peak_score;


    public bool IsRunning;
    public bool GameOver;  // FIXME: is this and IsRunning redundant?
            
    int _lives = 0;

#region properties
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

    public int Score {
        get {
            return _score;
        }
        set {
            _score = value;
            GameController.instance.SetScoreValue(value);
        }
    }
#endregion


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

    /***************************************************************************/
    public void AdjustScore( int score_offset )
    {
        Debug.Log("SCORE ADJ: " + score_offset);
        Score += score_offset;

        if (Score > _peak_score) {
            _peak_score = Score;
        }
        //CurrentDifficulty = UpdateDifficultyValues();
    }
}