using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinWindow : MonoBehaviour
{
    public GameObject unlockPanel;

    public GameObject totems;
    public GameObject carry;
    public GameObject rally;
    public GameObject offensive;
    public GameObject defensive;
    public GameObject bribe;
    public GameObject temptress;
    public GameObject graveyard;
    public GameObject spells;
    public GameObject potions;
    public GameObject mafia;

    List<GameObject> allSkills = new List<GameObject>();

    public Text titleText;
    public Text levelText;
    public Image levelUpProgressImage;
    public Image portraitImage;
    public Sprite portraitWonSprite;
    public Sprite portraitLostSprite;

    public GameObject[] fireworks;

    private void Awake()
    {
        allSkills.Add(totems);
        allSkills.Add(carry);
        allSkills.Add(rally);
        allSkills.Add(offensive);
        allSkills.Add(defensive);
        allSkills.Add(bribe);
        allSkills.Add(temptress);
        allSkills.Add(graveyard);
        allSkills.Add(spells);
        allSkills.Add(potions);
        allSkills.Add(mafia);
    }

    public void Initialize(bool leveledUp, int level, float levelUpProgress, bool won)
    {
        foreach (GameObject go in fireworks)
        {
            go.SetActive(won);
        }
        portraitImage.sprite = won ? portraitWonSprite : portraitLostSprite;
        titleText.text = won ? "You won!" : "You lost";
        levelText.text = "Level " + level;
        levelUpProgressImage.fillAmount = leveledUp ? 1 : levelUpProgress;

        foreach (GameObject go in allSkills)
        {
            go.SetActive(false);
        }

        unlockPanel.SetActive(leveledUp);
        if (leveledUp)
        {
            switch (level)
            {
                case 2:
                    totems.SetActive(true);
                    break;
                case 3:
                    carry.SetActive(true);
                    break;
                case 4:
                    rally.SetActive(true);
                    break;
                case 5:
                    offensive.SetActive(true);
                    defensive.SetActive(true);
                    break;
                case 6:
                    bribe.SetActive(true);
                    break;
                case 7:
                    temptress.SetActive(true);
                    graveyard.SetActive(true);
                    break;
                case 8:
                    spells.SetActive(true);
                    break;
                case 9:
                    potions.SetActive(true);
                    break;
                case 10:
                    potions.SetActive(true);
                    break;
            }
        }
    }
}