using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TestTextArchitect : MonoBehaviour
{
    public TextMeshProUGUI text;
    TextArchitect architect;
    [TextArea(5, 10)]
    public string say;
    // Start is called before the first frame update
    void Start()
    {
        architect = new TextArchitect(say);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(text.text != say);
        if (Input.GetKey(KeyCode.RightArrow))
        {
            text.text = architect.currentText;
            if (Input.GetKeyDown(KeyCode.Space) && text.text != say)
            {
                text.text = say;
            }
        }
    }
}
