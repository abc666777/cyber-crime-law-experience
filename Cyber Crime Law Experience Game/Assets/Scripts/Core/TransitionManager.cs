using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;
    public RawImage overlayImage;
    public Material transitionMaterialPrefab;
    void Awake() {
        instance = this;
        overlayImage.material = new Material(transitionMaterialPrefab);
    }
    
    static bool sceneVisible = true;
    public static void ShowScene(bool show, float speed = 1, bool smooth = false, Texture transitionEffect = null)
    {
        if(transitioningOverlay != null){
            instance.StopCoroutine(transitioningOverlay);
        }
        sceneVisible = show;

        if(transitionEffect != null){
            instance.overlayImage.material.SetTexture("_AlphaTex", transitionEffect);

        }

        transitioningOverlay = instance.StartCoroutine(TransitioningOverlay(show, speed, smooth));
    }

    static Coroutine transitioningOverlay = null;
    static IEnumerator TransitioningOverlay(bool show, float speed, bool smooth) 
    {
        float targVal = show ? 1 : 0;
        float curVal = instance.overlayImage.material.GetFloat("_Cutoff");

        while(curVal != targVal){
            curVal = smooth ? Mathf.Lerp(curVal, targVal, speed * Time.deltaTime) : Mathf.MoveTowards(curVal, targVal, speed * Time.deltaTime);
            instance.overlayImage.material.SetFloat("_Cutoff", curVal);
            yield return new WaitForEndOfFrame();
        }

        transitioningOverlay = null;
    }

    public static void transitionLayer(BackgroundManager.Layer layer, Texture targetImage, Texture transitionEffect, float speed = 1, bool smooth = false){
        if(layer.specialTransitionCoroutine != null){
            instance.StopCoroutine(layer.specialTransitionCoroutine);
        }

        if(targetImage != null){
            layer.specialTransitionCoroutine = instance.StartCoroutine(TransitioningLayer(layer, targetImage, transitionEffect, speed, smooth));
        }
        else{
            layer.specialTransitionCoroutine = instance.StartCoroutine(TransitioningLayerToNull(layer, transitionEffect, speed, smooth));
        }
    }

    static IEnumerator TransitioningLayer(BackgroundManager.Layer layer, Texture targetImage, Texture transitionEffect, float speed, bool smooth){
        GameObject ob = Instantiate(layer.newImageObjectReference, layer.newImageObjectReference.transform.parent) as GameObject;
        ob.SetActive(true);

        RawImage im = ob.GetComponent<RawImage>();
        im.texture = targetImage;

        layer.activeImage = im;
        layer.allImages.Add(im);

        im.material = new Material(instance.transitionMaterialPrefab);
        im.material.SetTexture("_AlphaTex", transitionEffect);
        im.material.SetFloat("_Cutoff", 1);
        float curVal = 1;
        
        while(curVal > 0){
            curVal = smooth ? Mathf.Lerp(curVal, 0, speed * Time.deltaTime) : Mathf.MoveTowards(curVal, 0, speed * Time.deltaTime);
            im.material.SetFloat("_Cutoff", curVal);
            yield return new WaitForEndOfFrame();
        }

        if(im != null){
            im.material = null;

            im.color = GlobalFunction.SetAlpha(im.color, 1);
        }

        for(int i = layer.allImages.Count - 1; i >= 0; i--){
            if(layer.allImages[i] == layer.activeImage && layer.activeImage != null){
                continue;
            }
            if(layer.allImages[i] != null){
                Destroy(layer.allImages[i].gameObject, 0.01f);
            }

            layer.allImages.RemoveAt(i);
        }

        layer.specialTransitionCoroutine = null;
    }
    #region TransitionToNull
    static IEnumerator TransitioningLayerToNull(BackgroundManager.Layer layer, Texture transitionEffect, float speed, bool smooth){
        List<RawImage> currentImagesOnLayer = new List<RawImage>();
        foreach(RawImage image in layer.allImages){
            image.material = new Material(instance.transitionMaterialPrefab);
            image.material.SetTexture("_AlphaTex", transitionEffect);
            image.material.SetFloat("_Cutoff", 0);
            currentImagesOnLayer.Add(image);
        }

        float curVal = 0;
        while(curVal < 1){
            curVal = smooth ? Mathf.Lerp(curVal, 1, speed * Time.deltaTime) : Mathf.MoveTowards(curVal, 1, speed * Time.deltaTime);
            foreach(RawImage image in layer.allImages){
                image.material.SetFloat("_Cutoff", curVal);
            }
            yield return new WaitForEndOfFrame();
        }

        foreach(RawImage image in currentImagesOnLayer){
            layer.allImages.Remove(image);
            if(image.material != null){
                Destroy(image.gameObject, 0.01f);
            }
        }

        layer.specialTransitionCoroutine = null;

    }

    #endregion

}
