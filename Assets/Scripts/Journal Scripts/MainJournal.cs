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
    public JournalCategory[] journal_list;
}

// ************************************************************************************
// Journal Category Class
// ************************************************************************************

[System.Serializable]
public class JournalCategory
{
    public int id;

    public string title;

    public JournalEntry[] entries;

    // Constructor
    public JournalCategory(int id, string title)
    {
        this.id = id;
        this.title = title;
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
    public Text json_file;

    [Tooltip("Journal UI GameObject Parent.")]
    public GameObject journal_object;

    [Tooltip("UI GameObject for New Entry Notification.")]
    public GameObject new_entry_notification_ui;

    [Tooltip("UI Prefab for Category.")]
    public GameObject category_prefab;

    [Tooltip("UI Prefab for Entry.")]
    public GameObject entry_prefab;

    [Tooltip("Key to Open Journal.")]
    public KeyCode open_key = KeyCode.J;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private JournalList journal_list;                                           // Journal Entries List

    private List<JournalCategory> player_journal = new List<JournalCategory>(); // Player Journal List

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Add Journal Category
    public void addCategory(int category_id)
    {
        JournalCategory new_category = new JournalCategory(category_id, "PLACEHOLDER");

        foreach (JournalCategory j_category in journal_list.journal_list)
        {
            if (j_category.id == category_id)
            {
                new_category.title = j_category.title;              // Assign Title
                new_category.entries[0] = j_category.entries[0];    // Add First Entry
            }
        }

        player_journal.Add(new_category);
    }

    // Add Journal Entry
    public void addEntry(int category_id, int entry_id)
    {
        // TODO: Add Functionality!
    }

    // Use this for initialization
    void Start()
    {
        journal_list = JsonUtility.FromJson<JournalList>(json_file.text);       // Deserialize
    }

    // Update is called once per frame
    void Update()
    {

    }
}