using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour {

    Transform Target;

    float Speed = 6f;

    public ParticleSystem PSFireball;
    public ParticleSystem PSExplosion;

    void SpawnPS(ParticleSystem prefab, Transform parent)
    {
        ParticleSystem ps = Instantiate(prefab);
        ps.transform.SetParent(parent, false);
        ps.Play();
    }

    private void Awake()
    {
    }

    public void Initialize(Transform target)
    {
        SpawnPS(PSFireball, transform);
        Target = target;
    }

    private void Update()
    {
        if (Target == null)
        {
            return;
        }

        float step = Speed * Time.deltaTime;

        transform.position = Vector3.MoveTowards(transform.position, Target.position, step);
        if (transform.position == Target.transform.position)
        {
            SpawnPS(PSExplosion, Target);
            DestroyImmediate(gameObject);
        }
    }
}

public class WaitForFireball : CustomYieldInstruction
{
    Fireball Fireball;

    public WaitForFireball(Fireball fireball)
    {
        Fireball = fireball;
    }

    public override bool keepWaiting
    {
        get
        {
            return Fireball != null;
        }
    }
}