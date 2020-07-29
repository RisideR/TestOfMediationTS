using GoogleMobileAds.Api;
using UnityEngine;

namespace Meta.Monetization
{ 
    internal sealed class AdMobInitializer : MonoBehaviour
    {
        private void Awake()
        {
            InitializeAdMob();
        }

        private void InitializeAdMob()
        {
            MobileAds.Initialize(initStatus => { });
        }
    }
}