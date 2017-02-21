using UnityEngine;
using GoogleMobileAds.Api;
using System;

public partial class Game {

    string adMobAndroidBannerId = "ca-app-pub-7443772263346599/3673645666";
    string adMobAndroidInterstitialId = "ca-app-pub-7443772263346599/5150378867";

    string adMobIOSBannerId = "ca-app-pub-7443772263346599/9580578463";
    string adMobIOSInterstitialId = "ca-app-pub-7443772263346599/2057311668";

    void StartAds()
    {
        SetupBanner();
        //bannerView.Hide();
        SetupInterstitial();
    }

    void OnScreenOrientationChanged()
    {
        //SetupBanner();
    }

    BannerView bannerView;
    InterstitialAd interstitial;

    void OnRequestShowBanner()
    {
        bannerView.Show();
    }

    void OnRequestHideBanner()
    {
        if (bannerView != null)
        {
            bannerView.Hide();
        }
    }

    void SetupBanner()
    {
        if (bannerView != null)
        {
            return;
        }

        string adUnitId = "unused";
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                adUnitId = adMobAndroidBannerId;
                break;
            case RuntimePlatform.IPhonePlayer:
                adUnitId = adMobIOSBannerId;
                break;
            default:
                adUnitId = "unexpected platform";
                break;
        }

        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            foreach (RectTransform t in Layouts)
            {
                t.offsetMax = new Vector2(t.offsetMax.x, -AdSize.Banner.Height - 20);
            }
        }

        bannerView.OnAdLoaded += OnAdLoaded;
        bannerView.OnAdFailedToLoad += OnAdFailedToLoad;
        bannerView.OnAdOpening += OnAdOpening;
        bannerView.OnAdClosed += OnAdClosed;
        bannerView.OnAdLeavingApplication += OnAdLeavingApplication;

        // Create an empty ad request.
        AdRequest request = CreateAdRequest();
        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

    AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().
                AddTestDevice("31FEC0BCA3A8E9869852136ABDECEB40"). // My LG G4
                AddTestDevice("530f83e0ae10284f234af488e9c1b339"). // My iPad
                Build();
    }

    void SetupInterstitial()
    {
        if (interstitial != null)
        {
            return;
        }

        string adUnitId = "unused";
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
                adUnitId = adMobAndroidInterstitialId;
                break;
            case RuntimePlatform.IPhonePlayer:
                adUnitId = adMobIOSInterstitialId;
                break;
            default:
                adUnitId = "unexpected platform";
                break;
        }

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = CreateAdRequest();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }

    public void ShowInterstitial()
    {
        if (interstitial != null && interstitial.IsLoaded())
        {
            interstitial.Show();
        }
    }

    #region Banner callback handlers

    public void OnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLoaded event received.");
    }

    public void OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("HandleFailedToReceiveAd event received with message: " + args.Message);
    }

    public void OnAdOpening(object sender, EventArgs args)
    {
        Debug.Log("HandleAdOpened event received");
    }

    void OnAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleAdClosing event received");
    }

    public void OnAdLeavingApplication(object sender, EventArgs args)
    {
        Debug.Log("HandleAdLeftApplication event received");
    }

    #endregion
}
