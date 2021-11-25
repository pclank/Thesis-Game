using UnityEngine;
using System.Collections;

// ************************************************************************************
// PowerBox Interaction Control
// ************************************************************************************

public class PowerBox : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    [Tooltip("Wwise Event SFX for Activation.")]
    public AK.Wwise.Event event_sfx;

    [Tooltip("Portal to Activate.")]
    public GameObject portal_object;

    [Tooltip("Turn On UI GameObject.")]
    public GameObject ui_object;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private bool ray_trig = false;                                      // Raycast Flag

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Set Raycast Flag
    public void setRaycast(bool flag)
    {
        ray_trig = flag;

        ui_object.SetActive(flag);
    }

    // Use this for initialization
    void Start()
    {
        // Check that Object is a Portal
        if (!portal_object.CompareTag("Portal"))
            Debug.LogError("Object Not a Portal!");
    }

    // Update is called once per frame
    void Update()
    {
        // Detect Interaction
        if (ray_trig && Input.GetKeyUp(KeyCode.Mouse0))
        {
            setRaycast(false);

            event_sfx.Post(gameObject);

            portal_object.GetComponent<PortalPhysics>().activatePortal();
        }
    }
}