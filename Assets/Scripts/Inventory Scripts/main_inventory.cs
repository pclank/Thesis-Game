using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// ************************************************************************************
// Helper Functions
// ************************************************************************************

public static class Helper
{
    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if (type != other.GetType()) return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach (var pinfo in pinfos)
        {
            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch { } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return comp as T;
    }

    public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
    {
        return go.AddComponent<T>().GetCopyOf(toAdd) as T;
    }
}

// ************************************************************************************
// Inventory Item Class
// ************************************************************************************
public class Item
{
    // Public Variables

    public string name;                             // Holds Item Name-Title
    public int id;                                  // Holds Item ID

    // Private Variables

    private Component mesh_filter;                 // Holds Mesh of Item
    private Component mesh_renderer;             // Holds Material of Item

    // Member Functions

    public Item(string item_name, int item_id, Component mesh_f, Component mesh_r)      // Item Constructor
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

    // Get Mesh Filter

    public Component getMeshFilter()
    {
        return this.mesh_filter;
    }
    
    // Get Mesh Renderer

    public Component getMeshRenderer()
    {
        return this.mesh_renderer;
    }
}

// ************************************************************************************
// Inventory Class
// ************************************************************************************
public class main_inventory : MonoBehaviour
{
    // Public Variables

    public float display_distance = 0.5f;

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

    public void buildItem(string item_name, int item_id, Component mesh_f, Component mesh_r)
    {
        Item new_item = new Item(item_name, item_id, mesh_f, mesh_r);   // Build Item

        addItem(new_item);                                              // Add to Inventory

        displayItem(new_item);                                          // Display Item
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

    // Display Item in Front of Camera

    private void displayItem(Item item)
    {
        GameObject new_go = new GameObject(item.getName(), typeof(MeshFilter), typeof(MeshRenderer));   // Create GameObject Object Using Constructor

        var sm = new_go.AddComponent<MeshFilter>(item.getMeshFilter());                                          // Add MeshFilter
        var se = new_go.AddComponent<MeshRenderer>(item.getMeshRenderer());                                      // Add MeshRenderer

        Vector3 camera_rotation = player_object.transform.localRotation.eulerAngles;                    // Get Camera Rotation
        float y_rotation = camera_rotation.y;                                                           // Get Y - Axis Rotation
        float x_rotation = camera_rotation.x;                                                           // Get X - Axis Rotation

        double x_space = display_distance * Math.Sin(y_rotation);                                       // Calculate X - Axis Distance
        double z_space = display_distance * Math.Cos(y_rotation);                                       // Calculate Z - Axis Distance
        double y_space = display_distance * Math.Cos(x_rotation);                                       // Calculate Y - Axis Distance

        if (y_rotation >= 0.0f && y_rotation < 90.0f)
        {
            // TODO: Add Functionality
        }
        else if (y_rotation >= 90.0f && y_rotation < 180.0f)
        {
            // TODO: Add Functionality
        }
        else if (y_rotation >= 180.0f && y_rotation < 270.0f)
        {
            // TODO: Add Functionality
        }
        else if (y_rotation >= 270.0f)
        {
            // TODO: Add Functionality
        }
    }

    // ************************************************************************************
    // Runtime Functions
    // ************************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        player_object = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
