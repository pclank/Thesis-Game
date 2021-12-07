using UnityEngine;
using System.Collections;

// ************************************************************************************
// Add Entry to Journal on Interaction
// ************************************************************************************

public class AddToJournalOnInteraction : MonoBehaviour
{

    public int category_id = 0;
    public int entry_id = 0;

    [Tooltip("Whether a New Category will Be Added.")]
    public bool add_category = false;

    // Add to Journal from External Script
    public void addToJournal()
    {
        if (add_category)
            addCategoryToJournal();
        else
            addEntryToJournal();
    }

    // Add Category To Journal On Pickup
    private void addCategoryToJournal()
    {
        GameObject.FindWithTag("Player").GetComponent<MainJournal>().addCategory(category_id);
    }

    // Add Entry to Journal On Pickup
    private void addEntryToJournal()
    {
        GameObject.FindWithTag("Player").GetComponent<MainJournal>().addEntry(category_id, entry_id);
    }
}