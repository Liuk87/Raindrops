using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Security.Cryptography.X509Certificates;

public class NickNameHandler : MonoSingleton<NickNameHandler>
{
    //public int NickID;
    public string NickName;
    private bool isNameSet = false;
    public bool IsNameSet { get{return isNameSet; }}

    private TMP_InputField NickInput;
    private TMP_Text NickInputText;
    private TMP_Text NicknameText;
    private Button EditButton;
    public TMP_Text MainPanelNickName;
    private Button NewGameButton;

    void Start()
    {
        NewGameButton = GameObject.Find("NewGameButton").GetComponent<Button>();
        if (NewGameButton == null)
        {
            Debug.LogError("GameObject 'NewGameButton' non trovato.");
            return;
        }

        NickInput = GetComponentInChildren<TMP_InputField>();
        if (NickInput == null)
        {
            Debug.LogError("Impossibile trovare l'InputField di TMPro.");
            return;
        }

        NickInput.Select();
        NickInputText = NickInput.GetComponentInChildren<TMP_Text>();

        
        GameObject childNameText = GameObject.Find("NicknameText");
        if (childNameText == null)
        {
            Debug.LogError("GameObject 'NicknameText' non trovato.");
            return;
        }

        NicknameText = childNameText.GetComponentInChildren<TMP_Text>(includeInactive: true);
        if (NicknameText == null)
        {
            Debug.LogError("Impossibile trovare il Text di TMPro.");
            return;
        }

        
        EditButton = GetComponentInChildren<Button>(includeInactive: true);
        if (EditButton == null)
        {
            Debug.LogError("Impossibile trovare il Button.");
            return;
        }
        NicknameText.gameObject.SetActive(false);
        NickInput.onEndEdit.AddListener(OnInputEndEdit);
    }

    public void SetNickname()
    {
        NickName = NickInputText.text;
        NickInput.gameObject.SetActive(false);
        NicknameText.gameObject.SetActive(true);
        NicknameText.text = NickName;
        Player.Instance.nickname = NickName;
        EditButton.gameObject.SetActive(true);
        isNameSet = true;
        MainPanelNickName.text = NickName;
        NewGameButton.interactable = true;
    }


    private void OnInputEndEdit(string _nickname)
    {
        if (!isNameSet && _nickname != "")
        {
            SetNickname();
        }
    }

    public void OnEditButtonClick()
    {
        isNameSet = false;
        NicknameText.gameObject.SetActive(false);
        NickInput.gameObject.SetActive(true);
        EditButton.gameObject.SetActive(false);
        NickInput.onEndEdit.AddListener(OnInputEndEdit);
        NewGameButton.interactable = false;
        NickInput.Select();
    }

    


}
