using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class Intro : MonoBehaviour {

    public Animator IntroAnimator;
    public Image UiImage;
    public Text UiText;

    public Sprite[] Sprites;
    public string[] scripts;

    private void Start()
    {
        StartCoroutine(IntroCR());
    }

    IEnumerator IntroCR()
    {
        for (int i = 0; i < Sprites.Length; i++)
        {
            Sprite sprite = Sprites[i];
            UiImage.sprite = sprite;
            string script = scripts[i];
            string scriptNl = script.Replace("\\n", "\n");
            UiText.text = "";
            yield return TypeTextCR(scriptNl);
            IntroAnimator.SetTrigger("FadeOut");
            yield return new WaitForSeconds(4f);
        }
    }

    IEnumerator TypeTextCR(string text)
    {
        for (int i = 0; i < text.Length; i++)
        {
            char nextChar = text[i];
            
            if (nextChar == '\\')
            {
                continue;
            }
            UiText.text += nextChar;

            float waitForSeconds = 0.1f;
            if (nextChar == '.')
            {
                waitForSeconds += 0.75f;
            }
            else if (nextChar == ',')
            {
                waitForSeconds += 0.5f;
            }
            yield return new WaitForSeconds(waitForSeconds);
        }
    }
}
