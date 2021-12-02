using System;
using System.IO;
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
// JSON Object Class
// ************************************************************************************
[System.Serializable]
public class Jitem
{
    public int id;
    public string dev_title;
    public Jitem_knowledge[] knowledge;
    public bool is_story;
}

// ************************************************************************************
// JSON Object List Class
// ************************************************************************************
[System.Serializable]
public class Jitem_list
{
    public Jitem[] items;
}

// ************************************************************************************
// JSON Object Knowledge List Class
// ************************************************************************************
[System.Serializable]
public class Jitem_knowledge
{
    public int level;
    public string title;
    public string description;
}

// ************************************************************************************
// Inventory Item Class
// ************************************************************************************
public class Item
{
    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private bool was_examined = false;              // Whether Item has been Examined

    //private string name;                            // Holds Item Name-Title
    private int knowledge_level = 0;                // Holds Item Level of Knowledge
    private int id;                                 // Holds Item ID

    private bool is_story;                          // Whether Item is a Story Item
    private bool uses_prefab;                       // Whether Item References a Prefab

    private float scale;                            // Holds Item Scale
    private float pickup_time;                      // Time Item was Picked Up. Changes for every Knowledge Increase

    private GameObject item_prefab;                 // Item Prefab GameObject

    private MeshFilter mesh_filter;                 // Holds Mesh of Item
    private MeshRenderer mesh_renderer;             // Holds Material of Item

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Item Constructor
    public Item(int item_id, MeshFilter mesh_f, MeshRenderer mesh_r, float item_scale)
    {
        id = item_id;

        is_story = false;                // Initialize is_story Flag

        uses_prefab = false;

        scale = item_scale;
        mesh_filter = mesh_f;
        mesh_renderer = mesh_r;
    }

    // Prefab Item Constructor
    public Item(int item_id, GameObject prefab, float item_scale)
    {
        id = item_id;

        is_story = false;               // Initialize is_story Flag

        scale = item_scale;

        uses_prefab = true;

        item_prefab = prefab;           // Set Prefab Reference
    }

    // Get Uses Prefab
    public bool isUsingPrefab()
    {
        return this.uses_prefab;
    }

    // Get Prefab
    public GameObject getPrefab()
    {
        try
        {
            return this.item_prefab;
        }
        catch (Exception)
        {
            Debug.LogError("No Prefab is Set.");

            return null;
        }
    }

    // Get Pickup Time
    public float getPickupTime()
    {
        return this.pickup_time;
    }

    // Update Pickup Time
    public void updatePickupTime()
    {
        this.pickup_time = Time.time;
    }

    // Get Examined Flag
    public bool wasExamined()
    {
        return this.was_examined;
    }

    // Get is_story Flag
    public bool getIsStory()
    {
        return this.is_story;
    }

    // Set is_story Flag
    public void setIsStory(bool flag)
    {
        this.is_story = flag;
    }

    // Set Examined Flag
    public void setExamined(bool flag)
    {
        this.was_examined = flag;
    }

    // Get Item ID
    public int getID()
    {
        return this.id;
    }

    // Get Item Knowledge Level
    public int getLevel()
    {
        return this.knowledge_level;
    }

    // Set Item Knowledge Level

    public void setLevel(int target_level)
    {
        this.knowledge_level = target_level;

        this.updatePickupTime();
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
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Whether to Display Item On Pickup.")]
    public bool display_on_pickup = false;              // Whether to Display Item on Pickup

    public float display_distance = 0.5f;               // Distance from Camera to Display Item
    public float rotation_speed = 100.0f;               // Speed of Rotation for Item Display
    public float scale_speed = 10.0f;                   // Speed of Scaling for Item Display
    [Tooltip("The Time the UI Element Will Stay Active.")]
    public float delay = 2.0f;                          // UI Element Active Delay

    public GameObject display_light;                    // Extra Light for Item Display
    public GameObject examine_ui;                       // Examine UI GameObject
    public GameObject title_text;                       // Item Title UI GameObject
    public GameObject ui_inventory;                     // Inventory UI GameObject
    public GameObject ui_main;                          // Main UI GameObject
    public GameObject item_ui_prefab;                   // Item UI Prefab
    public GameObject description_ui;                   // Item Description UI Element
    public GameObject invalid_ui;                       // Invalid Item UI Element
    public GameObject knowledge_acq_ui;                 // New Knowledge UI Element
    public GameObject button_layout;                    // Button UI Layout GameObject
    public Button examine_but;                          // Examine Button UI Element
    public Button use_but;                              // Use Button UI Element

    public Camera examination_camera;                   // Object Examination Camera

    public KeyCode inventory_key = KeyCode.I;           // Key that Opens Inventory Menu

    public TextAsset json_file_name;                    // JSON File with Item Information

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private List<Item> inventory = new List<Item>();    // List of Items in Inventory
    private List<Tuple<int, int>> levelup_queue = new List<Tuple<int, int>>();  // List of Queued Items to be Leveled Up

    private bool was_clicked = false;                   // Mouse Click Flag
    private bool display_on = false;                    // Item Being Displayed
    private bool inventory_open = false;                // Inventory is Open/Closed
    private bool examine_on = false;                    // In Inventory Examine is On/Off
    private bool item_slot_flag = false;                // Main Camera is Pointing to Item Slot
    private bool ray_trig = false;                      // Ray Hit Examinable GameObject

    private int selected_item;                          // ID of Currently Selected Item
    
    private float ui_counter_value = 0.0f;              // UI Counter Value

    private GameObject player_object;                   // Player GameObject
    private GameObject camera_object;                   // Camera GameObject
    private GameObject displayed_object;                // Object Being Displayed
    private GameObject selected_slot;                   // Currently Selected Slot
    private GameObject hit_gameobject;                  // Examinable GameObject Hit by Ray

    private Jitem_list items_in_json;                   // Items in JSON File

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Get Whether Inventory is Open
    public bool isInventoryOpen()
    {
        return inventory_open;
    }

    // Item Slot Set

    public void setSlotFlag(bool slot_flag, GameObject slot)
    {
        item_slot_flag = slot_flag;                         // Set Flag

        selected_slot = slot;                               // Set Slot GameObject
    }

    // Public Function to Query Inventory

    public bool startQuery(int q_id)
    {
        return queryInventory(q_id);
    }

    // Overload for Above Function

    public bool startQuery(int[] q_ids)
    {
        for (int i = 0; i < q_ids.Length; i++)
        {
            if (queryInventory(q_ids[i]))
            {
                return true;
            }
        }

        return false;
    }

    // Set Item Description in UI && item as Selected

    public void setDescription(int it_id)
    {
        selected_item = it_id;                                                              // Set Selected Item ID

        int it_level = getKnowledgeFromID(it_id);                                           // Get Item Knowledge Level from ID

        description_ui.GetComponent<Text>().text = getKnowledge(it_id, it_level).Item2;     // Set Description in UI Element

        button_layout.SetActive(true);                                                      // Enable Buttons
    }

    // Build Item
    public void buildItem(int item_id, MeshFilter mesh_f, MeshRenderer mesh_r, float item_scale)
    {
        Item new_item = new Item(item_id, mesh_f, mesh_r, item_scale);              // Build Item

        new_item.setIsStory(isStory(item_id));                                      // Set Whether Item is Story Item

        // Set Story Item as Examined to Keep it at Knowledge Level 0
        if (new_item.getIsStory())
        {
            new_item.setExamined(true);
        }

        addItem(new_item);                                                          // Add to Inventory

        if (display_on_pickup)
        {
            new_item.setLevel(1);                                                       // Set Level to 1
            new_item.setExamined(true);                                                 // Set As Examined

            displayItem(new_item);                                                      // Display Item
        }
    }

    // Build Item Overload with Prefab Reference
    public void buildItem(int item_id, GameObject prefab, float item_scale)
    {
        Item new_item = new Item(item_id, prefab, item_scale);                      // Build Item

        new_item.setIsStory(isStory(item_id));                                      // Set Whether Item is Story Item

        // Set Story Item as Examined to Keep it at Knowledge Level 0
        if (new_item.getIsStory())
        {
            new_item.setExamined(true);
        }

        addItem(new_item);                                                          // Add to Inventory

        if (display_on_pickup)
        {
            new_item.setLevel(1);                                                       // Set Level to 1
            new_item.setExamined(true);                                                 // Set As Examined

            displayItem(new_item);                                                      // Display Item
        }
    }

    // Increase Knowledge Level of Item

    public void increaseKnowledge(int i_id, int target_level)
    {
        bool existance = false;     // Target Item Is/Isn't in Inventory

        foreach (Item it in inventory)
        {
            if (it.getID() == i_id)         // Find Item
            {
                it.setLevel(target_level);      // Set Level

                existance = true;

                knowledge_acq_ui.SetActive(true);   // Activate UI Element
                ui_counter_value = Time.time;       // Set Timer

                break;
            }
        }

        // Item Not in Inventory
        if (!existance)
        {
            levelup_queue.Add(new Tuple<int, int>(i_id, target_level));     // Add Info to Level-Up Queue
        }
    }

    // Set Ray Trigger

    public void setRayTrig(bool ray_state, GameObject hit_object)
    {
        ray_trig = ray_state;

        if (ray_trig)
        {
            hit_gameobject = hit_object;

            examine_ui.SetActive(true);                                                                     // Enable Examine UI
        }
        else
        {
            examine_ui.SetActive(false);                                                                    // Disable Examine UI
        }
    }

    // Display Examinable Item

    private void examineItem()
    {
        examine_ui.SetActive(false);                                                                    // Disable Examine UI
        display_light.SetActive(true);                                                                  // Enable Light

        display_on = true;                                                                              // Enable Display Flag

        displayed_object = Instantiate(hit_gameobject.GetComponent<ExaminableItem>().getPrefab());      // Get GameObject Prefab
        displayed_object.tag = "Clone";                                                                 // Set Tag
        displayed_object.layer = 7;                                                                     // Assign to UI Layer

        // Process Potential Child GameObjects

        int child_index = 0;                                                                            // Index of Child of GameObject

        int num_children = hit_gameobject.transform.childCount;                                         // Get Child Count

        while (child_index < num_children)
        {
            displayed_object.transform.GetChild(child_index).gameObject.layer = 7;                          // Assign Child to UI Layer

            child_index++;                                                                                  // Increment Index
        }

        displayed_object.transform.position = camera_object.transform.position + transform.forward;     // Transform In Front of Camera

        float new_scale = hit_gameobject.GetComponent<ExaminableItem>().scale_factor;                   // Get Scale

        examination_camera.clearFlags = CameraClearFlags.Depth;                                         // Set Clear Flags
        examination_camera.enabled = true;                                                              // Enable Examination Camera

        displayed_object.transform.localScale = new Vector3(new_scale, new_scale, new_scale);           // Scale GameObject
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
            inventory.Add(item);                                // Add Item to Inventory List

            // Check Whether Knowledge Level-Up is Pending for Item

            int cnt = 0;                                        // Reference to List Index
            foreach (Tuple<int, int> queued in levelup_queue)
            {
                // Search for Item ID in Queue
                if (queued.Item1 == item.getID())
                {
                    item.setLevel(queued.Item2);                        // Update Knowledge Level

                    levelup_queue.RemoveAt(cnt);                        // Remove List Element After Update

                    break;
                }

                cnt++;
            }
        }
    }

    // Remove Item from Inventory

    private void removeItem(Item item)
    {
        bool flag = false;
        foreach (Item it in inventory)                      // Check that item is in Inventory
        {
            if (item.getID() == it.getID())
            {
                flag = true;

                break;
            }
        }

        if (flag)
        {
            inventory.Remove(item);
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

    // Get Knowledge Level from Item ID

    private int getKnowledgeFromID(int i_id)
    {
        int knowledge_level = 0;

        foreach (Item it in inventory)
        {
            if (compareID(i_id, it))
            {
                knowledge_level = it.getLevel();

                break;
            }
        }

        return knowledge_level;
    }

    // Display Item in Front of Camera

    private void displayItem(Item item)
    {
        // Check if Display was Started from Inventory
        if (examine_on)
        {
            closeInventory();
        }

        display_light.SetActive(true);                                                                  // Enable Light

        title_text.GetComponent<Text>().text = getKnowledge(item.getID(), item.getLevel()).Item1;       // Change Text to Item's Name

        title_text.SetActive(true);                                                                     // Enable Title Text

        display_on = true;                                                                              // Enable Display Flag

        GameObject new_go = new GameObject("DisplayedItem");                                            // Create GameObject Object Using Constructor

        new_go.layer = 7;                                                                               // Assign to UI Layer

        new_go.AddComponent<MeshFilter>(item.getMeshFilter());                                          // Add MeshFilter
        Material mat = item.getMaterial();                                                              // Get Material from Renderer
        var mr = new_go.AddComponent<MeshRenderer>(item.getMeshRenderer());                             // Add MeshRenderer
        mr.material = mat;                                                                              // Assign Material

        new_go.transform.position = camera_object.transform.position + transform.forward;

        float scale = item.getScale();                                                                  // Get Scale
        new_go.transform.localScale = new Vector3(scale, scale, scale);                                 // Set Scale

        examination_camera.clearFlags = CameraClearFlags.Depth;                                         // Set Clear Flags
        examination_camera.enabled = true;                                                              // Enable Examination Camera

        displayed_object = new_go;                                                                      // Set Displayed Object
    }

    // Display Prefab Item
    private void displayPrefabItem(Item item)
    {
        // Check if Display was Started from Inventory
        if (examine_on)
        {
            closeInventory();
        }

        title_text.GetComponent<Text>().text = getKnowledge(item.getID(), item.getLevel()).Item1;       // Change Text to Item's Name

        title_text.SetActive(true);                                                                     // Enable Title Text

        display_light.SetActive(true);                                                                  // Enable Light

        display_on = true;                                                                              // Enable Display Flag

        displayed_object = Instantiate(item.getPrefab());                                               // Get GameObject Prefab
        displayed_object.tag = "Clone";                                                                 // Set Tag
        displayed_object.layer = 7;                                                                     // Assign to UI Layer

        // Process Potential Child GameObjects

        int child_index = 0;                                                                            // Index of Child of GameObject

        int num_children = displayed_object.transform.childCount;                                       // Get Child Count

        while (child_index < num_children)
        {
            displayed_object.transform.GetChild(child_index).gameObject.layer = 7;                          // Assign Child to UI Layer

            child_index++;                                                                                  // Increment Index
        }

        displayed_object.transform.position = camera_object.transform.position + transform.forward;     // Transform In Front of Camera

        float new_scale = item.getScale();                                                              // Get Scale

        examination_camera.clearFlags = CameraClearFlags.Depth;                                         // Set Clear Flags
        examination_camera.enabled = true;                                                              // Enable Examination Camera

        displayed_object.transform.localScale = new Vector3(new_scale, new_scale, new_scale);           // Scale GameObject
    }

    // Exit Item Display

    private void exitDisplay()
    {
        Destroy(GameObject.FindWithTag("Clone"));                                                       // Black Magic Issue Fix

        display_light.SetActive(false);                                                                 // Disable Light
        title_text.SetActive(false);                                                                    // Disable Title Text
        display_on = false;                                                                             // Disable Display Flag

        player_object.GetComponent<FirstPersonMovement>().stop_flag = false;                            // UnFreeze Player Controller
        camera_object.GetComponent<FirstPersonLook>().stop_flag = false;                                // UnFreeze Camera Controller

        Destroy(displayed_object);                                                                      // Destroy GameObject
        displayed_object = null;                                                                        // Reset GameObject Variable

        // Check if Display was Started from Inventory
        if (examine_on)
        {
            openInventory();

            examine_on = false;
        }

        examination_camera.enabled = false;                                                             // Disable Examination Camera
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

    // Find Item Name and Description from JSON File Based on Knowledge

    private Tuple<string, string> getKnowledge(int i_id, int k_level)
    {
        string i_title = "PLACEHOLDER";                                                     // Initialize Title Variable
        string i_description = "PLACEHOLDER";                                               // Initialize Description Variable

        foreach (Jitem jitem in items_in_json.items)                                        // Get Knowledge of Requested Item
        {
            // Find Item with Requested ID and Get Knowledge
            if (jitem.id == i_id)
            {
                i_title = jitem.knowledge[k_level].title;
                i_description = jitem.knowledge[k_level].description;

                break;
            }
        }

        return new Tuple<string, string>(i_title, i_description);
    }

    // Get Whether Item is Story Item
    private bool isStory(int i_id)
    {   
        // TODO: #8 @pclank Set Exception Catching Here
        return items_in_json.items[i_id - 1].is_story;
    }

    // Open Item Inventory

    private void openInventory()
    {
        inventory_open = true;              // Set Inventory to Open

        player_object.GetComponent<FirstPersonMovement>().stop_flag = true;     // Freeze Player Controller
        camera_object.GetComponent<FirstPersonLook>().stop_flag = true;         // Freeze Camera Controller

        ui_main.SetActive(true);            // Enable Inventory GameObject

        // Fill Item List

        foreach (Item it in inventory)
        {
            GameObject temp_ui_item = item_ui_prefab;                                                           // Get Prefab Instance to Edit Before Adding to Inventory

            temp_ui_item.GetComponentInChildren<Text>().text = getKnowledge(it.getID(), it.getLevel()).Item1;   // Assign Item Name
            temp_ui_item.GetComponentInChildren<ui_click_detector>().item_id = it.getID();                      // Assign Item ID

            Instantiate(temp_ui_item, ui_inventory.transform);
        }

        Cursor.lockState = CursorLockMode.None;         // Unlock Cursor
        Cursor.visible = true;                          // Make Cursor Visible
    }

    // Close Item Inventory

    private void closeInventory()
    {
        inventory_open = false;                         // Set Inventory to Closed

        ui_main.SetActive(false);

        description_ui.GetComponent<Text>().text = "";  // Reset Description Text

        // Clear Item List

        foreach (Transform child in ui_inventory.transform)
        {
            Destroy(child.gameObject);                      // Destroy GameObject
        }

        player_object.GetComponent<FirstPersonMovement>().stop_flag = false;        // Unfreeze Player Controller
        camera_object.GetComponent<FirstPersonLook>().stop_flag = false;            // Unfreeze Camera Controller

        Cursor.lockState = CursorLockMode.Locked;                                   // Lock Cursor to Center
        Cursor.visible = false;                                                     // Hide Cursor

        button_layout.SetActive(false);                                             // Disable Buttons
    }

    // Function to Use Item from Inventory

    private void useItem(Item item_used)
    {
        bool cp = GameObject.FindWithTag("CassettePlayer").GetComponent<CassettePlayer>().getRaycast();

        // Check if User can Interact with Slot
        if (item_slot_flag)
        {
            bool validation = selected_slot.GetComponent<ItemSlot>().placeItem(item_used);          // Place Item on Slot

            // If Validation is True
            if (validation)
            {
                closeInventory();                   // Close Inventory

                removeItem(item_used);              // Remove Item from Inventory
            }
            else
            {
                invalid_ui.SetActive(true);         // Enable Element
            }
        }
        // Check Pointing at Cassette Player
        else if (cp)
        {
            bool validation = GameObject.FindWithTag("CassettePlayer").GetComponent<CassettePlayer>().placeInPlayer(item_used);     // Place Cassette in Tape

            // If Validation is True
            if (validation)
            {
                closeInventory();                   // Close Inventory

                removeItem(item_used);              // Remove Item from Inventory
            }
            else
            {
                invalid_ui.SetActive(true);         // Enable Element
            }
        }
    }

    // ************************************************************************************
    // Runtime Functions
    // ************************************************************************************

    // Start is called before the first frame update
    void Start()
    {
        items_in_json = JsonUtility.FromJson<Jitem_list>(json_file_name.text);   // Deserialize JSON File

        player_object = this.gameObject;                                // Get Player GameObject
        camera_object = GameObject.FindWithTag("MainCamera");           // Get Camera GameObject

        ui_main.SetActive(false);                                       // Disable Inventory UI on Start

        display_light.SetActive(false);                                 // Disable Light on Start

        // Check that Inventory UI GameObject has Been Assigned
        if (ui_inventory == null)
        {
            Debug.Log("Inventory GameObject not Found!");
        }

        // Register Button Listeners

        examine_but.onClick.AddListener(() => buttonCallBack(examine_but));
        use_but.onClick.AddListener(() => buttonCallBack(use_but));
    }

    // Update is called once per frame
    void Update()
    {
        if (inventory_open)
        {
            player_object.GetComponent<FirstPersonMovement>().stop_flag = true;     // Freeze Player Controller
            camera_object.GetComponent<FirstPersonLook>().stop_flag = true;         // Freeze Camera Controller
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            was_clicked = true;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            was_clicked = false;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && display_on)                           // Exit Item Display on Right Click
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

        // Examinable GameObject Examine Section
        
        if (!examine_on && ray_trig && was_clicked && !display_on)
        {
            examineItem();
        }

        // Disable Invalid Item UI Element
        if (!item_slot_flag)
        {
            invalid_ui.SetActive(false);
        }

        // Displaying Section

        if (display_on)                                                 // Item Being Displayed in Inventory
        {
            camera_object.transform.LookAt(displayed_object.transform);             // Look at Item
            player_object.GetComponent<FirstPersonMovement>().stop_flag = true;     // Freeze Player Controller
            camera_object.GetComponent<FirstPersonLook>().stop_flag = true;         // Freeze Camera Controller

            if (was_clicked)
            {
                float horizontal_rotation = Input.GetAxis("Mouse X") * rotation_speed;      // Calculate Horizontal Rotation
                float vertical_rotation = Input.GetAxis("Mouse Y") * rotation_speed;        // Calculate Vertical Rotation

                float scale_factor = Input.GetAxis("Mouse ScrollWheel") * scale_speed + displayed_object.transform.localScale.x;    // Calculate Zoom - Scaling

                displayed_object.transform.Rotate(vertical_rotation, horizontal_rotation, vertical_rotation);   // Apply Rotation
                displayed_object.transform.localScale = new Vector3(scale_factor, scale_factor, scale_factor);  // Set Scale
            }
        }

        // Retrieve Item from Slot Section

        if (item_slot_flag && was_clicked)                              // Pointing at Slot, and Left-Click Detected
        {
            Item removed_item = selected_slot.GetComponent<ItemSlot>().removeItem();            // Remove Item from Slot

            // If Item was Removed

            if (removed_item != null)
            {
                addItem(removed_item);                                  // Add Item Back to Inventory
            }
        }

        // UI Counter Section

        if (ui_counter_value != 0.0f && Time.time - ui_counter_value >= delay)
        {
            knowledge_acq_ui.SetActive(false);      // Disable UI Element

            ui_counter_value = 0.0f;                // Reset
        }
    }

    // Button CallBack Function

    private void buttonCallBack(Button but_pressed)
    {
        // Examine Button Pressed
        if (but_pressed == examine_but)
        {
            Item selected_it = null;                    // Initialize Selected Item

            // Find Selected Item

            foreach (Item it in inventory)
            {
                if (compareID(selected_item, it))           // Use Member Function to Compare IDs
                {
                    selected_it = it;                           // Set Item

                    break;
                }
            }

            // Check for Knowledge Update on First Item Examination

            if (!selected_it.wasExamined())
            {
                selected_it.setLevel(1);                    // Set Level to 1
                selected_it.setExamined(true);              // Set Item as Already Examined
            }

            // Debug Section to Show Current Item Knowledge Level
            Debug.Log("Knowledge Level: " + selected_it.getLevel());

            examine_on = true;

            if (selected_it.isUsingPrefab())
                displayPrefabItem(selected_it);             // Display Prefab Item
            else
                displayItem(selected_it);                   // Display Item
        }

        // Use Button Pressed
        else if (but_pressed == use_but)
        {
            Item selected_it = null;                    // Initialize Selected Item

            // Find Selected Item

            foreach (Item it in inventory)
            {
                if (compareID(selected_item, it))           // Use Member Function to Compare IDs
                {
                    selected_it = it;                           // Set Item

                    break;
                }
            }

            useItem(selected_it);                       // Call Use Function
        }
    }
}
