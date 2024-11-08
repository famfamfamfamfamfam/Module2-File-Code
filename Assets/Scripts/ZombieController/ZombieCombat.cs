using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieCombat : MonoBehaviour, IDie, IReact, IOnSpecialSkill
{
    Animator zombieController;
    private void OnEnable()
    {
        zombieController = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        ToAttack();
    }
    private void OnTriggerExit(Collider other)
    {
        zombieController.SetTrigger("trOutAttack");
    }
    void OnCollisionEnter(Collision collision)
    {
        ToAttack();
    }
    private void OnCollisionExit(Collision collision)
    {
        ToAttack();
    }

    void ToAttack()
    {
        if (!zombieController.GetCurrentAnimatorStateInfo(0).IsName("React"))
            zombieController.SetTrigger("trAttack");
    }

    void ToReact()
    {
        if (!zombieController.GetCurrentAnimatorStateInfo(0).IsName("React"))
            zombieController.SetTrigger("trReact");
    }
    void ToDie()
    {
        zombieController.SetTrigger("trDie");
    }

    public void Die()
    {
        ToDie();
    }
    public void React()
    {
        ToReact();
        GameManager.instance.SetZomHealth();
    }

    public void OnSpecialSkill()
    {
        if (!zombieController.GetCurrentAnimatorStateInfo(0).IsName("React"))
            zombieController.SetTrigger("trReactOnMoving");
    }


    [SerializeField]
    Transform theRightHand;
    public void CheckDamageAnimationEvent()
    {
        if (Physics.CheckSphere(theRightHand.position, 0.2f))
        {
            //IReact iReact = (IReact)communicator;
            //iReact?.React();
            CommunicateManager.instance.CanReactThing("Character")?.React();
        }
    }
}
