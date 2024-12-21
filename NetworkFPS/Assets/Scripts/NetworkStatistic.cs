using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkStatistic : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(UpdateConnectedPlayersCount());
    }
    private IEnumerator UpdateConnectedPlayersCount()
    {
        yield return new WaitForSeconds(6);
        while (true)
        {
            
        UiManager.Instance.ChangeConnectedPlayersText(1);
        yield return new WaitForSeconds(6);
        }
        
    }

    
    
}
