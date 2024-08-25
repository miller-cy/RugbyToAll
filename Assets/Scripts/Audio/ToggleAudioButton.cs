using UnityEngine;
using UnityEngine.UI;

public class ToggleAudioButton : MonoBehaviour
{
    private Toggle toggle;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(ToggleValueChanged);
    }

    private void Start()
    {
        AudioManager.instance.mixer.GetFloat("masterVolume", out float volume);

        toggle.isOn = volume == 0;
    }

    private void ToggleValueChanged(bool isOn)
    {
        AudioManager.instance.ToggleAudio(isOn);
    }

}
