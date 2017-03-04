using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoRequiresLevel : MonoBehaviour
{
    public int requiresLevel = 0;
    Game game;

    private void Awake()
    {
        game = FindObjectOfType<Game>();
        SetPlayerLevel();
        Game.OnLevelChanged += SetPlayerLevel;
    }

    public void SetPlayerLevel()
    {
        if (game.Level >= requiresLevel)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}