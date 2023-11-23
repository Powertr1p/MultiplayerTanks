using System.Threading.Tasks;
using Network.Enums;
using Unity.Services.Core;
using UnityEngine.SceneManagement;

namespace Network.Client
{
    public class ClientGameManager
    {
        private const string LobbySceneName = "Lobby";
        
        public async Task<bool> InitAsync()
        {
            await UnityServices.InitializeAsync();
            
            AuthState authState = await AuthWrapper.DoAuth();

            return authState == AuthState.Authenticated;
        }

        public void ShowMenu()
        {
            SceneManager.LoadScene(LobbySceneName);
        }
    }
}