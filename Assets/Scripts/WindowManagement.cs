using UnityEngine;
using UnityStandardAssets.ImageEffects;

public partial class Game : MonoBehaviour {
    public GameObject SpellWindow;
    public GameObject PotionWindow;
    public GameObject InfoWindow;
    public GameObject WinWindow;
    public GameObject LooseWindow;

    public void ToggleSpellWindow(bool visible)
    {
        Camera.main.GetComponent<BlurOptimized>().enabled = visible;
        SpellWindow.SetActive(visible);
    }

    public void TogglePotionWindow(bool visible)
    {
        Camera.main.GetComponent<BlurOptimized>().enabled = visible;
        PotionWindow.SetActive(visible);
    }

    public void ToggleInfoWindow(bool visible)
    {
        Camera.main.GetComponent<BlurOptimized>().enabled = visible;
        InfoWindow.SetActive(visible);
    }

    public void ToggleWinWindow(bool visible)
    {
        Camera.main.GetComponent<BlurOptimized>().enabled = visible;
        WinWindow.SetActive(visible);
    }

    public void ToggleLooseWindow(bool visible)
    {
        Camera.main.GetComponent<BlurOptimized>().enabled = visible;
        LooseWindow.SetActive(visible);
    }
}
