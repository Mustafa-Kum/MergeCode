namespace _Game.Scripts.General.AnalyticsManager
{
    public class UnityAnalyticsService : IAnalyticsService
    {
        public void LogEvent(string eventName, AnalyticData parameters)
        {
            // Implement Unity Analytics logic here
        }
    }

    public interface IAnalyticsService
    {
        void LogEvent(string eventName, AnalyticData parameters);
    }
}