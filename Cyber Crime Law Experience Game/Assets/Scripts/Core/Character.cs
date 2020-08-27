using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Character
{
    public string characterName;
    [HideInInspector] public RectTransform root;

    public bool enabled {
        get{return root.gameObject.activeInHierarchy;} 
        set{root.gameObject.SetActive(value);}
    }

    public Vector2 anchorPadding {get{return root.anchorMax - root.anchorMin;}}
    DialogueSystem dialogue;

    Coroutine transitioningBody = null;
    bool isTransitioningBody {get{return transitioningBody != null;}}

    public void TransitionBody(Texture texture, float speed, bool smooth){
        StopTransitioningBody();
        transitioningBody = CharacterManager.instance.StartCoroutine(TransitioningBody(texture, speed, smooth));
    }

    public void StopTransitioningBody(){
        if(isTransitioningBody) CharacterManager.instance.StopCoroutine(transitioningBody);
        transitioningBody = null;
    }

    public IEnumerator TransitioningBody(Texture texture, float speed, bool smooth){
        for(int i = 0; i < renderers.allRenderers.Count; i++){
            RawImage image = renderers.allRenderers[i];
            if(image.texture == texture){
                renderers.renderer = image;
                break;
            }
        }

        if(renderers.renderer.texture != texture){
            RawImage image = GameObject.Instantiate(renderers.renderer.gameObject, renderers.renderer.transform.parent).GetComponent<RawImage>();
            renderers.allRenderers.Add(image);
            renderers.renderer = image;
            image.color = GlobalFunction.SetAlpha(image.color, 0f);
            image.texture = texture;
        }

        while(GlobalFunction.TransitionImages(ref renderers.renderer, ref renderers.allRenderers, speed, smooth, true)){
            yield return new WaitForEndOfFrame();
        }

        StopTransitioningBody();
    }

    public Character(string _name, bool enableOnStart = true){
        CharacterManager cm = CharacterManager.instance;
        GameObject prefab = Resources.Load("Characters/Character["+_name+"]") as GameObject;
        GameObject ob = GameObject.Instantiate (prefab, cm.CharacterPanel);

        root = ob.GetComponent<RectTransform>();
        characterName = _name;

        renderers.renderer = ob.GetComponentInChildren<RawImage>();

        renderers.allRenderers.Add(renderers.renderer);

        dialogue = DialogueSystem.instance;

        enabled = enableOnStart;
    }

    public void Say(string speech, bool add = false){
        if(!enabled) enabled = true;
        dialogue.Say(speech, characterName, add);
    }

    Vector2 targetPosition;
    Coroutine moving;
    bool isMoving {get{return moving != null;}}
    public void MoveTo(Vector2 target, float speed, bool smooth = true){
        StopMoving();
        moving = CharacterManager.instance.StartCoroutine(Moving(target, speed, smooth));
    }

    public void SetPosition(Vector2 target){
        Vector2 padding = anchorPadding;

        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;

        Vector2 minAnchorTarget = new Vector2(maxX * target.x, maxY * target.y);
        root.anchorMin = minAnchorTarget;
        root.anchorMax = root.anchorMin + padding;
    }

    IEnumerator Moving(Vector2 target, float speed, bool smooth = true){
        targetPosition = target;

        Vector2 padding = anchorPadding;

        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;

        Vector2 minAnchorTarget = new Vector2(maxX * targetPosition.x, maxY * targetPosition.y);
        speed *= Time.deltaTime;

        while(root.anchorMin != minAnchorTarget){
            root.anchorMin = (!smooth) ? Vector2.MoveTowards(root.anchorMin, minAnchorTarget, speed) : Vector2.Lerp(root.anchorMin, minAnchorTarget, speed);
            root.anchorMax = root.anchorMin + padding;
            yield return new WaitForEndOfFrame();
        }

        StopMoving();
    }

    public void StopMoving(bool arriveAtTargetPositionImmediately = false){
        if(isMoving){
            CharacterManager.instance.StopCoroutine(moving);
            if(arriveAtTargetPositionImmediately){
                SetPosition(targetPosition);
            }
        }
        moving = null;
    }

    public void Flip(){
        root.localScale = new Vector3(root.localScale.x * -1, 1, 1);
    }

    public void FaceLeft(){
        root.localScale = Vector3.one;
    }

    public bool isFacingRight {get{return root.localScale.y == -1;}}
    public void FaceRight(){
        root.localScale = new Vector3(-1, 1, 1);
    }

    Texture lastTexture = null;
    public void FadeOut(float speed = 3f, bool smooth = false){
        Texture alphaTexture = Resources.Load<Texture>("Images/AlphaOnly");
        lastTexture = renderers.renderer.texture;

        TransitionBody(alphaTexture, speed, smooth);

    }

    public void FadeIn(float speed = 3f, bool smooth = false){
        if(lastTexture != null){
            TransitionBody(lastTexture, speed, smooth);
        }
    }

    public Texture GetTexture(int index = 0){
        Texture[] textures = Resources.LoadAll<Texture>("Images/Characters/" + characterName);
        //Debug.Log(textures.Length);
        return textures[index];
    }

    public Texture GetTexture(string textureName){
        Texture[] textures = Resources.LoadAll<Texture>("Images/Characters/" + characterName);
        foreach(Texture texture in textures){
            if(texture.name == textureName){
                return texture;
            }
        }
        return textures.Length > 0 ? textures[0] : null;
    }

    public void SetTexture(int index){
        renderers.renderer.texture = GetTexture(index);
    }

    public void SetTexture(string textureName){
        renderers.renderer.texture = GetTexture(textureName);
    }

    [System.Serializable]
    public class Renderers{
        public RawImage renderer;

        public List<RawImage> allRenderers = new List<RawImage>();
    }

    public Renderers renderers = new Renderers();

}
