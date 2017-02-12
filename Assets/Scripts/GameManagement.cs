using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public partial class Game : MonoBehaviour {
    int Round;

    void NextRound(bool randomShuffle = true)
    {
        uiPlaySpecialBar.Hide();
        AccumulatedCards = 0;
        NumActions = 0;
        LastCardPlayed = null;
        Direction = 1;
        //reverseDirectionPanel.SetActive(false);
        Round += 1;
        foreach (Card card in allCards)
        {
            card.Reset();
            card.SetParent(stackGo);
        }
        //uiResultsPanel.SetActive(false);
        uiButtonBarPickCrazy8.SetActive(false);

        SetInstruction("");
        CurrentState = GameState.RESTART;

        foreach (Seat seat in Seats)
        {
            seat.ResetToDefault(false);
        }

        Deal();
    }
}
