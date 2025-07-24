using UnityEngine;

namespace _Game.Scripts._GameLogic.Data.UI
{
    [CreateAssetMenu(fileName = nameof(GridObjectGenerateUIData), menuName = "Merge Valley/Data/Grid Object Generate UI Data", order = 0)]
    public class GridObjectGenerateUIData : ScriptableObject
    {
        [SerializeField] private int _maxObjectCount;
        [SerializeField] private double _maxTime;
        [SerializeField] private int _objectIncreaseCount;
        
        public int MaxObjectCount => _maxObjectCount;
        
        public int ObjectIncreaseCount => _objectIncreaseCount;
        public double MaxTime => _maxTime;
        
    }
}