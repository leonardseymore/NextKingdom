using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Zombie : CardHolder {

    public Animator Animator;
    public SpriteRenderer CardSprite;
    public SpriteRenderer SpriteRenderer;

    CardHolder GraveyardGo;
    Card CardToSteal;
    GameObject GoToKill;

    bool HasCard = false;
    bool StoleCard = false;
    bool KilledGo = false;

    float Speed = 3f;

    public ParticleSystem PSZombieAwakes;
    public ParticleSystem PSZombieDead;
    public ParticleSystem PSZombieBlood;

    void SpawnPS(ParticleSystem prefab, Transform trans)
    {
        ParticleSystem ps = Instantiate(prefab);
        ps.transform.position = trans.position;
        ps.Play();
    }

    private void Awake()
    {
    }

    public void Initialize(CardHolder graveyardGo, Card cardToSteal)
    {
        GraveyardGo = graveyardGo;
        transform.localPosition = GraveyardGo.transform.position;
        CardToSteal = cardToSteal;
        SpawnPS(PSZombieAwakes, GraveyardGo.transform);
    }

    public void KillAndDie(CardHolder graveyardGo, GameObject goToKill)
    {
        GraveyardGo = graveyardGo;
        transform.localPosition = GraveyardGo.transform.position;
        GoToKill = goToKill;
        SpawnPS(PSZombieAwakes, GraveyardGo.transform);
    }

    private void Update()
    {
        if (CardToSteal != null)
        {
            if (StoleCard)
            {
                return;
            }
            ContinueStealingCard();
        }
        else if (GoToKill != null)
        {
            if (KilledGo)
            {
                return;
            }
            Animator.SetInteger("state", 1);
            ContinueKillingGo();
        }
    }

    void ContinueStealingCard()
    {
        float step = Speed * Time.deltaTime;

        if (HasCard)
        {
            transform.position = Vector3.MoveTowards(transform.position, GraveyardGo.transform.position, step);
            SpriteRenderer.flipX = transform.position.x > GraveyardGo.transform.position.x;
            if (transform.position == GraveyardGo.transform.position)
            {
                StoleCard = true;
                Animator.SetInteger("state", 2);
                StartCoroutine(DestroyThis());
            }
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, CardToSteal.transform.position, step);
            SpriteRenderer.flipX = transform.position.x > CardToSteal.transform.position.x;
            if (transform.position == CardToSteal.transform.position)
            {
                HasCard = true;
                CardSprite.sprite = CardToSteal.FrontSprite;
                CardToSteal.Visible = false;
                Animator.SetInteger("state", 1);
            }
        }
    }

    void ContinueKillingGo()
    {
        float step = Speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, GoToKill.transform.position, step);
        SpriteRenderer.flipX = transform.position.x > GoToKill.transform.position.x;
        if (transform.position == GoToKill.transform.position)
        {
            SpawnPS(PSZombieBlood, transform);
            Animator.SetInteger("state", 2);
            StartCoroutine(DestroyThis());
            KilledGo = true;
        }
    }

    IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(1.2f);
        SpawnPS(PSZombieDead, GraveyardGo.transform);
        DestroyImmediate(gameObject);
    }
}

public class WaitForZombie : CustomYieldInstruction
{
    Zombie Zombie;

    public WaitForZombie(Zombie zombie)
    {
        Zombie = zombie;
    }

    public override bool keepWaiting
    {
        get
        {
            return Zombie != null;
        }
    }
}