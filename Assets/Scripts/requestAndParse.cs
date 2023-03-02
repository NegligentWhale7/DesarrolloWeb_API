using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;
using SimpleJSON;
using UnityEngine.Events;
using UnityEngine.UI;

public class requestAndParse: MonoBehaviour
{
    string resutado ="";
    public ContenedorPersonas contenedorPersonas;

    public UnityEvent evento;
    UnityWebRequestAsyncOperation operation, imageOP;
    [SerializeField] string pais = "";
    [SerializeField] int numberOfUniversities;
    [SerializeField] RawImage countryFlag;
    public bool hasEnteredCountry = false;

    Persona uni;
    private void Awake()
    {
        contenedorPersonas.personas.Clear();
    }
    void Start()
    {
        // await GetRequest();
        // Debug.Log("De regreso al metodo start");
        // Debug.Log("Resultado: "+ resutado);
    }
    
    public void GetCountry(string country)
    {
        Debug.Log(country);
        pais = country;        
        hasEnteredCountry= true;
    }
    public void SetNumberOfUniversities(string num)
    {
        numberOfUniversities = int.Parse(num);
        Debug.Log(num);
    }
    public async void IsReady()
    {
        await GetRequest();
        await GetCountryFlag();
    }
    public void ClearUniversitiesContainer()
    {
        contenedorPersonas.personas.Clear();
        /*string[] personaFolder = { "Assets/Personas" };
        foreach (var asset in AssetDatabase.FindAssets("", personaFolder))
        {
            var path = AssetDatabase.GUIDToAssetPath(asset);
            AssetDatabase.DeleteAsset(path);
        }*/
    }
    async Task GetRequest()
    {
        string Url = "http://universities.hipolabs.com/search?country=" + pais;

        using var www = UnityWebRequest.Get(Url);
        operation = www.SendWebRequest();
        while(!operation.isDone)
        await Task.Yield();

        if(www.result== UnityWebRequest.Result.Success)
        {
            Debug.Log($"Success: {www.downloadHandler.text}");
            resutado =www.downloadHandler.text; 
            try
            {
                JSONNode root = JSONNode.Parse(resutado);
                JSONArray jsArray = root.AsArray;
                for(int i = 0; i < numberOfUniversities; i++)
                {
                    var jsObject = jsArray[i];
                    Debug.Log(jsObject["name"]);
                    uni = ScriptableObject.CreateInstance<Persona>();
                    uni.nombre = jsObject["name"];
                    //AssetDatabase.CreateAsset(uni, "Assets/Personas/" + uni.nombre.Split('"')[0] + ".asset");
                    contenedorPersonas.personas.Add(uni);
                }
                    evento.Invoke();
                     
            }
            catch
            {
                Debug.Log("Algo salio mal convirtiendo la respuesta en objetos");
            }
        }
    }

    private async Task GetCountryFlag()
    {
        string imageUrl = "https://countryflagsapi.com/png/" + pais;
        UnityWebRequest imageWR = UnityWebRequestTexture.GetTexture(imageUrl);
        imageOP = imageWR.SendWebRequest();
        while(!imageOP.isDone)
        {
            await Task.Yield();
        }
        if (imageWR.result == UnityWebRequest.Result.Success)
        {
            var texture = DownloadHandlerTexture.GetContent(imageWR);
            countryFlag.texture = texture;
        }
        else
        {
            Debug.Log("Error");
        }
    }
}