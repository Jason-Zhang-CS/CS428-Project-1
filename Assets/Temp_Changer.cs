using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using TMPro;


[System.Serializable]
public class Coord    {
        public float lon; 
        public float lat; 
}
[System.Serializable]
public class Weather
{
    public int id;
    public string main;
    public string description;
    public string icon;
}
[System.Serializable]
public class Main
{
    public float temp;
    public float feels_like;
    public float temp_min;
    public float temp_max;
    public float pressure;
    public float humidity;
}
[System.Serializable]
public class Wind
{
    public float speed;
    public float deg;
}
[System.Serializable]
public class Clouds
{
    public float all;
}
[System.Serializable]
public class Sys
{
    public int type;
    public int id;
    public string country;
    public int sunrise;
    public int sunset;
}

[System.Serializable]
public class Root{
    public Coord coord; 
    public List<Weather> weather; 

    public string Base; 
    public Main main; 
    public int visibility; 
    public Wind wind; 
    public Clouds clouds; 
    public int dt; 
    public Sys sys; 
    public int timezone; 
    public int id; 
    public string name; 
    public int cod; 
}

public static class ExtensionMethods {
 
    public static float map (float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static string wind_direction(float degree){
        if(348.75<=degree&&degree<11.25){
            return "N";
        }
        else if(11.25<=degree&&degree<33.75){
            return "NNE";
        }
        else if(33.75<=degree&&degree<56.25){
            return "NE";
        }
        else if(56.25<=degree&&degree<78.25){
            return "ENE";
        }
        else if(78.25<=degree&&degree<101.25){
            return "E";
        }
        else if(101.25<=degree&&degree<123.75){
            return "ESE";
        }
        else if(123.75<=degree&&degree<146.25){
            return "SE";
        }
        else if(146.25<=degree&&degree<168.75){
            return "SSE";
        }
        else if(168.75<=degree&&degree<191.25){
            return "S";
        }
        else if(191.25<=degree&&degree<213.75){
            return "SSW";
        }
        else if(213.75<=degree&&degree<236.25){
            return "SW";
        }
        else if(236.25<=degree&&degree<258.75){
            return "WSW";
        }
        else if(258.75<=degree&&degree<281.25){
            return "W";
        }
        else if(281.25<=degree&&degree<303.75){
            return "WNW";
        }
        else if(303.75<=degree&&degree<326.25){
            return "NW";
        }
        else{
            return "NNW";
        }
    }
    
    public static void debugCondition(int condition,GameObject Sun,GameObject Lightning,GameObject Cloud,GameObject Rain_Cloud,GameObject Mist,GameObject Snow){
        if (condition == 0)
        {
            Sun.SetActive(true);
            Lightning.SetActive(false);
            Cloud.SetActive(false);
            Snow.SetActive(false);
            Rain_Cloud.SetActive(false);
            Mist.SetActive(false);
        }
        else if (condition == 1)
        {
            Sun.SetActive(true);
            Lightning.SetActive(false);
            Cloud.SetActive(true);
            Snow.SetActive(false);
            Cloud.GetComponent<ParticleSystem>().Stop();
            Rain_Cloud.SetActive(false);
            Mist.SetActive(false);
            condition = 1;
        }
        else if (condition == 2)
        {
            Sun.SetActive(false);
            Lightning.SetActive(false);
            Cloud.SetActive(true);
            Cloud.GetComponent<ParticleSystem>().Stop();
            Snow.SetActive(false);
            Rain_Cloud.SetActive(false);
            Mist.SetActive(false);
            condition = 2;
        }
        else if (condition == 3)
        {
            Sun.SetActive(false);
            Lightning.SetActive(false);
            Cloud.SetActive(true);
            Cloud.GetComponent<ParticleSystem>().Stop();
            Snow.SetActive(false);
            Rain_Cloud.SetActive(false);
            Mist.SetActive(false);
            condition = 3;
        }
        else if (condition == 4)
        {
            Sun.SetActive(false);
            Lightning.SetActive(false);
            Cloud.SetActive(false);
            Rain_Cloud.SetActive(true);
            Rain_Cloud.GetComponent<ParticleSystem>().Play();
            Snow.SetActive(false);
            Mist.SetActive(false);
            condition = 4;
        }
        else if (condition == 5)
        {
            Sun.SetActive(true);
            Lightning.SetActive(false);
            Cloud.SetActive(false);
            Rain_Cloud.SetActive(true);
            Rain_Cloud.GetComponent<ParticleSystem>().Play();
            Snow.SetActive(false);
            Mist.SetActive(false);
            condition = 5;
        }
        else if (condition == 6)
        {
            Sun.SetActive(false);
            Lightning.SetActive(true);
            Cloud.SetActive(true);
            Cloud.GetComponent<ParticleSystem>().Stop();
            Rain_Cloud.SetActive(true);
            Rain_Cloud.GetComponent<ParticleSystem>().Play();
            Snow.SetActive(false);
            Mist.SetActive(false);
            condition = 6;
        }
        else if (condition == 7)
        {
            Sun.SetActive(false);
            Lightning.SetActive(false);
            Cloud.SetActive(true);
            Cloud.GetComponent<ParticleSystem>().Play();
            Snow.SetActive(true);
            Rain_Cloud.SetActive(false);
            Mist.SetActive(false);
            condition = 7;
        }
        else if (condition == 8)
        {
            Sun.SetActive(false);
            Lightning.SetActive(false);
            Cloud.SetActive(false);
            Snow.SetActive(false);
            Rain_Cloud.SetActive(false);
            Mist.SetActive(true);
        }
    }
   
}


public class Temp_Changer : MonoBehaviour
{
    public static string api_key = "9dd34a89d4bc428c56e264f293b1d01f";
     public GameObject TempText;
     public GameObject HuText;
     public GameObject Temp3D;
     public GameObject Hu3D;
     private float minTem = -50;
     private float maxTem = 110;
    private string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID="+api_key+"&units=imperial";

   
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
                float temp = ExtensionMethods.map(info.main.temp,minTem,maxTem,0,1);
                Debug.Log("\nTemp: "+temp);
                float humid = ExtensionMethods.map(info.main.humidity,0,100,0,1);
                Debug.Log("\nHumidity: "+humid);
                
                TempText.GetComponent<TextMeshPro>().text = System.Math.Round(info.main.temp,1) + " F";
                yield return Lerp3D(Temp3D,new Vector3(1,temp,1));
                HuText.GetComponent<TextMeshPro>().text = info.main.humidity + "%";
                yield return Lerp3D(Hu3D,new Vector3(1,humid,1));
            }
        }
    }

    IEnumerator Lerp3D(GameObject a, Vector3 end){
        float i = 0;
        while(i<1.0f){
            i+=Time.deltaTime * 0.2f;
            a.GetComponent<Transform>().localScale = Vector3.Lerp(a.GetComponent<Transform>().localScale,end,i);
            yield return null;
        }
    }
    
}
