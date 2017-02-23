using System.Collections;
using UnityEngine;

public class Gangster : CardHolder {

    public Animator Animator;
    public AudioSource AudioSourceTeleport;

    private void Awake()
    {
    }

    public void Teleport()
    {
        AudioSourceTeleport.Play();
        Animator.SetTrigger("Teleport");
    }

    public void TeleportOut()
    {
        Animator.SetTrigger("TeleportOut");
    }
}

/*
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
*/