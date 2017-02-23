using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Game : MonoBehaviour {

    public GangsterPanel UiGangsterPanel;
    public Gangster GraveyardGangster;

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

    void KillGangster(Gangster gangster)
    {
        CurrentPlayer.DeadGangsters += 1;
        gangster.TeleportOut();
        if (IsMyTurn)
        {
            UiGangsterPanel.DeadGangsters = CurrentPlayer.DeadGangsters;
        }
    }

    IEnumerator MafiaDigJobCR()
    {
        Interactable = false;
        GraveyardGangster.Teleport();
        yield return new WaitForSeconds(1.5f);

        UnhighlightCard();
        if (graveyard.Count > 0)
        {
            yield return GetCardFromGraveyardCR();
            GraveyardGangster.TeleportOut();
        }
        else
        {
            KillGangster(GraveyardGangster);
            yield return SpawnZombieKillGoCR(GraveyardGangster.gameObject);
            
        }

        Interactable = true;
    }
}
