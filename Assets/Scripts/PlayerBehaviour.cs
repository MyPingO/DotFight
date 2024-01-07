using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public void Kill()
    {
        Destroy(gameObject);
    }

    public void Respawn(GameObject[] SpawnPoints)
    {
        // TODO: Implement respawn logic
    }
}
