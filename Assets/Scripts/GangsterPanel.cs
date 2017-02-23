using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GangsterPanel : CardHolder {

    public Sprite TombstoneSprite;
    public Sprite DonSprite;
    public Sprite[] GangsterSprites;
    public Image[] GangsterImages;

    private int _deadGangsters;
    public int DeadGangsters
    {
        get
        {
            return _deadGangsters;
        }
        set
        {
            if (_deadGangsters != value)
            {
                _deadGangsters = value;
                int maxIter = Mathf.Min(GangsterImages.Length, value);
                for (int i = 0; i < maxIter; i++)
                {
                    GangsterImages[i].sprite = TombstoneSprite;
                }
            }
        }
    }

    private void Awake()
    {
    }

    public void ResetToDefault()
    {
        _deadGangsters = 0;
        for (int i = 0; i < GangsterImages.Length - 1; i++)
        {
            Image img = GangsterImages[i];
            img.sprite = GangsterSprites[Random.Range(0, GangsterSprites.Length)];
        }
        GangsterImages[GangsterImages.Length - 1].sprite = DonSprite;
    }
}