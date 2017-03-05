using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Game : MonoBehaviour {

    public GangsterPanel UiGangsterPanel;
    public Gangster GraveyardGangster;

    public void DoSelectedMafiaJob()
    {
        StartCoroutine(DoSelectedMafiaJobCR(GetSelectedMafiaJobFromWindow()));
    }

    IEnumerator DoSelectedMafiaJobCR(MafiaJobType job)
    {
        switch (job)
        {
            case MafiaJobType.DigJob:
                yield return MafiaDigJobCR();
                break;
            case MafiaJobType.CannonFodder:
                yield return MafiaCannonFodderCR();
                break;
        }
    }

    public void MafiaCannonFodder()
    {
        StartCoroutine(MafiaCannonFodderCR());
    }

    IEnumerator MafiaCannonFodderCR()
    {
        Interactable = false;
        yield return ShootWasteCardCR();
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
