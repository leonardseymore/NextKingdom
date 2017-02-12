using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public partial class Game : MonoBehaviour {
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
        uiActionButton.interactable = true;
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
        CurrentState = GameState.WAIT_FOR_OPPONENT;
        //uiButtonEndTurn.SetActive(false); TODO: flip action button
        SetInstruction("Wait for opponent");
        uiButtonBarPickCrazy8.SetActive(false);
        uiActionButton.interactable = false;
        uiActionButtonText.text = "";
    }

    void OnHighlightedCardChanged()
    {
        if (HighlightedCard != null)
        {
            bool is8 = HighlightedCard.Rank == Rank.Eight;
            uiButtonBarPickCrazy8.SetActive(is8);
            if (is8)
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
                StartCoroutine(PlaySelectedCard());
                break;
            case Action.EndTurn:
                NextPlayer();
                break;
        }
    }

    IEnumerator PlaySelectedCard()
    {
        HighlightedCard.Highlighted = false;
        yield return PlayCardCR(HighlightedCard);
        if (IsEndsTurnCard(HighlightedCard))
        {
            NextPlayer();
        }
        else
        {
            Action = Action.EndTurn;
        }
        HighlightedCard = null;
        
    }

    void UpdateActionButton()
    {
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
                // TODO:
                break;
        }
    }

    public void OnPickCrazy8(int crazy8)
    {
        Crazy8 = (Suit)crazy8;
        OnActionButtonClicked();
    }
}
