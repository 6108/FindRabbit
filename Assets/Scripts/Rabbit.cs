using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    public void ClickRabbit()
    {
        GameManager.instance.GetRabbit();
        Destroy(gameObject);
    }
}
