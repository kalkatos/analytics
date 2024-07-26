// (c) 2023 Alex Kalkatos
// This code is licensed under MIT license (see LICENSE.txt for details)

#if UNITY_2018_1_OR_NEWER && KALKATOS_SCRIPTABLE

using UnityEngine;
using Kalkatos.UnityGame.Scriptable;

namespace Kalkatos.Analytics.Unity
{
    public class TemplateAnalytics : MonoBehaviour
    {
        [SerializeField] private Signal onConnectionSuccessSignal;
        [SerializeField] private Signal onConnectionFailureSignal;

        // Analytics event handles
        private const string SESSION_START = "session_start";
        private const string COUNT_NEW_DAY = "count_new_day";
        private const string CONNECTION_FAILURE = "connection_failure";
        private const string CONNECTION_SUCCESS = "connection_success";
        // Storage save handles
        private const string LAST_PLAYED_DAY = "LastPlayedDay";
        private const string PLAYED_DAYS_COUNT = "PlayedDaysCount";
        private const string SESSION_CLICKS_PLAY = "SessionClicksPlay";

        private void Awake ()
        {
            AnalyticsController.Initialize(new NetworkAnalyticsSender(this));
            AnalyticsController.SendEvent(SESSION_START);
            long currentDay = System.DateTime.Today.Ticks;
            long savedDay = long.Parse(Storage.Load(LAST_PLAYED_DAY, "0"));
            int dayCount = Storage.Load(PLAYED_DAYS_COUNT, -1);
            bool isNewDay = currentDay != savedDay;
            if (isNewDay)
            {
                AnalyticsController.SendEventWithNumber(COUNT_NEW_DAY, ++dayCount);
                Storage.Save(LAST_PLAYED_DAY, currentDay.ToString());
                Storage.Save(PLAYED_DAYS_COUNT, dayCount);
                Storage.Delete(SESSION_CLICKS_PLAY);
            }
            SubscribeToSignals();

            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy ()
        {
            UnsubscribeToSignals();
        }

        private void SubscribeToSignals ()
        {
            onConnectionSuccessSignal.OnSignalEmitted.AddListener(HandleConnectionSuccess);
            onConnectionFailureSignal.OnSignalEmitted.AddListener(HandleConnectionFailure);
        }
        private void UnsubscribeToSignals ()
        {
            onConnectionSuccessSignal.OnSignalEmitted.RemoveListener(HandleConnectionSuccess);
            onConnectionFailureSignal.OnSignalEmitted.RemoveListener(HandleConnectionFailure);
        }

        private void HandleConnectionSuccess ()
        {
            AnalyticsController.SendEvent(CONNECTION_SUCCESS);
        }

        private void HandleConnectionFailure ()
        {
            AnalyticsController.SendEvent(CONNECTION_FAILURE);
        }
    }
}

#endif