using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntityController : MonoBehaviour {

    public int baseHealth = 100;
    public int baseDamage = 5;
    public int level = 1;
    public string pseudo = "entity";

    private bool fade = false;
    public float fadeSpeed = 10f;

    private int damage = 5;
    private int health = 100;

    private bool alive = true;
    private bool dead = false;

    protected virtual void Start()
    {
        this.setHealth(this.baseHealth);
        this.setDamage(this.baseDamage);
    }

    protected virtual void Update()
    {
        if (this.fade)
            this.fadeOut();
    }

    public void fadeOut()
    {
        SkinnedMeshRenderer render = gameObject.GetComponentInChildren<SkinnedMeshRenderer>();

        if (render)
        {
            List<Material> materials = new List<Material>();
            render.GetMaterials(materials);
            foreach (Material material in materials)
            {
                var color = material.color;

                material.color = new Color(
                    color.r + (fadeSpeed * Time.deltaTime),
                    color.g + (fadeSpeed * Time.deltaTime),
                    color.b + (fadeSpeed * Time.deltaTime),
                    1.0f
                );
            }
            if (materials.Count <= 0)
            {
                render.enabled = false;
            }
        }
        else
        {
            if (this.gameObject.tag != "Player")
                this.gameObject.SetActive(false);
        }
    }

    public float getParticleSystemDuration()
    {
        if (GetComponent<ParticleSystem>() != null)
        {
            var main = GetComponent<ParticleSystem>().main;
            return main.duration;
        }
        return 0f;
    }

    public virtual IEnumerator deathExplode(float killTime, float explodeTime)
    {
        yield return new WaitForSeconds(killTime);
        SoundManager.instance.PlaySingle(SoundManager.instance.combatSource, SoundManager.instance.effectGenericExplode);
        this.fade = true;
        yield return new WaitForSeconds(explodeTime);
        ParticleSystem ps = Instantiate(CombatManager.instance.prefabDeathExplosion, this.transform.position, this.transform.rotation);
        Destroy(ps, ps.main.duration);
        this.dead = true;
        if (this.tag != "Player")
            Destroy(this.gameObject);
    }

    public virtual void Die()
    {
        SoundManager.instance.PlaySingle(SoundManager.instance.combatSource, SoundManager.instance.effectGenericDeath);
        if (GetComponent<Animator>() != null)
            GetComponent<Animator>().SetBool("Alive", false);
        this.alive = false;
        StartCoroutine(deathExplode(SoundManager.instance.effectGenericDeath.length, 1.4f));
    }

    public bool isAlive()
    {
        return this.alive;
    }

    public bool isDead()
    {
        return dead;
    }

    public void setDamage(int dmg)
    {
        if (dmg >= 0)
        {
            this.damage = dmg;
        } else
        {
            this.damage = 5;
        }
    }

    public void setLevel(int lvl)
    {
        if (lvl >= 1)
        {
            this.level = lvl;
        } else
        {
            this.level = 0;
        }
        this.setHealth(this.baseHealth * this.level);
        this.setDamage(this.baseDamage * this.level);
    }

    public void setHealth(int hp)
    {
        if (hp >= 0)
        {
            health = hp;
        } else
        {
            health = 0;
        }
    }

    public string getPseudo()
    {
        return pseudo;
    }

    public int getHealth()
    {
        return health;
    }

    public int getLevel()
    {
        return level;
    }

    public int getDamage()
    {
        return this.damage;
    }

    public virtual void takeDamage(int amount)
    {
        this.setHealth(this.health - amount);
        if (this.getHealth() == 0)
        {
            this.Die();
        }
    }

    public void nextLevel()
    {
        this.setLevel(this.level + 1);
    }

    public bool isTargetInRadius(GameObject target, float radius)
    {
        float distance = Vector3.Distance(target.transform.position, transform.position);

        if (distance < radius)
        {
            return true;
        }
        return false;
    }
}
