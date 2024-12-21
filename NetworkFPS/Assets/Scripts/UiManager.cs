using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;
    [SerializeField]
    private GameObject[] panels;
    private TMP_Text ConnectedPlayersText;
    private void Awake()
    {
        Instance = this;
        foreach(GameObject panel in panels)
        {
            panel.SetActive(false);
        }
    }
    private void Start()
    {
        OpenPanel("LoadingPanel");
    }

    public void OpenPanel(string panelName)
    {
        foreach (GameObject panel in panels)
        {
            if(panel.name == name)
            {
                panel.SetActive(true);
            }
            else
            {
                panel.SetActive(false);
            }
        }
    }
    public void ChangeConnectedPlayersText(int playersCount)
    {
        ConnectedPlayersText.text = $"Connected players: {playersCount}";
    }

   
}
