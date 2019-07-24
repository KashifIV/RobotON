﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class ProgressionUI : MonoBehaviour
{
    GameObject character; 
    GameObject Points; 
    bool glitching; 
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Fade").GetComponent<Fade>().onFadeIn(); 
        glitching = false; 
        Points = transform.Find("TotalPoints").gameObject; 
        Points.GetComponent<Text>().text = GlobalState.Stats.Points.ToString(); 
        character = transform.Find(GlobalState.Character).gameObject; 
        StartCoroutine(FadeIn(character));     
    }
    IEnumerator FadeIn(GameObject obj){
        Image image = obj.GetComponent<Image>(); 
        while(image.color.a < 1){
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a + 0.02f); 
            yield return null; 
        }
    }
    public void AnimateButtons(List<GameObject> buttons){
        StartCoroutine(AnimButtons(buttons)); 
    }
    /// <summary>
    /// Animate buttons and their text in one at a time.
    /// </summary>
    /// <param name="buttons">Buttons which are expected to have an Animator and child Text component</param>
    /// <returns></returns>
    IEnumerator AnimButtons(List<GameObject> buttons){
        yield return new WaitForSecondsRealtime(0.5f); 
        foreach(GameObject button in buttons){
            button.GetComponent<Animator>().SetTrigger("Start"); 
            yield return new WaitForSecondsRealtime(0.5f); 
            Text text = button.transform.GetChild(0).gameObject.GetComponent<Text>(); 
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1); 
            StartCoroutine(GlitchText(button.transform.GetChild(0).gameObject)); 
        }
    }
    public void UpdateText(string value){
        glitching = false; 
        Points.GetComponent<Text>().text = value; 
        StartCoroutine(GlitchText(Points)); 
    }
    /// <summary>
    /// Glitch the text with the same techniqe as in game.
    /// Note: Canvas UI text will not bug in unity but still provides a desired effect
    /// as the text should still be readable.
    /// </summary>
    /// <param name="obj">Gameobject with a text component</param>
    /// <returns></returns>
    IEnumerator GlitchText(GameObject obj){
        glitching = true; 
        Text text = obj.GetComponent<Text>(); 
        text.font = Resources.Load<Font>("Fonts/HACKED"); 
		yield return new WaitForSeconds(0.12f); 
		text.font = Resources.Load<Font>("Fonts/CFGlitchCity-Regular"); 
		yield return new WaitForSeconds(0.1f); 
		text.font = Resources.Load<Font>("Fonts/HACKED"); 
		yield return new WaitForSeconds(0.1f); 
		text.font = Resources.Load<Font>("Fonts/Inconsolata"); 
        yield return new WaitForSecondsRealtime(10); 
        glitching = false; 
    }


}