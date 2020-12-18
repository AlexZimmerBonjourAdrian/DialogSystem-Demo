using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CDialog : MonoBehaviour
{
    public TextMesh textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;


    public GameObject continueButton;
    void Start()
    {
        StartCoroutine(Type())
    }

    // Start is called before the first frame update
    void Update()
    {
        if(textDisplay.text == sentences[index])
        {
            continueButton.SetActive(true);
        }

        
    }

    IEnumerator Type()
    {
        foreach (char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
    public void NextSentence()
    {
        continueButton.SetActive(false);

        if(index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        }else
        {
            textDisplay.text = "";
            continueButton.SetActive(false);
        }
    }


    
}
