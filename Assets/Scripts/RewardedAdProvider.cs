using GoogleMobileAds.Api;
using System;
using System.Collections;
using UnityEngine;

namespace Meta.Monetization
{
    internal sealed class RewardedAdProvider : MonoBehaviour
    {
        private const string AdId = "ca-app-pub-8878529370048307/2928066691";
        private const int LoadAdAfterFailCooldown = 10;

        public static RewardedAdProvider Instance { get; private set; }

        private RewardedAd _rewardedAd;

        private RewardedAdEvents _adEvent;

        private void Start()
        {
            Instance = this;

            CreateAndLoadRewardedAd();
        }

        private void Update()
        {
            if (_adEvent != 0)
            {
                HandleAdEvent();
                _adEvent = 0;
            }
        }

        public void Show()
        {
            _adEvent = RewardedAdEvents.NeedToShow;
        }

        private void CreateAndLoadRewardedAd()
        {
            print($"ad id: {AdId}");

            Unsubscribe();

            _rewardedAd = new RewardedAd(AdId);

            Subscribe();

            AdRequest request = new AdRequest.Builder()
                .Build();

            _rewardedAd.LoadAd(request);
        }

        private void RewardedAdFailedToLoad()
        {
            StartCoroutine(TryToLoadNewAdAfterFail());
        }

        private void RewardedAdClosed()
        {
            CreateAndLoadRewardedAd();
        }

        private void HandleAdEvent()
        {
            if (_adEvent.HasFlag(RewardedAdEvents.NeedToShow))
            {
                TryToShow();
            }

            if (_adEvent.HasFlag(RewardedAdEvents.AdFailedToLoad))
            {
                RewardedAdFailedToLoad();
            }

            if (_adEvent.HasFlag(RewardedAdEvents.AdClosed))
            {
                RewardedAdClosed();
            }
        }

        private void TryToShow()
        {
            if (_rewardedAd.IsLoaded())
            {
                _rewardedAd.Show();
            }
            else
            {
                print("Ad hasn't loaded yet");
            }
        }

        private void Subscribe()
        {
            _rewardedAd.OnAdLoaded += OnRewardedAdLoaded;
            _rewardedAd.OnAdFailedToLoad += OnRewardedAdFailedToLoad;
            _rewardedAd.OnAdOpening += OnRewardedAdOpening;
            _rewardedAd.OnAdFailedToShow += OnRewardedAdFailedToShow;
            _rewardedAd.OnAdClosed += OnRewardedAdClosed;
            _rewardedAd.OnUserEarnedReward += OnRewardedAdUserEarnedReward;
        }

        private void Unsubscribe()
        {
            if (_rewardedAd == null)
            {
                return;
            }

            _rewardedAd.OnAdLoaded -= OnRewardedAdLoaded;
            _rewardedAd.OnAdFailedToLoad -= OnRewardedAdFailedToLoad;
            _rewardedAd.OnAdOpening -= OnRewardedAdOpening;
            _rewardedAd.OnAdFailedToShow -= OnRewardedAdFailedToShow;
            _rewardedAd.OnAdClosed -= OnRewardedAdClosed;
            _rewardedAd.OnUserEarnedReward -= OnRewardedAdUserEarnedReward;
        }

        private void OnRewardedAdUserEarnedReward(object sender, Reward e)
        {
            print("OnRewardedAdUserEarnedReward");

            _adEvent |= RewardedAdEvents.UserEarnedReward;
        }

        private void OnRewardedAdClosed(object sender, EventArgs e)
        {
            print("OnRewardedAdClosed");

            _adEvent |= RewardedAdEvents.AdClosed;
        }

        private void OnRewardedAdFailedToShow(object sender, AdErrorEventArgs e)
        {
            print("OnRewardedAdFailedToShow");
            print($"Error message: {e.Message}");

            _adEvent |= RewardedAdEvents.AdFailedToShow;
        }

        private void OnRewardedAdOpening(object sender, EventArgs e)
        {
            print("OnRewardedAdOpening");
            _adEvent |= RewardedAdEvents.AdOpening;
        }

        private void OnRewardedAdFailedToLoad(object sender, AdErrorEventArgs e)
        {
            print("OnRewardedAdFailedToLoad");
            print($"Error message: {e.Message}");

            _adEvent |= RewardedAdEvents.AdFailedToLoad;
        }

        private void OnRewardedAdLoaded(object sender, EventArgs e)
        {
            print("OnRewardedAdLoaded");

            _adEvent |= RewardedAdEvents.AdLoaded;
        }

        private IEnumerator TryToLoadNewAdAfterFail()
        {
            yield return new WaitForSeconds(LoadAdAfterFailCooldown);

            CreateAndLoadRewardedAd();
        }
    }
}