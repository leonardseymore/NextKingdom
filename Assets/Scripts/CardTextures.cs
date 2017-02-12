using Boo.Lang;
using System.Collections.Generic;
using UnityEngine;

public class CardTextures : MonoBehaviour {
    public Sprite CardBack;

    public Sprite[] StandardClubs;
    public Sprite[] StandardDiamonds;
    public Sprite[] StandardHearts;
    public Sprite[] StandardSpades;

    public Sprite[] Special;

    public Sprite[] SuitTextures;
    public Sprite[] RankTextures;

    public Sprite[] RuinSprites;

    public Dictionary<CardId, Sprite> SpriteLookup = new Dictionary<CardId, Sprite>();
    public Dictionary<Suit, Sprite> SuitSpriteLookup = new Dictionary<Suit, Sprite>();
    public Dictionary<Rank, Sprite> RankSpriteLookup = new Dictionary<Rank, Sprite>();

    void Awake()
    {
        PopulateStandardCardSprites(StandardClubs, Suit.Club);
        PopulateStandardCardSprites(StandardDiamonds, Suit.Diamond);
        PopulateStandardCardSprites(StandardHearts, Suit.Heart);
        PopulateStandardCardSprites(StandardSpades, Suit.Spade);
        PopulateSpecialCardSprites();
        PopulateSuitTextureSprites();
        PopulateRankTextureSprites();
    }

    // Standard Cards 0 = Rest, 1 = A, 2 = 2, 3 = 7, 4 = 8, 5 = J, 6 = Q, 7 = K
    void PopulateStandardCardSprites(Sprite[] sprites, Suit suit)
    {
        for (int i = 1; i <= 13; i++)
        {
            int spriteIdx = 0;
            if (i == 1)
            {
                spriteIdx = 1;
            }
            else if (i == 2)
            {
                spriteIdx = 2;
            }
            else if (i == 7)
            {
                spriteIdx = 3;
            }
            else if (i == 8)
            {
                spriteIdx = 4;
            }
            else if (i == 11)
            {
                spriteIdx = 5;
            }
            else if (i == 12)
            {
                spriteIdx = 6;
            }
            else if (i == 13)
            {
                spriteIdx = 7;
            }

            CardId cardId = new CardId(suit, (Rank)i);
            SpriteLookup[cardId] = sprites[spriteIdx];
        }
    }

    public Sprite GetSuitSprite(CardId cardId)
    {
        Sprite suitSprite;
        if ((int)cardId.Rank < -1)
        {
            return GetRuin(cardId.Rank);
        }
        else
        {
            SuitSpriteLookup.TryGetValue(cardId.Suit, out suitSprite);
        }
        
        if (suitSprite == null)
        {
            Debug.LogError("Failed to get suit sprite for " + cardId.Suit);
        }
        return suitSprite;
    }

    // Spell cards are rank <= -2
    Sprite GetRuin(Rank rank)
    {
        return RuinSprites[(-(int)rank) - 2];
    }

    public Sprite GetFrontSprite(CardId cardId)
    {
        Sprite frontSprite;
        SpriteLookup.TryGetValue(cardId, out frontSprite);
        if (frontSprite == null)
        { 
            Debug.LogError("Failed to get front sprite for " + cardId);
        }
        return frontSprite;
    }

    public Sprite GetRankSprite(CardId cardId)
    {
        if (cardId.Suit == Suit.Special)
        {
            switch (cardId.Rank)
            {
                case Rank.Joker:
                case Rank.Kraken:
                    return RankSpriteLookup[Rank.Two]; // 2 = offensive
            }
        }
        else
        {
            Sprite rankSprite;
            RankSpriteLookup.TryGetValue(cardId.Rank, out rankSprite);
            if (rankSprite == null)
            {
                Debug.LogError("Failed to get rank sprite for " + cardId.Rank);
            }
            return rankSprite;
        }

        return null;
    }

    void PopulateSpecialCardSprites()
    {
        for (int i = 1; i < Special.Length; i++)
        {
            CardId cardId = new CardId(Suit.Special, (Rank)(-i));
            SpriteLookup[cardId] = Special[i];
        }
    }

    void PopulateSuitTextureSprites()
    {
        SuitSpriteLookup[Suit.Special] = SuitTextures[0];
        for (int i = 1; i < 5; i++)
        {
            SuitSpriteLookup[(Suit)i] = SuitTextures[i];
        }
    }

    void PopulateRankTextureSprites()
    {
        for (int i = 0; i < 13; i++)
        {
            RankSpriteLookup[(Rank)(i + 1)] = RankTextures[i];
        }
    }
}
