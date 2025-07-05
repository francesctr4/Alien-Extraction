using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class Item_World : YmirComponent
{
    //private Item item = null;
    private Player player = null;

    public ITEM_RARITY itemRarity;

    public bool isEquipped = false;
    public string rarity_str = "";

    public void Start()
    {
        GetPlayerScript();

        switch (rarity_str)
        {
            case "COMMON":
                {
                    itemRarity = ITEM_RARITY.COMMON;
                }
                break;
            case "RARE":
                {
                    itemRarity = ITEM_RARITY.RARE;
                }
                break;
            case "EPIC":
                {
                    itemRarity = ITEM_RARITY.EPIC;
                }
                break;
            case "NONE":
                {
                    itemRarity = ITEM_RARITY.NONE;
                }
                break;
            default:
                break;
        }

        //item = UI_Inventory.SearchItemInDictionary(gameObject.Name);
        //player.itemsList.Add(item);
    }

    public void Update()
    {
        return;
    }

    private void GetPlayerScript()
    {
        GameObject gameObject = InternalCalls.GetGameObjectByName("Player");

        if (gameObject != null)
        {
            player = gameObject.GetComponent<Player>();
        }
    }
}