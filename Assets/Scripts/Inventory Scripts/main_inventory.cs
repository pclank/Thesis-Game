using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Inventory Item Class
public class Item
{
    // Public Variables

    public string name;
    public int id;

    // Member Functions

    public Item(string item_name, int item_id)      // Item Constructor
    {
        name = item_name;
        id = item_id;
    }

    // TODO: Add Functions Chief!
}

public class main_inventory : MonoBehaviour
{
    // Private Variables

    private List<Item> inventory = new List<Item>();    // List of Items in Inventory

    private bool was_clicked = false;                   // Mouse Click Flag
    private bool cam_trig = false;                      // Camera Pointing to Trigger Flag

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainCamera"))
        {
            cam_trig = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Build Item

    public void buildItem(string item_name, int item_id)
    {
        Item new_item = new Item(item_name, item_id);       // Build Item

        addItem(new_item);
    }

    // Add Item to Inventory

    private void addItem(Item item)
    {
        bool flag = true;
        foreach (Item it in inventory)
        {
            if (item.id != it.id)
            {
                flag = false;

                break;
            }
        }

        if (flag)
        {
            inventory.Add(item);
        }
    }

    public 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
