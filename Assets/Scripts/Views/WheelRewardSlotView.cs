using Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Views
{
    public class WheelRewardSlotView : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _amountText;

        public void Init(WheelRewardItem viewModel)
        {
            _nameText.text = viewModel.Id;
            _amountText.text = viewModel.Amount.ToString();
            _iconImage.sprite = viewModel.Icon;
        }
    }
}
