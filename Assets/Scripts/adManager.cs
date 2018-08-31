using UnityEngine;
using UnityEngine.SocialPlatforms;
using System.Collections;
using GoogleMobileAds;
using GoogleMobileAds.Android;
using GoogleMobileAds.Api;
public class adManager : MonoBehaviour {

	public const string AD_UNIT_ID = "ca-app-pub-4548534297442768/2144896134";
	public bool m_addHidden = true;
	BannerView m_bannerView;
	InterstitialAd m_interstitial;

	void Awake () 
	{
		DontDestroyOnLoad(this);
	}
	// Use this for initialization
	void Start () 
	{
		RequestBanner();
		RequestInterstitial();
	}

	// Update is called once per frame
	void Update ()
	{
	}

	void OnLevelWasLoaded(int level) {
		if (Application.loadedLevelName == "Endless")
			m_bannerView.Hide();
		
	}

	private void RequestBanner()
	{
		#if UNITY_ANDROID
		string adUnitId = AD_UNIT_ID;
		#else
		string adUnitId = "unexpected_platform";
		#endif

		if(m_bannerView != null)
			m_bannerView.Destroy();

		// Create a 320x50 banner at the top of the screen.
		m_bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
		m_bannerView.AdFailedToLoad += HandleAdFailedToLoad;
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().AddTestDevice("C3699CC65329C0121E4F68C2761A747C").Build();
		// Load the banner with the request.
		m_bannerView.LoadAd(request);
	}

	private void RequestInterstitial()
	{
		#if UNITY_ANDROID
		string adUnitId = AD_UNIT_ID;
		#else
		string adUnitId = "unexpected_platform";
		#endif

		if(m_interstitial != null)
			m_interstitial.Destroy();

		// Initialize an InterstitialAd.
		m_interstitial = new InterstitialAd(adUnitId);
		m_interstitial.AdClosed += HandleInterstitialAdClosed;
		m_interstitial.AdFailedToLoad += HandleInterstitialAdFailedToLoad;
		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().AddTestDevice("C3699CC65329C0121E4F68C2761A747C").Build();
		// Load the interstitial with the request.
		m_interstitial.LoadAd(request);
	}

	public void gameOver()
	{
		if(m_interstitial.IsLoaded())
			m_interstitial.Show();

		RequestBanner();
	}

	public void HandleInterstitialAdClosed(object sender, System.EventArgs args)
	{
		RequestInterstitial();
		// Handle the ad loaded event.
	}

	public void HandleInterstitialAdFailedToLoad(object sender, System.EventArgs args)
	{
		RequestInterstitial();
		// Handle the ad loaded event.
	}

	/*public void HandleAdLoaded(object sender, System.EventArgs args)
	{
		Debug.Log("HandleAdLoaded event received.");
		// Handle the ad loaded event.
	}*/

	public void HandleAdFailedToLoad(object sender, System.EventArgs args)
	{
		RequestBanner();
		// Handle the ad failed to load event.
	}
}
