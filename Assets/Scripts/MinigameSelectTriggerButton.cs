using SharedLibs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameHeaven.Temp
{
    [RequireComponent(typeof(EventTrigger))]
    public class MinigameSelectTriggerButton : MonoBehaviour
    {
        [SerializeField] private MinigameType type;

        [SerializeField] private Image iconImage;
        [SerializeField] private TextMeshProUGUI iconLabel;

        [SerializeField] private EventTrigger eventTrigger;

        private void Awake()
        {
            eventTrigger = GetComponent<EventTrigger>();

            var clickEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerClick };
            clickEntry.callback.AddListener(data => { MinigameSelectUI.SelectGame(type); });
            eventTrigger.triggers.Add(clickEntry);
        }

        public void SetButton(MinigameType type, Sprite icon, string label)
        {
            this.type = type;
            iconImage.sprite = icon;
            iconLabel.text = label;
        }
    }
}