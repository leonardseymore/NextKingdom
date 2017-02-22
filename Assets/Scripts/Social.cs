using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class Game {

    private static string LEADERBOARD_HIGH_SCORE = "LEADERBOARD_HIGH_SCORE";
    private static string ACHIEVEMENT_CAST_DRACULA = "ACHIEVEMENT_CAST_DRACULA";
    private static string ACHIEVEMENT_CAST_KRAKEN = "ACHIEVEMENT_CAST_KRAKEN";
    private static string ACHIEVEMENT_SWITCH_POTIONS = "ACHIEVEMENT_SWITCH_POTIONS";
    private static string ACHIEVEMENT_SPAWN_ZOMBIE = "ACHIEVEMENT_SPAWN_ZOMBIE";
    private static string ACHIEVEMENT_WIN_A_GAME = "ACHIEVEMENT_WIN_A_GAME";



    Dictionary<string, string> IDS
    {
        get
        {
            Dictionary<string, string> entries = new Dictionary<string, string>();
            if (Application.platform == RuntimePlatform.Android)
            {
                entries[LEADERBOARD_HIGH_SCORE] = "CgkIyNjSx78DEAIQAQ";
                entries[ACHIEVEMENT_CAST_DRACULA] = "CgkIyNjSx78DEAIQAg";
                entries[ACHIEVEMENT_CAST_KRAKEN] = "CgkIyNjSx78DEAIQAw";
                entries[ACHIEVEMENT_SWITCH_POTIONS] = "CgkIyNjSx78DEAIQBA";
                entries[ACHIEVEMENT_SPAWN_ZOMBIE] = "CgkIyNjSx78DEAIQBQ";
                entries[ACHIEVEMENT_WIN_A_GAME] = "CgkIyNjSx78DEAIQBg";
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                entries[LEADERBOARD_HIGH_SCORE] = "";
            }
            return entries;
        }
    }

    bool IsMobile
    {
        get
        {
            return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        }
    }

    void StartSocial()
    {
        if (!IsMobile){
            return;
        }
        Social.localUser.Authenticate((bool success) =>
        {
            Debug.Log("Social login success=" + success);
        });
    }

    public void UnlockAchievement(string achievementConst)
    {
        if (!IsMobile)
        {
            return;
        }

        if (!Social.localUser.authenticated)
        {
            return;
        }

        if (PlayerPrefs.GetInt(achievementConst, 0) == 1)
        {
            return;
        }

        Social.ReportProgress(IDS[achievementConst], 100.0f, (bool success) => {
            Debug.Log("Unlocked achievement " + success);
            if (success)
            {
                PlayerPrefs.SetInt(achievementConst, 1);
            }
        });
    }

    void OnGameWon(int finalScore)
    {
        if (!IsMobile)
        {
            return;
        }

        if (!Social.localUser.authenticated)
        {
            return;
        }

        UnlockAchievement(ACHIEVEMENT_WIN_A_GAME);
        Social.ReportScore(finalScore, IDS[LEADERBOARD_HIGH_SCORE], (bool success) => {
            Debug.Log("Report score leaderboard high score success=" + success);
        });
    }

    void OnGameLost()
    {
        if (!IsMobile)
        {
            return;
        }

        if (!Social.localUser.authenticated)
        {
            return;
        }
    }

    public void ShowLeaderboards()
    {
        if (!IsMobile)
        {
            return;
        }

        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                Debug.Log("Social login success=" + success);
                if (success)
                {
                    Social.ShowLeaderboardUI();
                }
            });
            return;
        }
        Social.ShowLeaderboardUI();
    }

    public void ShowAchievements()
    {
        if (!IsMobile)
        {
            return;
        }

        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                Debug.Log("Social login success=" + success);
                if (success)
                {
                    Social.ShowAchievementsUI();
                }
            });
            return;
        }
        Social.ShowAchievementsUI();
    }

    private void CheckAndLogIn(UnityAction callback)
    {
        if (!IsMobile)
        {
            return;
        }

        if (!Social.localUser.authenticated)
        {
            Social.localUser.Authenticate((bool success) =>
            {
                Debug.Log("Social login success=" + success);
                if (success)
                {
                    callback.Invoke();
                }
            });
            return;
        }
        callback.Invoke();
    }
}
