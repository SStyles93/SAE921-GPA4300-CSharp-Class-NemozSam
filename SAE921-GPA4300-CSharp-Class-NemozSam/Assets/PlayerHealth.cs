using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public void TakeDamage()
    {
        //TODO do something else when the player gets hit
        Destroy(gameObject);
    }
}
