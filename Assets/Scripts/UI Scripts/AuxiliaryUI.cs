using UnityEngine;
using System.Collections;

// ************************************************************************************
// Auxiliary UI Enabling Script
// ************************************************************************************

public class AuxiliaryUI : MonoBehaviour
{
    public GameObject[] ui_objects;

    public void controlUI(int index, bool flag)
    {
        ui_objects[index].SetActive(flag);
    }
}