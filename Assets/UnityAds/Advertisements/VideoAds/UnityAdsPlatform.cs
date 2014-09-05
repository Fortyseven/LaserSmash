using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UnityEngine.Advertisements {

  internal abstract class UnityAdsPlatform {
  
    private string _logTag = "UnityAds";

    public void Log(string message) {
      if(Debug.isDebugBuild) {
        Debug.Log(_logTag + "/" + message);
      }
    }

    public abstract void init(string gameId, bool testModeEnabled, bool debugModeEnabled, string gameObjectName);
    public abstract bool show(string zoneId, string rewardItemKey, string options);
    public abstract void hide();
    public abstract bool isSupported();
    public abstract string getSDKVersion();
    public abstract bool canShowAds();
    public abstract bool canShow();
    public abstract bool hasMultipleRewardItems();
    public abstract string getRewardItemKeys();
    public abstract string getDefaultRewardItemKey();
    public abstract string getCurrentRewardItemKey();
    public abstract bool setRewardItemKey(string rewardItemKey);
    public abstract void setDefaultRewardItemAsRewardItem();
	public abstract string getRewardItemDetailsWithKey(string rewardItemKey);
    public abstract string getRewardItemDetailsKeys();
    public abstract void setNetwork(string network);
  }
}