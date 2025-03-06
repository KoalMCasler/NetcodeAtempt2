using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : NetworkBehaviour
{
    public UIManager uIManager;
    public bool hasMatchStarted;
    public int numberOfPlayers;
    public Transform p1Point;
    public Transform p2Point;
    private float counter;
    public int matchDelay;
    public GameObject terrain;
    public int maxPlayers = 2;
    public float matchTime;
    public PlayerController[] activePlayers;
    private bool matchEnded;
    // Start is called before the first frame update
    void Start()
    {
        if(Time.timeScale != 1) Time.timeScale = 1;
        activePlayers = new PlayerController[maxPlayers];
        counter = matchDelay;
        hasMatchStarted = false;
        matchEnded = false;
    }
    // Update is called once per frame
    void Update()
    {
        if(uIManager.isSolo && !hasMatchStarted && !matchEnded|| numberOfPlayers >= maxPlayers && !hasMatchStarted && !matchEnded)
        {
            counter -= Time.deltaTime;
            if(counter <= 0)
            {
                StartMatch();
            }
        }
        if(hasMatchStarted)
        {
            matchTime += Time.deltaTime;
            CheckForLoser();
        }
    }

    public void StartMatch()
    {
        hasMatchStarted = true;
        matchTime = 0;
        GetActivePlayers();
    }

    public void EndMatch()
    {
        hasMatchStarted = false;
    }

    void CheckForLoser()
    {
        foreach(PlayerController player in activePlayers)
        {
            if(player.hasLost == true)
            {
                hasMatchStarted = false;
                matchEnded = true;
                uIManager.SetUIEnd(player);
                //Time.timeScale = 0;
                break;
            }
        }
    }

    void GetActivePlayers()
    {
        activePlayers = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
    }

}
