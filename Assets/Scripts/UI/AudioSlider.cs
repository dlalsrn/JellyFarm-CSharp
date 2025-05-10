using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    public enum AudioType
    {
        Bgm,
        Sfx,
    }

    [SerializeField]
    private AudioType audioType;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void Start()
    {
        switch (audioType)
        {
            case AudioType.Bgm:
                slider.value = GameManager.Instance.BgmVolume;
                break;
            case AudioType.Sfx:
                slider.value = GameManager.Instance.SfxVolume;
                break;
        }
    }

    private void OnSliderValueChanged(float value)
    {
        switch (audioType)
        {
            case AudioType.Bgm:
                GameManager.Instance.ChangeBgmVolume(value);
                break;
            case AudioType.Sfx:
                GameManager.Instance.ChangeSfxVolume(value);
                break;
        }
    }
}
