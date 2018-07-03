using UnityEngine;
using UnityEngine.UI;

[RequireComponent( typeof(Slider) )]
public class VolumeSlider : MonoBehaviour {
	Slider slider;
	[SerializeField] string volumeKey;

	void Awake()
	{
		slider = gameObject.GetComponent<Slider>();
		slider.onValueChanged.AddListener( delegate {
			ChangeVolume();
		} );
	}
	void ChangeVolume()
	{
		PlayerPrefs.SetFloat( volumeKey, slider.value );
		EventManager.TriggerEvent( "Change volume");
	}
}