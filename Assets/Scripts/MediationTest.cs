using System;
using UnityEngine;
using GoogleMobileAdsMediationTestSuite.Api;

namespace Meta.Monetization
{
    public class MediationTest : MonoBehaviour
    {
        private void Start()
        {
            MediationTestSuite.OnMediationTestSuiteDismissed += this.HandleMediationTestSuiteDismissed;
        }

        public void ShowMediationTestSuite()
        {
            MediationTestSuite.Show();
        }

        private void HandleMediationTestSuiteDismissed(object sender, EventArgs args)
        {
            Debug.LogWarning("HandleMediationTestSuiteDismissed event received");
        }
    }
}