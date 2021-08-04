using UnityEngine;
using System.Collections;

public class TogglePlayerGravity : MonoBehaviour
{
    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;           // Player GameObject

    // ************************************************************************************
    // Trigger Functions
    // ************************************************************************************

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player_object.GetComponent<Rigidbody>().useGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player_object.GetComponent<Rigidbody>().useGravity = true;
        }    
    }

    // Use this for initialization
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");   // Get Player GameObject
    }

    // Update is called once per frame
    void Update()
    {

    }
}