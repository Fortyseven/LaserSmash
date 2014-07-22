using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour
{
    [System.Serializable]
    public class DifficultyValues {
        public Color AmbienceColor;
        public float ScoreThreshold; // This score on up

        public float NewEnemyFrequencyMin;
        public float NewEnemyFrequencyMax;
    }

    public DifficultyValues[]   Difficulty;
    public DifficultyValues     CurrentDifficulty;

    public dfLabel mGUIScoreValue = null;
    public dfLabel mGUILivesValue = null;
    
    public bool isMobileMode = false;
    
    private GameState _gameState = null;

    private WaveGenerator mWaveGenerator = null ;

    private DifficultyValues    _lastDifficulty = null;


    /***************************************************************************/
    public DifficultyValues GetCurrentDifficultyValues()
    {
        for(int i = Difficulty.Length - 1; i >= 0; i-- ) {
            DifficultyValues diff = ((DifficultyValues)(Difficulty[i]));
            if (_gameState.Score >= diff.ScoreThreshold) {
                return diff;
            }
        }
        return Difficulty[0];
    }

    /***************************************************************************/
    void Start()
    {
        ConfigureGame();
        
        mWaveGenerator = GetComponent<WaveGenerator>() as WaveGenerator;
        mWaveGenerator.init( this );
                
        _gameState = new GameState();
        _gameState.Reset();
        
        //GameObject.Find("Label 3").GetComponent<dfTapGesture>().TapGesture += HandlerTapTappyTap;        
    }
    
    /***************************************************************************/
    void Update()
    {
        mGUIScoreValue.Text = _gameState.Score.ToString();
    }
        
    /***************************************************************************/
    public GameState getGameState()
    {
        return _gameState;
    }
    
    /***************************************************************************/
    private void ConfigureGame()
    {
        if (!isMobileMode) {
            DisableOnScreenControls();
        }
    }
    
    /***************************************************************************/
    private void DisableOnScreenControls()
    {
        GameObject pan = GameObject.Find("moveLeft") as GameObject;
        pan.GetComponent<dfPanel>().enabled = false;
        pan = GameObject.Find("moveRight") as GameObject;
        pan.GetComponent<dfPanel>().enabled = false;
    }
}
