using UnityEngine;
using System.Collections;

public class ExternalTutorialTrigger : MonoBehaviour
{
    public GameObject base_tutorial_object;

    public uint tut_index = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            base_tutorial_object.GetComponent<BaseTutorial>().initiateStart(tut_index);

            Destroy(gameObject);
        }
    }
}