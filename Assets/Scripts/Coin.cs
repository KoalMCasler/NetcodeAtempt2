using Unity.Netcode;
using UnityEngine;

public class Coin : NetworkBehaviour
{
    public GameObject prefab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(NetworkManager.Singleton.IsServer)
        {
            if(collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerController>().score.Value += 1;
                NetworkObject.Despawn();
            }
        }
        else
        {
            return;
        }
    }
}
