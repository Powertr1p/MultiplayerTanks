using System;
using System.Threading.Tasks;
using Network.Enums;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public static class AuthWrapper
{
    public static AuthState AuthState { get; private set; } = AuthState.NotAuthenticated;

    private static int _taskDelayTime = 1000;
    
    public static async Task<AuthState> DoAuth(int maxTries = 5)
    {
        if (AuthState == AuthState.Authenticated)
        {
            return AuthState;
        } 
        
        if (AuthState == AuthState.Authenticating)
        {
            Debug.LogWarning("Already authenticating!");
            await Authenticating();
            return AuthState;
        }
        
        await SignInAnnonymously(maxTries);
        
        return AuthState;
    }

    private static async Task<AuthState> Authenticating()
    {
        while (AuthState == AuthState.Authenticating || AuthState == AuthState.NotAuthenticated)
        {
            await Task.Delay(200);
        }

        return AuthState;
    }

    private static async Task SignInAnnonymously(int maxRetries)
    {
        AuthState = AuthState.Authenticating;
        
        int retries = 0;
        
        while (AuthState == AuthState.Authenticating && retries < maxRetries)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    AuthState = AuthState.Authenticated;
                    break;
                }
            }
            catch (AuthenticationException authEx)
            {
                ThrowException(authEx);
            }
            catch (RequestFailedException reqEx)
            {
                ThrowException(reqEx);
            }
            
            retries++;
            
            await Task.Delay(_taskDelayTime);
        }

        if (AuthState != AuthState.Authenticated)
        {
            Debug.LogWarning($"TimeOut authentication was raised after {retries} retries");
            AuthState = AuthState.TimeOut;
        }
           
    }

    private static void ThrowException(Exception exception)
    {
        Debug.LogError(exception);
        AuthState = AuthState.Error;
    }
}
