using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Character
{
    public string characterName;
    private DialogueSystem dialogue;
    public void Say(string speech, bool add = false)
    {
        if (speech.Length > 1)
            dialogue.Say(speech, add, characterName);
    }
    [HideInInspector] public RectTransform root;

    public bool enabled
    {
        get { return root.gameObject.activeInHierarchy; }
        set { root.gameObject.SetActive(value); visibleInScene = value; }
    }

    public Vector2 anchorPadding { get { return root.anchorMax - root.anchorMin; } }

    public Character(string _name, bool enabledOnStart = true)
    {
        if (_name != "narrator" && _name.Length > 2)
        {
            CharacterManager cm = CharacterManager.instance;
            GameObject prefab = AssetsLoader.instance.CharacterLoader($"Character[{_name}]");
            GameObject ob = GameObject.Instantiate(prefab, cm.characterPanel);

            root = ob.GetComponent<RectTransform>();
            characterName = _name;

            renderers.renderer = ob.GetComponentInChildren<Image>();
            renderers.allRenderers.Add(renderers.renderer);
            lastSprite = renderers.renderer.sprite;
            renderers.renderer.sprite = Resources.Load<Sprite>("Image/Character/AlphaOnly");

            dialogue = DialogueSystem.instance;
            enabled = false;
            if (enabledOnStart == true)
            {
                //FadeIn();
                TransitionExpression(GetSprite("laugh"), 5f, false);
            }
            enabled = enabledOnStart;
            visibleInScene = enabled;
        }
    }

    public Vector2 targetPosition;
    Coroutine moving;
    bool isMoving { get { return moving != null; } }

    public void MoveTo(Vector2 target, float speed, bool smooth = true)
    {
        StopMoving();
        moving = CharacterManager.instance.StartCoroutine(Moving(target, speed, smooth));
    }

    public void StopMoving(bool arriveAtTargetPositionImmediately = false)
    {
        if (isMoving)
        {
            CharacterManager.instance.StopCoroutine(moving);
            if (arriveAtTargetPositionImmediately)
            {
                SetPosition(targetPosition);
            }
        }
        moving = null;
    }

    public void SetPosition(Vector2 target)
    {
        targetPosition = target;
        Vector2 padding = anchorPadding;
        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;
        Vector2 minAnchorTarget = new Vector2(maxX * targetPosition.x, maxY * targetPosition.y);
        root.anchorMin = minAnchorTarget;
        root.anchorMax = root.anchorMin + padding;
    }

    IEnumerator Moving(Vector2 target, float speed, bool smooth)
    {
        targetPosition = target;
        Vector2 padding = anchorPadding;
        float maxX = 1f - padding.x;
        float maxY = 1f - padding.y;
        Vector2 minAnchorTarget = new Vector2(maxX * targetPosition.x, maxY * targetPosition.y);
        speed *= Time.deltaTime;

        while (root.anchorMin != minAnchorTarget)
        {
            root.anchorMin = (!smooth) ? Vector2.MoveTowards(root.anchorMin, minAnchorTarget, speed) : Vector2.Lerp(root.anchorMin, minAnchorTarget, speed);
            root.anchorMax = root.anchorMin + padding;
            yield return new WaitForEndOfFrame();
        }

        StopMoving();

    }

    public Sprite GetSprite(string expression)
    {
        return AssetsLoader.instance.GetCharacterSprite(characterName, expression);
    }

    public void SetExpression(string expression)
    {
        renderers.renderer.sprite = GetSprite(expression);
    }

    public void SetExpression(Sprite sprite)
    {
        renderers.renderer.sprite = sprite;
    }

    public bool isVisibleInScene { get { return visibleInScene; } }
    bool visibleInScene = true;
    Sprite lastSprite = null;
    public void FadeOut(float speed = 5f, bool smooth = false)
    {
        Sprite alphaSprite = Resources.Load<Sprite>("Image/Character/AlphaOnly");
        lastSprite = renderers.renderer.sprite;
        TransitionExpression(alphaSprite, speed, smooth);
        visibleInScene = false;
    }
    public void FadeIn(float speed = 5f, bool smooth = false)
    {

        if (lastSprite != null) TransitionExpression(lastSprite, speed, smooth);
        if (enabled) visibleInScene = true;
    }

    bool isTransitionExpression { get { return transitioningExpression != null; } }
    Coroutine transitioningExpression = null;

    public void TransitionExpression(Sprite sprite, float speed, bool smooth)
    {
        StopTransitionExpression();
        transitioningExpression = CharacterManager.instance.StartCoroutine(TransitioningExpression(sprite, speed, smooth));
    }

    void StopTransitionExpression()
    {
        if (isTransitionExpression)
            CharacterManager.instance.StopCoroutine(transitioningExpression);
        transitioningExpression = null;
    }

    public IEnumerator TransitioningExpression(Sprite sprite, float speed, bool smooth)
    {
        foreach (Image image in renderers.allRenderers)
        {
            if (image.sprite == sprite)
            {
                renderers.renderer = image;
                break;
            }
        }

        if (renderers.renderer.sprite != sprite)
        {
            Image image = GameObject.Instantiate(renderers.renderer.gameObject, renderers.renderer.transform.parent).GetComponent<Image>();
            renderers.allRenderers.Add(image);
            renderers.renderer = image;
            image.color = GlobalFunction.SetAlpha(image.color, 0f);
            image.sprite = sprite;
        }

        while (GlobalFunction.TransitionImages(ref renderers.renderer, ref renderers.allRenderers, speed, smooth, true))
            yield return new WaitForEndOfFrame();

        StopTransitionExpression();
    }

    [System.Serializable]
    public class Renderers
    {
        public Image renderer;
        public List<Image> allRenderers = new List<Image>();
    }

    public Renderers renderers = new Renderers();
}
