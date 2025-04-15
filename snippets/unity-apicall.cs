// CODE OM VANUIT UNITY JE EIGEN NODE API SERVER AAN TE ROEPEN
// GEBRUIKT ASYNC AWAIT EN TASK 

using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;
using System.Collections.Generic;

public class CatLoaderAsync : MonoBehaviour
{
    private string url = "http://localhost:3000/cat";

    private async void Start()
    {
        var data = await GetCatDataAsync();
        if (data != null)
        {
            foreach (var cube in data.cubes)
            {
                Debug.Log("Cube: " + string.Join(", ", cube));
            }
        }
    }

    private async Task<CatDataWrapper> GetCatDataAsync()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            var operation = request.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield(); // Non-blocking wait

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Fetch failed: " + request.error);
                return null;
            }

            string json = request.downloadHandler.text;
            return JsonUtility.FromJson<CatDataWrapper>(json);
        }
    }

    [System.Serializable]
    public class CatDataWrapper
    {
        public List<List<float>> cubes;
    }
}
