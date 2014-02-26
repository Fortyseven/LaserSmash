using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour
{
    public dfLabel mGUIScoreValue = null;
    public dfLabel mGUILivesValue = null;
    
    public bool isMobileMode = false;
    
    private WaveGenerator mWaveGenerator = null ;
    private GameState mGameState = null;

    void HandlerTapTappyTap(dfTapGesture tap)
    {
        Debug.Log("wat " + tap.transform.position.x);
    }
    
    void Start()
    {
        ConfigureGame();
        
        mWaveGenerator = GetComponent<WaveGenerator>() as WaveGenerator;
        mWaveGenerator.init( this );
                
        mGameState = new GameState();
        mGameState.Reset();
        
        GameObject.Find("Label 3").GetComponent<dfTapGesture>().TapGesture += HandlerTapTappyTap;
        
//      GameObject.Find("Label 3").GetComponent<dfTapGesture>().TapGesture += delegate(dfTapGesture gesture) {
//          
//      };
        
        //my_ufo = Instantiate(UFOPrefab, new Vector3(0,0), Quaternion.identity) as GameObject;
    }
    
    // Update is called once per frame
    void Update()
    {
        mGUIScoreValue.Text = mGameState.Score.ToString();
    }
        
    public GameState getGameState()
    {
        return mGameState;
    }
    
    private void ConfigureGame()
    {
        if (!isMobileMode) {
            DisableOnScreenControls();
        }
    }
    
    private void DisableOnScreenControls()
    {
        GameObject pan = GameObject.Find("moveLeft") as GameObject;
        pan.GetComponent<dfPanel>().enabled = false;
        pan = GameObject.Find("moveRight") as GameObject;
        pan.GetComponent<dfPanel>().enabled = false;
    }
}
