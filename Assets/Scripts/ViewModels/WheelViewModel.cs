using System;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEngine;

namespace ViewModels
{
    public class WheelViewModel
    {
        public List<WheelRewardItem> Items { get; private set; }

        public event Action<WheelRewardItem> OnSpinCompleted;
        public event Action<WheelState> OnStateChanged;
        public event Action<float, Action> OnAnimationParamsReady;
        
        private WheelState _state;
        private WheelConfig _config;
        private System.Random _random = new System.Random();

        public WheelViewModel(WheelConfig config)
        {
            _config = config;
            Items = config.Rewards.ToList();
            SetState(WheelState.Idle);
        }

        public void Spin()
        {
            if (_state != WheelState.Idle) 
                return;

            SetState(WheelState.Spinning);
            
            var reward = GetRandomReward();
            var targetAngle = CalculateTargetAngle(Items.IndexOf(reward));
            OnAnimationParamsReady?.Invoke(targetAngle, () =>
            {
                CompleteSpin(reward);
            });
        }

        private void CompleteSpin(WheelRewardItem reward)
        {
            SetState(WheelState.Reward);
            OnSpinCompleted?.Invoke(reward);
            SetState(WheelState.Idle);
        }

        private void SetState(WheelState newState)
        {
            _state = newState;
            OnStateChanged?.Invoke(_state);
        }

        private WheelRewardItem GetRandomReward()
        {
            var totalWeight = _config.Rewards.Sum(r => r.Weight);
            var roll = _random.NextDouble() * totalWeight;
            var cumulative = 0f;

            foreach (var reward in _config.Rewards)
            {
                cumulative += reward.Weight;
                if (roll <= cumulative)
                {
                    return reward;
                }
            }

            return _config.Rewards.Last(); // fallback
        }
        
        private int CalculateTargetAngle(int targetIndex)
        {
            var anglePerItem = 360f / Items.Count;
            var compensationAngle = -15f; // pointer sprite spec
            var targetAngle = (targetIndex * anglePerItem) + (anglePerItem / 2f) + compensationAngle; // centartion
            
            return (int)targetAngle;
        }
    }
}
