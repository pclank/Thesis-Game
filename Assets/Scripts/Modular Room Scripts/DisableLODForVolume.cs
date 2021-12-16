using UnityEngine;
using System.Collections;

// ************************************************************************************
// Script to Disable Target GameObjects Upon Entering this Volume
// ************************************************************************************

public class DisableLODForVolume : MonoBehaviour
{
    [Tooltip("GameObject to Disable.")]
    public GameObject tgt_lod;

    [Tooltip("Delay before Disabling.")]
    public float delay = 2.0f;

    private bool started = false;

    private float timer_value = 0.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (!started && other.CompareTag("Player"))
        {
            timer_value = Time.time;

            started = true;
        }
    }

    private void disableLOD()
    {
        tgt_lod.SetActive(false);

        Destroy(this);
    }

    void Update()
    {
        if (started && Time.time - timer_value >= delay)
            disableLOD();
    }
}