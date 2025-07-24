﻿using _Game.Scripts.Managers.Core;
using _Game.Scripts.ScriptableObjects.Saveable;
using UnityEngine;

namespace _Game.Scripts.UI.Buttons
{
    public class NextLevelButton : ButtonBase
    {
        protected override void OnClicked()
        {
            EventManager.InGameEvents.LoadLevel?.Invoke();
        }
    }
}
