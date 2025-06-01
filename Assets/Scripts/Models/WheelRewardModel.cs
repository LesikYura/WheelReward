using System.Collections.Generic;

namespace Models
{
    public class WheelRewardModel
    {
        public List<WheelRewardItem> RewardItems { get; private set; }

        public WheelRewardModel(List<WheelRewardItem> rewardItems)
        {
            RewardItems = rewardItems ?? new List<WheelRewardItem>();
        }
    }
}