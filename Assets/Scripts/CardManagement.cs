using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;

public partial class Game : MonoBehaviour {
    int SkipCounter;
    Card LastDrawnCard;
    List<Card> cardsToReshuffle;

    void Deal()
    {
        StartCoroutine(DealCR());
    }

    IEnumerator DealCR()
    {
        cardsToReshuffle = new List<Card>();
        deck.Clear();
        waste.Clear();
        graveyard.Clear();

        if (WasteCard != null && (int)WasteCard.Rank < -1)
        {
            DestroyImmediate(WasteCard.gameObject);
        }
        WasteCard = null;
        HighlightedCard = null;
        LastCardPlayed = null;

        Shuffle(allCards);

        foreach (Card card in allCards)
        {
            card.gameObject.SetActive(true);
            deck.Push(card);
        }

        for (int j = 0; j < tableaus.Length; j++)
        {
            Seat seat = Seats[j];
            if (seat.Eliminated)
            {
                continue;
            }
            for (int i = 0; i < 8; i++)
            {
                yield return seat.AddCard(PopCard(j > 0), true);
                //yield return new WaitForSeconds(0.05f);
            }
        }

        yield return EnqueueWasteCardCR(PopCard(false), true);

        if (IsMyTurn)
        {
            MyTurn();
            CurrentPlayer.OnMyTurnStarted();
        }
        else
        {
            instructionsText.text = "";
            NextPlayer();
        }

        yield return null;
    }

    public void Shuffle(IList<Card> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (System.Byte.MaxValue / n)));
            int k = (box[0] % n);
            n--;
            Card value = list[k];
            list[k] = list[n];
            list[n] = value;
        }

        if (WasteCard == null)
        {
            // keep shuffling till we have valid deck
            int index = NumPlayers * 8;
            if (IsSpecial(list[allCards.Count - index - 1]))
            {
                Shuffle(list);
            }
        }
    }

    IEnumerator DrawCards(int numCards, bool flipUp = false)
    {
        for (int i = 0; i < numCards; i++)
        {
            yield return DrawCard();
        }
    }

    Card PopCard(bool faceDown)
    {
        if (deck.Count == 0)
        {
            if (waste.Count > 1)
            {
                cardsToReshuffle.Add(waste.Dequeue());
            }
            if (waste.Count > 1)
            {
                cardsToReshuffle.Add(waste.Dequeue());
            }
            Shuffle(cardsToReshuffle);
            foreach (Card c in cardsToReshuffle)
            {
                RedeckCard(c);
            }
        }

        Card card = deck.Pop();
        card.FaceDown = faceDown;
        return card;
    }

    IEnumerator DrawCard()
    {
        CardHolder parent = tableaus[CurrentPlayerIdx];
        LastDrawnCard = PopCard(parent.name != "Bottom");
        yield return CurrentPlayer.AddCard(LastDrawnCard, false);
        NumActions++;
    }

    void AddCardToGraveyard(Card card)
    {
        card.FaceDown = false;
        graveyard.Push(card);
        card.SetParent(graveyardGo);
    }

    IEnumerator AddCardToGraveyardCR(Card card)
    {
        card.FaceDown = false;
        graveyard.Push(card);
        yield return card.SetParentCR(graveyardGo);
    }

    IEnumerator GetCardFromGraveyardCR()
    {
        if (graveyard.Count != 0)
        {
            CardHolder parent = tableaus[CurrentPlayerIdx];
            Card card = graveyard.Pop();
            ParticleSystem ps = Instantiate(ResurrectionPSPrefab, card.transform, false);
            ps.Play();
            yield return CurrentPlayer.AddCard(card, false);
        }
    }

    IEnumerator PlayCardCR(Card card)
    {
        uiPlaySpecialBar.ShowBasedOnCard(card, Crazy8);
        LastCardPlayed = card;
        card.FaceDown = false;
        CurrentPlayer.RemoveCard(card);
        

        switch (card.Rank)
        {
            case Rank.Ace:
                AccumulatedCards = 0;
                break;
            case Rank.Two:
                AccumulatedCards += 2;
                break;
            case Rank.Joker:
                AccumulatedCards += 5;
                break;
            case Rank.Seven:
                SkipCounter = SkipCounter == 1 ? 0 : 1;
                break;
            case Rank.Jack:
                SwitchDirection();
                break;
        }

        if (card.Rank == Rank.Eight && WasteCard.Rank == Rank.Eight)
        {
            Crazy8 = card.Suit;
        }

        if (card.Rank == Rank.Queen && WasteCard.Rank == Rank.Queen)
        {
            AccumulatedCards = 0;
            yield return SpawnZombieCR();
        }

        if (IsOffensive(card) && WasteCard.Rank == Rank.Kraken)
        {
            /*
            SpawnParticleSystem(wasteCard.transform, destroyKrakinPrefab);
            */
            DestroyImmediate(WasteCard.gameObject);
        }

        if (IsDefensive(card) && WasteCard.Rank == Rank.Alruana)
        {
            /*
            SpawnParticleSystem(wasteCard.transform, destroyKrakinPrefab);
            */
            DestroyImmediate(WasteCard.gameObject);
        }

        yield return EnqueueWasteCardCR(card);

        switch (card.Rank)
        {
            case Rank.Queen:
                if (AccumulatedCards > 0)
                {
                    Time.timeScale = 0.8f;
                    yield return DrawAccumulatedCards(2);
                    if (AccumulatedCards > 0)
                    {
                        SwitchDirection();
                        /*
                        reverseDirectionCounter = 0;
                        reverseDirectionKeepGoing = true;
                        */
                    }
                    Time.timeScale = 1.0f;
                }
                break;
        }

        /*
        if (pvp)
        {
            SendRealtimePokerHandMsg(new PokerHandMessage((int)PVP_CMDS.PLAY_CARD, currentPlayerIdx, card.dcIndex.ToString()));
        }
        */

        if (!CurrentPlayerHasCards)
        {
            // TODO: RoundOver();
        }

        NumActions++;

        yield return null;
    }

    bool StaysOnWaste(Card card)
    {
        return card.Rank != Rank.Dracula;
    }

    bool IsSpellCard(Card card)
    {
        return (int)card.Rank < -1;
    }

    IEnumerator EnqueueWasteCardCR(Card card, bool immediate = false)
    {
        if (waste.Count > 2)
        {
            Card wasteCard = waste.Dequeue();
            wasteCard.FaceDown = true;
            wasteCard.gameObject.SetActive(false);
            cardsToReshuffle.Add(wasteCard);
        }

        if (!IsSpellCard(card))
        {
            waste.Enqueue(card);
            
        }
        yield return card.SetParentCR(wasteGo, immediate);

        if (StaysOnWaste(card))
        {
            WasteCard = card;
        }
    }

    void RedeckCard(Card card)
    {
        card.gameObject.SetActive(true);
        card.FaceDown = true;
        card.SetParent(stackGo);
        deck.Push(card);
    }

    IEnumerator RedeckCardCR(Card card, bool immediate = false)
    {
        card.gameObject.SetActive(true);
        card.FaceDown = true;
        yield return card.SetParentCR(stackGo, immediate);
        deck.Push(card);
    }

    IEnumerator DrawAccumulatedCards(int maxCards = -1)
    {
        /*
        if (AccumulatedCards > waste.Count + deck.Count) // TODO: pull other players cards
        {

            Debug.Log("Not enough cards");
            NoMoreCards();
        }
        */

        int numCardsDrawn = 0;
        int nextSeatToStealFrom = CurrentPlayerIdx + Direction;
        while (AccumulatedCards > 0 && (numCardsDrawn < maxCards || maxCards == -1))
        {
            yield return DrawCard();
            AccumulatedCards--;
            numCardsDrawn++;
        }

        if (IsMyTurn)
        {
            if (GetBestCardToPlay(CurrentPlayerCards) != null)
            {
                SetInstruction("Play card or end turn");
                uiActionButtonText.text = "End Turn";
            }
            else
            {
                NextPlayer();
            }
        }
    }

    void NoMoreCards()
    {

        /*
        Winner w = CalculateScores(true);
        
        if (pvp)
        {
            SendRealtimePokerHandMsg(new PokerHandMessage((int)PVP_CMDS.OUT_OF_CARDS, w.seat.Player.PlayerIdx, w.score.ToString()));
        }
        
        NextRound(true);
        */
        throw new NotImplementedException("No more cards not implemented yet");
    }
}
