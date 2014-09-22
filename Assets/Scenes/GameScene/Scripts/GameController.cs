//#define TESTMODE

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;


//    public DifficultyValues[]   Difficulty;
//    public DifficultyValueEntry     CurrentDifficulty;//;; {
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
    private WaveController _wave_controller;
    private Player _player_component;

#region properties
    public WaveController WaveCon {
        get {return _wave_controller; }
    }
    public GameObject PlayerShip {
        get { return _playerShip; }
    }
    public Player PlayerComponent {
        get {return _player_component; }
    }
    public GameState State {
        get { return _gameState; }
    }
#endregion

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
//        Application.targetFrameRate = 30;
    }

    /***************************************************************************/
    void Start()
    {
        _playerShip = GameObject.Find( "PlayerShip" ) as GameObject;
        _player_component = _playerShip.GetComponent<Player>();
//        if ( _playerShip == null )
//            throw new UnityException( "Could not find PlayerShip object" );

        Physics2D.IgnoreLayerCollision( LayerMask.NameToLayer( "Enemy" ), LayerMask.NameToLayer( "Enemy" ) );
        ConfigureGame();

        _wave_controller = GetComponentInChildren<WaveController>();
        _gameState = new GameState();
        NewGame();
    }

    /***************************************************************************/
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.End)) {
            _playerShip.GetComponent<Player>().PlayerKilled();
        }
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
//        DifficultyController.Load();

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
        GameController.instance.State.Mode = GameState.GameMode.GAMEOVER;
        GameOverCanvas.gameObject.SetActive(true);
        Text peak_score_value = GameObject.Find("PeakScoreValue").GetComponent<Text>();
        peak_score_value.text = State.PeakScore.ToString();
    }

    public void NewGame()
    {
#if !TESTMODE
        GameOverCanvas.gameObject.SetActive(false);
#endif
        _gameState.Reset();
        PlayerShip.GetComponent<Player>().Reset();
        PlayerShip.GetComponent<Player>().enabled = true;
    }
}
