using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    [SerializeField]
    private TMP_Text roomNameText;
    
    public RoomInfo roomInfo { private get; set;}
    [SerializeField]
    private Button roomButton;

    private void Init()
    {
        roomNameText.text = roomInfo.Name;
        roomButton.onClick.AddListener(JoinRoom);
    }
    private void JoinRoom()
    {
      PhotonNetwork.JoinRoom(roomInfo.Name);
    }
    private void Start()
    {
        Init();
    }

    
    void Update()
    {
        
    }
}
