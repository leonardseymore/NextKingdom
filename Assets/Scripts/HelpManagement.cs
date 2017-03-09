using UnityEngine;

public partial class Game : MonoBehaviour {

    public GameObject blinkingCursor;
    
    void AwakeHelpManagement()
    {
        blinkingCursor.SetActive(false);
    }
    
    void HelpMe()
    {
        if (!IsMyTurn || GamesWon > 0)
        {
            return;
        }

        blinkingCursor.SetActive(true);

        if (HighlightedCard != null)
        {
            if (HighlightedCard.Rank == Rank.Eight)
            {
                blinkingCursor.gameObject.transform.SetParent(uiButtonBarPickCrazy8.transform, false);
            }
            else
            {
                blinkingCursor.gameObject.transform.SetParent(uiActionButtonText.transform, false);
            }
        }
        else
        {
            Card card = GetBestCardToPlay(CurrentPlayerCards);
            if (card == null)
            {
                blinkingCursor.gameObject.transform.SetParent(uiActionButtonText.transform, false);
            }
            else
            {
                blinkingCursor.gameObject.transform.SetParent(card.transform, false);
            }
        }
    }

    void HideHelp()
    {
        blinkingCursor.SetActive(false);
    }
}
