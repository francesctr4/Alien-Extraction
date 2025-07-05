using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class QueenXenomorphAttack : YmirComponent
{
    public GameObject thisReference = null;

    public GameObject queen;

    private float damageTimer;

    private float acidDamage = 600f;
    private float clawDamage = 600f;
    private float axeDamage = 1200f;
    private float dashDamage = 800f;

    private GameObject player;

    private Health healthScript;

    public void Start()
    {
        damageTimer = 0f;
        player = InternalCalls.GetGameObjectByName("Player");
        healthScript = player.GetComponent<Health>();

    }

    public void Update()
    {
        damageTimer -= Time.deltaTime;

        gameObject.SetRotation(queen.transform.globalRotation);

        //Since there's a small bug before attacking which causes the sensor not to move, the conditional is done this way for now
        if (queen.GetComponent<QueenXenomorphBaseScript>().GetState() != QueenState.CLAW && queen.GetComponent<QueenXenomorphBaseScript>().GetState() != QueenState.AXE_TAIL && queen.GetComponent<QueenXenomorphBaseScript>().GetState() != QueenState.ACID_SPIT)
        {
            gameObject.SetPosition(queen.transform.globalPosition);
        }
        else if (queen.GetComponent<QueenXenomorphBaseScript>().GetState() == QueenState.CLAW)
        {
            //gameObject.SetPosition(new Vector3(gameObject.transform.globalPosition.x, gameObject.transform.globalPosition.y, gameObject.transform.GetForward().z + 5));
            if ((queen.GetComponent<QueenXenomorphBaseScript>().CheckDistance(gameObject.transform.globalPosition, queen.transform.globalPosition, 25f)) && queen.GetComponent<QueenXenomorphBaseScript>().clawAniCounter >= 1.3f)
            {
                gameObject.SetVelocity(queen.transform.GetForward() * 3000f * Time.deltaTime);
            }
            else
            {
                gameObject.SetVelocity(queen.transform.GetForward() * 0f);
            }
        }
        else if (queen.GetComponent<QueenXenomorphBaseScript>().GetState() == QueenState.AXE_TAIL)
        {
            if ((queen.GetComponent<QueenXenomorphBaseScript>().CheckDistance(gameObject.transform.globalPosition, queen.transform.globalPosition, 50f)) && queen.GetComponent<QueenXenomorphBaseScript>().axeAniCounter >= 1f)
            {
                gameObject.SetVelocity(queen.transform.GetForward() * 8000f  * Time.deltaTime);
            }
            else
            {
                gameObject.SetVelocity(queen.transform.GetForward() * 0f);
            }
        }
        else if (queen.GetComponent<QueenXenomorphBaseScript>().GetState() == QueenState.ACID_SPIT)
        {
            if ((queen.GetComponent<QueenXenomorphBaseScript>().CheckDistance(gameObject.transform.globalPosition, queen.transform.globalPosition, 80f)) && queen.GetComponent<QueenXenomorphBaseScript>().acidSpitAniCounter >= 1.5f)
            {
                gameObject.SetVelocity(queen.transform.GetForward() * 15000f * Time.deltaTime);
            }
            else
            {
                gameObject.SetVelocity(queen.transform.GetForward() * 0f);
            }
        }

    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Name == "Player" && damageTimer <= 0)
        {
            if (queen.GetComponent<QueenXenomorphBaseScript>().GetState() == QueenState.CLAW)
            {
                damageTimer = 2.5f;
                healthScript.TakeDmg(clawDamage);
            }
            if (queen.GetComponent<QueenXenomorphBaseScript>().GetState() == QueenState.AXE_TAIL)
            {
                damageTimer = 3.5f;
                healthScript.TakeDmg(axeDamage);
            }
            if (queen.GetComponent<QueenXenomorphBaseScript>().GetState() == QueenState.DASH)
            {
                damageTimer = 2.5f;
                healthScript.TakeDmg(dashDamage);
            }
            if (queen.GetComponent<QueenXenomorphBaseScript>().GetState() == QueenState.ACID_SPIT)
            {
                damageTimer = 2f;
                healthScript.TakeDmg(acidDamage);
            }
        }

    }

}