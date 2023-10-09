// (c) 2023 Alex Kalkatos
// This code is licensed under MIT license (see LICENSE.txt for details)

namespace Kalkatos.Analytics
{
    public class AnalyticsController
    {
		public static bool SendDebug;

		private static IAnalyticsSender sender;

		public static void Initialize (IAnalyticsSender sender, bool sendDebug = true)
        {
			AnalyticsController.sender = sender;
			SendDebug = sendDebug;
        }

		public static void SendEvent (string name)
        {
			if (!CheckInitialization())
				return;
			if (SendDebug)
				Logger.Log($"[AnalyticsController] Event: {name}");
            sender.SendEvent(name);
        }

		public static void SendEventWithString (string name, string str)
        {
			if (!CheckInitialization())
				return;
			if (SendDebug)
				Logger.Log($"[AnalyticsController] Event: {name} with value: {str}");
			sender.SendEventWithString(name, str); 
        }

		public static void SendEventWithNumber (string name, float value)
        {
			if (!CheckInitialization())
				return;
			if (SendDebug)
				Logger.Log($"[AnalyticsController] Event: {name} with value: {value}");
			sender.SendEventWithNumber(name, value); 
        }

		public static void SendUniqueEvent (string name, string optValue = null)
        {
			if (!CheckInitialization())
				return;
			string key = string.IsNullOrEmpty(optValue) ? $"UniqueEvent:{name}" : $"UniqueEvent:{name}:{optValue}";
			if (!string.IsNullOrEmpty(Storage.Load(key, null)))
				return;
			SendEventWithString(name, optValue);
			Storage.Save(key, "");
        }

		public static void SendEventWithFilter (string name, float value, params float[] orderedFilterTiers)
        {
			if (!CheckInitialization())
				return;
			if (orderedFilterTiers == null)
            {
				Logger.LogError("[AnalyticsController] orderedFilterTiers is null.");
                return; 
            }
            for (int i = 0; i < orderedFilterTiers.Length; i++)
            {
				if (value <= orderedFilterTiers[i])
				{
					SendEventWithNumber(name, orderedFilterTiers[i]);
					return;
				}
            }
        }

		private static bool CheckInitialization ()
        {
			if (sender == null)
            {
                Logger.LogError("AnalyticsController must be initialized. Call Initialize first with a sender.");
				return false;
            }
			return true;
        }
	}
}