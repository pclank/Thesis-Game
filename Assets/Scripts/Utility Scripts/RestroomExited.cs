using UnityEngine;
using System.Collections;

// ************************************************************************************
// Checks that Player has Exited the Restroom
// ************************************************************************************

public class RestroomExited : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject.FindWithTag("Player").GetComponent<JSONReader>().start_trigger = true;

            Destroy(gameObject);
        }
    }
}