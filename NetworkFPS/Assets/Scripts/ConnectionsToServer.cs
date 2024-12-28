using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class ConnectionsToServer : MonoBehaviourPunCallbacks
{
  public static ConnectionsToServer Instance { get; private set;}
  [SerializeField]
  private TMP_InputField inputRoomName;
  [SerializeField]
  private TMP_Text roomName;
   
  private void Awake()
  {
    Instance = this;
    PhotonNetwork.ConnectUsingSettings();
  }


    public override void OnConnectedToMaster()
    {
       PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log($"Connected to Lobby!");
        UiManager.Instance.OpenPanel("MainMenuPanel");
    }
    public override void OnJoinedRoom()
    {
      UiManager.Instance.OpenPanel("GameRoomPanel");
      roomName.text = PhotonNetwork.CurrentRoom.Name;
    }
    public void LeaveRoom()
    {
      PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
      UiManager.Instance.OpenPanel("MainMenuPanel");
    }
    
    public void CreateNewRoom()
    {
      if(string.IsNullOrEmpty(inputRoomName.text)) return;

      PhotonNetwork.CreateRoom(inputRoomName.text);
    }
}
