using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int count = 0;
    public static GameManager instance;
    public Text countText;
    public GameObject rabbitPrefab;
    public GameObject particle;
    public bool isHint = true;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        //countText.text = "√£¿∫ ≈‰≥¢ " + countText + " / 5";
    }

    void Update()
    {
        if (isHint)
        {
            countText.text = "HINT!!";
        }
        else if (count < 5)
            countText.text = "Rabbit " + count + " / 5";
        else
            countText.text = "THANKYOU!!m\nI'll give you lots of rabbits !!m";
    }

    public void GetRabbit()
    {
        count++;
        if (count == 5)
        {
            gameObject.GetComponent<AudioSource>().Play();
            for (int i = 0; i < 1000; i++)
            {
                GameObject rabbit = Instantiate(rabbitPrefab);
                Vector3 rabbitPosition = transform.position;
                float randomP = Random.Range(0f, 51f);
                rabbitPosition.x += -25 + randomP;
                rabbitPosition.y += Random.Range(0, 30);
                rabbitPosition.z += -25 + randomP;
                float randomA = Random.Range(0, 360);
                rabbit.transform.eulerAngles = new Vector3(randomA, randomA, randomA);
                rabbit.transform.position = rabbitPosition;

            }
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
