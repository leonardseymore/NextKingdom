using UnityEngine;

public enum Suit
{
    Special = -1,
    Spade = 1, Diamond = 2, Heart = 3, Club = 4
}

public enum Rank
{
    Joker = -1, Kraken = -2, Alruana = -3, Dracula = -4,
    Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King
}

public enum PlayerType
{
    Human, Computer
}

public enum GameState
{
    RESTART, DEAL, MY_TURN, PICK_OPPONENTS, WAIT_FOR_OPPONENT
}

public enum Action
{
    DrawCard, PlayCard, EndTurn, PickCrazy8
}

public enum SpellType
{
    Krakin = 0, Alruana, Dracula
}

public class Spell
{
    public SpellType SpellType;
    public int Cost;
    public bool EndsTurn;

    public Spell(SpellType spellType, int cost, bool endsTurn)
    {
        SpellType = spellType;
        Cost = cost;
        EndsTurn = endsTurn;
    }
}

public enum PotionType
{
    FrekenKraken = 0, BasicShield, BasicSword, TemptressShield
}

public class CardId
{
    public Suit Suit;
    public Rank Rank;

    public CardId(){ }

    public CardId(Suit suit, Rank rank)
    {
        Suit = suit;
        Rank = rank;
    }

    public override bool Equals(object o)
    {
        if (this == o) return true;
        if (o == null || GetType() != o.GetType()) return false;

        CardId cardId = o as CardId;

        if (Suit != cardId.Suit) return false;
        return Rank == cardId.Rank;

    }

    public override int GetHashCode()
    {
        int result = (int)Suit;
        result = 31 * result + (int)Rank;
        return result;
    }

    public override string ToString()
    {
        return Suit + ":" + Rank;
    }
}

public class Globals
{
    public static float LerpDuration = 0.8f;
    public static float WaitTime = 0.8f;
}
