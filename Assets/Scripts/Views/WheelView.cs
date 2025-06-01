using System;
using System.Collections.Generic;
using DG.Tweening;
using Models;
using UnityEngine;
using UnityEngine.UI;
using ViewModels;

namespace Views
{
    public class WheelView : MonoBehaviour
    {
        [SerializeField] private List<WheelRewardSlotView> slots;
        [SerializeField] private WheelConfig config;
        [SerializeField] private Button spinButton;
        [SerializeField] private Transform pointer;
        [SerializeField] private Transform wheelContainer;
        [SerializeField] private GameObject itemViewPrefab;

        private WheelViewModel _viewModel;
        private float _itemAngleStep;
        
        private readonly List<WheelRewardSlotView> _itemViews = new List<WheelRewardSlotView>();

        private void Start()
        {
            _viewModel = new WheelViewModel(config);
            _viewModel.OnSpinCompleted += HandleReward;
            _viewModel.OnStateChanged += HandleStateChange;
            _viewModel.OnAnimationParamsReady += StartSpinAnimation;
            
            spinButton.onClick.AddListener(_viewModel.Spin);
            
            PopulateWheel();
        }
        
        private void OnDestroy()
        {
            if (_viewModel != null)
            {
                _viewModel.OnSpinCompleted -= HandleReward;
                _viewModel.OnStateChanged -= HandleStateChange;
                _viewModel.OnAnimationParamsReady -= StartSpinAnimation;
                
                spinButton.onClick.RemoveListener(_viewModel.Spin);
            }
        }

        private void PopulateWheel()
        {
            foreach (var itemView in _itemViews)
            {
                Destroy(itemView.gameObject);
            }

            _itemViews.Clear();

            if (_viewModel.Items == null || _viewModel.Items.Count == 0)
            {
                Debug.LogWarning("No bonus items to populate the wheel");
                return;
            }

            _itemAngleStep = 360f / _viewModel.Items.Count;

            for (var i = 0; i < _viewModel.Items.Count; i++)
            {
                var itemViewGO = Instantiate(itemViewPrefab, wheelContainer);
                var itemView = itemViewGO.GetComponent<WheelRewardSlotView>();
                if (itemView != null)
                {
                    itemView.Init(_viewModel.Items[i]);
                    _itemViews.Add(itemView);

                    var angle = i * _itemAngleStep;
                    itemViewGO.transform.localRotation = Quaternion.Euler(0, 0, angle);
                    
                    var position = 
                        new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), 
                            Mathf.Cos(angle * Mathf.Deg2Rad)) * config.WheelRadius;
                    
                    itemViewGO.GetComponent<RectTransform>().anchoredPosition = position;
                    itemViewGO.transform.localRotation = Quaternion.identity;
                }
            }
        }
        
        private void StartSpinAnimation(float targetAngle, Action onComplete)
        {
            var currentZRotation = wheelContainer.localEulerAngles.z;
            if (currentZRotation < 0)
            {
                // right
                currentZRotation += 360;
            }
            
            var relativeTargetAngle = targetAngle - currentZRotation;
            var finalRotation = currentZRotation + relativeTargetAngle + (config.FullRotations * 360f);

            wheelContainer.DORotate(new Vector3(0, 0, finalRotation), config.Duration, RotateMode.FastBeyond360) // 3 секунди тривалість
                .SetEase(Ease.OutCubic)
                .OnComplete(() => 
                {
                    wheelContainer.localEulerAngles = new Vector3(0,0, targetAngle); 
                    onComplete?.Invoke();
                });
        }

        private void HandleReward(WheelRewardItem reward)
        {
            Debug.Log($"You won: {reward.Id}");
        }

        private void HandleStateChange(WheelState state)
        {
            spinButton.interactable = state == WheelState.Idle;
        }
    }
}
