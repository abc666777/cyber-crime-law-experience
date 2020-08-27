using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//Adding and maintaining characters in the scene
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    public RectTransform CharacterPanel;
    public List<Character> characters = new List<Character>();

    public Dictionary<string, int> characterDictinary = new Dictionary<string, int>();
    void Awake() {
        instance = this;
    }

    public Character GetCharacter(string characterName, bool createCharacterIfDoesNotExist = true, bool enableCreatedCharacterOnStart = true){
        int index = -1;
        if(characterDictinary.TryGetValue(characterName, out index)){
            return characters[index];
        }
        else if(createCharacterIfDoesNotExist){
            return CreateCharacter(characterName, enableCreatedCharacterOnStart);
        }
        return null;
    }

    public Character CreateCharacter(string characterName, bool enableOnStart = true){
        Character newCharacter = new Character(characterName, enableOnStart);

        characterDictinary.Add(characterName, characters.Count);
        characters.Add(newCharacter);

        return newCharacter;
    } 

    public class CHARACTERPOSITIONS{
        public Vector2 left = new Vector2(0,0);
        public Vector2 right = new Vector2(1f,0);
        public Vector2 center = new Vector2(0.5f, 0);
    }

    public class CHARACTEREXPRESSION{
        //HAPPY SAD BLA BLA BLA
    }

    public static CHARACTERPOSITIONS characterPositions = new CHARACTERPOSITIONS();
    public static CHARACTEREXPRESSION characterExpression = new CHARACTEREXPRESSION();
}
