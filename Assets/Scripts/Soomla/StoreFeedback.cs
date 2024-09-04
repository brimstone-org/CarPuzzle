using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Tedrasoft_Rate;
using NativeInApps;

public class StoreFeedback : MonoBehaviour {
	[SerializeField]
	private GameObject _popup;
	[SerializeField]
	private Text _feedback;

	private string _refreshingPurchases;
	private string _refreshingFailed;
	private string _refreshCompleted;
	void Start() {
		_feedback.text = "";
		_refreshingPurchases = LanguageManager.instance.Get ("refreshingPurchases");
		_refreshingFailed = LanguageManager.instance.Get ("refreshingFailed");
		_refreshCompleted = LanguageManager.instance.Get ("refreshCompleted");
	}

	public void RefreshStarted() {
		_feedback.text = _refreshingPurchases;
		_popup.SetActive (true);
		_dotTime = 0f;
		_delay = 0f;
	}
	public void RefreshFailed(string error) {
		_feedback.text = _refreshingFailed;
	}
	public void RefreshEnded() {
		_feedback.text = _refreshCompleted;
	}
	public void Close() {
		_popup.SetActive (false);
		_feedback.text = "";
	}

	private float _dotTime;
	private float _delay;
	void Update() {
		if (!_popup.activeSelf)
			return;

		if (_feedback.text.StartsWith (_refreshingPurchases)) {
			_dotTime += Time.deltaTime;
			if (_dotTime >= .5f) {
				_dotTime = 0f;
				if (_feedback.text.EndsWith ("...")) {
					_feedback.text = _refreshingPurchases;
				} else {
					_feedback.text += ".";
				}
			}
		} else {
			_delay += Time.deltaTime;
			if (_delay >= 2f) {
				_popup.SetActive (false);
			}
		}
	}

	public void Refresh() {
		NativeInApp.Instance.RestorePurchases ();
	}
}
