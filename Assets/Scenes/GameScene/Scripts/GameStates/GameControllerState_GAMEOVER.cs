using System;
using UnityEngine;
using UnityEngine.UI;

public class GameControllerState_GAMEOVER : StateMachineMB.State
{
    private const float GAMEOVER_TIMEOUT = 4.0f;

    private Canvas _ui_game_over_canvas;
    private Flash _ui_flash;
    private float _game_over_timeout;
    private bool _game_over_message_enabled;

    public override Enum Name { get { return GameController.NewGameState.GAMEOVER; } }

    public override void Start()
    {
        _ui_game_over_canvas = GameObject.Find( "GameOver Canvas" ).GetComponent<Canvas>();
        _ui_flash = _ui_game_over_canvas.GetComponentInChildren<Flash>();
        _ui_game_over_canvas.gameObject.SetActive( false );
    }

    public override void OnStateEnter( StateMachineMB.State from_state )
    {
        _ui_game_over_canvas.gameObject.SetActive( true );
        Text peak_score_value = GameObject.Find( "PeakScoreValue" ).GetComponent<Text>();
        //peak_score_value.text = ( (GameController)Parent ).Status.PeakScore.ToString();
        peak_score_value.text = "FIXME";
        _game_over_timeout = Time.time + GAMEOVER_TIMEOUT;
        _game_over_message_enabled = false;
    }

    public override void OnStateExit( StateMachineMB.State to_state )
    {
        //#if !TESTMODE
        //        ( (GameController)Parent ).GameOverCanvas.gameObject.SetActive( false );
        //#endif

        _ui_flash.ResetFlashers();
        _ui_game_over_canvas.gameObject.SetActive( false );
    }

    public override void OnUpdate()
    {
        if ( Time.time >= _game_over_timeout && !_game_over_message_enabled ) {
            _ui_game_over_canvas.GetComponentInChildren<Flash>().Go();
            _game_over_message_enabled = true;
        }
        if ( Input.anyKeyDown && ( Time.time >= _game_over_timeout ) ) {
            Owner.ChangeState( GameController.NewGameState.RUNNING );
        }
    }
}