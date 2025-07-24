namespace _Game.Scripts._GameLogic.Logic.Grid.Native
{
    public static class RuntimeGridObjectGenerateDataCache
    {
        public static int CurrentObjectCount { get; set; }
        public static double CurrentTime { get; set; }
        
        public static int GetCurrentObjectCount()
        {
            return CurrentObjectCount;
        }
        
        public static double GetCurrentTime()
        {
            return CurrentTime;
        }
    }
}