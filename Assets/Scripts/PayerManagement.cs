using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Game : MonoBehaviour {
    int NumActions = 0;
    int Direction = 1;
    int CurrentPlayerIdx;
    int MyPlayerIdx;
    int NumPlayers = 4;
    int EliminatedPlayers = 0;
    int WinningScore
    {
        get
        {
            switch (RemainingPlayers)
            {
                case 4:
                    return 50;
                case 3:
                    return 30;
                case 2:
                    return 20;
            }
            return -1;
        }
    }

    int RemainingPlayers
    {
        get
        {
            return NumPlayers - EliminatedPlayers;
        }
    }

    List<Seat> Seats = new List<Seat>();

    bool IsMyTurn
    {
        get
        {
            return CurrentPlayerIdx == MyPlayerIdx;
        }
    }

    List<Card> CurrentPlayerCards
    {
        get
        {
            return Seats[CurrentPlayerIdx].Cards;
        }
    }

    Seat CurrentPlayer
    {
        get
        {
            return Seats[CurrentPlayerIdx];
        }
    }

    Seat Me
    {
        get
        {
            return Seats[MyPlayerIdx];
        }
    }

    List<Card> MyCards
    {
        get
        {
            return Me.Cards;
        }
    }

    bool CurrentPlayerHasCards
    {
        get
        {
            return CurrentPlayerCards.Count > 0;
        }
    }

    bool CurrentPlayerHasPotion(PotionType potion)
    {
        return CurrentPlayer.HasPotion(potion);
    }

    class Seat : IComparable<Seat>
    {
        public int PlayerIdx;
        public PlayerType PlayerType;
        public List<Card> Cards;
        public CardHolder Tableau;
        public PlayerAvatar PlayerAvatar;
        public int HandValue;
        public int Score;
        public int TotalScore;
        public int AvailableGangsters;
        public int DeadGangsters;
        public int RemainingGangsters
        {
            get
            {
                return AvailableGangsters - DeadGangsters;
            }
        }
        public bool HasGangsters
        {
            get
            {
                return RemainingGangsters > 0;
            }
        }

        HashSet<PotionType> ActivePotions;

        public void TogglePotion(PotionType potionType)
        {
            if (ActivePotions.Contains(potionType))
            {
                RemovePotion(potionType);
            }
            else
            {
                AddPotion(potionType);
            }
        }

        public void AddPotion(PotionType potionType)
        {
            ActivePotions.Add(potionType);
        }

        public void RemovePotion(PotionType potionType)
        {
            ActivePotions.Remove(potionType);
        }

        public bool HasPotion(PotionType potionType)
        {
            return ActivePotions.Contains(potionType);
        }

        public int AvailableRuins
        {
            get
            {
                return PlayerAvatar.AvailableRuins;
            }
            set
            {
                PlayerAvatar.AvailableRuins = value;
            }
        }

        bool _eliminated;
        public bool Eliminated
        {
            get
            {
                return _eliminated;
            }
            set
            {
                if (_eliminated != value)
                {
                    if (value)
                    {
                        PlayerAvatar.Elminitate();
                    }
                    else
                    {
                        PlayerAvatar.ResetToDefault();
                    }
                    _eliminated = value;
                }
            }
        }

        public Seat(int playerIdx, PlayerType playerType, CardHolder tableau, PlayerAvatar playerAvatar)
        {
            AvailableGangsters = Globals.GangstersPerPlayer;
            DeadGangsters = 0;
            PlayerIdx = playerIdx;
            PlayerType = playerType;
            Tableau = tableau;
            PlayerAvatar = playerAvatar;
            ResetToDefault();
        }

        public void ResetToDefault(bool completeReset = true)
        {
            if (completeReset)
            {
                ActivePotions = new HashSet<PotionType>();
                TotalScore = 0;
                Eliminated = false;
                AvailableRuins = 0;
            }

            if (WinProgress >= 1f)
            {
                Score = 0;
                WinProgress = 0;
            }
            Cards = new List<Card>();
        }

        public float WinProgress
        {
            get
            {
                return PlayerAvatar.WinProgress;
            }
            set
            {
                PlayerAvatar.WinProgress = value;
            }
        }

        public bool IsComputer
        {
            get
            {
                return PlayerType == PlayerType.Computer;
            }
        }

        public IEnumerator AddCard(Card card, bool immediate)
        {
            Cards.Add(card);
            HandValue += card.CardValue;
            yield return card.SetParentCR(Tableau, immediate);
        }

        public void RemoveCard(Card card)
        {
            HandValue -= card.CardValue;
            Cards.Remove(card);
        }

        void ActivateRandomPotions()
        {
            ActivePotions = new HashSet<PotionType>();
            if (UnityEngine.Random.Range(0, 10) < 5)
            {
                ActivePotions.Add(PotionType.BasicSword);
            }

            
            if (UnityEngine.Random.Range(0, 10) < 5)
            {
                ActivePotions.Add(DefensivePotions[UnityEngine.Random.Range(0, DefensivePotions.Count)]);
            }
        }

        public void OnMyTurnStarted()
        {
            if (IsComputer)
            {
                ActivateRandomPotions();
            }

            PlayerAvatar.Activate(true);
            AvailableRuins += 1;
        }

        public void OnMyTurnEnded()
        {
            PlayerAvatar.Activate(false);
        }

        public override string ToString()
        {
            return PlayerType + ": " + PlayerIdx;
        }

        public int CompareTo(Seat other)
        {
            return HandValue.CompareTo(other.HandValue);
        }
    }

    void NextPlayer()
    {
        if (CurrentPlayerCards.Count == 0)
        {
            StartCoroutine(RoundOverCR());
            SetInstruction(CurrentPlayer + " won this round!!!");
        }
        else
        {
            if (IsMyTurn)
            {
                OnMyTurnEnd();
            }
            CurrentPlayer.OnMyTurnEnded();
            
            NumActions = 0;
            LastCardPlayed = null;

            IncrPlayerIdx();
            while (CurrentPlayer.Eliminated)
            {
                IncrPlayerIdx();
            }

            CurrentPlayer.OnMyTurnStarted();

            StartCoroutine(NextPlayerCR());
        }
    }

    void IncrPlayerIdx()
    {
        CurrentPlayerIdx += Direction;
        if (CurrentPlayerIdx >= NumPlayers)
        {
            CurrentPlayerIdx = 0;
        }
        else if (CurrentPlayerIdx < 0)
        {
            CurrentPlayerIdx = NumPlayers - 1;
        }
    }

    void SwitchDirection()
    {
        Direction *= -1;
        // TODO: update direction indicator on UI
    }

    IEnumerator RoundOverCR()
    {
        int score = 0;
        foreach (Seat seat in Seats)
        {
            if (seat == CurrentPlayer)
            {
                continue;
            }

            foreach (Card card in seat.Cards)
            {
                card.FaceDown = false;
                yield return card.SetParentCR(CurrentPlayer.Tableau);
                CurrentPlayer.Score += card.CardValue;
                CurrentPlayer.TotalScore += card.CardValue;
                CurrentPlayer.WinProgress = CurrentPlayer.Score / (float)WinningScore;
            }
        }
        

        if (CurrentPlayer.WinProgress >= 1f)
        {
            // Eliminate weakest player
            List<Seat> seatRanks = new List<Seat>(Seats);
            seatRanks.Sort();
            seatRanks.Reverse();
            for (int i = 0; i < NumPlayers; i++)
            {
                Seat seat = seatRanks[i];
                if (!seat.Eliminated)
                {
                    seat.Eliminated = true;
                    yield return EliminatePlayer(seat);
                    EliminatedPlayers += 1;
                    break;
                }
            }
        }

        if (RemainingPlayers == 1 || Me.Eliminated)
        {
            EndGame();
        }
        else
        {
            Round += 1;
            NextRound();
        }
    }

    IEnumerator EliminatePlayer(Seat seat)
    {
        Time.timeScale = 0.3f;
        ParticleSystem ps = Instantiate(EliminationPSPrefab, seat.PlayerAvatar.PortraitImage.transform, false);
        ps.Play();
        yield return new WaitForSeconds(0.3f);
        Time.timeScale = 1f;
    }

    IEnumerator NextPlayerCR()
    {
        bool canPlay = true;
        if (IsOffensive(WasteCard))
        {
            if (!HasOffensiveOrDefensiveCards(CurrentPlayerCards))
            {
                switch (WasteCard.Rank)
                {
                    case Rank.Two:
                    case Rank.Joker:
                        //case USValue.Queen:
                        yield return DrawAccumulatedCards();
                        break;
                }
            }
        }
        else if (WasteCard.Rank == Rank.Kraken)
        {
            if (!HasOffensiveCards(CurrentPlayerCards))
            {
                if (!CurrentPlayer.HasPotion(PotionType.FrekenKraken))
                {
                    yield return DrawCards(2);
                }
                else
                {
                    yield return AnimatePotion(PotionType.FrekenKraken);
                }
            }
        }
        else if (WasteCard.Rank == Rank.Alruana)
        { 
            if (!HasAce(CurrentPlayerCards))
            {
                yield return DrawCards(1);

                List<Card> offensiveCards = GetOffensiveCards(CurrentPlayerCards);
                if (offensiveCards.Count > 0)
                {
                    yield return SpawnZombieCR(CurrentPlayer, offensiveCards[0]);
                    DestroyImmediate(WasteCard.gameObject);
                    WasteCard = waste.Tail();
                }
            }
        }
        else if (IsQueen(WasteCard) && AccumulatedCards > 0)
        {
            Card card = GetQueen(CurrentPlayerCards);
            if (card != null)
            {
                yield return PlayCardCR(card);
            }
            else
            {
                yield return DrawAccumulatedCards();
            }
        }
        else if (Is7(WasteCard) && SkipCounter > 0)
        {
            Card card = Get7(CurrentPlayerCards);
            if (card != null)
            {
                Time.timeScale = 0.6f;
                yield return PlayCardCR(card);
                yield return new WaitForSeconds(0.2f);
                Time.timeScale = 1.0f;
            }
            else
            {
                canPlay = false;
            }
            SkipCounter = 0;
        }

        if (canPlay)
        {
            if (CurrentPlayer.IsComputer)
            {
                yield return new WaitForSeconds(Globals.WaitTime);

                bool endTurn = false;
                if (!IsSpellCard(WasteCard))
                {
                    if (UnityEngine.Random.Range(0, 10) < 2)
                    {
                        yield return CastRandom();
                        endTurn = LastCastSpell != null && LastCastSpell.EndsTurn;
                    }
                }

                if (!endTurn)
                {
                    Card card = GetBestCardToPlay(CurrentPlayerCards);
                    if (card != null)
                    {
                        Debug.Log(CurrentPlayer + " has card " + card + " to play");
                        if (card.Rank == Rank.Eight)
                        {
                            Crazy8 = (Suit)UnityEngine.Random.Range(1, 5);
                        }
                        yield return PlayCardCR(card);
                        yield return new WaitForSeconds(Globals.WaitTime);

                        if (!IsEndsTurnCard(card))
                        {
                            card = GetBestCardToPlay(CurrentPlayerCards);
                            while (card != null && !IsEndsTurnCard(card))
                            {
                                Debug.Log(CurrentPlayer + " has another card " + card + " to play");
                                if (card.Rank == Rank.Eight)
                                {
                                    Crazy8 = (Suit)UnityEngine.Random.Range(1, 5);
                                }
                                yield return PlayCardCR(card);
                                card = GetBestCardToPlay(CurrentPlayerCards);
                                yield return new WaitForSeconds(Globals.WaitTime);
                            }
                        }
                    }
                    else
                    {
                        yield return DrawCard();
                        card = LastDrawnCard;

                        //Debug.Log(CurrentPlayer + " drew card " + card);
                        if (card == null)
                        {
                            EndGame();
                        }
                        else if (CanPlay(card))
                        {
                            if (card.Rank == Rank.Eight)
                            {
                                Crazy8 = (Suit)UnityEngine.Random.Range(1, 5);
                            }
                            yield return PlayCardCR(card);
                            //Debug.Log(CurrentPlayer + " played drawn card " + card);
                            yield return new WaitForSeconds(Globals.WaitTime);
                        }
                    }
                }

                if (!GameOver)
                {
                    NextPlayer();
                }
            }
            else if (CurrentPlayerIdx == MyPlayerIdx)
            {
                MyTurn();
            }
            /* TODO: pvp stuffs
            else if (pvp)
            {
                instructionsText.text = "Wait for " + CurrentPlayerSeat.Player.DisplayName;
                SendRealtimePokerHandMsg(new PokerHandMessage((int)PVP_CMDS.NEXT_PLAYER, currentPlayerIdx, null));
            }
            */
        }
        else
        {
            NextPlayer();
        }
        yield return null;
    }

    void SetInstruction(string text)
    {
        instructionsText.text = text;
    }

    string SuitColor(Suit suit)
    {
        switch (suit)
        {
            case Suit.Club:
                return "Purple";
            case Suit.Diamond:
                return "Green";
            case Suit.Heart:
                return "Blue";
            case Suit.Spade:
                return "Red";
        }
        return null;
    }

    string RankDesc(Rank rank)
    {
        switch (rank)
        {
            case Rank.Ace:
                return "Shield";
            case Rank.Two:
                return "Sword";
            case Rank.Three:
                return "Three";
            case Rank.Four:
                return "Four";
            case Rank.Five:
                return "Five";
            case Rank.Six:
                return "Six";
            case Rank.Seven:
                return "Bribe";
            case Rank.Eight:
                return "Totem";
            case Rank.Nine:
                return "Nine";
            case Rank.Ten:
                return "Ten";
            case Rank.Jack:
                return "Rally";
            case Rank.Queen:
                return "Temptress";
            case Rank.King:
                return "Carry";
            case Rank.Joker:
                return "Wild";
        }
        return null;
    }


    void MyTurn()
    {
        OnMyTurn();
        UpdateInstructions();
    }

    void UpdateInstructions()
    {
        if (WasteCard.Rank == Rank.Eight)
        {
            SetInstruction("Play <color=" + SuitColor(Crazy8) + ">" + SuitColor(Crazy8) + "</color>, a <b>Totem</b> or draw a card");
        }
        else if (WasteCard.Rank == Rank.Ace && WasteCard.Suit == Suit.Spade)
        {
            SetInstruction("Play any card");
        }
        else if (WasteCard.Rank == Rank.Eight)
        {
            SetInstruction("Play Temptress");
        }
        else if (!IsOffensive(WasteCard) && !IsSpellCard(WasteCard))
        {
            SetInstruction("Play <color=" + SuitColor(WasteCard.Suit) + ">" + SuitColor(WasteCard.Suit) + "</color>, a <b>" + RankDesc(WasteCard.Rank) + "</b> or draw a card");
        }
        else if (IsSpellCard(WasteCard))
        {
            switch (WasteCard.Rank)
            {
                case Rank.Kraken:
                    SetInstruction("Play <b>Offensive</b> card");
                    break;
                case Rank.Alruana:
                    SetInstruction("Play <b>Shield</b>");
                    break;
            }
        }
        else if (IsOffensive(WasteCard) && AccumulatedCards == 0)
        {
            if (WasteCard.Rank == Rank.Joker)
            {
                SetInstruction("Play any card");
            }
            else
            {
                SetInstruction("Play <color=" + SuitColor(WasteCard.Suit) + ">" + SuitColor(WasteCard.Suit) + "</color>, a <b>" + RankDesc(WasteCard.Rank) + "</b> or draw a card");
            }
        }
        else if (IsOffensive(WasteCard) && AccumulatedCards > 0)
        {
            SetInstruction("Play a defensive / offensive card");
        }
        else
        {
            SetInstruction("Play or draw a card");
        }
    }
}
