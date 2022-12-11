using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Febucci.UI;
using GameHeaven.Dialogue;
using SharedLibs;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueNotFoundException : Exception
{
    public DialogueNotFoundException() : base("Dialogue not found")
    {
    }
}

public class DialogueSceneManager : MonoBehaviour
{
    [SerializeField] private DialogueObject[] dialogues;
    [SerializeField] [ReadOnly] private DialogueObject currentDialogue;

    [SerializeField] private Image bgImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private AudioSource bgmAudio;
    [SerializeField] private AudioSource sfxAudio;

    [SerializeField] private TextAnimatorPlayer textAnimator;
    [SerializeField] [ReadOnly] private bool isTyping;

    [SerializeField] [ReadOnly] private int curIndex = 0;

    [SerializeField] private EventTrigger clickEvent;
    [SerializeField] private KeyCode[] nextKey = new[] { KeyCode.Space, KeyCode.Return };

    private static string nextSceneName = "ModeSelectScene";

    public static void LoadDialogue(string sceneName)
    {
        nextSceneName = sceneName;
        SceneLoader.LoadSceneAsync("DialogueScene");
    }

    private void Awake()
    {
        bgmAudio.volume = SoundManager.Instance.BGMVolume;
        sfxAudio.volume = SoundManager.Instance.SFXVolume;
        
        textAnimator.useTypeWriter = true;
        
        var entry = CreateDialogueEventEntry((evData) =>
        {
            ToNext();
        });
        clickEvent.triggers.Add(entry);
        
        try
        {
            currentDialogue = dialogues.First(obj => obj.SceneName.Equals(nextSceneName));
        }
        catch (Exception e)
        {
            currentDialogue = dialogues[0];
            Debug.LogError(e);
        }
    }

    private void Start() 
    {
        ToNext();
    }

    private void OnEnable()
    {
        textAnimator.onTypewriterStart.AddListener(() => isTyping = true);
        textAnimator.onTextShowed.AddListener(() => isTyping = false);
    }

    private void OnDisable()
    {
        textAnimator.onTypewriterStart.RemoveAllListeners();
        textAnimator.onTextShowed.RemoveAllListeners();
    }

    private void Update()
    {
        for (var i = 0; i < nextKey.Length; i++)
        {
            if (Input.GetKeyDown(nextKey[i]))
            {
                ToNext();
            }
        }
    }

    private void ToNext()
    {
        if (isTyping)
        {
            textAnimator.SkipTypewriter();
            return;
        }
        
        if (curIndex < currentDialogue.Count)
        {
            var data = currentDialogue.GetDialogue(curIndex++);
            ShowDialogue(data);
        }
        else
        {
            SceneLoader.LoadSceneAsync(nextSceneName);
        }
    }

    private void ShowDialogue(DialogueData data)
    {
        PlayBGM(data.BGM);
        if (data.Sfx) AudioSource.PlayClipAtPoint(data.Sfx, Vector3.zero);
        
        if (data.BgSprite) bgImage.sprite = data.BgSprite;
        
        if (!string.IsNullOrWhiteSpace(data.Name)) nameText.text = data.Name;
        if (!string.IsNullOrWhiteSpace(data.Sentence))
        {
            textAnimator.ShowText(data.Sentence);
        }
    }
    
    private void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        if (bgmAudio.isPlaying)
        {
            bgmAudio.CrossFadePlay(clip, 2.0f);
        }
        else
        {
            bgmAudio.clip = clip;
            bgmAudio.Play();
        }
    }

    private static EventTrigger.Entry CreateDialogueEventEntry(UnityAction<BaseEventData> action)
    {
        var entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener(action);
        return entry;
    }
}