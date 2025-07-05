using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class BH_Tail : YmirComponent
{
    Player player;
    private float timer;
    private bool destroyed;

    private Vector3 size;

    GameObject playerObject;

    public void Start()
	{
        size = new Vector3(1,2,1);
        gameObject.SetColliderSize(size);

        //timer = 0;
        destroyed = false;

        playerObject = InternalCalls.GetGameObjectByName("Player");

        if (playerObject != null)
        {
            player = playerObject.GetComponent<Player>();

            timer = player.swipeTimer;
        }
    }

	public void Update()
	{
        timer = player.swipeTimer;

        if(size.x <= 20)
        {
            size.x += 5;
            size.z += 5;
            gameObject.SetColliderSize(size);
        }
        
        gameObject.SetRotation(playerObject.transform.globalRotation);
        //gameObject.SetPosition(playerObject.transform.globalPosition + (playerObject.transform.GetForward() * -2.5f));
        gameObject.SetPosition(playerObject.transform.globalPosition);

        //gameObject.transform.globalPosition = playerObject.transform.globalPosition;

        if (player.swipeTimer <= 0 && !destroyed)
        {
            //Todo:Fransis
            InternalCalls.Destroy(gameObject);
            destroyed = true;
        }
    }

    public void OnCollisionStay(GameObject other)
    {

    }
}