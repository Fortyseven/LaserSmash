using UnityEngine;
using System.Collections;

using UnityEngine.UI;

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

    public Text _UIScoreValue;
    public Text _UILivesValue;
    public Text _UIMultValue;

    public  Canvas GameOverCanvas;

    private GameState _gameState;
    private GameObject _playerShip;


#region properties
    public GameObject PlayerShip {
        get { return _playerShip; }
    }

    public GameState State {
        get { return _gameState; }
    }
#endregion

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
        _playerShip = GameObject.Find( "PlayerShip" ) as GameObject;
        if ( _playerShip == null )
            throw new UnityException( "Could not find PlayerShip object" );

        Physics2D.IgnoreLayerCollision( LayerMask.NameToLayer( "Enemy" ), LayerMask.NameToLayer( "Enemy" ) );
        ConfigureGame();
        
        //        mWaveGenerator = GetComponent<WaveGenerator>() as WaveGenerator;
        //        mWaveGenerator.init( this );
        
        _gameState = new GameState();
        GameOverCanvas.gameObject.SetActive(false);
        _gameState.Reset();
    }

    /***************************************************************************/
    public void SetScoreValue( int score )
    {
        _UIScoreValue.text = score.ToString();
    }

    /***************************************************************************/
    public void SetMultValue( int mult )
    {
        _UIMultValue.text = mult.ToString() + "x";
    }

    /***************************************************************************/
    public void SetLivesValue( int lives )
    {
        _UILivesValue.text = lives.ToString();
    }

    /***************************************************************************/
    public GameState getGameState()
    {
        return _gameState;
    }
    
    /***************************************************************************/
    private void ConfigureGame()
    {
        DifficultyController.Load();

#if !UNITY_ANDROID
//        DisableOnScreenControls();
#endif
    }

#if !UNITY_ANDROID
//    /***************************************************************************/
//    private void DisableOnScreenControls()
//    {
//        GameObject pan = GameObject.Find( "Input_Left" ) as GameObject;
//        pan.SetActive(false);
//        pan = GameObject.Find( "Input_Right" ) as GameObject;
//        pan.SetActive(false);
//    }
#endif

    /***************************************************************************/
    public void OnGameOver()
    {
        Debug.Log( "GAME OVER" );
        GameOverCanvas.gameObject.SetActive(true);
        Text peak_score_value = GameObject.Find("PeakScoreValue").GetComponent<Text>();
        peak_score_value.text = State.PeakScore.ToString();
    }

    public void OnClick()
    {

    }
}
