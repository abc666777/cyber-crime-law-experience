using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test2 : MonoBehaviour
{
    public Character placeholder;

    public Vector2 moveTarget;
    public float moveSpeed;
    public bool smooth;
    public int index;
    public string Speaker;
    // Start is called before the first frame update
    void Start()
    {
        placeholder = CharacterManager.instance.GetCharacter("เอก", enableCreatedCharacterOnStart: false);
        placeholder.GetTexture(1);
    }

    public string[] speech;
    int i = 0;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            placeholder.MoveTo(moveTarget, moveSpeed, smooth);
            if(i < speech.Length) placeholder.Say(speech[i]);
            else DialogueSystem.instance.Close();
            i++;
        }

        if(Input.GetKeyDown(KeyCode.S)){
            placeholder.StopMoving(true);
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow)){
            placeholder.SetPosition(moveTarget);
        }

        if(Input.GetKeyDown(KeyCode.T)){
            placeholder.SetTexture(index);
        }

         if(Input.GetKeyDown(KeyCode.B)){
                placeholder.TransitionBody(placeholder.GetTexture(index), 5f, smooth);
            }
    }
}
