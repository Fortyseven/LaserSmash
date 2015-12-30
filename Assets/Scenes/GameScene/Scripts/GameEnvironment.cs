/************************************************************************
** GameEnvironment.cs
**
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameEnvironment
    {
        public WaveController WaveCon { get; private set; }
        public GameObject PlayerShip { get; private set; }
        public Player PlayerComponent { get; private set; }

        private int _score;
        private int _peak_score;
        private int _last_life_peak_score;
        private int _mult;
        private int _lives;

        private Text _ui_score_value;
        private Text _ui_lives_value;
        private Text _ui_mult_value;

        public GameEnvironment( GameObject owner )
        {
            //Egg_CockpitCamera.enabled = false;

            // Fetch UI objects
            _ui_score_value = GameObject.Find( "UI_ScoreValue" ).GetComponent<Text>();
            _ui_mult_value = GameObject.Find( "UI_MultValue" ).GetComponent<Text>();
            _ui_lives_value = GameObject.Find( "UI_LivesValue" ).GetComponent<Text>();

            PlayerShip = GameObject.Find( "PlayerBase" );

            if ( PlayerShip == null )
                throw new UnityException( "PlayerBase object not found" );

            PlayerComponent = PlayerShip.GetComponent<Player>();

            if ( PlayerComponent == null )
                throw new UnityException( "Player Component not found" );

            WaveCon = owner.GetComponentInChildren<WaveController>();
            WaveCon.Init();
            WaveCon.Paused = true;

            Score = 0;
            Multiplier = GameConstants.INITIAL_MULTIPLIER;
            Lives = GameConstants.INITIAL_LIVES;
        }

        public int Lives
        {
            get { return _lives; }
            set
            {
                // Only adjust lives when the game is running; once it's
                // at zero, only a Reset() will unlock it
                //if ( Mode == GameMode.GAMEOVER )
                //    return;

                _lives = value;
                SetLivesValue( value );
            }
        }

        public int Score
        {
            get { return _score; }
            set
            {
                _score = value;
                SetScoreValue( value );
                ValidateMultplier();
            }
        }

        public int PeakScore
        {
            get { return _peak_score; }
            set { _peak_score = value; }
        }

        public int Multiplier
        {
            get { return _mult; }
            set
            {
                _mult = value;
                SetMultValue( _mult );
            }
        }

        /***************************************************************************/

        public void SetScoreValue( int score )
        {
            _ui_score_value.text = score.ToString();
        }

        /***************************************************************************/

        public void SetMultValue( int mult )
        {
            _ui_mult_value.text = mult.ToString() + "x";
        }

        /***************************************************************************/

        public void SetLivesValue( int lives )
        {
            _ui_lives_value.text = lives.ToString();
        }

        public void AdjustScore( int score_offset )
        {
            Score += score_offset * Multiplier;

            if ( Score > _peak_score ) {
                _peak_score = Score;
                if ( _peak_score - _last_life_peak_score > 1000 ) {
                    _last_life_peak_score = _peak_score;
                    Lives++;
                    //TODO: Add 1UP noise
                }
            }
        }

        /***************************************************************************/

        private void ValidateMultplier()
        {
            int cur_mult = Multiplier;

            if ( Score < GameConstants.SCORE_THRESH_2X ) {
                Multiplier = 1;
            }
            else if ( Score < GameConstants.SCORE_THRESH_3X ) {
                Multiplier = 2;
            }
            else if ( Score < GameConstants.SCORE_THRESH_4X ) {
                Multiplier = 3;
            }
            else if ( Score < GameConstants.SCORE_THRESH_5X ) {
                Multiplier = 4;
            }
            else if ( Score < GameConstants.SCORE_THRESH_6X ) {
                Multiplier = 5;
            }
            else {
                Multiplier = 6;
            }

            if ( cur_mult != Multiplier )
                OnMultChange();
        }

        private void OnMultChange()
        {
            //switch ( Multiplier ) {
            //    case 2: // Blue
            //        GameController.instance.DoLevelTransition( new Color( 0, 0, 1.0f, 1.0f ) );
            //        break;
            //    case 3: // Purple
            //        GameController.instance.DoLevelTransition( new Color( 1.0f, 0, 1.0f, 1.0f ) );
            //        break;
            //    case 4: // Cyan
            //        GameController.instance.DoLevelTransition( new Color( 0, 1.0f, 1.0f, 1.0f ) );
            //        break;
            //    case 5: // Gray
            //        GameController.instance.DoLevelTransition( new Color( 1.0f, 0.75f, 0, 1.0f ) );
            //        break;
            //    default: // "Black"
            //        GameController.instance.DoLevelTransition( new Color( 0.5f, 0.5f, 0.5f, 1.0f ) );
            //        break;
            //}
        }
    }
}
