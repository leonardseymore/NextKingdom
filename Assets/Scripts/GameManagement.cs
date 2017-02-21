using UnityEngine;

public partial class Game : MonoBehaviour {
    bool GameOver;
    int Round;

    void NextRound(bool randomShuffle = true)
    {
        ShowInterstitial();
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
        NextRound();
    }

    public void Quit()
    {
        Application.Quit();
    }

    void EndGame()
    {
        GameOver = true;

        if (Me.Eliminated)
        {
            ToggleLooseWindow(true);
        }
        else
        {
            int finalScore = (int)Mathf.Pow(Me.TotalScore, 2.1f);
            OnGameWon(finalScore);
            finalScoreText.text = finalScore.ToString();
            ToggleWinWindow(true);
        }
    }
}
