using BitAura;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public partial class Game : MonoBehaviour {

    #region Properties
    public Card cardPrefab;
    public GameObject cardPlaceholderPrefab;
    public CardHolder graveyardGo;
    public CardHolder stackGo;
    public CardHolder wasteGo;
    public CardHolder HiddenGo;
    public CardHolder[] tableaus;

    public CardTextures CardTextures;

    List<Card> allCards = new List<Card>();
    BitAura.Stack<Card> deck = new BitAura.Stack<Card>();
    BitAura.Queue<Card> waste = new BitAura.Queue<Card>();
    BitAura.Stack<Card> graveyard = new BitAura.Stack<Card>();

    Card WasteCard;
    Card LastCardPlayed;
    Card HighlightedCard;

    GameState CurrentState;

    public GameObject uiButtonPanel;
    public Button uiActionButton;
    public Text uiActionButtonText;
    public PlaySpecialBar uiPlaySpecialBar;
    public Text instructionsText;
    public Text finalScoreText;

    public GameObject uiButtonBarPickCrazy8;
    public GameObject uiAccumulatedCardsGo;
    public Text uiAccumulatedCardsText;

    public PlayerAvatar[] playerAvatars;

    public ParticleSystem ResurrectionPSPrefab;
    public ParticleSystem EliminationPSPrefab;
    public ParticleSystem AccumulatedCardsLightingBolt;

    public Sprite DefaultCursorSprite;
    Texture2D DefaultCursorTex;

    Dictionary<SpellType, Spell> Spells = new Dictionary<SpellType, Spell>();
    public AudioSource AudioSourceCard;
    #endregion

    #region Lifecycle
    void Awake()
    {
        Spells.Add(SpellType.Krakin, new Spell(SpellType.Krakin, 10, true));
        Spells.Add(SpellType.Alruana, new Spell(SpellType.Alruana, 8, true));
        Spells.Add(SpellType.Dracula, new Spell(SpellType.Dracula, 3, false));

        DefaultCursorTex = Utils.TextureFromSprite(DefaultCursorSprite);
        SetDefaultCursor();
    }

    void SetDefaultCursor()
    {
        SetCursorTex(DefaultCursorTex, Vector2.zero);
    }

    void SetCursorTex(Texture2D tex, Vector2 hotspot)
    {
        Cursor.SetCursor(tex, hotspot, CursorMode.ForceSoftware);
    }

    void Start () {
        ToggleInfoWindow(true);
        CreateCards();
        for (int i = 0; i < NumPlayers; i++)
        {
            Seats.Add(new Seat(i, i == 0 ? PlayerType.Human : PlayerType.Computer, tableaus[i], playerAvatars[i]));
        }
        Restart();
	}
	
	void Update () {
        if (!IsMyTurn)
        {
            return;
        }
		if (Input.GetMouseButtonDown(0))
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current);

            pointerData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerData, results);

            if (results.Count > 0)
            {
                Card card = results[0].gameObject.GetComponent<Card>();
                if (card != null && MyCards.Contains(card))
                {
                    CardTapped(card);
                }
            }
        }
	}

    void CardTapped(Card card)
    {
        if (!CanPlay(card))
        {
            return;
        }
        if (card == HighlightedCard)
        {
            HighlightedCard = null;
            card.Highlighted = false;
        }
        else
        {
            if (HighlightedCard != null)
            {
                HighlightedCard.Highlighted = false;
            }
            HighlightedCard = card;
            card.Highlighted = true;
        }
        OnHighlightedCardChanged();
    }
    #endregion

    #region Init
    void CreateCards()
    {
        foreach (Suit suit in Enum.GetValues(typeof(Suit)))
        {
            if (suit < 0) { continue; }
            foreach (Rank rank in Enum.GetValues(typeof(Rank)))
            {
                if (rank < 0) { continue; }
                InstantiateCard(suit, rank);
            }
        }

        InstantiateCard(Suit.Special, Rank.Joker);
        InstantiateCard(Suit.Special, Rank.Joker);
    }

    
    Card InstantiateCard(Suit suit, Rank rank)
    {
        Card card = Instantiate(cardPrefab);
        card.SetParent(stackGo);
        card.Suit = suit;
        card.Rank = rank;
        card.CardPlaceholderPrefab = cardPlaceholderPrefab;

        card.BackSprite = CardTextures.CardBack;
        card.FrontSprite = CardTextures.GetFrontSprite(card.CardId);
        card.SuitSprite = CardTextures.GetSuitSprite(card.CardId);
        card.RankSprite = CardTextures.GetRankSprite(card.CardId);

        card.Reset();
        if ((int)card.Rank >= -1)
        {
            allCards.Add(card);
        }
        return card;
    }

    #endregion

}
