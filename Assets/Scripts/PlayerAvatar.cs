using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAvatar : MonoBehaviour {
    public GameObject Torches;
    public Image ProgressImage;

    Behaviour PortraitHalo;
    public Image PortraitImage;
    public Sprite PortraitSprite;
    public Sprite PortraitEliminatedSprite;

    public Text AvailableRuinsText;
    public CardHolder SpellSpawnArea;

    public Image PotionImage;
    public GameObject DefensiveShieldRays;

    public GameObject[] Cannons;

    private void Awake()
    {
        ResetToDefault();
        PortraitHalo = (Behaviour)PortraitImage.GetComponent("Halo");
    }

    public void Activate(bool active)
    {
        Torches.SetActive(active);
        PortraitHalo.enabled = active;
    }

    public float WinProgress
    {
        get
        {
            return ProgressImage.fillAmount;
        }
        set
        {
            ProgressImage.fillAmount = value;
        }
    }

    public void ResetToDefault()
    {
        AvailableRuins = 0;
        PortraitImage.sprite = PortraitSprite;

        foreach (_2dxFX_GrayScale comp in GetComponentsInChildren<_2dxFX_GrayScale>())
        {
            DestroyImmediate(comp);
        }
    }

    public void Elminitate()
    {
        PortraitImage.sprite = PortraitEliminatedSprite;

        foreach (Image image in GetComponentsInChildren<Image>())
        {
            image.gameObject.AddComponent<_2dxFX_GrayScale>();
        }
    }

    int _availableRuins;
    public int AvailableRuins
    {
        get
        {
            return _availableRuins;
        }
        set
        {
            if (_availableRuins != value)
            {
                AvailableRuinsText.text = value.ToString();
                _availableRuins = value;
            }
        }
    }

    public Sprite PotionSprite
    {
        get
        {
            return PotionImage.sprite;
        }
        set
        {
            PotionImage.sprite = value;
        }
    }

    public bool ShowDefensiveShieldRays
    {
        get
        {
            return DefensiveShieldRays.activeSelf;
        }
        set
        {
            DefensiveShieldRays.SetActive(value);
        }
    }

    public bool ShowPotionImage
    {
        get
        {
            return PotionImage.gameObject.activeSelf;
        }
        set
        {
            PotionImage.gameObject.SetActive(value);
        }
    }

    public GameObject GetRandomCannon
    {
        get
        {
            if (Cannons.Length > 0)
            {
                return Cannons[Random.Range(0, Cannons.Length)];
            }
            return null;
        }
    }
}
