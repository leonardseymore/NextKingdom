using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public partial class Game : MonoBehaviour {
    bool _interactable = true;
    bool Interactable
    {
        get
        {
            return _interactable;
        }
        set
        {
            _interactable = value;
            foreach (GameObject btn in GameObject.FindGameObjectsWithTag("PlayerControls"))
            {
                btn.GetComponent<Button>().interactable = value;
            }
        }
    }

    Action _action = Action.DrawCard;
    Action Action
    {
        get { return _action; }
        set {
            if (_action != value)
            {
                _action = value;
                UpdateActionButton();
            }
            
        }
    }

    void OnMyTurn()
    {
        HelpMe();

        uiButtonPanel.SetActive(true);
        if (IsOffensive(WasteCard))
        {
            Action = Action.PlayCard;
        }
        else
        {
            Action = Action.DrawCard;
        }
    }

    void OnMyTurnEnd()
    {
        Debug.Log("End My turn");
        HideHelp();
        
        if (HighlightedCard != null)
        {
            HighlightedCard.Highlighted = false;
            HighlightedCard = null;
        }
        SetInstruction("Wait for opponent");
        uiButtonBarPickCrazy8.SetActive(false);
        uiButtonPanel.SetActive(false);
        uiActionButtonText.text = "";
        CurrentState = GameState.WAIT_FOR_OPPONENT;
    }

    void OnHighlightedCardChanged()
    {
        HelpMe();

        uiButtonBarPickCrazy8.SetActive(HighlightedCard != null && HighlightedCard.Rank == Rank.Eight && WasteCard.Rank != Rank.Eight);
        if (HighlightedCard != null)
        {
            bool is8 = HighlightedCard.Rank == Rank.Eight;
            if (is8 && WasteCard.Rank != Rank.Eight)
            {
                SetInstruction("Pick a race");
                Action = Action.PickCrazy8;
            }
            else
            {
                Action = Action.PlayCard;
            }
        }
        else if (NumActions > 0)
        {
            Action = Action.EndTurn;
        }
        else
        {
            Action = Action.DrawCard;
        }
    }

    public void OnActionButtonClicked()
    {
        HelpMe();

        switch (Action)
        {
            case Action.DrawCard:
                if (NumActions == 0)
                {
                    StartCoroutine(DrawCard());
                }
                Action = Action.EndTurn;
                break;
            case Action.PickCrazy8:
            case Action.PlayCard:
                Interactable = false;
                StartCoroutine(PlaySelectedCard());
                break;
            case Action.EndTurn:
                NextPlayer();
                break;
        }
    }

    void OnDrawCard()
    {
         HelpMe();
    }

    IEnumerator PlaySelectedCard()
    {
        if (!IsMyTurn)
        {
            yield break;
        }

        HighlightedCard.Highlighted = false;
        yield return PlayCardCR(HighlightedCard);
        if (IsEndsTurnCard(HighlightedCard))
        {
            NextPlayer();
        }
        else
        {
            if (IsCarry(HighlightedCard))
            {
                SetInstruction("Play <color=" + SuitColor(HighlightedCard.Suit) + ">" + SuitColor(HighlightedCard.Suit) + "</color>, Carry or end turn");
            }
            else
            {
                SetInstruction("Play <b>" + RankDesc(HighlightedCard.Rank) + "</b>, <color=" + SuitColor(HighlightedCard.Suit) + ">Carry</color> or end turn");
            }
            
            Action = Action.EndTurn;
        }
        HighlightedCard = null;
        Interactable = true;

        HelpMe();
    }

    void UpdateActionButton()
    {
        uiActionButton.interactable = true;
        switch (Action)
        {
            case Action.DrawCard:
                uiActionButtonText.text = "Draw Card";
                break;
            case Action.PlayCard:
                uiActionButtonText.text = "Play Card";
                break;
            case Action.EndTurn:
                uiActionButtonText.text = "End Turn";
                break;
            case Action.PickCrazy8:
                uiActionButtonText.text = "";
                uiActionButton.interactable = false;
                break;
        }
    }

    public void OnPickCrazy8(int crazy8)
    {
        Crazy8 = (Suit)crazy8;
        OnActionButtonClicked();
    }

    public void LoadSceneIntro()
    {
        StopAllCoroutines();
        PlayerPrefs.SetInt("WatchedIntro", 0);
        SceneManager.LoadScene("Intro", LoadSceneMode.Single);
    }
}
