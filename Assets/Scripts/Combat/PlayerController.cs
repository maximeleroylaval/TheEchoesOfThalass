using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : EntityController {

    public Color hudColor = Color.green;
    public int hudSize = 28;

    public GameObject activeWeapon = null;

    private Camera characterCamera;

    private EntityController entityAimed = null;

    private GUIStyle style = new GUIStyle();

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        characterCamera = GameObject.FindObjectOfType<Camera>();
        style.normal.textColor = hudColor;
        Cursor.visible = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (!this.isAlive())
            return;

        this.displayEntityInfoAimed();
    }

    void OnGUI()
    {
        // Entity aimed info
        if (entityAimed != null && !this.isDead())
        {
            this.displayGUI(
                entityAimed.getPseudo() + " - Level " + entityAimed.getLevel() + " - " + entityAimed.getHealth() + " HP",
                characterCamera.pixelWidth / 2,
                (characterCamera.pixelHeight / 10) * 9,
                hudSize,
                style,
                true
            );
        }
        else
        {
            this.resetGUI(
                characterCamera.pixelWidth / 2,
                (characterCamera.pixelHeight / 10) * 9,
                hudSize,
                true
            );
        }

        // Dead message
        if (this.isDead())
        {
            this.displayGUI(
                "You are dead",
                characterCamera.pixelWidth / 2,
                characterCamera.pixelHeight / 2,
                hudSize,
                style,
                true
            );
        } else
        {
            this.resetGUI(
                characterCamera.pixelWidth / 2,
                characterCamera.pixelHeight / 2,
                hudSize,
                true
            );
        }
    }

    public override void Die()
    {
        base.Die();
    }

    void resetGUI(float posX, float posY, float size, bool center)
    {
        this.displayGUI("", posX, posY, size, GUIStyle.none, center);
    }

    void displayGUI(string toDisplay, float posX, float posY, float size, GUIStyle style, bool center)
    {
        if (center)
        {
            posX = posX - ((size / 4) * (toDisplay.Length / 2));
            posY = posY - (size / 2) - (characterCamera.pixelHeight / 20);
        }
        GUI.Label(new Rect(posX, posY, size, size), toDisplay, style);
    }

    void displayEntityInfo(EntityController entity)
    {
        entityAimed = entity;
    }

    void displayEntityInfoAimed()
    {
        GameObject gameObject = this.getGameObjectAimed();
        if (gameObject != null)
        {
            EntityController entity = gameObject.GetComponent<EnemyController>();
            if (entity != null)
            {
                this.displayEntityInfo(entity);
            } else
            {
                entityAimed = null;
            }
        }
        else
        {
            entityAimed = null;
        }
    }

    void shoot()
    {
        GameObject entity = this.getGameObjectAimed();
        if (entity != null && activeWeapon != null)
        {
            activeWeapon.GetComponent<WeaponController>().attack(this, entity);
        }
    }

    public GameObject getGameObjectAimed()
    {
        Ray ray = characterCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform.gameObject;
        }
        return null;
    }

    public override void takeDamage(int damage)
    {
        if (this.GetComponent<Thalass.SubmarineController>() != null)
        {
            this.GetComponent<Thalass.SubmarineController>().GetDamaged(damage);
            if (!this.GetComponent<Thalass.SubmarineController>().isAlive())
            {
                this.Die();
            }
        }
        else
            base.takeDamage(damage);
    }
}
