using System.Threading.Tasks;
using Network.Enums;
using Unity.Services.Authentication;

public static class AuthWrapper
{
    public static AuthState AuthState { get; private set; } = AuthState.NotAuthenticated;

    private static int _taskDelayTime = 1000;
    
    public static async Task<AuthState> DoAuth(int maxTries = 5)
    {
        if (AuthState == AuthState.Authenticated) 
            return AuthState;

        AuthState = AuthState.Authenticating;
        int currentTry = 0;

        while (AuthState == AuthState.Authenticating && currentTry < maxTries)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

            if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
            {
                AuthState = AuthState.Authenticated;
                break;
            }

            currentTry++;
            
            await Task.Delay(_taskDelayTime);
        }

        return AuthState;
    }
}
