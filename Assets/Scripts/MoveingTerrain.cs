using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveingTerrain : MonoBehaviour
{
    public float speed;
    private GameManager gameManager;
    public GameObject nextToActivate;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.hasMatchStarted)
        {
            transform.position += Vector3.down * speed * Time.deltaTime;
        }
    }

}
