using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    [System.Serializable]
    private class DoorRotation
    {
        public Transform doorTransform;
        public float rotationY = 85f;
    }

    Animator anim;

    [SerializeField] KeyCode talkKey = KeyCode.E;
    [SerializeField] KeyCode nextPhraseKey = KeyCode.Return;

    [SerializeField] string[] phrases;

    [SerializeField] float talkingLettersSpeed = 0.1f;
    [SerializeField] float talkingWordSpeed = 0.5f;
    [SerializeField] bool typeWords = false;

    [SerializeField] GameObject phraseArea;
    [SerializeField] TMP_Text phraseTextArea;

    [Header("Door Settings")]
    [SerializeField] bool canOpenDoor = false;
    [SerializeField] float doorOpenDuration = 1f;
    [SerializeField] List<DoorRotation> doorsToOpen = new List<DoorRotation>();

    bool talking = false;
    bool typingPhrase = false;
    int talkingIndex = 0;
    int talkParameter = Animator.StringToHash("talk");
    bool doorsOpened = false;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(talkKey) && !talking)
        {
            talking = true;
            phraseArea.SetActive(true);
            anim.SetBool(talkParameter, talking);
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

                if (canOpenDoor && !doorsOpened && doorsToOpen.Count > 0)
                {
                    StartCoroutine(OpenDoors());
                    doorsOpened = true;
                }

                StopTalking();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        StopTalking();
    }

    void StopTalking()
    {
        talking = false;
        talkingIndex = 0;
        typingPhrase = false;
        StopCoroutine("Talk");
        anim.SetBool(talkParameter, talking);
        phraseArea.SetActive(false);
    }

    IEnumerator Talk(string phrase)
    {
        typingPhrase = true;
        phraseTextArea.text = "";

        if (typeWords)
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
            for (int i = 0; i < phrase.Length; i++)
            {
                phraseTextArea.text += phrase[i];
                yield return new WaitForSeconds(talkingLettersSpeed);
            }
        }

        if (talkingIndex < phrases.Length - 1)
        {
            phraseTextArea.text += "...";
        }

        typingPhrase = false;
    }

    IEnumerator OpenDoors()
    {
        float elapsed = 0f;

        List<Quaternion> startRotations = new List<Quaternion>();
        List<Quaternion> endRotations = new List<Quaternion>();

        foreach (var door in doorsToOpen)
        {
            if (door.doorTransform == null) continue;

            Quaternion start = door.doorTransform.rotation;
            Quaternion end = start * Quaternion.Euler(0f, 0f, door.rotationY);
            startRotations.Add(start);
            endRotations.Add(end);
        }

        while (elapsed < doorOpenDuration)
        {
            for (int i = 0; i < doorsToOpen.Count; i++)
            {
                var door = doorsToOpen[i];
                if (door.doorTransform == null) continue;

                door.doorTransform.rotation = Quaternion.Slerp(startRotations[i], endRotations[i], elapsed / doorOpenDuration);
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < doorsToOpen.Count; i++)
        {
            var door = doorsToOpen[i];
            if (door.doorTransform == null) continue;

            door.doorTransform.rotation = endRotations[i];
        }
    }
}
