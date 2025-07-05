using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class UI_Bullets : YmirComponent
{
    //public GameObject player = null;
    public GameObject bulletsBar = null;

    Player player;

    public void Start()
    {
        GetAnotherScript();
        bulletsBar = InternalCalls.GetGameObjectByName("Bullets Text");
        if (bulletsBar != null )
        {
            UI.TextEdit(bulletsBar, player.currentWeapon.ammo.ToString() + "/" + player.currentWeapon.ammo.ToString());
        }        
    }

    public void Update()
    {
        if (Input.GetKey(YmirKeyCode.V) == KeyState.KEY_DOWN)
        {
            Debug.Log("Debug Bullet");
            UseBullets();
        }

        return;
    }
    public void UseBullets()
    {
        UI.TextEdit(bulletsBar, player.currentWeapon.currentAmmo.ToString() + "/" + player.currentWeapon.ammo.ToString());
    }

    private void GetAnotherScript()
    {
        GameObject gameObject = InternalCalls.GetGameObjectByName("Player");

        if (gameObject != null)
        {
            player = gameObject.GetComponent<Player>();
        }
    }
}