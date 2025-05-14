using TMPro;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    Animator anim;

    [SerializeField]
    KeyCode talkKey = KeyCode.E;

    [SerializeField]
    KeyCode nextPhrase = KeyCode.Return;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
