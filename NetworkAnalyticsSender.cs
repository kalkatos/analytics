// (c) 2024 Alex Kalkatos
// This code is licensed under MIT license (see LICENSE.txt for details)

using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;


#if KALKATOS_NETWORK
using Kalkatos.Network.Unity;
#endif

namespace Kalkatos.Analytics.Unity
{
    public class NetworkAnalyticsSender : IAnalyticsSender
    {
        private string id;
#if KALKATOS_NETWORK
        private UnityWebRequestComnunicator communicator;
        private string functionsPrefix;
#endif

        public NetworkAnalyticsSender (MonoBehaviour mono)
        {
            id = Storage.Load("NetworkAnalyticsSender-Id", "");
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();
                Storage.Save("NetworkAnalyticsSender-Id", id);
            }
#if KALKATOS_NETWORK
            communicator = new UnityWebRequestComnunicator(mono);
            string prefix = Storage.Load("UrlPrefix", functionsPrefix);
            if (string.IsNullOrEmpty(prefix))
            {
                Logger.LogError("[NetworkAnalytics] Couldn't find network prefix.");
                return;
            }
            functionsPrefix = prefix;
#else
            Logger.LogError("[NetworkAnalytics] Kalkatos.Network module is not available.");
#endif
        }
        public void SendEvent (string name)
        {
            Dictionary<string, string> obj = new()
            {
                { "PlayerId", id },
                { "Key", name },
                { "Value", "" }
            };
            communicator.Post(functionsPrefix + "SendEvent", JsonConvert.SerializeObject(obj), null);
        }

        public void SendEventWithNumber (string name, float value)
        {
            Dictionary<string, string> obj = new()
            {
                { "PlayerId", id },
                { "Key", name },
                { "Value", value.ToString() }
            };
            communicator.Post(functionsPrefix + "SendEvent", JsonConvert.SerializeObject(obj), null);
        }

        public void SendEventWithString (string name, string str)
        {
            Dictionary<string, string> obj = new()
            {
                { "PlayerId", id },
                { "Key", name },
                { "Value", str }
            };
            communicator.Post(functionsPrefix + "SendEvent", JsonConvert.SerializeObject(obj), null);
        }
    }
}
