using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Tp_Beacon : YmirComponent
{
    // Loading scene
    private GameObject loadSceneImg;
    private bool loadScene = false;
    public GameObject image_go_base;
    public GameObject button_base;
    public GameObject button_no;

    public void Start()
    {
        loadSceneImg = InternalCalls.GetGameObjectByName("Loading Scene Canvas");
        button_base = InternalCalls.GetGameObjectByName("Go_Base");
        button_no = InternalCalls.GetGameObjectByName("Stay_On_Map");
        image_go_base = InternalCalls.GetGameObjectByName("Image_Go_Base");

        if (loadSceneImg != null)
        {
            loadSceneImg.SetActive(false);
        }
        if (image_go_base != null)
        {
            image_go_base.SetActive(false);
        }
        if (button_base != null)
        {
            button_base.SetActive(false);
        }
        if (button_no != null)
        {
            button_no.SetActive(false);
        }

        loadScene = false;
    }

    public void Update()
    {
        if (loadScene)
        {
            if (loadSceneImg != null)
            {
                loadSceneImg.SetActive(true);
            }
            //InternalCalls.LoadScene("Assets/BASE_FINAL/LVL_BASE_COLLIDERS.yscene");
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
            image_go_base.SetActive(true);
            loadScene = true;

            Globals.GetPlayerScript().SavePlayer();
        }
    }

}