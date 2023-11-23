using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] private ClientSingleton _clientPrefab;
    [SerializeField] private HostSingleton _hostSingleton;
    
    private async void Start()
    {
        DontDestroyOnLoad(gameObject);

        await LaunchInMode(SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null);
    }

    private async Task LaunchInMode(bool isDedicatedServer)
    {
        if (isDedicatedServer)
        {
            
        }
        else
        {
            HostSingleton hostInstance = Instantiate(_hostSingleton);
            hostInstance.CreateHost();
            
            ClientSingleton clientInstance = Instantiate(_clientPrefab);
            bool authenticated = await clientInstance.CreateClient();

            if (authenticated)
            {
                clientInstance.ClientManager.ShowMenu();
            }
        }
    }
}
