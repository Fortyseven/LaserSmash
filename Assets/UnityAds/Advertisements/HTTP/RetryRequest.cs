namespace UnityEngine.Advertisements.HTTPLayer {

  using UnityEngine;
  using UnityEngine.Advertisements.Event;
  using System.Collections;

  internal class RetryRequest {
    private int retryPosition;
    private int[] retryDelayTable;
    private HTTPRequest request;
    private System.Action<HTTPResponse> callback;
    private bool keepRetrying;
    private bool callbackDelivered;
    private bool useDeadline = false;
    private float retryDeadline = 0;
    private int deadlineDelay = 0;

    public RetryRequest(int[] delays, int maxDelay, HTTPRequest req) {
      retryPosition = 0;
      retryDelayTable = delays;

      if(maxDelay > 0) {
        deadlineDelay = maxDelay;
        useDeadline = true;
      }

      request = req;
    }

    public void execute(System.Action<HTTPResponse> eventCallback) {
      callback = eventCallback;

      keepRetrying = true;
      callbackDelivered = false;

      if(useDeadline) {
        retryDeadline = Time.realtimeSinceStartup + deadlineDelay;
      }

      retry();

      if(useDeadline) {
        AsyncExec.runWithDelay(deadlineDelay, executeDeadline);
      }
    }

    private void HTTPCallback(HTTPResponse res) {
      //Utils.Log("HTTPCallback at " + Time.realtimeSinceStartup);

      // network error
      if(res.error) {
        if(!keepRetrying && !callbackDelivered) {
          failedCallback("Network error");
        }
  
        return;
      }

      EventJSON jsonResponse = new EventJSON(System.Text.Encoding.UTF8.GetString(res.data, 0, res.data.Length));

      // check that server response has status "ok"
      if(jsonResponse.hasInt("status")) {
        if(jsonResponse.getInt("status") == 200) {
          // event delivery successful
          keepRetrying = false;

          if(!callbackDelivered) {
            callbackDelivered = true;
            callback(res);
          }

          return;
        } 
      }

      // if we didn't get status "ok", then whatever we got will be treated as error

      if(jsonResponse.hasBool("retryable")) {
        bool retry = jsonResponse.getBool("retryable");

        if(!retry) {
          // We have received an error and retrying has been explicitly forbidden
          keepRetrying = false;

          if(!callbackDelivered) {
            failedCallback("Retrying forbidden by remote server");
          }

          return;
        }
      }

      // We have received an error so if there are no more retries, deliver the callback
      if(!keepRetrying && !callbackDelivered) {
        failedCallback("Error");
      }
    }

    private void retry() {
      if(!keepRetrying) {
        return;
      }

      HTTPRequest req = request.getClone();
      req.execute(HTTPCallback);

      if(retryPosition < retryDelayTable.Length && (!useDeadline || Time.realtimeSinceStartup < retryDeadline)) {
        int delay = retryDelayTable[retryPosition++];

        if(delay > 0) {
          AsyncExec.runWithDelay(delay, retry);
        } else {
          keepRetrying = false;
        }
      } else {
        keepRetrying = false;
      }
    }

    private void executeDeadline() {
      keepRetrying = false;

      if(!callbackDelivered) {
        failedCallback("Retry deadline exceeded");
      }
    }

    private void failedCallback(string msg) {
      callbackDelivered = true;

      HTTPResponse res = new HTTPResponse();
      res.url = request.url;
      res.error = true;
      res.errorMsg = msg;

      callback(res);
    }
  }
}
