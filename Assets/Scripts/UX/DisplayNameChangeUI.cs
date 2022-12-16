using System;
using TMPro;
using UnityEngine;

namespace GameHeaven.UIUX
{
    public class DisplayNameChangeUI : MonoBehaviour
    {
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private TextMeshProUGUI placeHolderText;
        
        private Color normalPlaceholderColor = new Color(0.35f, 0.35f, 0.35f, 1.0f);
        private Color errorPlaceholderColor = new Color(0.8f, 0.0f, 0.0f, 1.0f);

        private void OnEnable()
        {
            nameInputField.placeholder.color = new Color(0.35f, 0.35f, 0.35f, 1.0f);
            nameInputField.text = GameDatabase.NickName;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameDatabase.Instance.IsFirstTime) return;
                gameObject.SetActive(false);
            }
        }

        public void SetDisplayName()
        {
            var displayName = nameInputField.text.Trim();
            
            if (string.IsNullOrWhiteSpace(displayName))
            {
                SetPlaceholder("이름을 입력해주세요 (16자 이하)", "error");
            }
            else
            {
                PlayFabManager.Instance.PostDisplayName(displayName, () =>
                {
                    if (GameDatabase.Instance.IsFirstTime) GameDatabase.Instance.IsFirstTime = false;
                    gameObject.SetActive(false);
                }, () =>
                {
                    nameInputField.text = string.Empty;
                    SetPlaceholder("이미 존재하는 이름입니다", "error");
                });
            }
        }

        private void SetPlaceholder(string message, string state)
        {
            nameInputField.placeholder.color = state switch
            {
                "normal" => normalPlaceholderColor,
                "error" => errorPlaceholderColor,
                _ => nameInputField.placeholder.color
            };
            placeHolderText.text = message;
        }
    }
}