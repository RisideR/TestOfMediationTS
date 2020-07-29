using System;

namespace Meta.Monetization
{
    [Flags]
    internal enum RewardedAdEvents
    {
        None = 0,
        NeedToShow = 1,
        AdLoaded = 2,
        AdFailedToLoad = 4,
        AdOpening = 8,
        AdFailedToShow = 16,
        AdClosed = 32,
        UserEarnedReward = 64
    }
}