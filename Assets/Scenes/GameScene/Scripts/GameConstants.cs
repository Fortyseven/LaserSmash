namespace Game
{
    public class GameConstants
    {
        public const float  SCREEN_X_BOUNDS     = 14.0f;
        public const float  SCREEN_Y_GEN_OFFSET = 17.0f;
        public const float  SCREEN_Y_FLOOR      = 0.0f;
        public const int    SCORE_ASTEROID_LG   = 10;
        public const int    SCORE_ASTEROID_SM   = 20;
        public const int    SCORE_BOMB_LG       = 40;
        public const int    SCORE_BOMB_SM       = 80;
        public const int    SCORE_KILLSAT       = 50;
        public const int    SCORE_UFO           = 100;
        public const int    SCORE_PLAYERDEATH   = -100;
        public const int    MAX_MULTIPLIER      = 6;

        public static readonly float[] MULT_TIMEOUT_RAMP = { 1.0f, 0.875f, 0.750f, 0.600f, 0.475f, 0.350f };

        /*
            1x  level : Black background : Score up to 999
            2x  level : Blue background : Score 1,000-4,999
            3x  level : Purple background : Score 5,000-19,999
            4x  level : Turquoise background : Score 20,000-49,999
            5x  level : Gray background : Score 50,000-99,999
            6x  level : Black background : Score 100,000 and over
        */

        public const int SCORE_THRESH_1X    = 0;
        public const int SCORE_THRESH_2X    = 1000;
        public const int SCORE_THRESH_3X    = 5000;
        public const int SCORE_THRESH_4X    = 20000;
        public const int SCORE_THRESH_5X    = 50000;
        public const int SCORE_THRESH_6X    = 100000;

        public const int INITIAL_LIVES      = 3;
        public const int INITIAL_MULTIPLIER = 1;

    }
}