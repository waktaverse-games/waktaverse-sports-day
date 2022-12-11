using System;
using TMPro;
using UnityEngine;

namespace GameHeaven.UIUX
{
    public class DisplayNameChangeUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private TextMeshProUGUI placeHolderText;

        public void SetDisplayName()
        {
            gameObject.SetActive(true);
            string displayName = nameInputField.text;
            if (string.IsNullOrWhiteSpace(nameInputField.text))
            {
                nameInputField.placeholder.color = new Color(1.0f, 0.0f, 0.0f, 0.6f);
                placeHolderText.text = "이름을 입력해주세요 (16자 이하)";
            }
            else
            {
                PlayFabManager.Instance.UpdateDisplayName(displayName);
                if (GameDatabase.Instance.IsFirstTime) GameDatabase.Instance.IsFirstTime = false;
            }
        }

        private void OnEnable()
        {
            nameInputField.placeholder.color = new Color(0.35f, 0.35f, 0.35f, 1.0f);
        }
    }
}