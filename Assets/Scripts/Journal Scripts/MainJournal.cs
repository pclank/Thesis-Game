using UnityEngine;
using UnityEngine.UI;
using System;
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
    public List<JournalLine> line;

    // Constructor
    public JournalEntry(int id, string title, List<JournalLine> line)
    {
        this.id = id;
        this.title = title;
        this.line = line;
    }
}

// ************************************************************************************
// Journal Entry Line Class
// ************************************************************************************

[System.Serializable]
public class JournalLine
{
    public string line;
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

    [Tooltip("Duration of New Entry Notification.")]
    public float timer_duration = 2.0f;

    [Tooltip("UI Prefab for Category.")]
    public GameObject category_prefab;

    [Tooltip("UI Prefab for Entry.")]
    public GameObject entry_prefab;

    [Tooltip("Open Journal SFX Event.")]
    public AK.Wwise.Event open_sfx;

    [Tooltip("Close Journal SFX Event.")]
    public AK.Wwise.Event close_sfx;

    [Tooltip("New Entry SFX Event")]
    public AK.Wwise.Event new_entry_sfx;

    [Tooltip("Key to Open Journal.")]
    public KeyCode open_key = KeyCode.J;

    [Header("Development Mode Options")]
    [Tooltip("Starts Development Mode.")]
    public bool development_mode = false;

    [Tooltip("Static Emotion Index for Development Mode.")]
    public int development_mode_index = 0;

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

    private float timer_value = 0.0f;                                           // Timer Current Value

    private int emotion_index = 0;                                              // Current Emotion Index

    private bool journal_open = false;                                          // Whether Journal is Open
    private bool notification_active = false;                                   // Whether Notification UI is Enabled

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Check Whether the Journal is Open from External Script
    public bool isJournalOpen()
    {
        return journal_open;
    }

    // Add Journal Category
    public void addCategory(int category_id)
    {
        bool duplicate_found = false;

        // Check for Duplicates
        foreach (JournalCategory j_category in player_journal)
        {
            if (j_category.id == category_id)
                duplicate_found = true;
        }

        // No Duplicates Found, then Add Category
        if (!duplicate_found)
        {
            JournalCategory new_category = new JournalCategory(category_id, "PLACEHOLDER");

            JournalEntry new_entry = new JournalEntry(0, "PLACEHOLDER", new List<JournalLine>());

            new_entry.title = journal_list.journal_list[category_id].entries[0].title;

            Tuple<int, float> prediction = GameObject.FindWithTag("Player").GetComponent<JSONReader>().readEmotionIndex();  // Get Prediction

            if (!development_mode && prediction.Item2 >= 40.0f)
                emotion_index = prediction.Item1;
            else if (development_mode)
                emotion_index = development_mode_index;

            new_category.title = journal_list.journal_list[category_id].title;              // Assign Title

            if (emotion_index == 0 || journal_list.journal_list[category_id].entries[0].line.Count == 1)
                new_entry.line.Add(journal_list.journal_list[category_id].entries[0].line[0]);
            else if (emotion_index != 0 && journal_list.journal_list[category_id].entries[0].line.Count == 2)    // TODO: Consider Change to Simple else!
                new_entry.line.Add(journal_list.journal_list[category_id].entries[0].line[1]);

            new_category.entries.Add(new_entry);                                            // Add First Entry

            player_journal.Add(new_category);                                               // Add Category Object to Journal

            showNotification(new_category.title);                                           // Show New Entry Notification
        }
    }

    // Add Journal Entry
    public void addEntry(int category_id, int entry_id)
    {
        bool found = false;

        JournalEntry new_entry = new JournalEntry(entry_id, "PLACEHOLDER", new List<JournalLine>());

        Tuple<int, float> prediction = GameObject.FindWithTag("Player").GetComponent<JSONReader>().readEmotionIndex();  // Get Prediction

        if (!development_mode && prediction.Item2 >= 40.0f)
            emotion_index = prediction.Item1;
        else if (development_mode)
            emotion_index = development_mode_index;

        foreach (JournalCategory j_category in player_journal)
        {
            // Find Category with Requested ID
            if (j_category.id == category_id && !isEntryInJournal(j_category, entry_id))
            {
                new_entry.title = journal_list.journal_list[category_id].entries[entry_id].title;
                
                if (emotion_index == 0 || journal_list.journal_list[category_id].entries[entry_id].line.Count == 1)
                    new_entry.line.Add(journal_list.journal_list[category_id].entries[entry_id].line[0]);
                else if (emotion_index != 0 && journal_list.journal_list[category_id].entries[entry_id].line.Count == 2)    // TODO: Consider Change to Simple else!
                    new_entry.line.Add(journal_list.journal_list[category_id].entries[entry_id].line[1]);

                j_category.entries.Add(new_entry);                                  // Add New Entry to Journal

                showNotification(journal_list.journal_list[category_id].title);     // Show New Entry Notification

                found = true;

                break;
            }
            else if (j_category.id == category_id && isEntryInJournal(j_category, entry_id))
            {
                found = true;                                                       // To Circumvent Last If Statement

                break;
            }
        }

        // If Category Hasn't being Added
        if (!found)
        {
            addCategory(category_id);                                           // Add Category if Missing

            new_entry.title = journal_list.journal_list[category_id].entries[entry_id].title;

            if (emotion_index == 0 || journal_list.journal_list[category_id].entries[entry_id].line.Count == 1)
                new_entry.line.Add(journal_list.journal_list[category_id].entries[entry_id].line[0]);
            else if (emotion_index != 0 && journal_list.journal_list[category_id].entries[entry_id].line.Count == 2)    // TODO: Consider Change to Simple else!
                new_entry.line.Add(journal_list.journal_list[category_id].entries[entry_id].line[1]);

            player_journal[player_journal.Count - 1].entries.Add(new_entry);    // Add New Entry to Journal
        }
    }

    // Expand Entries of Selected Category
    public void expandCategory(int id, GameObject category_object)
    {
        category_object.GetComponent<Image>().color = category_expanded_color;

        category_object.GetComponentInParent<VerticalLayoutGroup>().spacing = 500.0f;

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

        category_object.GetComponentInParent<VerticalLayoutGroup>().spacing = 40.0f;

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

        foreach (JournalEntry j_entry in player_journal[entry_object.GetComponentInParent<JournalCategoryUI>().id].entries)
        {
            if (j_entry.id == id)
            {
                journal_line.GetComponent<Text>().text = j_entry.line[0].line;

                break;
            }
        }

        //journal_line.GetComponent<Text>().text = player_journal[entry_object.GetComponentInParent<JournalCategoryUI>().id].entries[id].line;

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

        journal_ui_parent.GetComponent<VerticalLayoutGroup>().spacing = 40.0f;

        journal_ui.SetActive(true);                                             // Enable Journal UI

        open_sfx.Post(gameObject);                                              // Play SFX

        int counter = 0;

        // Fill Category - Entry List
        foreach (JournalCategory j_category in player_journal)
        {
            GameObject temp_ui_category = category_prefab;

            temp_ui_category.GetComponentInChildren<Text>().text = j_category.title;

            temp_ui_category.GetComponent<JournalCategoryUI>().id = counter;

            Instantiate(temp_ui_category, journal_ui_parent.transform);

            counter++;
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

    // Check if Entry has Already Being Added
    private bool isEntryInJournal(JournalCategory j_category, int entry_id)
    {
        foreach (JournalEntry j_entry in j_category.entries)
        {
            if (j_entry.id == entry_id)
                return true;
        }

        return false;
    }

    // Enable New Entry Notification
    private void showNotification(string title)
    {
        if (!notification_active)
        {
            new_entry_sfx.Post(gameObject);                                     // Play New Entry SFX

            new_entry_notification_ui.GetComponentInChildren<Text>().text = "Journal Entry for \""+ title + "\" has been Updated";
            new_entry_notification_ui.SetActive(true);

            timer_value = Time.time;

            notification_active = true;
        }
    }

    // Disable New Entry Notification
    private void hideNotification()
    {
        new_entry_notification_ui.SetActive(false);

        notification_active = false;
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
        if (!journal_open && Input.GetKeyUp(open_key) && !GetComponent<main_inventory>().isInventoryOpen())
            openJournal();
        else if (journal_open && Input.GetKeyUp(open_key))
            closeJournal();

        // Fix Player Unfreezing Issue
        if (journal_open)
        {
            player_object.GetComponent<FirstPersonMovement>().stop_flag = true;     // Freeze Player Controller
            camera_object.GetComponent<FirstPersonLook>().stop_flag = true;         // Freeze Camera Controller
        }

        // Notification Timer Section
        if (notification_active && Time.time - timer_value >= timer_duration)
            hideNotification();
    }
}