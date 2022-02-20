using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour
{
    public void TakeDamage()
    {
        SendMessage("OnTakeDamage");
    }
}
