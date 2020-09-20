using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using TMPro;


public class windTeller : MonoBehaviour
{
    public static string api_key = "9dd34a89d4bc428c56e264f293b1d01f";
    public GameObject WindText;
    public GameObject Direction;
    public GameObject Speed;
    private string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=" + api_key + "&units=imperial";


    void Start()
    {

    // wait a couple seconds to start and then refresh every 30 seconds

       InvokeRepeating("GetDataFromWeb", 2f, 30f);
   }

   void GetDataFromWeb()
   {

       StartCoroutine(GetRequest(url));
   }

    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();


            if (webRequest.isNetworkError)
            {
                Debug.Log(": Error: " + webRequest.error);
            }
            else
            {
                // print out the weather data to make sure it makes sense
                string jsonString = webRequest.downloadHandler.text;
                Debug.Log(":\nReceived: " + jsonString);
                Root info = JsonUtility.FromJson<Root>(jsonString);
                Debug.Log("\nInfo: "+JsonUtility.ToJson(info));
                WindText.GetComponent<TextMeshPro>().text = info.wind.speed + " mph "+ ExtensionMethods.wind_direction(info.wind.deg);
                Direction.GetComponent<Transform>().localRotation = Quaternion.Euler(0,info.wind.deg,0);
                Speed.GetComponent<Cloth>().externalAcceleration = new Vector3(0,0,info.wind.speed);
                Speed.GetComponent<Cloth>().randomAcceleration = new Vector3(0,0,info.wind.speed);
            }
        }
    }

}
