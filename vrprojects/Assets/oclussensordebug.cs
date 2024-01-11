using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;



public class oclussensordebug : MonoBehaviour
{
    public static oclussensordebug Instance;
    /// <summary>
    /// vr handle transform
    /// </summary>
    /*public GameObject lefthand;
    public GameObject righthand;
   
    public List<ObjectData> objectList = new List<ObjectData>();
    [Serializable]
    public class ObjectData
    {
 
        public Vector3 rightposition;

        public Vector3 leftposition;
    }
    [Serializable]
    private class ObjectDataListWrapper
    {
        public List<ObjectData> objectDataList;
    }
    // Start is called before the first frame update
    void Start()
    {
        SendDebugJSON();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i=0; i< objectList.Count; i++)
        {

        lefthand.transform.position = objectList[i].leftposition;
        righthand.transform.position = objectList[i].rightposition;
        }

        
        Debug.Log("lefthand: " + lefthand.transform.position);
        Debug.Log("righthand: " + righthand.transform.position);
       
        
    }
    private string url = "https://22a6-122-172-85-170.ngrok-free.app/api/v1/vr/raw-data-ingest"; // Replace with your API endpoint

    public void SendDebugJSON()
    {
        // Create a JSON payload string
        string value = righthand.transform.position.ToString();
      string value1 =  value.Replace("(", "\"");
        string value2 = value1.Replace(")", "\"");
        Debug.Log(value2);
       string jsonPayload = "{\"payload\":" + value2 +"}";
        Debug.Log(jsonPayload);
     // string jsonPayload = "{"payload":"ab"}";
       *//* object jsonObj = CreateFromJSON(jsonPayload);
        Debug.Log(jsonObj);*//*
        StartCoroutine(_updateExperience(jsonPayload));
    }
    public ThingModel CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<ThingModel>(jsonString);
    }



    private IEnumerator _updateExperience(string json)
    {
        // Create the API endpoint URL with path variables


        // Create the UnityWebRequest
        UnityWebRequest request =new UnityWebRequest(url, "POST");
        Debug.Log("url: " + url);
       


        Debug.Log("sending json: " + json);

        // Convert the request body to JSON
       // string requestBodyJson = "\"data\"{" + json + "}";
        //JsonConvert.SerializeObject(requestBody);



        Debug.Log("sending json: " + json);

        // Set the request body as the raw JSON string
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler =(UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        // Send the request
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string jsonResponse = request.downloadHandler.text;
            // Process the response as needed
            Debug.Log(jsonResponse);
        }
        else
        {
            Debug.Log("Error: " + request.error);
        }
    }
    [System.Serializable]
    public class ThingModel
    {
        public object payload { get; set; }
        
    }*/
    private void Awake()
    {

        Instance = this;
    }
        void Start()
    {
       // StartCoroutine(CallAPI());
    }
    /// <summary>
    /// audio api vr
    /// </summary>
    /// <returns></returns>
   public IEnumerator SendAudio()
    {
        // Path to the audio file
        string filePath = Application.persistentDataPath + "/audio_file.wav";



        byte[] mp3Data = System.IO.File.ReadAllBytes(filePath);
        WWWForm form = new WWWForm();
        form.AddField("audio", filePath);
        form.AddField("selectedTopic", oclusActiveCanvas.Instance.topic);
        form.AddBinaryData("audio", mp3Data, filePath);
        // Create a new UnityWebRequest
        //UnityWebRequest www = UnityWebRequest.Post("https://768d-122-172-83-222.ngrok-free.app/api/v1/vr/audio", form);
        using (UnityWebRequest www = UnityWebRequest.Post("http://65.2.42.229:3000/api/v1/speaker/audio", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Error uploading MP3: " + www.error);
            }
            else
            {
                Debug.Log(UnityWebRequest.Result.Success);
            }
        }
                                                                 

        // Send the request
      
    }
    private string apiUrl = "http://65.2.42.229:3000/api/v1/speaker/topics";

   

    private IEnumerator CallAPI()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string response = webRequest.downloadHandler.text;
                // Process the API response here
            }
            else
            {
                Debug.LogError("API request failed. Error: " + webRequest.error);
            }
        }
    }
}
