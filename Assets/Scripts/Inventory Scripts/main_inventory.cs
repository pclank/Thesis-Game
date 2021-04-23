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

    // Private Variables

    private string name;                            // Holds Item Name-Title
    private int id;                                 // Holds Item ID
    private float scale;                            // Holds Item Scale

    private MeshFilter mesh_filter;                 // Holds Mesh of Item
    private MeshRenderer mesh_renderer;             // Holds Material of Item

    // Member Functions

    public Item(string item_name, int item_id, MeshFilter mesh_f, MeshRenderer mesh_r, float item_scale)    // Item Constructor
    {
        name = item_name;
        id = item_id;
        scale = item_scale;
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

    // Get Item Scale

    public float getScale()
    {
        return this.scale;
    }    

    // Get Mesh Filter

    public MeshFilter getMeshFilter()
    {
        return this.mesh_filter;
    }
    
    // Get Mesh Renderer

    public MeshRenderer getMeshRenderer()
    {
        return this.mesh_renderer;
    }

    public Material getMaterial()
    {
        return this.getMeshRenderer().material;
    }
}

// ************************************************************************************
// Inventory Class
// ************************************************************************************
public class main_inventory : MonoBehaviour
{
    // Public Variables

    public float display_distance = 0.5f;
    public GameObject display_background;
    public GameObject display_light;

    // Private Variables

    private List<Item> inventory = new List<Item>();    // List of Items in Inventory

    private bool was_clicked = false;                   // Mouse Click Flag
    private bool cam_trig = false;                      // Camera Pointing to Trigger Flag

    private GameObject player_object;                   // Player GameObject
    private GameObject camera_object;                   // Camera GameObject

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

    public void buildItem(string item_name, int item_id, MeshFilter mesh_f, MeshRenderer mesh_r, float item_scale)
    {
        Item new_item = new Item(item_name, item_id, mesh_f, mesh_r, item_scale);   // Build Item

        addItem(new_item);                                                          // Add to Inventory

        displayItem(new_item);                                                      // Display Item
    }

    // Add Item to Inventory

    private void addItem(Item item)
    {
        bool flag = true;
        foreach (Item it in inventory)                      // Check that item isn't in Inventory
        {
            if (item.getID() != it.getID())
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
        display_background.SetActive(true);                                                             // Enable Background
        display_light.SetActive(true);                                                                  // Enable Light

        GameObject new_go = new GameObject(item.getName());                                             // Create GameObject Object Using Constructor

        new_go.AddComponent<MeshFilter>(item.getMeshFilter());                                          // Add MeshFilter
        Material mat = item.getMaterial();                                                              // Get Material from Renderer
        var mr = new_go.AddComponent<MeshRenderer>(item.getMeshRenderer());                             // Add MeshRenderer
        mr.material = mat;                                                                              // Assign Material

        float x_tr = player_object.transform.position.x;                                                // Get X - Axis Position
        float z_tr = player_object.transform.position.z;                                                // Get Z - Axis Position
        new_go.transform.position = new Vector3(x_tr, 1.82f, z_tr);                                     // Set Initial Position to Camera Position

        Vector3 camera_rotation = player_object.transform.localRotation.eulerAngles;                    // Get Camera (Player GameObject) Rotation
        float y_rotation = camera_rotation.y;                                                           // Get Y - Axis Rotation
        float x_rotation = camera_rotation.x;                                                           // Get X - Axis Rotation

        float x_space = display_distance * (float)Math.Sin(y_rotation);                                 // Calculate X - Axis Distance
        float z_space = display_distance * (float)Math.Cos(y_rotation);                                 // Calculate Z - Axis Distance
        float y_space = display_distance * (float)Math.Cos(x_rotation);                                 // Calculate Y - Axis Distance

        if (y_rotation >= 0.0f && y_rotation < 90.0f)
        {
            new_go.transform.Translate(x_space, 0, z_space);
        }
        else if (y_rotation >= 90.0f && y_rotation < 180.0f)
        {
            new_go.transform.Translate(x_space, 0, -z_space);
        }
        else if (y_rotation >= 180.0f && y_rotation < 270.0f)
        {
            new_go.transform.Translate(-x_space, 0, -z_space);
        }
        else if (y_rotation >= 270.0f)
        {
            new_go.transform.Translate(-x_space, 0, z_space);
        }

        float scale = item.getScale();                                                                  // Get Scale
        new_go.transform.localScale = new Vector3(scale, scale, scale);                                 // Set Scale

        // TODO: Factor In Horizontal Rotation.
    }

    // ************************************************************************************
    // Runtime Functions
    // ************************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        player_object = this.gameObject;                                // Get Player GameObject
        camera_object = GameObject.FindWithTag("MainCamera");           // Get Camera GameObject

        display_background.SetActive(false);                            // Disable Background on Start
        display_light.SetActive(false);                                 // Disable Light on Start
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
