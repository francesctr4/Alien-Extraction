using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Teleport_OnCollision : YmirComponent
{
    public string scene = "";

    // Loading scene
    private GameObject loadSceneImg;
    private bool loadScene = false;

    public void Start()
    {
        loadSceneImg = InternalCalls.GetGameObjectByName("Loading Scene Canvas");

        if (loadSceneImg != null)
        {
            loadSceneImg.SetActive(false);
        }

        loadScene = false;
    }

    public void Update()
    {
        if (loadScene)
        {
            InternalCalls.LoadScene(scene);
            loadScene = false;

            return;
        }

        return;
    }

    public void OnCollisionEnter(GameObject other)
    {
        if (other.Tag == "Player")
        {
            Audio.StopAllAudios();
            //loadSceneImg.SetActive(true);
            
            if (loadSceneImg != null)
            {
                loadSceneImg.SetActive(true);
            }

            loadScene = true;
            Globals.GetPlayerScript().SavePlayer();
        }
    }
}