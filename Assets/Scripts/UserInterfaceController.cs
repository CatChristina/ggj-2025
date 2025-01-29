using UnityEngine;
using UnityEngine.UI;

public class UserInterfaceController : MonoBehaviour
{
    public GameObject storeScreen;
    public GameObject researchScreen;
    public GameObject settingScreen;

    public AudioSource music;
    public Slider musicSlider;

    private void Start()
    {
        storeScreen.SetActive(false);
        researchScreen.SetActive(false);
        settingScreen.SetActive(false);
        musicSlider.value = music.volume;
    }

    public void ToggleHomeScreen()
    {
        storeScreen.SetActive(false);
        researchScreen.SetActive(false);
        settingScreen.SetActive(false);
    }

    public void ToggleStoreScreen()
    {
        storeScreen.SetActive(!storeScreen.activeInHierarchy);
        researchScreen.SetActive(false);
    }

    public void ToggleResearchScreen()
    {
        researchScreen.SetActive(!researchScreen.activeInHierarchy);
        storeScreen.SetActive(false);
    }

    public void ToggleSettingsScreen()
    {
        settingScreen.SetActive(!settingScreen.activeInHierarchy);
        researchScreen.SetActive(false);
        storeScreen.SetActive(false);
    }

    public void MusicVolume()
    {
        music.volume = musicSlider.value;
    }
}
