using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;


public class SetPlayerName : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_InputField nameInput;

    
    public override void OnConnectedToMaster()
    {
        LoadNickName();
    }
    private void LoadNickName()
    {
        string playerName = PlayerPrefs.GetString("SavedNickName");
        if(string.IsNullOrEmpty(playerName))
        {
            playerName = $"Bot {Random.Range(1, 101)}";
        }
        PhotonNetwork.NickName = playerName;
        nameInput.text = playerName;
    }
    public void ChangeName()
    {
        PlayerPrefs.SetString("SavedNickName", nameInput.text);
        LoadNickName();
    }
}
