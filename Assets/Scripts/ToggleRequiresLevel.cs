using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleRequiresLevel : MonoBehaviour
{
    Toggle toggle;
    public Image iconImage;
    public Sprite iconDefaultSprite;
    public Sprite lockedSprite;
    public int requiresLevel = 0;
    Game game;

    private void Awake()
    {
        toggle = GetComponent<Toggle>();
        game = FindObjectOfType<Game>();
        SetPlayerLevel();
        Game.OnLevelChanged += SetPlayerLevel;
    }

    public void SetPlayerLevel()
    {
        if (game.Level >= requiresLevel)
        {
            iconImage.sprite = iconDefaultSprite;
            toggle.interactable = true;
        }
        else
        {
            iconImage.sprite = lockedSprite;
            toggle.interactable = false;
        }
    }
}