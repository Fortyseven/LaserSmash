/************************************************************************
** GameConstants.cs
**
** Copyright (c) 2016, BytesTemplar.com
** For information on usage and redistribution, and for a DISCLAIMER 
** OF ALL WARRANTIES, see the text file, "LICENSE" in this distribution.
*************************************************************************/

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class Init : StateMachineMB
    {
        private static Init _instance = null;
        public static bool Debug { get; set; }

        public static Init instance {
            get {
                if (_instance == null) {
                    Construct(false);
                }
                return _instance;
            }
        }

        private const string SCENE_NAME_MAIN_MENU = "MainMenu";
        private const string SCENE_NAME_GAME = "Game";
        private const string SCENE_NAME_HELP = "Help";

        public enum GameStates
        {
            INTRO,
            MAIN_MENU,
            HELP,
            GAME_LOOP
        };

        /********************************************/
        private class GameState_MainMenu : State
        {
            public override Enum Name { get { return GameStates.MAIN_MENU; } }

            public override void Start()
            {
            }

            public override void OnStateEnter(State from_state)
            {
                SceneManager.LoadScene(SCENE_NAME_MAIN_MENU);
            }

            public override void OnStateExit(State to_state)
            {
                SceneManager.UnloadScene(SCENE_NAME_MAIN_MENU);
            }

            public override void OnUpdate()
            {
                ;
            }
        }

        /********************************************/
        private class GameState_GameLoop : State
        {
            public override Enum Name { get { return GameStates.GAME_LOOP; } }

            public override void Start()
            {
            }

            public override void OnStateEnter(State from_state)
            {
                SceneManager.LoadScene(SCENE_NAME_GAME);
            }

            public override void OnStateExit(State to_state)
            {
                SceneManager.UnloadScene(SCENE_NAME_GAME);
            }

            public override void OnUpdate()
            {
                ;
            }
        }

        /********************************************/
        private class GameState_Help : State
        {
            public override Enum Name { get { return GameStates.HELP; } }

            public override void Start()
            {
            }

            public override void OnStateEnter(State from_state)
            {
                SceneManager.LoadScene(SCENE_NAME_HELP);
            }

            public override void OnStateExit(State to_state)
            {
                SceneManager.LoadScene(SCENE_NAME_HELP);
            }

            public override void OnUpdate()
            {
                ;
            }
        }

        /********************************************/
        public static void Construct(bool debug_mode = false)
        {
            if (_instance != null) return;

            Debug = debug_mode;
            var go = new GameObject("Init");
            go.AddComponent<Init>();
        }

        public void Start()
        {
            DontDestroyOnLoad(this.gameObject);

            Init._instance = this;

            AddState(new GameState_MainMenu());
            AddState(new GameState_GameLoop());
            AddState(new GameState_Help());

            if (Debug)
                return;

            ChangeState(GameStates.MAIN_MENU);
        }

        public new void Update()
        {
            base.Update();
        }
    }
}
