
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using TMPro;


public class condition_control : MonoBehaviour
{
    public static string api_key = "9dd34a89d4bc428c56e264f293b1d01f";
    public GameObject ConditionText;
    public GameObject Cloud;
    public GameObject Rain_Cloud;
    public GameObject Mist;
    public GameObject Lightning;
    public GameObject Sun;
    public GameObject Snow;
    private int condition = -1;
    private string url = "http://api.openweathermap.org/data/2.5/weather?lat=41.88&lon=-87.6&APPID=" + api_key + "&units=imperial";


    void Start()
    {

    // wait a couple seconds to start and then refresh every 30 seconds
       InvokeRepeating("GetDataFromWeb", 2f, 30f);
   }
    void Update() {
       if(Input.GetKeyDown(KeyCode.LeftArrow)){
           if(condition == 8){
               condition = 0;
           }
           else{
               condition++;
           }
       }
       else if(Input.GetKeyDown(KeyCode.RightArrow)){
           if(condition == 0){
               condition = 8;
           }
           else{
               condition--;
           }
       }
       ExtensionMethods.debugCondition(condition,Sun,Lightning,Cloud,Rain_Cloud,Mist,Snow);
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
                ConditionText.GetComponent<TextMeshPro>().text = info.weather[0].description;
                string icon = info.weather[0].icon;
                Debug.Log("\nICON: "+icon);
                if(icon.Equals("01d") || icon.Equals("01n")){
                    Sun.SetActive(true);
                    Lightning.SetActive(false);
                    Cloud.SetActive(false);
                    Rain_Cloud.SetActive(false);
                    Snow.SetActive(false);
                    Mist.SetActive(false);
                    condition = 0;
                }
                else if(icon.Equals("02d") || icon.Equals("02n")){
                    Sun.SetActive(true);
                    Lightning.SetActive(false);
                    Cloud.SetActive(true);
                    Cloud.GetComponent<ParticleSystem>().Stop();
                    Snow.SetActive(false);
                    Rain_Cloud.SetActive(false);
                    Mist.SetActive(false);
                    condition = 1;
                }
                else if(icon.Equals("03d") || icon.Equals("03n")){
                    Sun.SetActive(false);
                    Lightning.SetActive(false);
                    Cloud.SetActive(true);
                    Cloud.GetComponent<ParticleSystem>().Stop();
                    Snow.SetActive(false);
                    Rain_Cloud.SetActive(false);
                    Mist.SetActive(false);
                    condition = 2;
                }
                else if(icon.Equals("04d") || icon.Equals("04n")){
                    Sun.SetActive(false);
                    Lightning.SetActive(false);
                    Cloud.SetActive(true);
                    Cloud.GetComponent<ParticleSystem>().Stop();
                    Snow.SetActive(false);
                    Rain_Cloud.SetActive(true);
                    Rain_Cloud.GetComponent<ParticleSystem>().Stop();
                    Mist.SetActive(false);
                    condition = 3;
                }
                else if(icon.Equals("09d") || icon.Equals("09n")){
                    Sun.SetActive(false);
                    Lightning.SetActive(false);
                    Cloud.SetActive(false);
                    Snow.SetActive(false);
                    Rain_Cloud.SetActive(true);
                    Rain_Cloud.GetComponent<ParticleSystem>().Play();
                    Mist.SetActive(false);
                    condition = 4;
                }
                else if(icon.Equals("10d") || icon.Equals("10n")){
                    Sun.SetActive(true);
                    Lightning.SetActive(false);
                    Cloud.SetActive(false);
                    Snow.SetActive(false);
                    Rain_Cloud.SetActive(true);
                    Rain_Cloud.GetComponent<ParticleSystem>().Play();
                    Mist.SetActive(false);
                    condition = 5;
                }
                else if(icon.Equals("11d") || icon.Equals("11n")){
                    Sun.SetActive(false);
                    Lightning.SetActive(true);
                    Cloud.SetActive(true);
                    Cloud.GetComponent<ParticleSystem>().Stop();
                    Snow.SetActive(false);
                    Rain_Cloud.SetActive(true);
                    Rain_Cloud.GetComponent<ParticleSystem>().Play();
                    Mist.SetActive(false);
                    condition = 6;
                }
                else if(icon.Equals("13d") || icon.Equals("13n")){
                    Sun.SetActive(false);
                    Lightning.SetActive(false);
                    Cloud.SetActive(true);
                    Cloud.GetComponent<ParticleSystem>().Play();
                    Snow.SetActive(true);
                    Rain_Cloud.SetActive(false);
                    Mist.SetActive(false);
                    condition = 7;
                }
                else if(icon.Equals("50d") || icon.Equals("50n")){
                    Sun.SetActive(false);
                    Lightning.SetActive(false);
                    Cloud.SetActive(false);
                    Snow.SetActive(false);
                    Rain_Cloud.SetActive(false);
                    Mist.SetActive(true);
                    Mist.GetComponent<ParticleSystem>().Play();
                    condition = 8;
                }
            }
        }
    }

}
