using BitAura;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public partial class Game : MonoBehaviour {
    public RectTransform[] Layouts;

    public GameObject SpellWindow;
    public GameObject PotionWindow;
    public GameObject InfoWindow;
    public GameObject WinWindow;
    public GameObject LooseWindow;
    public GameObject MafiaWindow;

    public ToggleGroup MafiaWindowToggleGroup;
    public ToggleGroup SpellWindowToggleGroup;

    public SpellType GetSelectedSpellFromWindow()
    {
        Toggle activeToggle = null;
        foreach (Toggle t in SpellWindowToggleGroup.ActiveToggles())
        {
            activeToggle = t;
            break;
        }
        SpellType spellType = Utils.ParseEnum<SpellType>(activeToggle.name);
        return spellType;
    }

    public MafiaJobType GetSelectedMafiaJobFromWindow()
    {
        Toggle activeToggle = null;
        foreach (Toggle t in MafiaWindowToggleGroup.ActiveToggles())
        {
            activeToggle = t;
            break;
        }
        MafiaJobType job = Utils.ParseEnum<MafiaJobType>(activeToggle.name);
        return job;
    }

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

    public void ToggleMafiaWindow(bool visible)
    {
        Camera.main.GetComponent<BlurOptimized>().enabled = visible;
        MafiaWindow.SetActive(visible);
    }
}
