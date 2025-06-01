using System.Collections.Generic;
using Models;
using UnityEngine;
using ViewModels;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class WheelViewModelTests 
    {
        [Test]
        public void Spin_Generates_ValidReward()
        {
            var config = ScriptableObject.CreateInstance<WheelConfig>();
            config.Rewards = new List<WheelRewardItem>
            {
                new WheelRewardItem { Id = "coins_100", Amount = 100, Weight = 1f },
            };

            config.FullRotations = 1;
            config.Duration = 1;
            config.WheelRadius = 300;

            var vm = new WheelViewModel(config);
            vm.OnSpinCompleted += CheckResult;

            vm.Spin();
        }

        private void CheckResult(WheelRewardItem result)
        {
            Assert.IsNotNull(result);
            Assert.AreEqual("coins_100", result.Id);
        }
    }
}
