using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Advertisements.Event;
using UnityEngine.Advertisements.HTTPLayer;

namespace UnityEngine.Advertisements {
	internal class PictureAdsManager {	
    PictureAdsFrameManager framesManager = null;
    PictureAdsRequestsManager requestManager = null;
    PictureAd currentAd = null;
		bool jsonDownloaded = false;
		bool resourcesAreDownloaded = false;
		string _network = null;
    
    public delegate void PictureAdClosed();
    private PictureAdClosed _pictureAdClosedDelegate;
    
    public void setPictureAdClosedDelegate (PictureAdClosed action) {
      _pictureAdClosedDelegate = action;
    }
    
    public delegate void PictureAdFailed();
    private PictureAdFailed _pictureAdFailedDelegate;
    
    public void setPictureAdFailedDelegate (PictureAdFailed action) {
      _pictureAdFailedDelegate = action;
    }
    
    public PictureAdsManager(string network) {
			requestManager = PictureAdsRequestsManager.sharedInstance();
			_network = network;
    }

    public void init() {
			EventManager.sendAdreqEvent(Engine.Instance.AppId);
      currentAd = null;
	  	jsonDownloaded = false;
	  	resourcesAreDownloaded = false;
      if (requestManager != null)
	  		requestManager.downloadJson(_network, this);
   	}
    
    public void pictureAdClosed() {
			framesManager = null;
			GameObject framesManagerHolder = GameObject.Find(@"FramesManagerHolder");
			GameObject.Destroy(framesManagerHolder);
      _pictureAdClosedDelegate();
    }

		public void pictureAdFailed() {
			framesManager = null;
			GameObject framesManagerHolder = GameObject.Find(@"FramesManagerHolder");
			GameObject.Destroy(framesManagerHolder);
			_pictureAdFailedDelegate(); 
		}

		void removeLocalResources (PictureAd ad) {
			if (!ad.adIsValid()) return;
			System.IO.File.Delete(ad.getLocalImageURL(ImageOrientation.Landscape, ImageType.Close));
			System.IO.File.Delete(ad.getLocalImageURL(ImageOrientation.Landscape, ImageType.Frame));
			System.IO.File.Delete(ad.getLocalImageURL(ImageOrientation.Landscape, ImageType.Base));
			System.IO.File.Delete(ad.getLocalImageURL(ImageOrientation.Portrait, ImageType.Close));
			System.IO.File.Delete(ad.getLocalImageURL(ImageOrientation.Portrait, ImageType.Base));
			System.IO.File.Delete(ad.getLocalImageURL(ImageOrientation.Portrait, ImageType.Frame));
		}

		public void resourcesAvailableDelegate () {
			resourcesAreDownloaded = true;
		}

    public void jsonAvailableDelegate(string jsonData) {
	  	jsonDownloaded = true;
      currentAd = PictureAdsParser.parseJSONString(jsonData, Application.temporaryCachePath + "/");
			if(currentAd == null || !currentAd.adIsValid()) {pictureAdFailed();return;}
      	requestManager.downloadResourcesForAd(_network, this, currentAd);
    }

    bool areResourcesReady() {
		  return jsonDownloaded && resourcesAreDownloaded;
    }

    public bool isAdAvailable() {
			return areResourcesReady() ? (currentAd.adIsValid() && currentAd.resourcesAreValid() && (framesManager != null ? framesManager.adIsClosed() : true) && !isShowingAd()) : false;
    }
    
    public bool isShowingAd() {
			return (framesManager != null ? framesManager.isShowingAd() : false);
    }

    public void showAd() {
			GameObject framesManagerHolder = GameObject.Find(@"FramesManagerHolder");
			if (framesManagerHolder == null) {
				framesManagerHolder = new GameObject(@"FramesManagerHolder");
				framesManager = framesManagerHolder.AddComponent<PictureAdsFrameManager>();
				framesManager.manager = this;
			}

      if(isAdAvailable()) {
        if(framesManager.adIsClosed())
          framesManager.initAd(currentAd);
        EventManager.sendViewEvent(Engine.Instance.AppId, currentAd.id);
        framesManager.showAd();
      }
    }
  }
}