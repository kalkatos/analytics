#if UNITY_2018_1_OR_NEWER

using UnityEngine;
using GameAnalyticsSDK;

namespace Kalkatos.Analytics.Unity
{
    public class GameAnalyticsSender : MonoBehaviour, IAnalyticsSender
    {
        public void SendEvent (string name)
        {
            GameAnalytics.NewDesignEvent(name);
        }

        public void SendEventWithNumber (string name, float value)
        {
            GameAnalytics.NewDesignEvent(name, value);
        }

        public void SendEventWithString (string name, string str)
        {
            GameAnalytics.NewDesignEvent($"{name}:{str}");
        }
    }
}

#endif