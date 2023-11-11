#if UNITY_2018_1_OR_NEWER && GAME_ANALYTICS

using GameAnalyticsSDK;

namespace Kalkatos.Analytics.Unity
{
    public class GameAnalyticsSender : IAnalyticsSender
    {
        public GameAnalyticsSender ()
        {
            GameAnalytics.Initialize();
        }

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