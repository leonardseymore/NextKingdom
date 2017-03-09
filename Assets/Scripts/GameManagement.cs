using UnityEngine;

public partial class Game : MonoBehaviour
{
    bool GameOver;
    int Round;

    void NextRound(bool randomShuffle = true)
    {
        HideHelp();
        Interactable = true;
        if (Level > 2)
        {
            ShowInterstitial();
        }
        uiPlaySpecialBar.Hide();
        AccumulatedCards = 0;
        NumActions = 0;
        LastCardPlayed = null;
        //reverseDirectionPanel.SetActive(false);
        Round += 1;
        foreach (Card card in allCards)
        {
            card.Reset();
            card.SetParent(stackGo);
        }

        //uiResultsPanel.SetActive(false);
        uiButtonBarPickCrazy8.SetActive(false);

        SetInstruction("");
        CurrentState = GameState.RESTART;

        foreach (Seat seat in Seats)
        {
            seat.ResetToDefault(false);
        }

        Deal();
    }

    public void Restart()
    {
        ResurrectionPoints = LoosingStreak;
        LeveledUp = false;
        StopAllCoroutines();
        GameOver = false;
        Round = 0;
        Direction = 1;
        EliminatedPlayers = 0;
        foreach (Seat seat in Seats)
        {
            seat.ResetToDefault();
        }
        if (HighlightedCard != null)
        {
            HighlightedCard.Highlighted = false;
            HighlightedCard = null;
        }
        UiGangsterPanel.ResetToDefault();
        NextRound();
    }

    public void Quit()
    {
        Application.Quit();
    }

    void EndGame()
    {
        GameOver = true;

        int finalScore = (int)Mathf.Pow(Me.TotalScore, 2.1f);
        
        finalScoreText.text = finalScore.ToString();
        if (Me.Eliminated)
        {
            OnGameLost();
            GamesLost++;
        }
        else
        {
            OnGameWon(finalScore);
            GamesWon++;
        }
        
        ToggleWinWindow(true);
    }
}