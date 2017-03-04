using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevManagement : MonoBehaviour {

    Game game;
    public GameObject devPanel;

    public InputField levelInput;

    private void Awake()
    {
        game = FindObjectOfType<Game>();
        levelInput.text = game.Level.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.BackQuote))
        {
            devPanel.SetActive(!devPanel.activeSelf);
        }
    }

    public void SetLevel()
    {
        int level = Convert.ToInt32(levelInput.text);
        game.Xp = level * 9;
        game.Restart();
    }
}
