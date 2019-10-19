using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prize : MonoBehaviour
{
    public void GetCaptured()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
