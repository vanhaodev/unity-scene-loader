using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace vanhaodev.sceneloader
{
	public class SceneLoaderUI : MonoBehaviour
	{
		#region Private properties
		[SerializeField] private Image _imagePercentFill;
		[SerializeField] private TextMeshProUGUI _txPercent;
		private float _currentProgress;
		private float _targetProgress;
		#endregion

		#region Private methods

		private void OnEnable()
		{
			_currentProgress = 0;
			_imagePercentFill.fillAmount = _currentProgress;
			_txPercent.text = (_currentProgress * 100f).ToString("0") + "%";
		}

		private void Update()
		{
			_currentProgress = Mathf.Lerp(_currentProgress, _targetProgress, Time.deltaTime * 6f);

			// tránh kẹt 99%
			if (Mathf.Abs(_currentProgress - _targetProgress) < 0.001f)
				_currentProgress = _targetProgress;

			_imagePercentFill.fillAmount = _currentProgress;
			_txPercent.text = (_currentProgress * 100f).ToString("0") + "%";
		}
		#endregion

		#region Public methods

		public void SetProgress(float progress)
		{
			_targetProgress = progress;
		}
		public void ShowLoading()
		{
			SetProgress(0);
			gameObject.SetActive(true);
		}

		public void HideLoading()
		{
			SetProgress(1);
			gameObject.SetActive(false);
		}

		#endregion
	}
}