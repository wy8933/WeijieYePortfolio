using UnityEngine;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{
    public Slider SFXSlider;
    public Slider BGMSlider;
    public AudioSource musicSource;
    public void Start()
    {
        SFXSlider.value = Settings.Instance.SFXVolumn;
        BGMSlider.value = Settings.Instance.BGMVolumn;
        musicSource.volume = Settings.Instance.BGMVolumn;
        musicSource.Play();
    }

    private void Update()
    {
        musicSource.volume = Settings.Instance.BGMVolumn;
    }
}
