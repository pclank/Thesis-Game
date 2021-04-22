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

    // TODO: Add Functions Chief!
}

public class main_inventory : MonoBehaviour
{
    // Private Variables

    private List<Item> inventory = new List<Item>();      // List of Items in Inventory

    // Member Functions

    public void addItem(Item item)
    {
        inventory.Add(item);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
