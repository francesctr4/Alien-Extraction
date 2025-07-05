using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using YmirEngine;

public class UI_Crafting_Recipe : YmirComponent
{
    private Player player = null;
    public string craftName = "";
    private ITEM_RARITY _rarity;

    public void Start()
    {
        //_itemButtonList = new List<UI_Item_Button>(); 

        //for (int i = 0; i < InternalCalls.CS_GetChildrenSize(gameObject) - 1; i++) // Don't check last element (item to create)
        //{
        //    GameObject button = InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, i), 2); // (Slot(Button))
        //    _itemButtonList.Add(button.GetComponent<UI_Item_Button>());
        //}

        _rarity = ITEM_RARITY.COMMON;

        GetPlayerScript();
    }

    public void Update()
    {
        return;
    }

    public void Check() // Check if all slots are filled to craft item
    {
        int count = 0;

        for (int i = 0; i < InternalCalls.CS_GetChildrenSize(gameObject) - 1; i++) // Don't check last element (item to create)
        {
            GameObject button = InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, i), 2); // (Slot(Button))
            Debug.Log(button.Name);
            Debug.Log("NAME ITEM: " + button.GetComponent<UI_Item_Button>().item.name.ToString());
            Debug.Log("TYPE ITEM: " + button.GetComponent<UI_Item_Button>().item.itemType.ToString());
            Debug.Log("RARITY ITEM: " + button.GetComponent<UI_Item_Button>().item.itemRarity.ToString());
            Debug.Log("CURRENT SLOT ITEM: " + button.GetComponent<UI_Item_Button>().item.currentSlot.ToString());

            if (button.GetComponent<UI_Item_Button>().item.itemType != ITEM_SLOT.NONE) // Check if it's empty
            {
                count++;
                Debug.Log(count.ToString());
            }
        }

        if (count == InternalCalls.CS_GetChildrenSize(gameObject) - 1) // If all slots are filled, craft item
        {
            Craft();
            Debug.Log("CRAFT");
        }

        else
        {
            Debug.Log("NO CRAFT");
        }
    }

    private void Craft()
    {
        // Delete item recipe
        for (int i = 0; i < InternalCalls.CS_GetChildrenSize(gameObject) - 1; i++) // Don't check last element (item to create)
        {
            // Check rarity, if greater, increase item rarity
            if ((int)InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, i), 2).GetComponent<UI_Item_Button>().item.itemRarity > (int)_rarity)
            {
                _rarity = InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, i), 2).GetComponent<UI_Item_Button>().item.itemRarity;
            }

            Debug.Log(InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, i), 2).GetComponent<UI_Item_Button>().item.itemRarity.ToString());
            Debug.Log(_rarity.ToString());

            // Delete item from player list
            GameObject goDelete = InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, i), 2);
            player.itemsList.Remove(goDelete.GetComponent<UI_Item_Button>().item);

            // Change current item to an empty one
            goDelete.GetComponent<UI_Item_Button>().item.itemType = ITEM_SLOT.NONE;
            goDelete.GetComponent<UI_Item_Button>().itemType = ITEM_SLOT.NONE;
            goDelete.GetComponent<UI_Item_Button>().SetInspectorType(ITEM_SLOT.NONE);
            goDelete.GetComponent<UI_Item_Button>().item.itemRarity = ITEM_RARITY.NONE;
            goDelete.GetComponent<UI_Item_Button>().itemRarity = ITEM_RARITY.NONE;
            goDelete.GetComponent<UI_Item_Button>().item = goDelete.GetComponent<UI_Item_Button>().CreateItemBase();
        }

        //Debug.Log(InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, InternalCalls.CS_GetChildrenSize(gameObject) - 1), 2).Name);
        //Debug.Log(InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, InternalCalls.CS_GetChildrenSize(gameObject) - 1), 2).GetComponent<UI_Item_Button>().item.name);

        // Add new item
        Item item = null;

        if (Equals(craftName, "upgradevessel"))
        {
            Debug.Log(craftName + "_mythic");
            item = Globals.SearchItemInDictionary(craftName + "_mythic");
        }

        else
        {
            switch (_rarity)
            {
                case ITEM_RARITY.COMMON:
                    item = Globals.SearchItemInDictionary(craftName + "_common");
                    break;
                case ITEM_RARITY.RARE:
                    item = Globals.SearchItemInDictionary(craftName + "_rare");
                    break;
                case ITEM_RARITY.EPIC:
                    item = Globals.SearchItemInDictionary(craftName + "_epic");
                    break;
                case ITEM_RARITY.MYTHIC:
                    item = Globals.SearchItemInDictionary(craftName + "_epic");
                    break;
                case ITEM_RARITY.NONE:
                    break;
                default:
                    break;
            }
        }

        Debug.Log(item.name);
        InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, InternalCalls.CS_GetChildrenSize(gameObject) - 1), 2).GetComponent<UI_Item_Button>().SetItem(item);

        player.itemsList.Add(InternalCalls.CS_GetChild(InternalCalls.CS_GetChild(gameObject, InternalCalls.CS_GetChildrenSize(gameObject) - 1), 2).GetComponent<UI_Item_Button>().item);

        _rarity = ITEM_RARITY.COMMON;
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