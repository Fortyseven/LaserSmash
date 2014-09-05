#if UNITY_EDITOR

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements.HTTPLayer;

namespace UnityEngine.Advertisements {
  
	internal class UnityAdsEditor : UnityAdsPlatform {
  	private static bool initialized = false;
  	private static bool ready = false;
		private UnityAdsEditorPlaceholder placeHolder = null;
    public override void init (string gameId, bool testModeEnabled, bool debugModeEnabled, string gameObjectName) {
	    if(initialized) return;
    	initialized = true;

      Log ("UnityEditor: init(), gameId=" + gameId + ", testModeEnabled=" + testModeEnabled + ", gameObjectName=" + gameObjectName + ", debugModeEnabled=" + debugModeEnabled);

    	string url = "https://impact.applifier.com/mobile/campaigns?platform=editor&gameId=" + WWW.EscapeURL(gameId) + "&unityVersion=" + WWW.EscapeURL(Application.unityVersion);
    	HTTPRequest req = new HTTPRequest(url);
    	req.execute(handleResponse);

  	}
    
  	private void handleResponse(HTTPResponse res) {
	    bool success = false;

    if (res.error) {
      Utils.Log("UnityAdsEditor error: Failed to contact server: " + res.errorMsg);
    } else {
      string json = System.Text.Encoding.UTF8.GetString(res.data, 0, res.data.Length);
        
      bool validResponse = false;
        
      object parsedData = MiniJSON.Json.Deserialize(json);
      if(parsedData is Dictionary<string,object>) {
        Dictionary<string,object> parsedJson = (Dictionary<string,object>)parsedData;
        if(parsedJson.ContainsKey("status")) {
          string value = (string)parsedJson["status"];
          if(value.Equals("ok")) {
            validResponse = true;
          } else {
            if(parsedJson.ContainsKey("errorMessage")) {
              Utils.Log("UnityAdsEditor error: Server returned error message: " + (string)parsedJson["errorMessage"]);
            }
          }
        } else {
          Utils.Log("UnityAdsEditor error: JSON response does not have status field: " + json);
        }
      } else {
        Utils.Log("UnityAdsEditor error: unable to parse JSON: " + json);
      }
        
      if(validResponse) {
        success = true;
      } else {
        Utils.Log("ApplifierImpactEditor error: Failed to fetch campaigns");
      }
    }
    
    if(success) {
      UnityAds.SharedInstance.onFetchCompleted();
	    ready = true;
    } else {
      UnityAds.SharedInstance.onFetchFailed();
    }
  }

    public override bool show (string zoneId, string rewardItemKey, string options) {
      Log ("UnityEditor: show()");
			GameObject placeHolderObject = GameObject.Find(@"PlaceHolderObject");
			if (placeHolderObject == null) {
				placeHolderObject = new GameObject(@"PlaceHolderObject");
				placeHolder = placeHolderObject.AddComponent<UnityAdsEditorPlaceholder>();
				placeHolder.init();
			}
			placeHolder.Show();
      return true;
    }
    
    public override void hide () {
      Log ("UnityEditor: hide()");
    }
    
    public override bool isSupported () {
      Log ("UnityEditor: isSupported()");
      return false;
    }
    
    public override string getSDKVersion () {
      Log ("UnityEditor: getSDKVersion()");
      return "EDITOR";
    }
    
    public override bool canShowAds () {
      Log ("UnityEditor: canShowAds()");
      return ready;
    }
    
    public override bool canShow () {
      Log ("UnityEditor: canShow()");
      return ready;
    }
    
    public override bool hasMultipleRewardItems () {
      Log ("UnityEditor: hasMultipleRewardItems()");
      return false;
    }
    
    public override string getRewardItemKeys () {
      Log ("UnityEditor: getRewardItemKeys()");
      return "";
    }
    
    public override string getDefaultRewardItemKey () {
      Log ("UnityEditor: getDefaultRewardItemKey()");
      return "";
    }
    
    public override string getCurrentRewardItemKey () {
      Log ("UnityEditor: getCurrentRewardItemKey()");
      return "";
    }
    
    public override bool setRewardItemKey (string rewardItemKey) {
      Log ("UnityEditor: setRewardItemKey() rewardItemKey=" + rewardItemKey);
      return false;
    }
    
    public override void setDefaultRewardItemAsRewardItem () {
      Log ("UnityEditor: setDefaultRewardItemAsRewardItem()");
    }
    
    public override string getRewardItemDetailsWithKey (string rewardItemKey) {
      Log ("UnityEditor: getRewardItemDetailsWithKey() rewardItemKey=" + rewardItemKey);
      return "";
    }
    
    public override string getRewardItemDetailsKeys () {
      return "name;picture";
    }

    public override void setNetwork(string network) {
      Log("UnityEditor: setNetwork() network=" + network);
    }
  }
}

#endif
