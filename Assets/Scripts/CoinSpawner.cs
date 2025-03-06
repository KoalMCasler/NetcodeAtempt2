using Unity.Netcode;
using UnityEngine;

public class CoinSpawner : NetworkBehaviour
{
    public GameObject coinPrefab;
    public int coinSecondsTillSpawn;
    public bool canSpawnCoins;
    public bool clearToSpawn;
    private float counter;

    void Start()
    {
        if(!IsServer)
        {
            this.enabled=false;
            return;
        }
        counter = coinSecondsTillSpawn;
        clearToSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(counter > 0 && !canSpawnCoins)
        {
            counter -= Time.deltaTime;
        }
        if(counter <= 0)
        {
            canSpawnCoins = true;
            if(canSpawnCoins && clearToSpawn)
            {
                counter = coinSecondsTillSpawn;
                NetworkObject obj = Instantiate(coinPrefab).GetComponent<NetworkObject>();
                obj.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                obj.Spawn(destroyWithScene: true);
            }
        }

    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            clearToSpawn = false;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            clearToSpawn = true;
        }
    }

}
