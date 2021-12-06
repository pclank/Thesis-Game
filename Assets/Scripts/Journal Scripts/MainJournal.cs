using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

// ************************************************************************************
// Journal List Class
// ************************************************************************************

[System.Serializable]
public class JournalList
{
    public JournalCategoryJSON[] journal_list;
}

// ************************************************************************************
// Journal Category JSON Class
// ************************************************************************************

[System.Serializable]
public class JournalCategoryJSON
{
    public int id;

    public string title;

    public JournalEntry[] entries;

    // Constructor
    public JournalCategoryJSON(int id, string title)
    {
        this.id = id;
        this.title = title;
    }
}

// ************************************************************************************
// Journal Category Class
// ************************************************************************************

public class JournalCategory
{
    public int id;

    public string title;

    public List<JournalEntry> entries;

    // Constructor
    public JournalCategory(int id, string title)
    {
        this.id = id;
        this.title = title;

        this.entries = new List<JournalEntry>();
    }
}

// ************************************************************************************
// Journal Entry Class
// ************************************************************************************

[System.Serializable]
public class JournalEntry
{
    public int id;

    public string title;
    public string line;

    // Constructor
    public JournalEntry(int id, string title, string line)
    {
        this.id = id;
        this.title = title;
        this.line = line;
    }
}

// ************************************************************************************
// Player Journal Main Control Script
// ************************************************************************************

public class MainJournal : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("JSON File Containing Entries.")]
    public TextAsset json_file;

    [Tooltip("Journal UI GameObject Parent.")]
    public GameObject journal_ui;

    [Tooltip("Journal Entry Line.")]
    public GameObject journal_line;

    [Tooltip("Category Parent GameObject.")]
    public GameObject journal_ui_parent;

    [Tooltip("UI GameObject for New Entry Notification.")]
    public GameObject new_entry_notification_ui;

    [Tooltip("UI Prefab for Category.")]
    public GameObject category_prefab;

    [Tooltip("UI Prefab for Entry.")]
    public GameObject entry_prefab;

    [Tooltip("Open Journal SFX Event.")]
    public AK.Wwise.Event open_sfx;

    [Tooltip("Close Journal SFX Event.")]
    public AK.Wwise.Event close_sfx;

    [Tooltip("Key to Open Journal.")]
    public KeyCode open_key = KeyCode.J;

    [Header("Color Options")]
    [Tooltip("Category Expanded Color.")]
    public Color category_expanded_color = Color.blue;

    [Tooltip("Category Default Color.")]
    public Color category_default_color = Color.white;

    [Tooltip("Entry Selected Color.")]
    public Color entry_selected_color = Color.cyan;

    [Tooltip("Entry Default Color.")]
    public Color entry_default_color = Color.white;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;                                           // Player GameObject
    private GameObject camera_object;                                           // Camera GameObject
    private GameObject selected_entry;                                          // Selected Entry UI Object

    private JournalList journal_list;                                           // Journal Entries List

    private List<JournalCategory> player_journal = new List<JournalCategory>(); // Player Journal List

    private bool journal_open = false;                                          // Whether Journal is Open
    private bool notification_active = false;                                   // Whether Notification UI is Enabled

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Add Journal Category
    public void addCategory(int category_id)
    {
        JournalCategory new_category = new JournalCategory(category_id, "PLACEHOLDER");

        new_category.title = journal_list.journal_list[category_id].title;              // Assign Title
        new_category.entries.Add(journal_list.journal_list[category_id].entries[0]);    // Add First Entry

        player_journal.Add(new_category);                                               // Add Category Object to Journal

        showNotification();                                                             // Show New Entry Notification
    }

    // Add Journal Entry
    public void addEntry(int category_id, int entry_id)
    {
        JournalEntry new_entry = new JournalEntry(entry_id, "PLACEHOLDER", "PLACEHOLDER");

        new_entry.title = journal_list.journal_list[category_id].entries[entry_id].title;
        new_entry.line = journal_list.journal_list[category_id].entries[entry_id].line;

        // Check if Category has been Added
        if (player_journal[category_id] != null)
            player_journal[category_id].entries.Add(new_entry);     // Add New Entry to Journal
        else
        {
            addCategory(category_id);                               // Add Category if Missing
            player_journal[category_id].entries.Add(new_entry);     // Add New Entry to Journal
        }

        showNotification();                                                             // Show New Entry Notification
    }

    // Expand Entries of Selected Category
    public void expandCategory(int id, GameObject category_object)
    {
        category_object.GetComponent<Image>().color = category_expanded_color;

        JournalCategory j_category = player_journal[id];            // Get Category

        // Add Entries in Category
        foreach (JournalEntry j_entry in j_category.entries)
        {
            GameObject temp_ui_entry = entry_prefab;

            temp_ui_entry.GetComponentInChildren<Text>().text = j_entry.title;

            temp_ui_entry.GetComponent<JournalEntryUI>().id = j_entry.id;

            Instantiate(temp_ui_entry, category_object.GetComponentInChildren<VerticalLayoutGroup>().transform);
        }
    }

    // Collapse Entries of Expanded Category
    public void collapseCategory(int id, GameObject category_object)
    {
        category_object.GetComponent<Image>().color = category_default_color;

        foreach (Transform child in category_object.GetComponentInChildren<VerticalLayoutGroup>().transform)
        {
            Destroy(child.gameObject);
        }
    }

    // Process Selected Entry
    public void processSelectedEntry(int id, GameObject entry_object)
    {
        // Deselect Previous Entry
        if (selected_entry != null)
            processDeselectedEntry(selected_entry);

        entry_object.GetComponent<Image>().color = entry_selected_color;

        journal_line.GetComponent<Text>().text = player_journal[entry_object.GetComponentInParent<JournalEntryUI>().id].entries[id].line;

        selected_entry = entry_object;                                          // Update Selected Entry UI Object
    }

    // Process Deselected Entry
    private void processDeselectedEntry(GameObject entry_object)
    {
        entry_object.GetComponent<Image>().color = entry_default_color;

        journal_line.GetComponent<Text>().text = "";

        selected_entry = null;
    }

    // Open Journal
    private void openJournal()
    {
        journal_open = true;                                                    // Set Inventory to Open

        player_object.GetComponent<FirstPersonMovement>().stop_flag = true;     // Freeze Player Controller
        camera_object.GetComponent<FirstPersonLook>().stop_flag = true;         // Freeze Camera Controller

        journal_ui.SetActive(true);                                             // Enable Journal UI

        open_sfx.Post(gameObject);                                              // Play SFX

        // Fill Category - Entry List
        foreach (JournalCategory j_category in player_journal)
        {
            GameObject temp_ui_category = category_prefab;

            temp_ui_category.GetComponentInChildren<Text>().text = j_category.title;

            temp_ui_category.GetComponent<JournalCategoryUI>().id = j_category.id;

            Instantiate(temp_ui_category, journal_ui_parent.transform);
        }

        Cursor.lockState = CursorLockMode.None;         // Unlock Cursor
        Cursor.visible = true;                          // Make Cursor Visible
    }

    // Close Journal
    private void closeJournal()
    {
        journal_open = false;                                                   // Set Inventory to Open

        journal_ui.SetActive(false);                                            // Enable Journal UI

        close_sfx.Post(gameObject);                                             // Play SFX

        journal_line.GetComponent<Text>().text = "";

        // Clear List
        foreach (Transform child in journal_ui_parent.transform)
        {
            Destroy(child.gameObject);
        }

        player_object.GetComponent<FirstPersonMovement>().stop_flag = false;    // Unfreeze Player Controller
        camera_object.GetComponent<FirstPersonLook>().stop_flag = false;        // Unfreeze Camera Controller

        Cursor.lockState = CursorLockMode.Locked;                               // Lock Cursor to Center
        Cursor.visible = false;                                                 // Hide Cursor
    }

    // Enable New Entry Notification
    private void showNotification()
    {
        // TODO: Add Implementation!
    }

    // Use this for initialization
    void Start()
    {
        journal_list = JsonUtility.FromJson<JournalList>(json_file.text);       // Deserialize

        player_object = GameObject.FindWithTag("Player");
        camera_object = GameObject.FindWithTag("MainCamera");

        journal_ui.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!journal_open && Input.GetKeyUp(open_key))
            openJournal();
        else if (journal_open && Input.GetKeyUp(open_key))
            closeJournal();
    }
}