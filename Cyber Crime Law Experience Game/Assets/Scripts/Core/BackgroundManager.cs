using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance;

    public Layer background = new Layer();
    public Layer cinematic = new Layer();
    public Layer foreground = new Layer();

    public void Awake(){
        instance = this;
    }

    [System.Serializable]
    public class Layer{
        public GameObject root;
        public GameObject newImageObjectReference;
        public RawImage activeImage;
        public List<RawImage> allImages = new List<RawImage>();

        public Coroutine specialTransitionCoroutine = null;

        public void SetTexture(Texture texture, bool ifMovieThenLoop = true)
        {
            if(activeImage != null && activeImage.texture != null)
            {
                MovieTexture mov = texture as MovieTexture;
                if(mov != null){
                    mov.Stop();
                }
            }

            if(texture != null)
            {
                if(activeImage == null)
                    CreateNewActiveImage();

                activeImage.texture = texture;
                activeImage.color = GlobalFunction.SetAlpha(activeImage.color, 1f);
                
                MovieTexture mov = texture as MovieTexture;
                if(mov != null){
                    mov.loop = ifMovieThenLoop;
                    mov.Play();
                }
            }
            else
            {
                if(activeImage != null)
                {
                    allImages.Remove(activeImage);
                    GameObject.Destroy(activeImage.gameObject);
                    activeImage = null;
                }
            }
        }
    Coroutine transitioning = null;
    public bool isTransitioning {get{return transitioning != null;}}

    public void TransitionToTexture(Texture texture, float speed = 2, bool smooth = false, bool ifMovieThenLoop = true){
    
        if(activeImage != null && activeImage.texture == texture){ 
            return;
        }
        StopTransitioning();
        transitioning = BackgroundManager.instance.StartCoroutine(Transitioning(texture, speed, smooth, ifMovieThenLoop));
    }

    public void StopTransitioning(){
        if(isTransitioning) BackgroundManager.instance.StopCoroutine(transitioning);
        transitioning = null;
    }

    public IEnumerator Transitioning(Texture texture, float speed, bool smooth, bool ifMovieThenLoop){
        if(texture != null)
        {
            for(int i = 0; i < allImages.Count; i++)
            {
            RawImage image = allImages[i];
            if(image.texture == texture)
            {
                activeImage = image;
                break;
            }
        }

            if(activeImage == null || activeImage.texture != texture)
            {
                CreateNewActiveImage();
                activeImage.texture = texture;
                activeImage.color = GlobalFunction.SetAlpha(activeImage.color, 0f);
                MovieTexture mov = texture as MovieTexture;
                if(mov != null)
                {
                    mov.loop = ifMovieThenLoop;
                    mov.Play();
                }
            }
        }
        else{
            activeImage = null;
        }
        while(GlobalFunction.TransitionImages(ref activeImage, ref allImages, speed, smooth)){
            yield return new WaitForEndOfFrame();
        }

        StopTransitioning();
        
    }

        void CreateNewActiveImage(){
            GameObject ob = Instantiate(newImageObjectReference, root.transform) as GameObject;
            ob.SetActive(true);
            RawImage raw = ob.GetComponent<RawImage>();
            activeImage = raw;
            allImages.Add(raw);
        }
    }
}
