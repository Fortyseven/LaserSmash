using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;


//    public DifficultyValues[]   Difficulty;
    public DifficultyValueEntry     CurrentDifficulty;//;; {
//        get {
////            DifficultyController.instance
//        }
//    };
//    private DifficultyValues    _lastDifficulty = null;

    /*---------------------------------------------------------*/

    public dfLabel mGUIScoreValue = null;
    public dfLabel mGUILivesValue = null;
    public bool isMobileMode = false;
    private GameState _gameState = null;

//    private WaveGenerator mWaveGenerator = null ;

    /***************************************************************************/
//    private DifficultyValues UpdateDifficultyValues()
//    {
//        for ( int i = Difficulty.Length - 1; i >= 0; i-- ) {
//            DifficultyValues diff = ( (DifficultyValues)( Difficulty[ i ] ) );
//            if ( _gameState.Score >= diff.ScoreThreshold ) {
//                return diff;
//            }
//        }
//        return Difficulty[ 0 ];
//    }

    /***************************************************************************/
    void Awake()
    {
        GameController.instance = this;
    }
    /***************************************************************************/
    void Start()
    {
        ConfigureGame();
        
//        mWaveGenerator = GetComponent<WaveGenerator>() as WaveGenerator;
//        mWaveGenerator.init( this );
                
        _gameState = new GameState();
        _gameState.Reset();
    }
    
    /***************************************************************************/
    public void AdjustScore( int score_offset )
    {
        _gameState.Score += score_offset;
        mGUIScoreValue.Text = _gameState.Score.ToString();
        //CurrentDifficulty = UpdateDifficultyValues();
    }
    /***************************************************************************/
//    void Update()
//    {
//
//    }
        
    /***************************************************************************/
    public GameState getGameState()
    {
        return _gameState;
    }
    
    /***************************************************************************/
    private void ConfigureGame()
    {
        if ( !isMobileMode ) {
            DisableOnScreenControls();
        }

        DifficultyController.Load();
    }
    
    /***************************************************************************/
    private void DisableOnScreenControls()
    {
        GameObject pan = GameObject.Find( "moveLeft" ) as GameObject;
        pan.GetComponent<dfPanel>().enabled = false;
        pan = GameObject.Find( "moveRight" ) as GameObject;
        pan.GetComponent<dfPanel>().enabled = false;
    }
}
