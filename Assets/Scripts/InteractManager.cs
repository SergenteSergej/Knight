using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    Animator anim;

    [SerializeField]
    KeyCode talkKey = KeyCode.E;

    [SerializeField]
    KeyCode nextPhraseKey = KeyCode.Return;

    [SerializeField]
    string[] phrases;

    bool talking = false;

    bool typingPhrase = false;

    int talkingIndex = 0;

    int talkParemeter = Animator.StringToHash("talk");

    [SerializeField]
    float talkingLettersSpeed = 0.1f;

    [SerializeField]
    float talkingWordSpeed = 0.5f;

    [SerializeField]
    bool typeWorlds = false;

    [SerializeField]
    GameObject phraseArea;

    [SerializeField]
    TMP_Text phraseTextArea;


    void Start()
    {
        anim = GetComponent<Animator>();    
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(talkKey) && !talking)
        {
            talking = true;

            phraseArea.SetActive(true);

            anim.SetBool(talkParemeter, talking);

            StartCoroutine(Talk(phrases[talkingIndex]));
        }
        else if (Input.GetKeyDown(nextPhraseKey) && talking && !typingPhrase)
        {
            if (talkingIndex + 1 < phrases.Length)
            {
                talkingIndex++;
                StartCoroutine(Talk(phrases[talkingIndex]));
            }
            else
            {
                phraseArea.SetActive(false);

                StopTalking();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StopTalking();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void StopTalking()
    {
        talking = false;
        talkingIndex = 0;
        typingPhrase = false;
        StopCoroutine("Talk");
        anim.SetBool(talkParemeter, talking);
        phraseArea.SetActive(false );
    }
    IEnumerator Talk(string phrase)
    {
        typingPhrase = true;

        phraseTextArea.text = "";

        if (typeWorlds)
        {
            string[] words = phrase.Split(" ");
            
            foreach (string word in words)
            {
                phraseTextArea.text += $" {word}";
                yield return new WaitForSeconds(talkingWordSpeed);
            }
        }
        else
        {
            int lenght = phrase.Length;

            for (int i = 0; i < lenght; i++)
            {
                phraseTextArea.text += phrase[i];
                yield return new WaitForSeconds(talkingLettersSpeed);
            }
        }

        if (talkingIndex < phrase.Length - 1)
        {
            phraseTextArea.text += "...";
        }

        typingPhrase = false;
    }
}
