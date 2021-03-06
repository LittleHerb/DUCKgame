﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerDuckFlight : DuckFlightScript
{
    float slowStrength = 0.9f;
    float slowTime = 0;
    float abilityCooldown = 0;
    public float initialAbilityCooldown = 1;
    float slowCooldown = 0;
    public int health = 20;
    public int healthLoss = 10;
    float damageCooldown;
    bool invulnerable = false;
    public GameObject textHealth;
    public int maxHealth = 20;

    public void ModifyJump(float modifier)
    {
        jumpForce += modifier;
    }

    public void ActivateAbility(int abilityID)
    {
        switch (abilityID)
        {
            default:
                slowStrength *= 0.9f;
                if (slowTime <= 0)
                {
                    slowTime = 0.5f;
                }
                break;
            case 1:
                if (slowTime <= 0)
                {
                    slowTime = 0.5f;
                }
                else
                {
                    slowTime += 0.25f;
                }
                break;
            case 2:
                if (health < maxHealth)
                {
                    health = Mathf.Clamp(health + 5, 0, maxHealth);
                }
                if (textHealth.GetComponent<Text>())
                {
                    textHealth.GetComponent<Text>().text = "Health: " + health + "/" + maxHealth;
                }
                break;
            case 3:
                maxHealth++;
                health++;
                if (textHealth.GetComponent<Text>())
                {
                    textHealth.GetComponent<Text>().text = "Health: " + health + "/" + maxHealth;
                }
                break;
        }
    }

    public override void Start()
    {
        base.Start();
        if (textHealth.GetComponent<Text>())
        {
            textHealth.GetComponent<Text>().text = "Health: " + health + "/" + maxHealth;
        }
    }

    public override void OnTriggerEnter (Collider other)
    {
        //Detects collision with powerups
        if (other.CompareTag("Powerup"))
        {
            if (other.GetComponent<BreadScript>())
            {
                other.GetComponent<BreadScript>().ConsumeBread();
            }
        }

        //Detects collision with enemies
        if (other.CompareTag("Obstacle") && !invulnerable)
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        damageCooldown = 1.2f;
        health = Mathf.Clamp(health - healthLoss, 0, maxHealth);
        if (textHealth.GetComponent<Text>())
        {
            textHealth.GetComponent<Text>().text = "Health: " + health + "/" + maxHealth;
        }
        if (health <= 0)
        {
            TargetHit();
        }
        else
        {
            invulnerable = true;
            if (GetComponent<SpriteRenderer>())
            {
                GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 40);
            }
        }
    }

    public override void Update()
    {
        base.Update();
        if (!dead)
        {
            if (abilityCooldown <= 0)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    if (slowTime > 0)
                    {
                        BeginSlow();
                    }
                    Debug.Log("Shift Ability");
                    abilityCooldown = initialAbilityCooldown;
                }
            }
            else
            {
                abilityCooldown -= Time.deltaTime;
            }

            if (slowCooldown > 0)
            {
                slowCooldown -= Time.deltaTime * (1 / slowStrength);
            }
            else if (Time.timeScale != 1 && Time.timeScale != 0)
            {
                EndSlow();
                Debug.Log("print");
            }

            if (damageCooldown > 0)
            {
                damageCooldown -= Time.deltaTime;
            }
            else if (invulnerable)
            {
                invulnerable = false;
                if (GetComponent<SpriteRenderer>())
                {
                    GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
                }
            }
        }
    }

    void BeginSlow()
    {
        Time.timeScale = slowStrength;
        slowCooldown = slowTime;
    }

    void EndSlow()
    {
        Time.timeScale = 1;
    }
}
