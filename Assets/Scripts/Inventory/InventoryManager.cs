using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject itemCursor;

    [SerializeField] private ItemClass itemToAdd;
    [SerializeField] private ItemClass itemToRemove;
    [SerializeField] private GameObject slotsHolder;

    [SerializeField] private GameObject inventoryPanel;
    private bool activated= false;

    [SerializeField] private SlotClass[] startingItems;
    private SlotClass[] items;
    private GameObject[] slots;

    private SlotClass movingSlot;
    private SlotClass tempSlot;
    private SlotClass originalSlot;
    bool isMovingItem;

    private void Start()
    {
        inventoryPanel.SetActive(activated);

        slots = new GameObject[slotsHolder.transform.childCount];
        items = new SlotClass[slots.Length];
        for (int i = 0; i < items.Length; i++)
        {
            items[i]= new SlotClass();
        }
        for (int i = 0; i < startingItems.Length; i++)
        {
            items[i] = startingItems[i];
        }

        for (int i = 0; i < slotsHolder.transform.childCount; i++)
        {
            slots[i] = slotsHolder.transform.GetChild(i).gameObject;
        }

        RefreshUI();
        Add(itemToAdd, 1);
        Remove(itemToRemove);
        
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isMovingItem)
            {
                EndItemMove();
            }
            else BeginItemMove();
        }
        itemCursor.SetActive(isMovingItem);

        if(SceneManager.GetActiveScene().name == "Menu")
        {
            Vector3 pos = Input.mousePosition;
            pos.z = 6f; // MUY IMPORTANTE (distancia desde la cámara)

            itemCursor.transform.position = Camera.main.ScreenToWorldPoint(pos);
        }
        else itemCursor.transform.position= Input.mousePosition;
        
        if (isMovingItem)
        {           
            itemCursor.GetComponent<Image>().sprite = movingSlot.GetItem().itemSprite;
        }
        
    }
    #region InventoryUtils
    public void RefreshUI()
    {
        for (int i = 0;i < slots.Length;i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = items[i].GetItem().itemSprite;
                if (items[i].GetItem().isStackable)
                {
                    slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = items[i].GetQuantity() + "";
                }else slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";

            }
            catch 
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                slots[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
            }
            
        }
    }
    public bool Add(ItemClass item, float quantity)
    {
        SlotClass slot = Contains(item);
        if(slot != null  && slot.GetItem().isStackable)
        {
            slot.AddQuantity(1);
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].GetItem() == null)
                {
                    items[i].AddItem(item, quantity);
                    break;
                }
            }

 /*           if(items.Count < slots.Length)
                items.Add(new SlotClass(item, 1));
            else return false;*/
        }
        RefreshUI();
        return true;
        
    }
    public bool Remove(ItemClass item)
    {
        SlotClass temp = Contains(item);
        if(temp != null) 
        {
            if(temp.GetQuantity() > 1) temp.TakeQuantity(1);
            else
            {
                int slotToRemoveIndex = 0;

                for(int i = 0; i < items.Length;  i++)
                {
                    if (items[i].GetItem() == item)
                    {
                        slotToRemoveIndex  = i;
                        break;
                    }      
                }
                items[slotToRemoveIndex].Clear();
            }
        }
        else return false;


        RefreshUI();
        return true;

        //items.Remove(item); 
        
    }
    public SlotClass Contains(ItemClass item)
    {
        for(int i = 0;i < items.Length;i++)
        {
            if (items[i].GetItem() == item)
                return items[i];
        }

        return null;
    }
    #endregion InventoryUtils
    #region MovingStuff
    private bool BeginItemMove()
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null|| originalSlot.GetItem() == null) 
            return false;

        movingSlot = new SlotClass(originalSlot);
        originalSlot.Clear();
        isMovingItem = true;
        RefreshUI();    
        return true;
    }
    private bool EndItemMove() 
    {
        originalSlot = GetClosestSlot();
        if (originalSlot == null)
        {
            originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
            movingSlot.Clear();
        }
        else
        {
            if (originalSlot.GetItem() != null)
            {
                if (originalSlot.GetItem() == movingSlot.GetItem())
                {
                    if (originalSlot.GetItem().isStackable)
                    {
                        originalSlot.AddQuantity(movingSlot.GetQuantity());
                    }
                    else return false;

                }
                else
                {
                    tempSlot = new SlotClass(originalSlot);
                    originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                    movingSlot.AddItem(tempSlot.GetItem(), tempSlot.GetQuantity());
                    RefreshUI();
                    return true;
                }
            }
            else
            {
                originalSlot.AddItem(movingSlot.GetItem(), movingSlot.GetQuantity());
                movingSlot.Clear();
            }
        }
        isMovingItem = false;
        RefreshUI();
        return true;
    }
    private SlotClass GetClosestSlot()
    {
        if(SceneManager.GetActiveScene().name == "Menu")
        {
            for (int i = 0; i < slots.Length; i++)
            {
                Vector2 screenPos = Camera.main.WorldToScreenPoint(slots[i].transform.position);

                if (Vector2.Distance(screenPos, Input.mousePosition) <= 32)
                    return items[i];
            }

            return null;
        }
        else
        {
            for(int i = 0; i < slots.Length; i++)
            {
                if (Vector2.Distance(slots[i].transform.position, Input.mousePosition) <= 32)
                            return items[i];
            }

            return null;
        }
        
    } 
    #endregion MovingStuff
}
