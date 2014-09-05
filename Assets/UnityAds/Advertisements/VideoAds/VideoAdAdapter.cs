using System;
using System.Collections.Generic;

namespace UnityEngine.Advertisements {

  internal class VideoAdAdapter : Adapter {

    private bool _isShowing = false;
    private bool _campaignsAvailable = false;
    private static Dictionary<string, Dictionary<string, object>> _configurations = new Dictionary<string, Dictionary<string, object>>();

    public VideoAdAdapter(string adapterId) : base(adapterId) {}

    public override void Initialize(string zoneId, string adapterId, Dictionary<string, object> configuration) {
      UnityAds.OnCampaignsAvailable += UnityAdsCampaignsAvailable;
      UnityAds.OnCampaignsFetchFailed += UnityAdsCampaignsFetchFailed;
        
      triggerEvent(EventType.initStart, EventArgs.Empty);
        
      UnityAds.SharedInstance.Init(Engine.Instance.AppId, false, false);

      _configurations.Add(zoneId + adapterId, configuration);
    }

    public override void RefreshAdPlan() {}
    public override void StartPrecaching() {}
    public override void StopPrecaching() {}
    
    public override bool isReady() {
      return _campaignsAvailable;
    }
    
    public override void Show(string zoneId, string adapterId) {
      Dictionary<string, object> configuration = _configurations[zoneId + adapterId];
      if(configuration != null && configuration.ContainsKey("network")) {
        UnityAds.setNetwork((string)configuration["network"]);
      }
      string videoZoneId = null;
      string rewardItem = "";
      if(configuration != null && configuration.ContainsKey("zone")) {
        videoZoneId = (string)configuration["zone"];
      }
      if(configuration != null && configuration.ContainsKey("rewardItem")) {
        rewardItem = (string)configuration["rewardItem"];
      }

      UnityAds.OnShow += UnityAdsShow;
      UnityAds.OnHide += UnityAdsHide;
      UnityAds.OnVideoCompleted += UnityAdsVideoCompleted;
      UnityAds.OnVideoStarted += UnityAdsVideoStarted;

      UnityAds.show(videoZoneId, rewardItem);
    }
    
    public override bool isShowing() {
      return _isShowing;
    }
    
    private void UnityAdsCampaignsAvailable() {
      Utils.Log("UNITY ADS: CAMPAIGNS READY!");
      _campaignsAvailable = true;
      triggerEvent(EventType.initComplete, EventArgs.Empty);
      triggerEvent(EventType.adAvailable, EventArgs.Empty);
    }
    
    private void UnityAdsCampaignsFetchFailed() {
      Utils.Log("UNITY ADS: CAMPAIGNS FETCH FAILED!");
      triggerEvent(EventType.initFailed, EventArgs.Empty);
    }
    
    private void UnityAdsShow() {
      Utils.Log("UNITY ADS: SHOW!"); 
      _isShowing = true;
      triggerEvent(EventType.adWillOpen, EventArgs.Empty);
      triggerEvent(EventType.adDidOpen, EventArgs.Empty);
    }
    
    private void UnityAdsHide() {
      Utils.Log("UNITY ADS: HIDE!");
      _isShowing = false;

      UnityAds.OnShow -= UnityAdsShow;
      UnityAds.OnHide -= UnityAdsHide;
      UnityAds.OnVideoCompleted -= UnityAdsVideoCompleted;
      UnityAds.OnVideoStarted -= UnityAdsVideoStarted;

      triggerEvent(EventType.adWillClose, EventArgs.Empty);
      triggerEvent(EventType.adDidClose, EventArgs.Empty);
    }
    
    private void UnityAdsVideoCompleted(string rewardItemKey, bool skipped) {
      Utils.Log("UNITY ADS: VIDEO COMPLETE : " + rewardItemKey + " - " + skipped);
      if(skipped) {
        triggerEvent(EventType.adSkipped, EventArgs.Empty);
      } else {
        triggerEvent(EventType.adFinished, EventArgs.Empty);
      }
    }
    
    private void UnityAdsVideoStarted() {
      Utils.Log("UNITY ADS: VIDEO STARTED!");
      triggerEvent(EventType.adStarted, EventArgs.Empty);
    }
    
  }
  
}
