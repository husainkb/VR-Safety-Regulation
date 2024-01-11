
using UnityEngine;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class oclusActiveCanvas : MonoBehaviour
{
    public static oclusActiveCanvas Instance;
    public GameObject menu;
    public float spawnDistance = 2;
    public InputActionProperty showButton;
    public Transform head;
    private void Awake()
    {

        Instance = this;
    }
    void Update()
    {
        // Check if the "A" button on the Oculus remote is pressed
        if (showButton.action.WasPressedThisFrame())
        {
            Debug.Log("burhan");
            menu.SetActive(!menu.activeSelf);
            menu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;


        }
        menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
        menu.transform.forward *= -1;

        
    }
    //////
    ///UI
    ///

    public GameObject topics;
    public GameObject audio;
    public string topic;
    public GameObject prompt;

    public void topicSelection(Text text)
    {
        
        topics.SetActive(false);
        audio.SetActive(true);
        topic = text.text.ToString();

    }
    public void returntopicSelection()
    {

        topics.SetActive(true);
        audio.SetActive(false);
        

    }
    public void withpromptSelection()
    {

        topics.SetActive(true);
        prompt.SetActive(false);



    }
    public void withoutpromptSelection()
    {

        audio.SetActive(true);
        prompt.SetActive(false);



    }
    public void returnpromptSelection()
    {

        topics.SetActive(false);
        prompt.SetActive(true);



    }

}
