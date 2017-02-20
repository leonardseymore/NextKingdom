using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Tornado : CardHolder {

    CardHolder TargetGo;
    Card CardToDrop;

    bool HasDroppedCard = false;

    float Speed = 1f;

    public ParticleSystem PSTornado;

    void SpawnPS(ParticleSystem prefab, Transform trans)
    {
        ParticleSystem ps = Instantiate(prefab);
        ps.transform.SetParent(trans, false);
        ps.Play();
    }

    private void Awake()
    {
    }

    public void Initialize(CardHolder targetGo, Card cardToDrop)
    {
        TargetGo = targetGo;
        CardToDrop = cardToDrop;
        SpawnPS(PSTornado, transform);
    }

    private void Update()
    {
        if (CardToDrop == null || HasDroppedCard)
        {
            return;
        }

        float step = Speed * Time.deltaTime;
        
        transform.position = Vector3.MoveTowards(transform.position, TargetGo.transform.position, step);
        if (transform.position == TargetGo.transform.position)
        {
            HasDroppedCard = true;
            StartCoroutine(DestroyThis());
        }
    }

    IEnumerator DestroyThis()
    {
        yield return new WaitForSeconds(1.2f);
        DestroyImmediate(gameObject);
    }
}

public class WaitForTornado : CustomYieldInstruction
{
    Tornado Tornado;

    public WaitForTornado(Tornado tornado)
    {
        Tornado = tornado;
    }

    public override bool keepWaiting
    {
        get
        {
            return Tornado != null;
        }
    }
}