using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Electric_trap : YmirComponent
{

    private float time = 0f;
    private bool disabled = true;
    private bool hitPlayer = false;
    private Vector3 originalPosition;
    public float damage = 220;
    private GameObject toParticle;
    //int aux = 0;

    public void Start()
    {
        originalPosition = gameObject.transform.globalPosition;
        toParticle = InternalCalls.CS_GetChild(gameObject, 0);
        Particles.PlayParticles(toParticle);
    }

    public void Update()
    {   


        if (disabled)
        {
            if (time < 3f)
            {
                gameObject.SetPosition(Vector3.negativeInfinity * Time.deltaTime * 1f);
                //InternalCalls.get(toParticle).SetActive(false);
                //InternalCalls.CS_GetChild(gameObject, 1).SetActive(false);

            }
            else
            {
                disabled = false;
                time = 0f;
            }
        }
        else
        {
            if (time < 3f)
            {
                //InternalCalls.CS_GetChild(gameObject, 1).SetActive(true);
                gameObject.SetPosition(originalPosition);
                if (hitPlayer)
                {
                    disabled = true;
                    hitPlayer = false;
                    time = 0f;
                    //Debug.Log("Hit Player");
                }
            }
            else
            {
                disabled = true;
                time = 0f;
            }
        }

        time += Time.deltaTime;
        return;
    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Player" && !disabled)
        {

            other.GetComponent<Health>().TakeDmg(damage);
            hitPlayer = true;
            //Debug.Log("" + other.GetComponent<Health>().currentHealth);
            // Debug.Log("" + aux);
            // aux++;
            
        }
    }

}