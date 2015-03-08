namespace Game
{
    public class GameConstants
    {
        public static float SCREEN_X_BOUNDS = 14.0f;
        public static float SCREEN_Y_GEN_OFFSET = 17.0f;
        public static float SCREEN_Y_FLOOR = 0.0f;
        public static int SCORE_ASTEROID_LG = 10;
        public static int SCORE_ASTEROID_SM = 20;
        public static int SCORE_BOMB_LG = 40;
        public static int SCORE_BOMB_SM = 80;
        public static int SCORE_KILLSAT = 50;
        public static int SCORE_UFO = 100;
        public static int SCORE_PLAYERDEATH = -100;
        public static int MAX_MULTIPLIER = 6;

        public static float[] MULT_TIMEOUT_RAMP = { 1.0f, 0.875f, 0.750f, 0.600f, 0.475f, 0.350f };
    }
}