using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class UIManager : NetworkBehaviour
{
    public GameManager gameManager;
    public NetworkManager networkManager;
    public GameObject mainMenu;
    public GameObject hud;
    public bool isSolo;
    public GameObject sharedHUD;
    public GameObject endScreen;
    public TextMeshProUGUI endText;

    void Awake()
    {
        endScreen.SetActive(false);
        isSolo = false;
        hud.SetActive(false);
        mainMenu.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        sharedHUD = GameObject.FindGameObjectWithTag("HUD");
    }

    // Update is called once per frame
    void Update()
    {
        sharedHUD.GetComponent<SharedHUD>().UpdateScore(gameManager.matchTime);
    }

    public void JoinGame()
    {
        networkManager.StartClient();
        SetUIToGame();
    }

    public void HostGame()
    {
        networkManager.StartHost();
        SetUIToGame();
        sharedHUD.SetActive(true);
        
    }

    public void SoloGame()
    {
        isSolo = true;
        networkManager.StartHost();
        SetUIToGame();
        sharedHUD.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    void SetUIToGame()
    {
        mainMenu.SetActive(false);
        hud.SetActive(true);
    }

    public void SetUIEnd(PlayerController player)
    {
        int winPlayerNum;
        if(player.playerNum.Value == 1)
        {
            winPlayerNum = 2;
        }
        else
        {
            winPlayerNum = 1;
        }
        if(isSolo)
        {
            endText.text = string.Format("Good Try!\n{0:0.00} Total Match Time!", gameManager.matchTime);
        }
        else
        {
            endText.text = string.Format("Player {0} WINS!\n{1:0.00} Total Match Time!",winPlayerNum,gameManager.matchTime);
        }
        endScreen.SetActive(true);
    }

}
