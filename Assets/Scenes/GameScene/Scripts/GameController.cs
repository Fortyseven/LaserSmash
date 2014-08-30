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

    public dfLabel mGUIScoreValue;
    public dfLabel mGUILivesValue;
    public bool isMobileMode = false;

    private GameState _gameState;
    private GameObject _playerShip;
    public GameObject PlayerShip {
        get {return _playerShip; }
    }

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
        _playerShip = GameObject.Find("PlayerShip") as GameObject;
        if(_playerShip == null) throw new UnityException("Could not find PlayerShip object");

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"));
        ConfigureGame();
        
        //        mWaveGenerator = GetComponent<WaveGenerator>() as WaveGenerator;
        //        mWaveGenerator.init( this );
        
        _gameState = new GameState();
        _gameState.Reset();    
    }
    
    /***************************************************************************/
    public void AdjustScore( int score_offset )
    {
        Debug.Log(score_offset + " - " + _gameState);
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
