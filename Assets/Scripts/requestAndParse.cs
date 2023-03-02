using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System;

using SimpleJSON;

using UnityEditor;
using UnityEngine.Events;
public class requestAndParse: MonoBehaviour
{
    string resutado ="";
    public ContenedorPersonas contenedorPersonas;

    public UnityEvent evento;
    UnityWebRequestAsyncOperation operation;
    [SerializeField] string pais = "";
    [SerializeField] int numberOfUniversities;
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
async Task GetRequest(){
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
                //Debug.Log(jsArray.ToString());
                //var jsObject = jsArray[0];
             //Debug.Log(jsObject["name"]);
                for(int i = 0; i < numberOfUniversities; i++)
                {
                    var jsObject = jsArray[i];
                    Debug.Log(jsObject["name"]);
                    uni = ScriptableObject.CreateInstance<Persona>();
                    uni.nombre = jsObject["name"];
                    //AssetDatabase.CreateAsset(uni, "Assets/Personas/" + uni.nombre.Split('"')[0] + ".asset");
                    contenedorPersonas.personas.Add(uni);
                }

            // Debug.Log("Raiz: "+ root);
            // Debug.Log(root["results"]);
            /*foreach (var r in jsObject["results"])
            {
                // Debug.Log(r);
                // Debug.Log(r.Value);

                // Debug.Log(r.Value["name"]); 
                Debug.Log(r.Value["name"]["first"]);
                Persona p = ScriptableObject.CreateInstance<Persona>();
                p.nombre=r.Value["name"]["first"];
                p.apellido=r.Value["name"]["last"];
                // Debug.Log(r.Value["name"]); 
                // Debug.Log(r.Value["name"]["first"]);


                AssetDatabase.CreateAsset(p, "Assets/Personas/"+p.nombre+".asset");
                        contenedorPersonas.personas.Add(p);
            }*/
                    evento.Invoke();
                     
        }
        catch{
            Debug.Log("Algo salio mal convirtiendo la respuesta en objetos");
        }
    }
}
}