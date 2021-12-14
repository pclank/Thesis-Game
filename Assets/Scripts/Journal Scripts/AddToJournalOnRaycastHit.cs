using UnityEngine;
using System.Collections;

// ************************************************************************************
// Add Entry to Journal on Raycast Hit
// ************************************************************************************

public class AddToJournalOnRaycastHit : MonoBehaviour
{
    public int category_id = 0;
    public int entry_id = 0;

    [Tooltip("Whether a New Category will Be Added.")]
    public bool add_category = false;

    private bool added = false;                             // Whether it has Already being Added in this Instance

    // Add to Journal from On Raycast Hit
    public void addToJournal()
    {
        if (!added && add_category)
            addCategoryToJournal();
        else if (!added && !add_category)
            addEntryToJournal();

        added = true;
    }

    // Add Category To Journal On Pickup
    private void addCategoryToJournal()
    {
        GameObject.FindWithTag("Player").GetComponent<MainJournal>().addCategory(category_id);

        //Destroy(this);
    }

    // Add Entry to Journal On Pickup
    private void addEntryToJournal()
    {
        GameObject.FindWithTag("Player").GetComponent<MainJournal>().addEntry(category_id, entry_id);

        //Destroy(this);
    }
}