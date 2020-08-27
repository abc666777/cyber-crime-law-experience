using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalFunction : MonoBehaviour
{
    public static bool TransitionImages(ref RawImage activeImage, ref List<RawImage> allImages, float speed, bool smooth, bool isCharacter = false){
        bool anyValueChanged = false;

        speed *= Time.deltaTime;
        for(int i = allImages.Count - 1; i >= 0; i--){
            RawImage image = allImages[i];

            if(image == activeImage){
                if(image.color.a < 1f){
                    float _speed = isCharacter ? speed * 2 : speed;
                    image.color = SetAlpha(image.color, smooth ? Mathf.Lerp(image.color.a, 1f, _speed) : Mathf.MoveTowards(image.color.a, 1f, _speed));
                    anyValueChanged = true;
                }
                
            }
            else{
                if(image.color.a > 0){
                    image.color = SetAlpha(image.color, smooth ? Mathf.Lerp(image.color.a, 0, speed) : Mathf.MoveTowards(image.color.a, 0, speed));
                    anyValueChanged = true;
                }
                
               else{
                    MovieTexture mov = image.texture as MovieTexture;
                    if(mov != null){
                        mov.Stop();
                    }
                    allImages.RemoveAt(i) ;
                    Destroy(image.gameObject);
                    continue;
                }
            }

        }

        return anyValueChanged;
    }

    public static Color SetAlpha(Color color, float alpha){
        return new Color(color.r, color.g, color.b, alpha);
    }
}
