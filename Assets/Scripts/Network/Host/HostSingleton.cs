using UnityEngine;

public class HostSingleton : MonoBehaviour
{
    public static HostSingleton Instance
    {
        //TODO: refactor this singleton!!
        get
        {
            if (_instance != null) return _instance;
            
            _instance = FindObjectOfType<HostSingleton>();

            if (_instance == null)
            {
                Debug.LogError("put host on scene!");
                return null;
            }

            return _instance;
        }
    }

    public HostGameManager Host => _host;
    
    private static HostSingleton _instance;
    private HostGameManager _host;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CreateHost()
    {
       _host = new HostGameManager();
    }
}
