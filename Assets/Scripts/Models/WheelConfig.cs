using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [CreateAssetMenu(menuName = "WheelReward/WheelConfig")]
    public class WheelConfig : ScriptableObject
    {
        public List<WheelRewardItem> Rewards;
        
        [Header("Animation")]
        public float WheelRadius = 300; 
        public int FullRotations = 5; 
    }
}
