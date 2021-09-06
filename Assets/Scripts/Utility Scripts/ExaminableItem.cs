using UnityEngine;
using System.Collections;

public class ExaminableItem : MonoBehaviour
{
    // ************************************************************************************
    // Public Variables
    // ************************************************************************************

    public Vector3 adjustment_vector = new Vector3(0, 0, 0);

    public float scale_factor = 1.0f;

    public GameObject gb_prefab;

    // ************************************************************************************
    // Member Functions
    // ************************************************************************************

    // Get GameObject Prefab
    public GameObject getPrefab()
    {
        return gb_prefab;
    }
}