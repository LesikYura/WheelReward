using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class WheelRewardItem
    {
        public string Id;
        public Sprite Icon;
        public int Amount;
        public float Weight; // Drop chance weight
    }
}
