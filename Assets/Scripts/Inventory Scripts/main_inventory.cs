using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

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

    public float display_distance = 0.5f;               // Distance from Camera to Display Item
    public float rotation_speed = 100.0f;               // Speed of Rotation for Item Display
    public float scale_speed = 10.0f;                   // Speed of Scaling for Item Display

    public bool background_enable = false;              // Enable Background for Item Display

    public GameObject display_background;
    public GameObject display_light;
    public GameObject title_text;                       // Item Title UI GameObject
    public GameObject ui_inventory;                     // Inventory UI GameObject
    public GameObject ui_main;                          // Main UI GameObject
    public GameObject item_ui_prefab;                   // Item UI Prefab

    public KeyCode inventory_key = KeyCode.I;           // Key that Opens Inventory Menu

    // Private Variables

    private List<Item> inventory = new List<Item>();    // List of Items in Inventory

    private bool was_clicked = false;                   // Mouse Click Flag
    private bool cam_trig = false;                      // Camera Pointing to Trigger Flag
    private bool display_on = false;                    // Item Being Displayed
    private bool inventory_open = false;                // Inventory is Open/Closed

    private GameObject player_object;                   // Player GameObject
    private GameObject camera_object;                   // Camera GameObject

    private GameObject displayed_object;                // Object Being Displayed

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

    // Public Function to Query Inventory

    public bool startQuery(int q_id)
    {
        return queryInventory(q_id);
    }

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
            if (item.getID() == it.getID())
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

    private bool compareID(int item1, Item item2)
    {
        if (item1 == item2.getID())
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
        if (background_enable)
        {
            display_background.SetActive(true);                                                             // Enable Background
        }

        display_light.SetActive(true);                                                                  // Enable Light

        title_text.GetComponent<Text>().text = item.getName();                                          // Change Text to Item's Name

        title_text.SetActive(true);                                                                     // Enable Title Text

        display_on = true;                                                                              // Enable Display Flag

        GameObject new_go = new GameObject(item.getName());                                             // Create GameObject Object Using Constructor

        new_go.AddComponent<MeshFilter>(item.getMeshFilter());                                          // Add MeshFilter
        Material mat = item.getMaterial();                                                              // Get Material from Renderer
        var mr = new_go.AddComponent<MeshRenderer>(item.getMeshRenderer());                             // Add MeshRenderer
        mr.material = mat;                                                                              // Assign Material

        new_go.transform.position = camera_object.transform.position + transform.forward;

        float scale = item.getScale();                                                                  // Get Scale
        new_go.transform.localScale = new Vector3(scale, scale, scale);                                 // Set Scale

        displayed_object = new_go;                                                                      // Set Displayed Object
    }

    // Exit Item Display

    private void exitDisplay()
    {
        display_background.SetActive(false);                                                            // Disable Background
        display_light.SetActive(false);                                                                 // Disable Light
        title_text.SetActive(false);                                                                    // Disable Title Text
        display_on = false;                                                                             // Disable Display Flag

        player_object.GetComponent<FirstPersonMovement>().stop_flag = false;                            // UnFreeze Player Controller

        Destroy(displayed_object);                                                                      // Destroy GameObject
        displayed_object = null;                                                                        // Reset GameObject Variable
    }

    // Query Inventory for Item

    private bool queryInventory(int query_item)
    {
        bool result = false;                                                                            // Initialize Returned Variable

        foreach (Item it in inventory)
        {
            if (compareID(query_item, it))                                                                      // Use Member Function to Compare IDs
            {
                result = true;

                return result;                                                                                      // Return
            }
        }

        return result;
    }

    // Open Item Inventory

    private void openInventory()
    {
        inventory_open = true;              // Set Inventory to Open

        ui_main.SetActive(true);            // Enable Inventory GameObject

        // Fill Item List

        foreach (Item it in inventory)
        {
            GameObject temp_ui_item = item_ui_prefab;                           // Get Prefab Instance to Edit Before Adding to Inventory

            temp_ui_item.GetComponentInChildren<Text>().text = it.getName();    // Assign Item Name
            Debug.Log(it.getName());

            Instantiate(temp_ui_item, ui_inventory.transform);
        }
    }

    // Close Item Inventory

    private void closeInventory()
    {
        inventory_open = false;             // Set Inventory to Closed

        ui_main.SetActive(false);

        // Clear Item List

        foreach (Transform child in ui_inventory.transform)
        {
            Destroy(child.gameObject);          // Destroy GameObject
        }
    }

    // ************************************************************************************
    // Runtime Functions
    // ************************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        player_object = this.gameObject;                                // Get Player GameObject
        camera_object = GameObject.FindWithTag("MainCamera");           // Get Camera GameObject

        ui_main.SetActive(false);                                       // Disable Inventory UI on Start

        display_background.SetActive(false);                            // Disable Background on Start
        display_light.SetActive(false);                                 // Disable Light on Start

        // Check that Inventory UI GameObject has Been Assigned
        if (ui_inventory == null)
        {
            Debug.Log("Inventory GameObject not Found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            was_clicked = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            was_clicked = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))                           // Exit Item Display on Right Click
        {
            exitDisplay();
        }

        if (Input.GetKeyDown(inventory_key) && !inventory_open)
        {
            openInventory();
        }
        else if (Input.GetKeyDown(inventory_key) && inventory_open)
        {
            closeInventory();
        }

        if (display_on)                                                 // Item Being Displayed in Inventory
        {
            camera_object.transform.LookAt(displayed_object.transform);             // Look at Item
            player_object.GetComponent<FirstPersonMovement>().stop_flag = true;     // Freeze Player Controller

            if (was_clicked)
            {
                float horizontal_rotation = Input.GetAxis("Mouse X") * rotation_speed;      // Calculate Horizontal Rotation
                float vertical_rotation = Input.GetAxis("Mouse Y") * rotation_speed;        // Calculate Vertical Rotation

                float scale_factor = Input.GetAxis("Mouse ScrollWheel") * scale_speed + displayed_object.transform.localScale.x;    // Calculate Zoom - Scaling

                displayed_object.transform.Rotate(vertical_rotation, horizontal_rotation, vertical_rotation);   // Apply Rotation
                displayed_object.transform.localScale = new Vector3(scale_factor, scale_factor, scale_factor);  // Set Scale
            }
        }
    }
}
