using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine;

public class Intersititial : MonoBehaviour
{
    private InterstitialAd _interstitialAd;

#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/1033173712"; // Test Ad Unit for Android
#elif UNITY_IPHONE
    private string _adUnitId = "ca-app-pub-3940256099942544/4411468910"; // Test Ad Unit for iOS
#else
    private string _adUnitId = "unused";
#endif

    private void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            Debug.Log("Google Mobile Ads SDK initialized.");
        });

        // Load the interstitial ad.
        LoadInterstitialAd();
    }
    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null)
        {
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogWarning("Interstitial ad is not ready yet.");
        }
    }

    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
            });
    }
}
