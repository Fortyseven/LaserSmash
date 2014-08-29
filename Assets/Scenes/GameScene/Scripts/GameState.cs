using UnityEngine;
using System.Collections;

public class GameState
{
    public const int INITIAL_LIVES = 3;
    public int Lives;
    public int Score;
    public bool IsRunning;
        
    public GameState()
    {
        Reset();
    }
        
    public void Reset()
    {
        Lives = INITIAL_LIVES;
        Score = 0;      
        IsRunning = false;
    }
}