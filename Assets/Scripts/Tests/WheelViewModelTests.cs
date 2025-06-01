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
                new WheelRewardItem { Id = "Gold", Amount = 100, Weight = 1f }
            };

            var vm = new WheelViewModel(config);
            WheelRewardItem result = null;
            vm.OnSpinCompleted += r => result = r;

            vm.Spin();

            Assert.IsNotNull(result);
            Assert.AreEqual("Gold", result.Id);
        }
    }
}
