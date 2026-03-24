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
		[SerializeField] protected Image _imagePercentFill;
		[SerializeField] protected TextMeshProUGUI _txPercent;
		[SerializeField] protected TextMeshProUGUI _txLoadName;
		protected float _currentProgress;
		protected float _targetProgress;
		#endregion

		#region Private methods

		protected virtual void OnEnable()
		{
			_currentProgress = 0;
			SetPercent();
		}

		protected virtual void Update()
		{
			_currentProgress = Mathf.Lerp(_currentProgress, _targetProgress, Time.deltaTime * 6f);

			// avoid stop at 99%
			if (Mathf.Abs(_currentProgress - _targetProgress) < 0.001f)
				_currentProgress = _targetProgress;

			SetPercent();
		}

		protected virtual void SetPercent()
		{
			if (_imagePercentFill) _imagePercentFill.fillAmount = _currentProgress;
			if (_txPercent) _txPercent.text = (_currentProgress * 100f).ToString("0") + "%";
		}
		#endregion

		#region Public methods

		public virtual void SetProgress(float progress)
		{
			_targetProgress = progress;
		}

		public virtual void SetLoadName(string loadName)
		{
			if (_txLoadName) _txLoadName.text = $"{loadName}...";
		}
		public virtual void ShowLoading()
		{
			SetProgress(0);
			gameObject.SetActive(true);
		}

		public virtual void HideLoading()
		{
			SetProgress(1);
			gameObject.SetActive(false);
		}

		#endregion
	}
}