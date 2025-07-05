using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class BackZoneDoor_Sensor : YmirComponent
{
    public bool active = true;

    public void OnCollisionStay(GameObject other)
    {
        if (other.Tag == "Player" && active)
        {
            gameObject.parent.GetComponent<BackZoneDoor>().ChangeDoorState();
        }
    }
}