using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_Text playerNameText;


    public Player playerInfo { private get; set; }
    private void Init()
    {
        playerNameText.text = playerInfo.NickName;
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
      if(playerInfo == otherPlayer)
      {
        Destroy(gameObject);
      }
    }
    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
    void Start()
    {
        Init();
    }


}
