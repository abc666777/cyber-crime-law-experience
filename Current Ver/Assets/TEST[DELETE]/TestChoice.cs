using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestChoice : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ChoiceScreen.Show("Bitch", "Pussy");
        }
    }
}
