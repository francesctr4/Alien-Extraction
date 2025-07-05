using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Alien_Trap : YmirComponent
{
    public float velocity = 10f;
    public bool axisZ = false;
    public float damage = 10f;
    private float time = 0f;
    private bool active = true;
    private bool hitPlayer = false;
    private Vector3 originalPosition;
    public bool sensorTrap = false;
    public void Start()
    {
        originalPosition = gameObject.transform.localPosition;
        time = 0f;
    }

    public void Update()
    {
        if (!axisZ)
        {
            if (time < 0.3f)
            {

                gameObject.SetPosition(originalPosition + new Vector3(velocity * time, 0, 0));
                time += Time.deltaTime;
                if (hitPlayer)
                {
                    active = false;
                }
            }
            if (time > 0.3f)
            {

                gameObject.SetPosition(originalPosition);
                time = 0f;
                InternalCalls.GetGameObjectByName("Alien_Trap").SetActive(false);
                active = true;

            }
        }

        if (axisZ)
        {
            if (time < 0.3f)
            {
                gameObject.SetPosition(originalPosition + new Vector3(0, 0, velocity * time));
                time += Time.deltaTime;
                if (hitPlayer)
                {
                    active = false;
                }
            }
            if (time > 0.3f)
            {
                gameObject.SetPosition(originalPosition);
                time = 0f;
                InternalCalls.GetGameObjectByName("Alien_Trap").SetActive(false);
                active = true;

            }
        }

        return;
    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Player")
        {
            if (active)
            {
                other.GetComponent<Health>().TakeDmg(damage);
                hitPlayer = true;
                // Debug.Log("" + other.GetComponent<Health>().currentHealth);
            }
        }
    }

}