#if UNITY_IPHONE

using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

namespace UnityEngine.Advertisements {
	internal static class UnityAdsIosBridge {
		[DllImport ("__Internal")]
		public static extern void init (string gameId, bool testModeEnabled, bool debugModeEnabled, string gameObjectName);
		
		[DllImport ("__Internal")]
		public static extern bool show (string zoneId, string rewardItemKey, string options);
		
		[DllImport ("__Internal")]
		public static extern void hide ();
		
		[DllImport ("__Internal")]
		public static extern bool isSupported ();
		
		[DllImport ("__Internal")]
		public static extern string getSDKVersion ();
		
		[DllImport ("__Internal")]
		public static extern bool canShowAds ();
		
		[DllImport ("__Internal")]
		public static extern bool canShow ();
		
		[DllImport ("__Internal")]
		public static extern bool hasMultipleRewardItems ();
		
		[DllImport ("__Internal")]
		public static extern string getRewardItemKeys ();
		
		[DllImport ("__Internal")]
		public static extern string getDefaultRewardItemKey ();
		
		[DllImport ("__Internal")]
		public static extern string getCurrentRewardItemKey ();
		
		[DllImport ("__Internal")]
		public static extern bool setRewardItemKey (string rewardItemKey);
		
		[DllImport ("__Internal")]
		public static extern void setDefaultRewardItemAsRewardItem ();
		
		[DllImport ("__Internal")]
		public static extern string getRewardItemDetailsWithKey (string rewardItemKey);
		
		[DllImport ("__Internal")]
		public static extern string getRewardItemDetailsKeys ();

    [DllImport ("__Internal")]
    public static extern void setNetwork(string network);
	}
 }

#endif