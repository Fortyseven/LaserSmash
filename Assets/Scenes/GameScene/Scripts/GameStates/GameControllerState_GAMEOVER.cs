using System;
using UnityEngine;
using UnityEngine.UI;

// ReSharper disable once ClassNeverInstantiated.Global
public class GameControllerState_GAMEOVER : StateBehavior
{
    private const float GAMEOVER_TIMEOUT = 4.0f;

    private float _game_over_timeout;
    private bool _game_over_message_enabled;

    public override void OnEnter( Enum changing_from )
    {
        //GameState.instance.State.Mode = GameState.GameMode.GAMEOVER;

        ( (GameController)Parent ).GameOverCanvas.gameObject.SetActive( true );
        Text peak_score_value = GameObject.Find( "PeakScoreValue" ).GetComponent<Text>();
        peak_score_value.text = ( (GameController)Parent ).State.PeakScore.ToString();
        _game_over_timeout = Time.time + GAMEOVER_TIMEOUT;
        _game_over_message_enabled = false;
    }

    public override void OnUpdate()
    {
        if ( Time.time >= _game_over_timeout && !_game_over_message_enabled ) {
            ( (GameController)Parent ).GameOverCanvas.GetComponentInChildren<Flash>().Go();
            _game_over_message_enabled = true;
        }
        if ( Input.anyKeyDown && ( Time.time >= _game_over_timeout ) ) {
            Machine.SwitchStateTo( GameController.NewGameState.RUNNING );
        }
    }
}