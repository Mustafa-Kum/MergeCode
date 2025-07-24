using System.Collections.Generic;

namespace _Game.Scripts._GameLogic.Logic.Grid.Native
{
    public interface IGridObjectClickableAction
    {
        void OnObjectGenerated(GridObject gridObject);
        void OnObjectClick(GridObject gridObject);
        void OnObjectMerge(GridObject gridObject);
    }
}