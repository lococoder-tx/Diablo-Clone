
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private CanvasGroup fade;

        //explain: assuming 60 frames in one second, this fucntion is called 60 * num of seconds time
        public IEnumerator FadeOut(float time = 2f)
        {
            
            
            while (fade.alpha < 1)
            {
                fade.alpha += Time.deltaTime / time;
                yield return null;
            }
        }


        public IEnumerator FadeIn(float time = 2f)
        {
            
            while (fade.alpha > 0)
            {
                fade.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }

        public IEnumerator FadeOutImmediately()
        {
            fade.alpha = 1;
            yield return null;
        }
       
        //test method for nested coroutines
        public IEnumerator FadeOutIn(float timeOut = 2f, float timeIn = 2f)
        {
            yield return FadeOut(timeOut);
            print("1");
            yield return FadeIn(timeIn);
            print("finished");
        }
        
    }
}
