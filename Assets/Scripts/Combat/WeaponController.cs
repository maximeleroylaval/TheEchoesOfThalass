using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : EntityController {

    public float radiusAttack = 5.0f;
    public AudioClip attackSound;

    private bool lockFire = false;
	
	// Update is called once per frame
	protected override void Update () {
        base.Update();
        if (!this.isReady())
        {
            this.resetAttack();
        }
	}

    public bool isReady()
    {
        if (!GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack") && !this.lockFire)
        {
            return true;
        }
        return false;
    }

    public void attack(EntityController origin, GameObject target)
    {
        EntityController entityTarget = target.GetComponent<EntityController>();

        if (!entityTarget || !this.isReady() || !target.GetComponent<EntityController>().isAlive() ||
            !origin.isTargetInRadius(target, this.radiusAttack))
        {
            return;
        }

        GetComponent<Animator>().SetBool("Attack", true);
        entityTarget.takeDamage(this.getDamage());
        SoundManager.instance.PlaySingle(SoundManager.instance.efxSource, attackSound);
        this.lockFire = true;
    }

    public void resetAttack()
    {
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            this.lockFire = false;
            GetComponent<Animator>().SetBool("Attack", false);
        }
    }
}
