using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Game : MonoBehaviour {

    public void MafiaCannonFodder()
    {
        StartCoroutine(MafiaCannonFodderCR());
    }

    IEnumerator MafiaCannonFodderCR()
    {
        Interactable = false;
        yield return ShootCannonCR(CurrentPlayer, WasteCard.transform);
        yield return AddCardToGraveyardCR(WasteCard);
        yield return RemoveTopWasteCard();
        Interactable = true;
    }

    public void MafiaDigJob()
    {
        StartCoroutine(MafiaDigJobCR());
    }

    IEnumerator MafiaDigJobCR()
    {
        Interactable = false;
        yield return ShootCannonCR(CurrentPlayer, WasteCard.transform);
        waste.Remove(WasteCard);
        yield return AddCardToGraveyardCR(WasteCard);
        WasteCard = waste.Tail();
        uiPlaySpecialBar.ShowBasedOnCard(WasteCard, Crazy8);
        Interactable = true;
    }
}
