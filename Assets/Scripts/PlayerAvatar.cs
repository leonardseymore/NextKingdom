using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAvatar : MonoBehaviour {
    public GameObject Torches;
    public Image ProgressImage;

    public Image PortraitImage;
    public Sprite PortraitSprite;
    public Sprite PortraitEliminatedSprite;

    public Text AvailableRuinsText;

    private void Awake()
    {
        ResetToDefault();
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
    }

    public void Elminitate()
    {
        PortraitImage.sprite = PortraitEliminatedSprite;
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
}
