using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Game : MonoBehaviour {
    public Image levelUpProgressImage;
    public Text playerLevelText;

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
                playerLevelText.text = "Level " + value.ToString();

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
                Level = XpToLevel(value);
                UpdateLevelUpProgress();
            }
        }
    }

    public int LevelToXp(int level)
    {
        if (level == 0)
        {
            return 9;
        }
        return (int) (4.5f * level * (level + 1));
    }

    public int XpToLevel(int xp)
    {
        if (xp == 0)
        {
            return 1;
        }
        return (int) (-0.5f + (1/6f) * Mathf.Sqrt(8 * xp + 9));
    }

    public void SetLevelAndAdjustXp(int level)
    {
        Xp = LevelToXp(level);
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
                LoosingStreak = 0;
                _gamesWon = value;
                PlayerPrefs.SetInt("gamesWon", value);
                Xp += 9;
            }
        }
    }

    private int _roundsWon;
    public int RoundsWon
    {
        get
        {
            return _roundsWon;
        }
        set
        {
            if (_roundsWon != value)
            {
                _roundsWon = value;
                PlayerPrefs.SetInt("roundsWon", value);
                Xp += 1;
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
                LoosingStreak++;
                _gamesLost = value;
                PlayerPrefs.SetInt("gamesLost", value);
                Xp += 3;
            }
        }
    }

    public int ResurrectionPoints;

    private int _loosingStreak;
    public int LoosingStreak
    {
        get
        {
            return _loosingStreak;
        }
        set
        {
            if (_loosingStreak != value)
            {
                _loosingStreak = value;
                PlayerPrefs.SetInt("loosingStreak", value);
            }
        }
    }

    public float LevelUpProgress
    {
        get
        {
            return (Xp - LevelToXp(Level)) / (float)(LevelToXp(Level + 1) - LevelToXp(Level));
        }
    }

    public int CardsToDealBasedOnLevel
    {
        get
        {
            if (Level <= 1)
            {
                return 4;
            } else if (Level <= 2)
            {
                return 5;
            } else if (Level <= 3)
            {
                return 6;
            }
            return -1;
        }
    }

    public void ResetProgress()
    {
        Xp = 9;
        LoosingStreak = 0;
        Restart();
    }

    public HashSet<Rank> AvailableRanks;

    void AwakeLevelManagement()
    {
        _xp = PlayerPrefs.GetInt("xp", 0);
        _level = PlayerPrefs.GetInt("level", 1);
        _gamesWon = PlayerPrefs.GetInt("gamesWon", 0);
        _gamesLost = PlayerPrefs.GetInt("gamesLost", 0);
        _roundsWon = PlayerPrefs.GetInt("roundsWon", 0);
        _loosingStreak = PlayerPrefs.GetInt("loosingStreak", 0);
        playerLevelText.text = "Level " + _level.ToString();
        PopulateAvailableCards();
        UpdateLevelUpProgress();
    }

    void UpdateLevelUpProgress()
    {
        levelUpProgressImage.fillAmount = LevelUpProgress;
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

        if (Level >= LevelRequirement.Eight)
        {
            AvailableRanks.Add(Rank.Eight);
        }
        if (Level >= LevelRequirement.King)
        {
            AvailableRanks.Add(Rank.King);
        }
        if (Level >= LevelRequirement.Jack)
        {
            AvailableRanks.Add(Rank.Jack);
        }
        if (Level >= LevelRequirement.OffensiveAndDefensive)
        {
            AvailableRanks.Add(Rank.Two);
            AvailableRanks.Add(Rank.Joker);
            AvailableRanks.Add(Rank.Ace);
        }
        if (Level >= LevelRequirement.Seven)
        {
            AvailableRanks.Add(Rank.Seven);
        }
        if (Level >= LevelRequirement.Queen)
        {
            AvailableRanks.Add(Rank.Queen);
        }
    }

    bool SpellsUnlocked
    {
        get
        {
            return Level >= LevelRequirement.Spells;
        }
    }

    bool PotionsUnlocked
    {
        get
        {
            return Level >= LevelRequirement.Potions;
        }
    }

    bool MafiaUnlocked
    {
        get
        {
            return Level >= LevelRequirement.Mafia;
        }
    }
}
