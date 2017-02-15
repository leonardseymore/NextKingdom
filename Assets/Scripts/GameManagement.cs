using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public partial class Game : MonoBehaviour {
    bool GameOver;
    int Round;

    void NextRound(bool randomShuffle = true)
    {
        uiPlaySpecialBar.Hide();
        AccumulatedCards = 0;
        NumActions = 0;
        LastCardPlayed = null;
        //reverseDirectionPanel.SetActive(false);
        Round += 1;
        foreach (Card card in allCards)
        {
            card.Reset();
            if (RemainingPlayers == 2 &&
                card.Rank == Rank.Seven ||
                card.Rank == Rank.Jack)
            {
                card.SetParent(HiddenGo);
            }
            else
            {
                card.SetParent(stackGo);
            }
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

    void Restart()
    {
        GameOver = false;
        Round = 0;
        Direction = 1;
        EliminatedPlayers = 0;
        foreach (Seat seat in Seats)
        {
            seat.ResetToDefault();
        }
        NextRound();
    }

    void EndGame()
    {
        GameOver = true;
    }
}
