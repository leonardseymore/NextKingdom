using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySpecialBar : MonoBehaviour {
    public GameObject SpecialBarContainer;
    public GameObject Clubs;
    public GameObject Hearts;
    public GameObject Diamonds;
    public GameObject Spades;
    public GameObject Krakin;

    List<GameObject> AllSpecial;
    List<GameObject> AllSpecialSuits;
    Dictionary<Suit, GameObject> SpecialSuitLookup;

    private void Awake()
    {
        AllSpecialSuits = new List<GameObject>(new GameObject[]{ Clubs, Hearts, Diamonds, Spades});
        AllSpecial = new List<GameObject>(AllSpecialSuits);
        AllSpecial.Add(Krakin);

        SpecialSuitLookup = new Dictionary<Suit, GameObject>();
        SpecialSuitLookup[Suit.Club] = Clubs;
        SpecialSuitLookup[Suit.Heart] = Hearts;
        SpecialSuitLookup[Suit.Diamond] = Diamonds;
        SpecialSuitLookup[Suit.Spade] = Spades;
    }

    public void Hide()
    {
        SpecialBarContainer.SetActive(false);
    }

    public void ShowBasedOnCard(Card card, Suit crazy8 = 0)
    {
        foreach (GameObject go in AllSpecial)
        {
            go.SetActive(false);
        }

        bool showSpecialBar = false;
        Rank cardRank = card.Rank;
        switch (cardRank)
        {
            case Rank.Kraken:
                Krakin.SetActive(true);
                showSpecialBar = true;
                break;
            case Rank.Eight:
                SpecialSuitLookup[crazy8].SetActive(true);
                showSpecialBar = true;
                break;
            case Rank.Ace:
                if (card.Suit == Suit.Spade)
                {
                    foreach (GameObject go in AllSpecialSuits)
                    {
                        go.SetActive(true);
                    }
                    showSpecialBar = true;
                }
                break;
        }
        SpecialBarContainer.SetActive(showSpecialBar);
    }
}
