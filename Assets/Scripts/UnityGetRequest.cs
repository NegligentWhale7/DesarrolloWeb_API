using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class UnityGetRequest: MonoBehaviour{
string resutado ="";

async void Start(){
    await GetRequest();
    Debug.Log("De regreso al metodo start");
    Debug.Log("Resultado: "+ resutado);
}

async Task GetRequest(){
     string Url = "https://www.boredapi.com/api/activity";

     using var www = UnityWebRequest.Get(Url);
     var operation = www.SendWebRequest();
     while(!operation.isDone)
        await Task.Yield();
    if(www.result== UnityWebRequest.Result.Success){
        Debug.Log($"Success: {www.downloadHandler.text}");
        resutado =www.downloadHandler.text; 
    }
}
}