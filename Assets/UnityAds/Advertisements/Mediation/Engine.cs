using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.Advertisements {

  internal class Engine {

    private static float _savedTimeScale;
    private static float _savedAudioVolume;
    private static bool _isShowing = false;
    static private readonly Engine _sharedInstance = new Engine();

    static public Engine Instance {
      get {
        return _sharedInstance;
      }
    }

    private Engine() {
    }

    private bool _initialized = false;

    public bool isInitialized {
      get {
        return _initialized;
      }
    }

    public string AppId { get; private set; }

    public bool allowPrecache {
      get {
        return true;
      }
      set {
          
      }
    }

    public void Initialize(string appId) {
      if(_initialized) {
        return;
      }

      if(Application.internetReachability == NetworkReachability.NotReachable) {
        return;
      }

	    Event.EventManager.sendStartEvent(appId);

      AppId = appId;
      ConfigManager.Instance.RequestConfig();
    
      Event.EventManager.sendMediationInitEvent(appId);

      _initialized = true;
    }

    public void Show(string zoneId, ShowOptions options) {
      if(!_initialized) {
        return;
      }

      if(!ConfigManager.Instance.IsReady()) {
        Event.EventManager.sendMediationCappedEvent(Engine.Instance.AppId, null, "global", ConfigManager.Instance.globalIntervals.NextAvailable());
        return;
      }

      Zone zone = ZoneManager.Instance.GetZone(zoneId);
      if(zone == null) {
        return;
      }

      Adapter adapter = zone.SelectAdapter();
      if(adapter == null) {
        return;
      }

      Utils.Log("Consuming global ad slot");
      ConfigManager.Instance.globalIntervals.Consume();

      if(ConfigManager.Instance.globalIntervals.IsEmpty()) {
        Utils.Log("Global ad interval list empty");
        ConfigManager.Instance.RequestAdSources();
      }

      if(options != null) {
        if(options.pause) {
          PauseGame();
        }

        EventHandler finishedHandler = null;
        EventHandler skippedHandler = null;
        EventHandler failedHandler = null;
        EventHandler closeHandler = null;

        finishedHandler = (object sender, EventArgs e) => {
          _isShowing = false;
					if(options.resultCallback != null)
          options.resultCallback(ShowResult.Finished);
          if(options.pause) {
            ResumeGame();
          }
          adapter.Unsubscribe(Adapter.EventType.adFinished, finishedHandler);
          finishedHandler = null;
        };
        adapter.Subscribe(Adapter.EventType.adFinished, finishedHandler);      

        skippedHandler = (object sender, EventArgs e) => {
          _isShowing = false;
					if(options.resultCallback != null)
          options.resultCallback(ShowResult.Skipped);
          if(options.pause) {
            ResumeGame();
          }
          adapter.Unsubscribe(Adapter.EventType.adSkipped, skippedHandler);
          skippedHandler = null;
        };
        adapter.Subscribe(Adapter.EventType.adSkipped, skippedHandler);      

        failedHandler = (object sender, EventArgs e) => {
          _isShowing = false;
					if(options.resultCallback != null)
          options.resultCallback(ShowResult.Failed);
          if(options.pause) {
            ResumeGame();
          }
          adapter.Unsubscribe(Adapter.EventType.error, failedHandler);
          failedHandler = null;
        };
        adapter.Subscribe(Adapter.EventType.error, failedHandler);

        closeHandler = (object sender, EventArgs e) => {
          if(finishedHandler != null && skippedHandler != null && failedHandler != null) {
            _isShowing = false;
						if(options.resultCallback != null)
            options.resultCallback(ShowResult.Skipped);
						if(options.pause) {
							ResumeGame();
						}
          }
          adapter.Unsubscribe(Adapter.EventType.adDidClose, closeHandler);
        };
        adapter.Subscribe(Adapter.EventType.adDidClose, closeHandler);
      }

      Event.EventManager.sendMediationShowEvent(AppId, zone.Id, adapter.Id);
      adapter.Show(zone.Id, adapter.Id);
      _isShowing = true;
    }

    public bool isReady(string zoneId) {
      return ConfigManager.Instance.IsReady() && ZoneManager.Instance.IsReady(zoneId);
    }
  
    public bool isShowing() {
      return _isShowing;
    }

    private static void PauseGame() {
      _savedAudioVolume = AudioListener.volume;
      AudioListener.pause = true;
      AudioListener.volume = 0;
      _savedTimeScale = Time.timeScale;
      Time.timeScale = 0;
    }
    
    private static void ResumeGame() {
      Time.timeScale = _savedTimeScale;
      AudioListener.volume = _savedAudioVolume;
      AudioListener.pause = false;
    }
  }
}
