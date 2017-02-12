using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Game : MonoBehaviour {

    public ParticleSystem PSBatsPrefab;

    public void CastKrakin()
    {
        StartCoroutine(CastKrakinCR());
    }

    IEnumerator CastKrakinCR()
    {
        Card card = InstantiateCard(Suit.Special, Rank.Kraken);
        yield return DrawCards(2);
        yield return PlayCardCR(card);
        NextPlayer();
    }

    public void CastAlruana()
    {
        StartCoroutine(CastAlruanaCR());
    }

    IEnumerator CastAlruanaCR()
    {
        Card card = InstantiateCard(Suit.Special, Rank.Alruana);
        yield return DrawCards(1);
        yield return PlayCardCR(card);
        NextPlayer();
    }

    public void CastDracula()
    {
        StartCoroutine(CastDraculaCR());
    }

    IEnumerator CastDraculaCR()
    {
        Card card = InstantiateCard(Suit.Special, Rank.Dracula);
        yield return PlayCardCR(card);

        if (graveyard.Count > 0)
        {
            ParticleSystem bats = Instantiate(PSBatsPrefab, graveyardGo.transform, false);
            bats.Play();

            yield return DrawCards(1);
            yield return GetCardFromGraveyardCR();
            yield return new WaitForSeconds(2f);
        }
        else
        {
            yield return SpawnZombieCR();
        }
        
        DestroyImmediate(card.gameObject);
    }
}
