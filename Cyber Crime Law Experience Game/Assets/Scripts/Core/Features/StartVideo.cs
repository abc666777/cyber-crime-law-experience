using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartVideo : MonoBehaviour
{  
    [SerializeField] private MovieTexture mov;
    // Start is called before the first frame update
    void Start()
    {
        mov.loop = true;
        mov.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
