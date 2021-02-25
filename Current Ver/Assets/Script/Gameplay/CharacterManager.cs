using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    public RectTransform characterPanel;
    private List<Character> characters = new List<Character>();
    private Dictionary<string, int> characterDictionary = new Dictionary<string, int>();

    void Awake() {
        instance = this;
    }
    public Character GetCharacter(string characterName, bool createCharacterIfDoesNotExist = true, bool enabledCreatedCharacterOnStart = true){
        int index = -1;
        if(characterDictionary.TryGetValue(characterName, out index)){
            return characters[index];
        }
        else if(createCharacterIfDoesNotExist){
            return CreateCharacter(characterName, enabledCreatedCharacterOnStart);
        }
        return null;
    }
    public Character CreateCharacter(string characterName, bool enabledOnStart = true){
        Character newCharacter = new Character (characterName, enabledOnStart);

        characterDictionary.Add(characterName, characters.Count);
        characters.Add(newCharacter);

        return newCharacter;
    }
}
