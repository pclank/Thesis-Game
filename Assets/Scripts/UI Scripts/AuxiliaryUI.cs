using UnityEngine;
using System.Collections;

// ************************************************************************************
// Auxiliary UI Enabling Script
// ************************************************************************************

public class AuxiliaryUI : MonoBehaviour
{
    public GameObject ui_object;

    public void controlUI(bool flag)
    {
        ui_object.SetActive(flag);
    }
}