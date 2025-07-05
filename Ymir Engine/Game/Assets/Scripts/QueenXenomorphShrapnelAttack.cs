using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class QueenXenomorphShrapnelAttack : YmirComponent
{
    public GameObject thisReference = null;

    private float movementSpeed;

    private float damage = 150f;

    private GameObject player;

    private Health healthScript;

    private GameObject boss;
    private QueenXenomorphBaseScript bossScript;

    private bool destroyed;

    private float destroyTimer;

    public void Start()
    {
        movementSpeed = 2000f;
        player = InternalCalls.GetGameObjectByName("Player");
        healthScript = player.GetComponent<Health>();
        boss = InternalCalls.GetGameObjectByName("Boss");
        bossScript = boss.GetComponent<QueenXenomorphBaseScript>();
        gameObject.SetImpulse(gameObject.transform.GetForward() * movementSpeed * Time.deltaTime);
        destroyed = false;
        destroyTimer = 0f;
    }

    public void Update()
    {
        destroyTimer += Time.deltaTime;

        if (destroyed || destroyTimer >= 1f)
        {
            InternalCalls.Destroy(gameObject);
        }

    }

    public void OnCollisionStay(GameObject other)
    {
        if (other.Name == "Player" && destroyed == false && player.GetComponent<Player>().vulnerable)
        {
            healthScript.TakeDmg(bossScript.acidShrapnelDMG);
            destroyed = true;
        }
    }
}