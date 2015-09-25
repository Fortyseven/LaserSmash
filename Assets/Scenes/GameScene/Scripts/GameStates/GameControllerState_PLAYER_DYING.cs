﻿using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Game;

public class GameControllerState_PLAYER_DYING : StateMachineMB.State
{

    // Use this for initialization
    public override Enum Name { get { return GameController.GameState.PLAYER_DYING; } }
    private const float DYING_DURATION_SECS = 3.0f;
    private float _timer;
    private Image _death_panel_image; // The red death filter while exploding

    public override void Start()
    {
        _death_panel_image = GameObject.Find( "DeathPanel" ).GetComponent<Image>();
        _death_panel_image.gameObject.SetActive( false );
    }

    public override void OnStateEnter( StateMachineMB.State from_state )
    {
        _death_panel_image.gameObject.SetActive( true );

        GameController.instance.GameEnv.Lives--;
        GameController.instance.GameEnv.AdjustScore( GameConstants.SCORE_PLAYERDEATH );

        GameController.instance.GameEnv.WaveCon.Paused = true;
        GameController.instance.GameEnv.WaveCon.Reset();

        _timer = DYING_DURATION_SECS;
    }

    public override void OnStateExit( StateMachineMB.State to_state )
    {
        _death_panel_image.gameObject.SetActive( false );
    }

    public override void OnUpdate()
    {
        _timer -= Time.deltaTime;

        if ( _timer <= 0 ) {
            if ( GameController.instance.GameEnv.Lives <= 0 ) {
                Owner.ChangeState( GameController.GameState.GAMEOVER );
            }
            else {
                Owner.ChangeState( GameController.GameState.RUNNING );
            }
        }
    }
}
