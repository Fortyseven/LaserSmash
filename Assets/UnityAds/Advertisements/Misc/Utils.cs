using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityEngine.Advertisements {

  internal static class Utils {

    public static string addUrlParameters(string url, Dictionary<string, object> parameters) {
      if(url.IndexOf('?') != -1) {
        url += "&";
      } else {
        url += "?";
      }

      List<string> pairs = new List<string>();
      foreach(KeyValuePair<string, object> entry in parameters) {
        if(entry.Value != null) {
          pairs.Add(entry.Key + "=" + entry.Value.ToString());
        }
      }

      return url + String.Join("&", pairs.ToArray());
    }

    public static string Join(IEnumerable enumerable, string separator) {
      string result = "";
      foreach(object entry in enumerable) {
        result += entry.ToString() + separator;
      }
      return result.Length > 0 ? result.Substring(0, result.Length - separator.Length) : result;
    }

    public static T Optional<T>(Dictionary<string, object> jsonObject, string key, object defaultValue = null) {
      try {
        return (T)jsonObject[key];
      } catch {
        return (T)defaultValue;
      }
    }

    public static void Log(string message) {
      if(Debug.isDebugBuild) {
        Debug.Log(message);
      }
    }

  }

}
