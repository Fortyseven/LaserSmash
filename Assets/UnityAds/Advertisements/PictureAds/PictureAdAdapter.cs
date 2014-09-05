using System;
using System.Collections.Generic;

namespace UnityEngine.Advertisements {

  internal class PictureAdAdapter : Adapter {
		PictureAdsManager _manager;
    public PictureAdAdapter(string adapterId) : base(adapterId) {}

    public override void Initialize(string zoneId, string adapterId, Dictionary<string, object> configuration) {
      string network = null;
      string platform = null;
      
      triggerEvent(EventType.initStart, EventArgs.Empty);
			if (configuration != null && configuration.ContainsKey(@"network"))
			network = (string)configuration[@"network"];

			platform = DeviceInfo.currentPlatform();
			if (network == null || network.Length == 0) {
				switch(platform) {
					case @"ios":
							network = @"picture_ios";
						break;
					case @"android":
							network = @"picture_android";
						break;
					default: 
							network = @"picture_editor";
						break;
				}
			}

			_manager = new PictureAdsManager(network);
			_manager.setPictureAdClosedDelegate(onPictureAdClosed);
			_manager.setPictureAdFailedDelegate(onPictureAdFailed);
			_manager.init();
    }
    
    void onPictureAdFailed() {
      triggerEvent(EventType.error, EventArgs.Empty);
    }
    
    void onPictureAdClosed() {
      triggerEvent(EventType.adFinished, EventArgs.Empty);
    }

    public override void RefreshAdPlan() { Utils.Log("Got refresh ad plan request for picture ads"); }
    public override void StartPrecaching() {}
    public override void StopPrecaching() {}

    public override bool isReady() {
			return _manager.isAdAvailable();
    }

    public override void Show(string zoneId, string adapterId) {
			_manager.showAd();
    }

    public override bool isShowing() {
			return _manager.isShowingAd();
    }
  }
}
