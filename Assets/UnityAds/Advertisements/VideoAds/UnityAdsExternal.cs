using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UnityEngine.Advertisements {

  internal static class UnityAdsExternal {
  
    private static string _logTag = "UnityAds";
	private static UnityAdsPlatform impl;
	private static bool initialized = false;

    public static void Log(string message) {
      if(Debug.isDebugBuild) {
        Debug.Log(_logTag + "/" + message);
      }
    }

	private static UnityAdsPlatform getImpl() {
		if (!initialized) {
			initialized = true;
#if UNITY_EDITOR
			impl = new UnityAdsEditor();
#elif UNITY_ANDROID
			impl = new UnityAdsAndroid();
#elif UNITY_IOS
			impl = new UnityAdsIos();
#endif
		}

		return impl;
	}

    public static void init (string gameId, bool testModeEnabled, bool debugModeEnabled, string gameObjectName) {
		getImpl().init(gameId, testModeEnabled, debugModeEnabled, gameObjectName);
	}
    
    public static bool show (string zoneId, string rewardItemKey, string options) {
		return getImpl().show(zoneId, rewardItemKey, options);
    }
    
    public static void hide () {
		getImpl().hide();
	}
    
    public static bool isSupported () {
		return getImpl().isSupported();
    }
    
    public static string getSDKVersion () {
		return getImpl().getSDKVersion();
    }
    
    public static bool canShowAds () {
		return getImpl().canShowAds();
    }
    
    public static bool canShow () {
		return getImpl().canShow();
    }
    
    public static bool hasMultipleRewardItems () {
		return getImpl().hasMultipleRewardItems();
    }
    
    public static string getRewardItemKeys () {
		return getImpl().getRewardItemKeys();
    }
  
    public static string getDefaultRewardItemKey () {
		return getImpl().getDefaultRewardItemKey();
    }
    
    public static string getCurrentRewardItemKey () {
		return getImpl().getCurrentRewardItemKey();
    }
  
    public static bool setRewardItemKey (string rewardItemKey) {
		return getImpl().setRewardItemKey(rewardItemKey);
    }
    
    public static void setDefaultRewardItemAsRewardItem () {
		getImpl().setDefaultRewardItemAsRewardItem();
    }
    
    public static string getRewardItemDetailsWithKey (string rewardItemKey) {
		return getImpl().getRewardItemDetailsWithKey(rewardItemKey);
    }
    
    public static string getRewardItemDetailsKeys () {
		return getImpl().getRewardItemDetailsKeys();
    }  

    public static void setNetwork(string network) {
      getImpl().setNetwork(network);
    }
  }
}
