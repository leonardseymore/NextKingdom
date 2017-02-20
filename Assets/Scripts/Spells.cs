using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Game : MonoBehaviour {

    Spell LastCastSpell;
    public ParticleSystem PSBatsPrefab;
    public ParticleSystem PSSpellSpawnPrefab;

    Card SpawnSpellCard(Rank rank)
    {
        ParticleSystem castPs = Instantiate(PSSpellSpawnPrefab, CurrentPlayer.PlayerAvatar.SpellSpawnArea.transform, false);
        castPs.Play();

        Card card = InstantiateCard(Suit.Special, rank);
        card.FaceDown = false;
        card.SetParent(CurrentPlayer.PlayerAvatar.SpellSpawnArea);
        return card;
    }

    IEnumerator CastCR(SpellType spellType, bool endTurn = false)
    {
        Spell spell = Spells[spellType];
        if (CurrentPlayer.AvailableRuins < spell.Cost)
        {

        }
        else
        {
            CurrentPlayer.AvailableRuins -= spell.Cost;
            switch (spellType)
            {
                case SpellType.Krakin:
                    yield return CastKrakinCR();
                    break;
                case SpellType.Alruana:
                    yield return CastAlruanaCR();
                    break;
                case SpellType.Dracula:
                    yield return CastDraculaCR();
                    break;
            }
            LastCastSpell = spell;
            if (endTurn)
            {
                NextPlayer();
            }
        }
    }

    IEnumerator CastRandom()
    {
        List<Spell> affordableSpells = new List<Spell>();
        foreach (Spell spell in Spells.Values)
        {
            if (spell.Cost <= CurrentPlayer.AvailableRuins)
            {
                affordableSpells.Add(spell);
            }
        }
        
        if (affordableSpells.Count > 0)
        {
            int spellIdx = Random.Range(0, affordableSpells.Count);
            Spell spell = affordableSpells[spellIdx];
            LastCastSpell = spell;
            yield return CastCR(spell.SpellType);
        }
        else
        {
            LastCastSpell = null;
        }
    }

    public void CastKrakin()
    {
        StartCoroutine(CastCR(SpellType.Krakin, true));
    }

    IEnumerator CastKrakinCR()
    {
        Card card = SpawnSpellCard(Rank.Kraken);
        yield return new WaitForSeconds(1f);
        yield return PlayCardCR(card);

        if (!CurrentPlayer.HasPotion(PotionType.FrekenKraken))
        {
            yield return DrawCards(2);
        }
        else
        {
            yield return AnimatePotion(PotionType.FrekenKraken);
        }
    }

    public void CastAlruana()
    {
        StartCoroutine(CastCR(SpellType.Alruana, true));
    }

    IEnumerator CastAlruanaCR()
    {
        Card card = SpawnSpellCard(Rank.Alruana);
        yield return new WaitForSeconds(1f);
        yield return PlayCardCR(card);

        yield return DrawCards(1);
    }

    public void CastDracula()
    {
        StartCoroutine(CastCR(SpellType.Dracula));
    }

    IEnumerator CastDraculaCR()
    {
        Card card = SpawnSpellCard(Rank.Dracula);
        yield return new WaitForSeconds(1f);
        yield return PlayCardCR(card);

        if (graveyard.Count > 0)
        {
            ParticleSystem bats = Instantiate(PSBatsPrefab, graveyardGo.transform, false);
            bats.Play();
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
