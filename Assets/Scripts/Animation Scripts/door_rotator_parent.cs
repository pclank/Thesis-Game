using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door_rotator_parent : MonoBehaviour
{
    // Move Door Icon GameObject Variable
    public GameObject icon_object;
    private void OnTriggerEnter(Collider other)
    {
        // If the Trigger is the Player
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponentInChildren<door_rotator>().OnPlEnter(other);

            icon_object.SetActive(true);
        }
    }

    // Player Exits Interaction Position
    private void OnTriggerExit(Collider other)
    {
        // If the Trigger is the Player
        if (other.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponentInChildren<door_rotator>().OnPlExit(other);

            icon_object.SetActive(false);
        }
    }

    void Start()
    {
        // Get Icon Object
        if (icon_object == null)
        {
            icon_object = GameObject.Find("Move Door - Text");
        }

        icon_object.SetActive(false);

        if (icon_object == null)
        {
            Debug.Log("Move Icon Not Found!");
        }
    }
}
