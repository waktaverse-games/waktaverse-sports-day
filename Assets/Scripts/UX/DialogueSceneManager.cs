using System;
using System.Collections.Generic;
using System.Linq;
using GameHeaven.Dialogue;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueSceneManager : MonoBehaviour
{
    [SerializeField] private DialogueObject[] dialogues;
    [SerializeField] [ReadOnly] private DialogueObject currentDialogue;

    [SerializeField] private Image bgImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI sentenceText;

    [SerializeField] [ReadOnly] private int curIndex = 0;

    [SerializeField] private EventTrigger clickEvent;

    private static string nextSceneName;

    private void Awake()
    {
        currentDialogue = dialogues.First(dlg => dlg.SceneName.Equals(nextSceneName));
            
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((eventData) =>
        {
            ShowDialogue();
        });
        clickEvent.triggers.Add(entry);
    }

    private void Start()
    {
        ShowDialogue();
    }

    public static void LoadDialogue(string sceneName)
    {
        nextSceneName = sceneName;
        SceneLoader.LoadSceneAsync("DialogueScene");
    }

    public void ShowDialogue()
    {
        if (curIndex < currentDialogue.Count)
        {
            var data = currentDialogue.GetDialogue(curIndex++);

            if (data.BgSprite != null)
            {
                bgImage.sprite = data.BgSprite;
            }
            nameText.text = data.Name;
            sentenceText.text = data.Sentence;
        }
        else
        {
            SceneLoader.LoadSceneAsync(nextSceneName);
        }
    }
}