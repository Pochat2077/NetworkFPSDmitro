using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class ConnectionsToServer : MonoBehaviourPunCallbacks
{
  public static ConnectionsToServer Instance { get; private set;}
  [SerializeField]
  private TMP_InputField inputRoomName;
  [SerializeField]
  private TMP_Text roomName;
  [SerializeField]
  private Transform transformRoomList;
  [SerializeField]
  private GameObject roomItemPref;
  [SerializeField]
  private GameObject playerListPrefab;
  [SerializeField]
  private Transform playerListT;
  [SerializeField]
  private GameObject StartGameButton;
  [SerializeField]
  private GameObject mapSelector;
  [SerializeField]
  private string[] allMapNames;
  [SerializeField]
  private TMP_Text mapNameText;
  private int maxMapIndex, currentMapIndex = 1;
   
    private void Awake()
    {
     Instance = this;
     PhotonNetwork.ConnectUsingSettings();
     maxMapIndex = SceneManager.sceneCountInBuildSettings;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
      foreach(Transform room in transformRoomList)
      {
        Destroy(room.gameObject);
      }
      for(int i = 0; i < roomList.Count; i++)
      {
        Instantiate(roomItemPref, transformRoomList).GetComponent<RoomItem>().roomInfo = roomList[i];//Init(roomList[i]);
      }
    }


    public override void OnConnectedToMaster()
    {
      PhotonNetwork.JoinLobby();
      PhotonNetwork.AutomaticallySyncScene = true;

    }
    public override void OnJoinedLobby()
    {
        Debug.Log($"Connected to Lobby!");
        UiManager.Instance.OpenPanel("MainMenuPanel");
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
      mapSelector.SetActive(PhotonNetwork.IsMasterClient);
      StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
    public override void OnJoinedRoom()
    {
      UiManager.Instance.OpenPanel("GameRoomPanel");
      mapSelector.SetActive(PhotonNetwork.IsMasterClient);
      StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
     
      
      roomName.text = PhotonNetwork.CurrentRoom.Name;

      Player[] players = PhotonNetwork.PlayerList;
      foreach(Transform player in playerListT)
      {
        Destroy(player.gameObject);
      }
      for(int i = 0; i < players.Length; i++)
      {
        Instantiate(playerListPrefab, playerListT).GetComponent<PlayerListItem>().playerInfo = players[i];
      }
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
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
      Instantiate(playerListPrefab, playerListT).GetComponent<PlayerListItem>().playerInfo = newPlayer;
    }
    public void ConnectedToRandomRoom()
    {
      PhotonNetwork.JoinRandomRoom();
    }
  public void StartLevel()
  {
    SceneManager.LoadScene(currentMapIndex);
  }
  public void NextMap()
  {
    currentMapIndex++;
    if(currentMapIndex >= maxMapIndex) currentMapIndex = 1;
    mapNameText.text = allMapNames[currentMapIndex];
  }
  public void PrevMap()
  {
    currentMapIndex --;
    if(currentMapIndex < 1) currentMapIndex = maxMapIndex;
    mapNameText.text = allMapNames[currentMapIndex];
  }

}
