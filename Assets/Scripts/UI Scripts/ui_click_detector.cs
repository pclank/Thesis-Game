using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ui_click_detector : MonoBehaviour, IPointerClickHandler
{
    public int item_id;                     // ID of Item Associated with Object

    private GameObject player_object;       // Player GameObject

    // Detect Click on UI Element

    public void OnPointerClick(PointerEventData eventData)
    {
        player_object.GetComponent<main_inventory>().setDescription(item_id);
    }

    // On Start Frame

    void Start()
    {
        player_object = GameObject.FindWithTag("Player");       // Get Player GameObject
    }
}
