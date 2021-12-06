using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

// ************************************************************************************
// Script to Detect and Process Clicks in Journal Category UI GameObjects
// ************************************************************************************

public class JournalCategoryUI : MonoBehaviour, IPointerClickHandler
{
    public int id = 0;

    private bool expanded = false;

    // Detect Click on UI Element
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!expanded)
        {
            GameObject.FindWithTag("Player").GetComponent<MainJournal>().expandCategory(id, this.gameObject);

            expanded = true;
        }
        else
        {
            GameObject.FindWithTag("Player").GetComponent<MainJournal>().collapseCategory(id, this.gameObject);

            expanded = false;
        }
    }
}