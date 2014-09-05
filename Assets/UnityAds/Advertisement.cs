using System;

namespace UnityEngine.Advertisements {

  public static class Advertisement {

    static public bool isSupported {
      get {
        return 
          Application.isEditor ||
          Application.platform == RuntimePlatform.IPhonePlayer || 
          Application.platform == RuntimePlatform.Android;
      }
    }

    static public bool isInitialized {
      get {
        return Engine.Instance.isInitialized;
      }
    }

    static public void Initialize(string appId) {
      Engine.Instance.Initialize(appId);
    }

    static public void Show(string zoneId = null, ShowOptions options = null) {
      Engine.Instance.Show(zoneId, options);
    }

    static public bool allowPrecache { 
      get {
        return Engine.Instance.allowPrecache;
      }
      set {
        Engine.Instance.allowPrecache = value;
      }
    }

    static public bool isReady(string zoneId = null) {
      return Engine.Instance.isReady(zoneId);
    }

    static public bool isShowing { 
      get {
        return Engine.Instance.isShowing();
      }
    }

  }

}