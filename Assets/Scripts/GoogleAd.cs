using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using UnityEngine.UI;

//TODO App id: ca-app-pub-1128252923742458~6886626020
public class GoogleAd : MonoBehaviour {    
    
    private const string InterstialIdName = "ca-app-pub-1128252923742458/4507874697";
    private const string ResumeRewardIdName = "ca-app-pub-1128252923742458/1634299342";
    private const string BannerIdName = "ca-app-pub-1128252923742458/2835700373";
    
    private InterstitialAd _interstitialAd;
    private RewardedAd _rewardedAdForResume;
    private BannerView _bannerView;

    public bool _bannerAdLoaded;
    
    //TEMP
    //public Text debugText;


    private void Start() {
        
        Initialize();
    }

    
    private void Initialize() {
        
        MobileAds.Initialize(status => { });

        OnInitializedInterstitial();
        OnInitializedRewardForResume();
        OnInitializedBanner();
    }



    #region Interstitial
    private void OnInitializedInterstitial() {
        
        _interstitialAd?.Destroy();

        _interstitialAd = new InterstitialAd(InterstialIdName);
        
        _interstitialAd.OnAdLoaded += HandleOnAdLoaded;
        _interstitialAd.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        _interstitialAd.OnAdOpening += HandleOnAdOpened;
        _interstitialAd.OnAdClosed += HandleOnAdClosed;
        //_interstitialAd.onad
        
        _interstitialAd.LoadAd(new AdRequest.Builder().Build());
    }
    
    public void ShowInterstialAD()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable) return;

        if (!_interstitialAd.IsLoaded())
        {
            //debugText.text += ":NO:";
            OnInitializedInterstitial();
            return;
        }

        _interstitialAd.Show();
        //debugText.text += ":YES:";
    }
    #endregion
    
    
    
    #region Reward

    private void OnInitializedRewardForResume()
    {

        _rewardedAdForResume?.Destroy();

        _rewardedAdForResume = new RewardedAd(ResumeRewardIdName);

        _rewardedAdForResume.OnAdLoaded += HandleOnAdLoaded;
        _rewardedAdForResume.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        _rewardedAdForResume.OnAdOpening += HandleOnAdOpened;
        _rewardedAdForResume.OnAdClosed += HandleOnAdClosed;
        _rewardedAdForResume.OnUserEarnedReward += HandleUserEarnedReward;

        _rewardedAdForResume.LoadAd(new AdRequest.Builder().Build());
    }

    public void ShowReloadRewardAd()
    {
        //GameObject.Find("AudioManager").GetComponent<AudioManager>().PlaySound("OtherButtons");

        if (!_rewardedAdForResume.IsLoaded())
        {

            OnInitializedRewardForResume();
            return;
        }
        _rewardedAdForResume.Show();
    }
    #endregion

    #region Banner
    private void OnInitializedBanner() {
        
        _bannerView?.Destroy();

        _bannerView = new BannerView(BannerIdName, AdSize.SmartBanner, AdPosition.Bottom);
        
        _bannerView.OnAdLoaded += HandleOnAdLoaded;
        _bannerView.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        _bannerView.OnAdOpening += HandleOnAdOpened;
        _bannerView.OnAdClosed += HandleOnAdClosed;
        
        _bannerView.LoadAd(new AdRequest.Builder().Build());
    }

    public void ShowBannerAd() {

        _bannerAdLoaded = true;
        _bannerView.Show();
    }

    public void HideBannerAd() {
        
        _bannerView.Hide();
    }

    public void DestroyBannerAd()
    {
        _bannerView?.Destroy();        
    }
    #endregion


    private string GetAdId(object sender) {

        if (sender.Equals(_bannerView)) return "bannerView";
        else if (sender.Equals(_interstitialAd)) return "interstitialAd";
        else if (sender.Equals(_rewardedAdForResume)) return "rewardedAdForResume";
        else return null;
    }
    private void HandleOnAdLoaded(object sender, EventArgs args) {

        //debugText.text += "\n" + GetAdId(sender) + ":loaded:";
    }

    private void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args) {
        
        //debugText.text += "\n" + GetAdId(sender) + ":Failed:";
        DestroyAndInitialize(sender);
    }

    private void HandleOnAdOpened(object sender, EventArgs args) {
        
        //debugText.text += "\n" + GetAdId(sender) + ":Opened:";
    }

    private void HandleOnAdClosed(object semder, EventArgs args) {

        DestroyAndInitialize(semder);
        //debugText.text += ":Closed:";
    }

    private void HandleUserEarnedReward(object sender, Reward args) {
        
        string type = args.Type;
        double amount = args.Amount;
        //debugText.text += "\n" + ":Just Take It:";
    }

    private void DestroyAndInitialize(object sender)
    {
        if (sender.Equals(_bannerView)) {
            
            //debugText.text += ":Ban Id:";
            //_bannerView.Destroy();
            OnInitializedBanner();
        }
        else if (sender.Equals(_interstitialAd)) {
            
            //debugText.text += ":Inter:";

            //UnityEngine.SceneManagement.SceneManager.LoadScene(0);

            //_interstitialAd.Destroy();
            OnInitializedInterstitial();
        }

        else if (sender.Equals(_rewardedAdForResume))
        {
            //debugText.text += ":ReloadReward:";

            //UnityEngine.SceneManagement.SceneManager.LoadScene(1);

            //_rewardedAd.Destroy();
            OnInitializedRewardForResume();
        }
    }
}
