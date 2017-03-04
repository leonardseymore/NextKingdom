using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour {

    #region Properties
    public Image CardImage;
    public Image SuitImage;
    public Image RankImage;

    public Sprite BackSprite;
    public Sprite FrontSprite;

    public GameObject CardPlaceholderPrefab;

    public Sprite SuitSprite
    {
        get
        {
            return SuitImage.sprite;
        }
        set
        {
            SuitImage.sprite = value;
        }
    }

    public Sprite RankSprite
    {
        get
        {
            return RankImage.sprite;
        }
        set
        {
            RankImage.sprite = value;
        }
    }

    public CardId CardId = new CardId();

    public Rank Rank
    {
        get
        {
            return CardId.Rank;
        }
        set
        {
            CardId.Rank = value;
        }
    }

    public Suit Suit
    {
        get
        {
            return CardId.Suit;
        }
        set
        {
            CardId.Suit = value;
        }
    }

    bool _faceDown = true;
    public bool FaceDown
    {
        get{ return _faceDown; }
        set
        {
            if (CardImage == null)
            {
                return;
            }

            _faceDown = value;
            
            CardImage.sprite = value ? BackSprite : FrontSprite;

            SuitImage.gameObject.SetActive(!value);
            RankImage.gameObject.SetActive(!value && RankSprite != null);
        }
    }

    bool _highlighted;
    public bool Highlighted
    {
        get { return _highlighted; }
        set
        {
            if (_highlighted != value)
            {
                CardImage.color = value ? Color.red : Color.white;
                _highlighted = value;
            }
        }
    }

    bool _visible = true;
    public bool Visible
    {
        get { return _visible; }
        set
        {
            if (_visible != value)
            {
                CardImage.enabled = value;
                SuitImage.enabled = value;
                RankImage.enabled = value;
                _visible = value;
            }
        }
    }

    public int CardValue
    {
        get
        {
            if (Rank == Rank.Eight)
            {
                return 40;
            }
            else if (Rank == Rank.Two)
            {
                return 25;
            }
            else if (Rank == Rank.Seven)
            {
                return 14;
            }
            else if (Rank == Rank.Joker)
            {
                return 50;
            }
            else if (Rank == Rank.Ace)
            {
                if (Suit == Suit.Spade)
                {
                    return 100;
                }
                return 11;
            }
            else if ((int)Rank >= 10)
            {
                return 10;
            }
            else
            {
                return (int)Rank;
            }
        }
    }
    #endregion

    Game game;

    private void Awake()
    {
        game = FindObjectOfType<Game>();
    }

    public void Reset()
    {
        FaceDown = true;
    }

    public override string ToString()
    {
        return CardId.ToString();
    }

    public void SetParent(CardHolder parent)
    {
        transform.SetParent(parent.transform, false);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;
    }

    public IEnumerator SetParentCR(CardHolder parent, bool immediate = false)
    {
        if (immediate)
        {
            transform.SetParent(parent.transform, false);
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
        }
        else
        {
            GameObject target = Instantiate(CardPlaceholderPrefab);
            target.transform.SetParent(parent.transform, false);
            target.transform.localScale = Vector2.one;
            target.transform.localPosition = Vector2.zero;

            yield return null;

            Vector3 start = transform.position;
            Vector3 end = target.transform.position;

            Quaternion startRot = transform.rotation;
            Quaternion endRot = parent.transform.rotation;

            float cardScale = GetComponentInParent<CardHolder>().CardScale;
            float newCardScale = parent.CardScale;

            Vector3 startScale = transform.localScale;
            Vector3 endScale = Vector2.one * (newCardScale / cardScale);

            float startTime = Time.time;
            float duration = Globals.LerpDuration;

            while (Time.time - startTime < duration)
            {
                transform.position = Vector3.Slerp(start, end, (Time.time - startTime) / duration);
                transform.rotation = Quaternion.Slerp(startRot, endRot, (Time.time - startTime) / duration);
                transform.localScale = Vector3.Lerp(startScale, endScale, (Time.time - startTime) / duration);
                yield return null;
            }
            DestroyImmediate(target);
            transform.SetParent(parent.transform, false);
            transform.localScale = Vector3.one;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
}
