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
		#endregion

		#region Public methods

		public void UpdateProgress(float progress)
		{
			_imagePercentFill.fillAmount = progress;
			_txPercent.text = (progress * 100f).ToString("0.00") + "%";
		}
		public void ShowLoading()
		{
			
		}

		public void HideLoading()
		{
			
		}

		#endregion
	}
}