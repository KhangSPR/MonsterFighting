using System;
using System.Collections.Generic;
using UnityEngine;

namespace UIGameDataMap
{
    [Serializable]
    public struct ReWardIcon
    {
        public Sprite icon;
        public ItemType resourceType;
    }
    [CreateAssetMenu(fileName = "Assets/Resources/GameData/Icons", menuName = "UIGameDataMap/Icons", order = 10)]
    public class GameMapIconSO : ScriptableObject
    {
        public List<ReWardIcon> rewardIcons;

        public Sprite GetReWardIcon(ItemType resourceType)
        {
            if (rewardIcons == null || rewardIcons.Count == 0)
                return null;

            ReWardIcon match = rewardIcons.Find(x => x.resourceType == resourceType);
            return match.icon;
        }
    }

}
