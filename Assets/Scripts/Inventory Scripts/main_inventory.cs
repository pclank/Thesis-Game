using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ************************************************************************************
// Inventory Item Class
// ************************************************************************************
public class Item
{
    // Public Variables

    public string name;                             // Holds Item Name-Title
    public int id;                                  // Holds Item ID

    // Private Variables

    private MeshFilter mesh_filter;                 // Holds Mesh of Item
    private MeshRenderer mesh_renderer;             // Holds Material of Item

    // Member Functions

    public Item(string item_name, int item_id, MeshFilter mesh_f, MeshRenderer mesh_r)      // Item Constructor
    {
        name = item_name;
        id = item_id;
        mesh_filter = mesh_f;
        mesh_renderer = mesh_r;
    }

    // Get Item ID

    public int getID()
    {
        return this.id;
    }

    // Get Item Name
    public string getName()
    {
        return this.name;
    }
}

public class main_inventory : MonoBehaviour
{
    // Private Variables

    private List<Item> inventory = new List<Item>();    // List of Items in Inventory

    private bool was_clicked = false;                   // Mouse Click Flag
    private bool cam_trig = false;                      // Camera Pointing to Trigger Flag

    private GameObject player_object;                   // Player GameObject

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
        if (other.gameObject.CompareTag("MainCamera"))
        {
            cam_trig = false;
        }
    }

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Build Item

    public void buildItem(string item_name, int item_id, MeshFilter mesh_f, MeshRenderer mesh_r)
    {
        Item new_item = new Item(item_name, item_id, mesh_f, mesh_r);   // Build Item

        addItem(new_item);                                              // Add to Inventory
    }

    // Add Item to Inventory

    private void addItem(Item item)
    {
        bool flag = true;
        foreach (Item it in inventory)                      // Check that item isn't in Inventory
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

    // Compare Item Tags

    private bool compareID(Item item1, Item item2)
    {
        if (item1.getID() == item2.getID())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // ************************************************************************************
    // Runtime Functions
    // ************************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
