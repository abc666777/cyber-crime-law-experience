using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BackgroundManager : MonoBehaviour
{
    public static BackgroundManager instance;
    public Layer background = new Layer();
    public Layer foreground = new Layer();
    public Layer cinematic = new Layer();
    private void Awake()
    {
        instance = this;
    }
    [System.Serializable]
    public class Layer
    {
        public GameObject root;
        public GameObject newImageObjectRefererence;
        private RawImage activeImage;
        private List<RawImage> allImages = new List<RawImage>();
        public void SetTexture(Texture texture)
        {
            /*if(activeImage != null && activeImage.texture != null){ //NOT USE
                MovieTexture mov = texture as MovieTexture; //DEPRICATED
                if(mov != null){
                    mov.Stop();
                }
            }*/
            if (texture != null)
            {
                if (activeImage == null)
                    CreateNewActiveImage();

                activeImage.texture = texture;
                activeImage.color = GlobalFunction.SetAlpha(activeImage.color, 1f);

                /*MovieTexture mov = texture as MovieTexture; //NOT USE AND DEPRICATED
                if(mov != null){
                    mov.loop = true;
                    mov.Play();
                }*/
            }
            else
            {
                if (activeImage != null)
                {
                    allImages.Remove(activeImage);
                    GameObject.DestroyImmediate(activeImage.gameObject);
                    activeImage = null;
                }
            }
        }

        public void TransitionToTexture(Texture texture, float speed = 2f, bool smooth = false)
        {
            if (activeImage != null && activeImage.texture == texture)
                return;
            StopTransition();
            transitioning = BackgroundManager.instance.StartCoroutine(Transitioning(texture, speed, smooth));
        }

        private void StopTransition()
        {
            if (isTransitioning)
                BackgroundManager.instance.StopCoroutine(transitioning);
            transitioning = null;
        }
        public bool isTransitioning { get { return transitioning != null; } }
        Coroutine transitioning = null;
        IEnumerator Transitioning(Texture texture, float speed, bool smooth = false)
        {
            if (texture != null)
            {
                foreach (RawImage image in allImages)
                {
                    if (image.texture == texture)
                    {
                        activeImage = image;
                        break;
                    }
                }

                if (activeImage == null || activeImage.texture != texture)
                {
                    CreateNewActiveImage();
                    activeImage.texture = texture;
                    activeImage.color = GlobalFunction.SetAlpha(activeImage.color, 0f);
                }
            }
            else
                activeImage = null;

            while (GlobalFunction.TransitionRawImages(ref activeImage, ref allImages, speed, smooth))
                yield return new WaitForEndOfFrame();

        }

        void CreateNewActiveImage()
        {
            GameObject ob = Instantiate(newImageObjectRefererence, root.transform) as GameObject;
            ob.SetActive(true);
            RawImage raw = ob.GetComponent<RawImage>();
            activeImage = raw;
            allImages.Add(raw);
        }
    }
}