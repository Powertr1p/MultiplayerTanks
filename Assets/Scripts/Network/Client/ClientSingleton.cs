using System.Threading.Tasks;
using Network.Client;
using UnityEngine;

public class ClientSingleton : MonoBehaviour
{
    public static ClientSingleton Instance
    {
        //TODO: refactor this singleton!!
        get
        {
            if (_instance != null) return _instance;
            
            _instance = FindObjectOfType<ClientSingleton>();

            if (_instance == null)
            {
                Debug.LogError("put client on scene!");
                return null;
            }

            return _instance;
        }
    }

    public ClientGameManager ClientManager => _client;
    
    private static ClientSingleton _instance;
    private ClientGameManager _client;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public async Task<bool> CreateClient()
    {
        _client = new ClientGameManager();
        return await _client.InitAsync();
    }
}
