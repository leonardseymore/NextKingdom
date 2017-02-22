using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class Game : MonoBehaviour {

    public Zombie ZombiePrefab;
    public Fireball FireballPrefab;

    Zombie SpawnZombie(Card cardToSteal)
    {
        Zombie zombie = Instantiate(ZombiePrefab);
        UnlockAchievement(ACHIEVEMENT_SPAWN_ZOMBIE);
        zombie.Initialize(graveyardGo, cardToSteal);
        return zombie;
    }

    IEnumerator SpawnZombieCR(Seat player = null, Card cardToSteal = null)
    {
        if (cardToSteal == null)
        {
            int playerToStealFrom = Random.Range(0, NumPlayers);
            player = Seats[playerToStealFrom];
            while (player.Eliminated)
            {
                playerToStealFrom += 1;
                if (playerToStealFrom >= NumPlayers)
                {
                    playerToStealFrom = 0;
                }
                player = Seats[playerToStealFrom];
            }
            int cardToStealIdx = Random.Range(0, player.Cards.Count);
            cardToSteal = player.Cards[cardToStealIdx];
        }
        Zombie zombie = SpawnZombie(cardToSteal);
        yield return new WaitForZombie(zombie);
        cardToSteal.Visible = true;
        player.Cards.Remove(cardToSteal);
        AddCardToGraveyard(cardToSteal);
    }

    IEnumerator ShootCannonCR(Seat player, Transform target)
    {
        GameObject cannon = player.PlayerAvatar.GetRandomCannon;

        Vector3 dir = target.position - cannon.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        cannon.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        if (cannon != null)
        {
            Fireball fireball = Instantiate(FireballPrefab, cannon.transform, false);
            fireball.Initialize(target);
            yield return new WaitForFireball(fireball);
        }
    }
}
