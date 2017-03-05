using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Game : MonoBehaviour {
    bool LeveledUp;

    public delegate void LevelUpAction();
    public static event LevelUpAction OnLevelUp;
    public static event LevelUpAction OnLevelChanged;

    private int _level;
    public int Level
    {
        get
        {
            return _level;
        }
        set
        {
            if (_level != value)
            {
                bool leveledUp = _level < value;
                _level = value;
                PlayerPrefs.SetInt("level", value);

                if (OnLevelChanged != null)
                {
                    OnLevelChanged();
                }

                PopulateAvailableCards();
                if (leveledUp)
                {
                    LeveledUp = true;
                    if (OnLevelUp != null)
                    {
                        OnLevelUp();
                    }
                }
            }
        }
    }

    private int _xp;
    public int Xp
    {
        get
        {
            return _xp;
        }
        set
        {
            if (_xp != value)
            {
                _xp = value;
                PlayerPrefs.SetInt("xp", value);
                Level = _xp / 9;
            }
        }
    }

    private int _gamesWon;
    public int GamesWon
    {
        get
        {
            return _gamesWon;
        }
        set
        {
            if (_gamesWon != value)
            {
                _gamesWon = value;
                PlayerPrefs.SetInt("gamesWon", value);
                Xp += 9;
            }
        }
    }

    private int _gamesLost;
    public int GamesLost
    {
        get
        {
            return _gamesLost;
        }
        set
        {
            if (_gamesLost != value)
            {
                _gamesLost = value;
                PlayerPrefs.SetInt("gamesLost", value);
                Xp += 3;
            }
        }
    }

    public float LevelUpProgress
    {
        get
        {
            return (Xp % 9) / 9f;
        }
    }

    public int CardsToDealBasedOnLevel
    {
        get
        {
            if (Level < 1)
            {
                return 4;
            } else if (Level < 2)
            {
                return 5;
            } else if (Level < 3)
            {
                return 6;
            }
            return -1;
        }
    }

    public void ResetProgress()
    {
        Xp = 0;
        Level = 0;
        Restart();
    }

    public int MaxLevel = 20;

    public HashSet<Rank> AvailableRanks;

    void AwakeLevelManagement()
    {
        _xp = PlayerPrefs.GetInt("xp", 0);
        _level = PlayerPrefs.GetInt("level", 0);
        _gamesWon = PlayerPrefs.GetInt("gamesWon", 0);
        _gamesLost = PlayerPrefs.GetInt("gamesLost", 0);
        PopulateAvailableCards();
    }

    void PopulateAvailableCards()
    {
        AvailableRanks = new HashSet<Rank>();
        foreach (Rank rank in Enum.GetValues(typeof(Rank)))
        {
            if (IsSpecial(rank))
            {
                continue;
            }
            AvailableRanks.Add(rank);
        }

        if (Level > 0)
        {
            AvailableRanks.Add(Rank.Eight);
        }
        if (Level > 1)
        {
            AvailableRanks.Add(Rank.King);
        }
        if (Level > 2)
        {
            AvailableRanks.Add(Rank.Jack);
        }
        if (Level > 3)
        {
            AvailableRanks.Add(Rank.Two);
            AvailableRanks.Add(Rank.Joker);
            AvailableRanks.Add(Rank.Ace);
        }
        if (Level > 4)
        {
            AvailableRanks.Add(Rank.Seven);
        }
        if (Level > 5)
        {
            AvailableRanks.Add(Rank.Queen);
        }
    }
}
