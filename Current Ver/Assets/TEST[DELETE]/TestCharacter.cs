using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCharacter : MonoBehaviour
{
    public Character Bill;

    public Vector2 moveTarget;
    public float moveSpeed;
    public bool smooth;
    // Start is called before the first frame update
    void Start()
    {
        Bill = CharacterManager.instance.GetCharacter("บิล", enabledCreatedCharacterOnStart: true);
    }

    public string[] speech;
    int i = 0;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            if(i < speech.Length)
                Bill.Say(speech[i]);
            i++;
        }
        if(Input.GetKey(KeyCode.M)){
            Bill.MoveTo(moveTarget, moveSpeed, smooth);
        }
        if(Input.GetKey(KeyCode.S)){
            Bill.StopMoving(true);
        }

        if(Input.GetKeyDown(KeyCode.Q)){
            Bill.TransitionExpression(Bill.GetSprite("sad"), 5f, false);
            //Bill.SetExpression("sad");
        }
    }
}
