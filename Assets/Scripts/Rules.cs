using System.Collections.Generic;
using UnityEngine;

public partial class Game : MonoBehaviour {

    Suit _crazy8;
    Suit Crazy8
    {
        get { return _crazy8; }
        set
        {
            if (_crazy8 != value)
            {
                _crazy8 = value;
            }
        }
    }

    int _accumulatedCards;
    int AccumulatedCards
    {
        get { return _accumulatedCards; }
        set
        {
            uiAccumulatedCardsGo.SetActive(value > 0);
            uiAccumulatedCardsText.text = value.ToString();
            _accumulatedCards = value;
        }
    }

    public bool CanPlay(Card card)
    {
        Rank wasteRank = WasteCard.Rank;
        Suit wasteSuit = WasteCard.Suit;
        Rank cardRank = card.Rank;
        Suit cardSuit = card.Suit;
        

        if (wasteRank == Rank.Kraken)
        {
            return IsOffensive(card);
        }

        if (wasteRank == Rank.Alruana)
        {
            return IsDefensive(card);
        }

        if (cardRank == Rank.Joker || (wasteRank == Rank.Joker && IsOffensiveOrDefensive(card)))
        {
            return true;
        }

        if (wasteRank == Rank.Joker && AccumulatedCards == 0)
        {
            return true;
        }

        if (cardRank == Rank.Ace && card.Suit == Suit.Spade)
        {
            return true;
        }

        if (LastCardPlayed != null && LastCardPlayed.Rank != Rank.Seven)
        {
            Rank lastCardPlayedRank = LastCardPlayed.Rank;
            Suit lastCardPlayedSuit = LastCardPlayed.Suit;

            if (cardRank == Rank.King && wasteSuit == cardSuit)
            {
                return true;
            }

            if (lastCardPlayedRank == Rank.King)
            {
                return cardRank == lastCardPlayedRank || cardSuit == lastCardPlayedSuit;
            }

            return cardRank == lastCardPlayedRank;
        }

        if (wasteRank == Rank.Eight)
        {
            return cardSuit == Crazy8 || cardRank == Rank.Eight;
        }

        if (IsOffensive(WasteCard) && AccumulatedCards > 0)
        {
            return IsOffensiveOrDefensive(card);
        }

        if (WasteCard.Rank == Rank.Ace)
        {
            return WasteCard.Suit == Suit.Spade || (card.Rank == WasteCard.Rank || card.Suit == WasteCard.Suit);
        }

        if (card.Rank == Rank.Two)
        {
            return card.Rank == WasteCard.Rank || card.Suit == WasteCard.Suit || WasteCard.Rank == Rank.Joker;
        }

        return card.Rank == WasteCard.Rank || card.Suit == WasteCard.Suit || card.Rank == Rank.Eight;
    }

    bool Is7(Card card)
    {
        return card.Rank == Rank.Seven;
    }

    bool IsQueen(Card card)
    {
        return card.Rank == Rank.Queen;
    }

    bool IsOffensiveOrDefensive(Card card)
    {
        return IsOffensive(card) || IsDefensive(card);
    }

    bool IsOffensive(Card card)
    {
        return card.Rank == Rank.Two || card.Rank == Rank.Joker;
    }

    bool IsDefensive(Card card)
    {
        return card.Rank == Rank.Ace || card.Rank == Rank.Queen;
    }

    bool IsEndsTurnCard(Card card)
    {
        return (card.Rank == Rank.Ace && card.Suit == Suit.Spade) ||
                card.Rank == Rank.Joker ||
                card.Rank == Rank.Two ||
                card.Rank == Rank.Seven ||
                card.Rank == Rank.Jack ||
                card.Rank == Rank.Queen ||
                card.Rank == Rank.Eight;
    }

    bool IsSpecial(Card card)
    {
        return card.Rank == Rank.Joker ||
                card.Rank == Rank.Two ||
                card.Rank == Rank.Seven ||
                card.Rank == Rank.Jack ||
                card.Rank == Rank.King ||
                card.Rank == Rank.Eight ||
                card.Rank == Rank.Queen ||
                card.Rank == Rank.Ace;
    }

    Card GetBestCardToPlay(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            if (!CanPlay(card))
            {
                continue;
            }
            return card;
        }

        return null;
    }

    Card GetQueen(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            if (card.Rank == Rank.Queen)
            {
                return card;
            }
        }
        return null;
    }

    Card Get7(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            if (Is7(card))
            {
                return card;
            }
        }
        return null;
    }

    bool HasOffensiveOrDefensiveCards(List<Card> cards)
    {
        List<Card> specialCards = new List<Card>();
        foreach (Card card in cards)
        {
            if (IsOffensiveOrDefensive(card))
            {
                return true;
            }
        }
        return false;
    }

    bool HasOffensiveCards(List<Card> cards)
    {
        List<Card> specialCards = new List<Card>();
        foreach (Card card in cards)
        {
            if (IsOffensive(card))
            {
                return true;
            }
        }
        return false;
    }

    bool HasDefensiveCards(List<Card> cards)
    {
        List<Card> specialCards = new List<Card>();
        foreach (Card card in cards)
        {
            if (IsDefensive(card))
            {
                return true;
            }
        }
        return false;
    }

    List<Card> GetOffensiveOrDefensiveCards(List<Card> cards)
    {
        List<Card> specialCards = new List<Card>();
        foreach (Card card in cards)
        {
            if (IsOffensiveOrDefensive(card))
            {
                specialCards.Add(card);
            }
        }
        return specialCards;
    }

    List<Card> GetOffensiveCards(List<Card> cards)
    {
        List<Card> specialCards = new List<Card>();
        foreach (Card card in cards)
        {
            if (IsOffensive(card))
            {
                specialCards.Add(card);
            }
        }
        return specialCards;
    }

    List<Card> GetSpecialCards(List<Card> cards)
    {
        List<Card> specialCards = new List<Card>();
        foreach (Card card in cards)
        {
            IsSpecial(card);
            specialCards.Add(card);
        }
        return specialCards;
    }
}
