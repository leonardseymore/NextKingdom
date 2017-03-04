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
    }

    public void Initialize(bool leveledUp, int level, float levelUpProgress, bool won)
    {
        foreach (GameObject go in fireworks)
        {
            go.SetActive(won);
        }
        portraitImage.sprite = won ? portraitWonSprite : portraitLostSprite;
        titleText.text = won ? "You won!" : "You lost";
        levelText.text = "Level " + (level + 1);
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
                case 1:
                    totems.SetActive(true);
                    break;
                case 2:
                    carry.SetActive(true);
                    break;
                case 3:
                    rally.SetActive(true);
                    break;
                case 4:
                    offensive.SetActive(true);
                    defensive.SetActive(true);
                    break;
                case 5:
                    bribe.SetActive(true);
                    break;
                case 6:
                    temptress.SetActive(true);
                    graveyard.SetActive(true);
                    break;
                case 7:
                    spells.SetActive(true);
                    break;
                case 8:
                    potions.SetActive(true);
                    break;
            }
        }
    }
}