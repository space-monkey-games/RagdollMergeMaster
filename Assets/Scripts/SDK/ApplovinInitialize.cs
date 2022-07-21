using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplovinInitialize : MonoBehaviour
{
    string bannerAdUnitId = "afcf927ec69ee9ad"; // Retrieve the ID from your account

    // Start is called before the first frame update
    void Start()
    {
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
        {

            // AppLovin SDK is initialized, start loading ads
            // Banners are automatically sized to 320Ũ50 on phones and 728Ũ90 on tablets
            // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
            MaxSdk.CreateBanner(bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);

            // Set background or background color for banners to be fully functional
            MaxSdk.SetBannerBackgroundColor(bannerAdUnitId, Color.black);
            MaxSdk.ShowBanner(bannerAdUnitId);
        };

        MaxSdk.SetSdkKey("6AQkyPv9b4u7yTtMH9PT40gXg00uJOTsmBOf7hDxa_-FnNZvt_qTLnJAiKeb5-2_T8GsI_dGQKKKrtwZTlCzAR");
        //MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();
        /*
        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
            // Show Mediation Debugger
            MaxSdk.ShowMediationDebugger();
        };
        */
    }
}
