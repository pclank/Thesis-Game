using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

// ************************************************************************************
// Script to Detect and Process Clicks in Journal Entry UI GameObjects
// ************************************************************************************

public class JournalEntryUI : MonoBehaviour, IPointerClickHandler
{
    public int id = 0;

    // Detect Click on UI Element
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.FindWithTag("Player").GetComponent<MainJournal>().processSelectedEntry(id, this.gameObject);
    }
}