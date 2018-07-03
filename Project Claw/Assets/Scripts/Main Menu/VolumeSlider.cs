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
	void OnEnable()
	{
		if ( EventManager.Exists )
		{
			EventManager.StartListening( "Volume slider", SetSlider );
		}
	}
	void OnDisable()
	{
		if ( EventManager.Exists )
		{
			EventManager.StopListening( "Volume slider", SetSlider );
		}
	}
	void Start()
	{
		SetSlider();
	}
	void ChangeVolume()
	{
		PlayerPrefs.SetFloat( volumeKey, slider.value );
		EventManager.TriggerEvent( "Change volume");
	}
	void SetSlider()
	{
		Debug.Log("Slider");
		if ( PlayerPrefs.HasKey( volumeKey ) )
		{
			slider.value = PlayerPrefs.GetFloat( volumeKey );
		}
	}
}