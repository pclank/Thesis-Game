using UnityEngine;
using System.Collections;

// ************************************************************************************
// Item Animated Overlay, by Shifting the Base Color Transparency
// ************************************************************************************

public class OverlayBreathing : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public float speed = 0.01f;

    // ************************************************************************************
    // Private Variables
    // ************************************************************************************

    private Color mat_color;

    private float mat_alpha;

    private bool forwards = false;

    // Use this for initialization
    void Start()
    {
        mat_color = gameObject.GetComponent<MeshRenderer>().material.color;

        mat_alpha = mat_color.a;
    }

    // Update is called once per frame
    void Update()
    {
        // Decreasing Alpha (More Transparent)
        if (!forwards)
        {
            mat_alpha -= (speed * Time.deltaTime);

            // Edge Case
            if (mat_alpha <= 0.0f)
            {
                mat_alpha = 0.0f;

                forwards = true;
            }
        }
        // Increasing Alpha (More Opaque)
        else
        {
            mat_alpha += (speed * Time.deltaTime);

            // Edge Case
            if (mat_alpha >= 1.0f)
            {
                mat_alpha = 1.0f;

                forwards = false;
            }
        }

        gameObject.GetComponent<MeshRenderer>().material.color = new Color(mat_color.r, mat_color.g, mat_color.b, mat_alpha);       // Update Color
    }
}