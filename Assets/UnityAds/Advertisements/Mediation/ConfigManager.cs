using System;
using System.Collections.Generic;
using MiniJSON;

namespace UnityEngine.Advertisements {

  using HTTPLayer;

  internal class ConfigManager {
    
    public string configId { get; private set; }
    
    public long adSourceTtl { get; private set; }

    public long serverTimestamp { get; private set; }

    public long localTimestamp { get; private set; }

    public IntervalManager globalIntervals { get; private set; }

    static private readonly ConfigManager _sharedInstance = new ConfigManager();
    
    static public ConfigManager Instance {
      get {
        return _sharedInstance;
      }
    }
    
    private ConfigManager() {
    }

    public bool IsReady() {
      if(globalIntervals != null) {
        if((long)Math.Round(Time.realtimeSinceStartup) >= localTimestamp + adSourceTtl && !_requestingAdSources) {
          Utils.Log("Ad Source TTL expired");
          RequestAdSources();
        }
        return globalIntervals.IsAvailable();
      }
      return false;
    }

    private bool _requestingConfig = false;

    public void RequestConfig() {
      if(_requestingConfig) {
        return;
      }
      _requestingConfig = true;
      string configRequestUrl = Settings.mediationEndpoint + "/v1/games/" + Engine.Instance.AppId + "/config";
      //string configRequestUrl = Settings.serverEndpoint + "/testConfig.json";
      HTTPRequest request = new HTTPRequest("POST", configRequestUrl);
      request.addHeader("Content-Type", "application/json");
      if(configId != null) {
        request.addHeader("If-None-Match", configId);
      }
      request.setPayload(DeviceInfo.getJson());

      Utils.Log("Requesting new config from " + configRequestUrl);
      request.execute(HandleConfigResponse);
    }

    private void HandleConfigResponse(HTTPResponse response) {
      Dictionary<string, object> data;
      try {
        data = ParseResponse(response);
      }
      catch(Exception) {
        return;
      }

      if(data == null) {
        return;
      }

      Utils.Log("Received config response");

      if(response.headers != null && response.headers.ContainsKey("ETAG")) {
        string etag = response.headers["ETAG"];
        configId = etag.Substring(3, etag.Length - 4);
      }

      ZoneManager.Instance.ResetZones((List<object>)data["zones"]);
      adSourceTtl = (long)data["adSourceTtl"];
      serverTimestamp = (long)data["serverTimestamp"];

      RequestAdSources();

      _requestingConfig = false;
    }

    private bool _requestingAdSources = false;

    public void RequestAdSources(List<string> zoneIds = null) {
      if(_requestingAdSources) {
        return;
      }
      _requestingAdSources = true;
      string adSourcesRequestUrl = Settings.mediationEndpoint + "/v1/games/" + Engine.Instance.AppId + "/adSources";
      //string adSourcesRequestUrl = Settings.serverEndpoint + "/testAdSources.json";

      adSourcesRequestUrl = Utils.addUrlParameters(adSourcesRequestUrl, new Dictionary<string, object>() {
        {"zones", zoneIds != null ? String.Join(",", zoneIds.ToArray()) : null},
        {"config", configId}
      });

      HTTPRequest request = new HTTPRequest("POST", adSourcesRequestUrl);
      request.addHeader("Content-Type", "application/json");
      string payload = MiniJSON.Json.Serialize(new Dictionary<string, object>() {
        {"lastServerTimestamp", ConfigManager.Instance.serverTimestamp},
        {"adTimes", ZoneManager.Instance.GetConsumeTimes(ConfigManager.Instance.serverTimestamp)}
      });
      request.setPayload(payload);

      Utils.Log("Requesting new ad sources from " + adSourcesRequestUrl + " with payload: " + payload);
      Event.EventManager.sendMediationAdSourcesEvent(Engine.Instance.AppId);
      request.execute(HandleAdSourcesResponse);
    }

    private void HandleAdSourcesResponse(HTTPResponse response) {
      Dictionary<string, object> data;
      try {
        data = ParseResponse(response);
      }
      catch(Exception) {
        return;
      }

      if(data == null) {
        return;
      }

      Utils.Log("Received ad sources response");

      globalIntervals = new IntervalManager((List<object>)data["adIntervals"]);

      Utils.Log("Got " + globalIntervals + " intervals for global");

      serverTimestamp = (long)data["serverTimestamp"];
      localTimestamp = (long)Math.Round(Time.realtimeSinceStartup);

      ZoneManager.Instance.UpdateIntervals((Dictionary<string, object>)data["adSources"]);

      _requestingAdSources = false;
    }

    private Dictionary<string, object> ParseResponse(HTTPResponse response) {
			// TODO fix exception ArgumentNullException: Argument cannot be null. when connection is down 
			if (response == null) return null;
      string jsonString = System.Text.Encoding.UTF8.GetString(response.data, 0, response.dataLength);
      object jsonObject = MiniJSON.Json.Deserialize(jsonString);
      if(jsonObject != null && jsonObject is Dictionary<string, object>) {
        Dictionary<string, object> json = (Dictionary<string, object>)jsonObject;
        long statusCode = (long)json["status"];
        if(statusCode != 200) {
          string errorMessage = (string)json["error"];
          throw new Exception(errorMessage);
        }
        Dictionary<string, object> data = (Dictionary<string, object>)json["data"];
        return data;
      }
      return null;
    }

  }

}

