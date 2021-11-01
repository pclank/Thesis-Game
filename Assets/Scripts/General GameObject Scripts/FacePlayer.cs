using UnityEngine;
using System.Collections;

// ************************************************************************************
// Make GameObject Rotate to Face Player on Y-Axis
// ************************************************************************************

public class FacePlayer : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Damping for Rotation.")]
    public float damping = 1.0f;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private GameObject player_object;

    // Use this for initialization
    void Start()
    {
        player_object = GameObject.FindWithTag("Player");                   // Get Player GameObject
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 look_position = player_object.transform.position - transform.position;
        look_position.y = 0;

        Quaternion rotation = Quaternion.LookRotation(look_position);
        rotation.eulerAngles = new Vector3(90.0f, rotation.eulerAngles.y, rotation.eulerAngles.z);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }
}