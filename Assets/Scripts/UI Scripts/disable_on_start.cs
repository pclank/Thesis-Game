using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disable_on_start : MonoBehaviour
{
    private int cnt = 1;

    // Update is called once per frame
    void Update()
    {
        if (cnt == 2)
        {
            gameObject.SetActive(false);
        }
        else if (cnt < 2)
        {
            cnt++;
        }
    }
}
