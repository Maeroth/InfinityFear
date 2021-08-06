using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PlayFabAuthenticator : MonoBehaviour
{
    private string PlayerIdCache = "";

    //Run the entire thing on awake
    private void Awake()
    {
        AuthWithPlayFab();    
    }

    /*
     * Step 1
     * We authenticate current PlayFab user normally.
     * In this case we use LoginWithCustomID API call for simplicity.
     * You can absolutely use any Login method you want.
     * We use PlayFabSettings.DeviceUniqueIdentifier as our custom ID.
     * We pass RequestPhotonToken as a callback to be our next step, if
     * authentication was successful.
     */
    private void AuthWithPlayFab()
    {
        Debug.Log("PlayFab autheticating using Custom ID...");
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest();
        request.CreateAccount = true;
        request.CustomId = PlayFabSettings.DeviceUniqueIdentifier;
        PlayFabClientAPI.LoginWithCustomID(request, RequestToken, OnError);

    }

    /*
     * Step 2
     * We request Photon authentication token from PlayFab.
     * This is a crucial step, because Photon uses different authentication tokens
     * than PlayFab. Thus, you cannot directly use PlayFab SessionTicket and
     * you need to explicitly request a token. This API call requires you to
     * pass Photon App ID. App ID may be hard coded, but, in this example,
     * We are accessing it using convenient static field on PhotonNetwork class
     * We pass in AuthenticateWithPhoton as a callback to be our next step, if
     * we have acquired token successfully
   */
    void RequestToken(LoginResult result)
    {
        Debug.Log("PlayFab authenticated. Requesting photon token...");
        PlayerIdCache = result.PlayFabId;
        GetPhotonAuthenticationTokenRequest request = new GetPhotonAuthenticationTokenRequest();
        request.PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime; //ID DE PHOTON
        PlayFabClientAPI.GetPhotonAuthenticationToken(request, AuthWithPhoton, OnError);
    }

    /*
     * Step 3
     * This is the final and the simplest step. We create new AuthenticationValues instance.
     * This class describes how to authenticate a players inside Photon environment.
     */
    void AuthWithPhoton(GetPhotonAuthenticationTokenResult result)
    {
        Debug.Log("Photon token acquired: " + result.PhotonCustomAuthenticationToken + " Authentication complete");

        //We set AuthType to custom, meaning we bring our own, PlayFab authentication procedure.
        var CustomAuth = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom };

        //We add "username" parameter. Do not let it confuse you: PlayFab is expecting this parameter to contain player PlayFab ID (!) and not username.
        CustomAuth.AddAuthParameter("username", PlayerIdCache);
        //We add "token" parameter. PlayFab expects it to contain Photon Authentication Token issues to your during previous step.
        CustomAuth.AddAuthParameter("token", result.PhotonCustomAuthenticationToken);

        //We finally tell Photon to use this authentication parameters throughout the entire application.
        PhotonNetwork.AuthValues = CustomAuth;

    }

    void OnError(PlayFabError error)
    {
        Debug.LogError($"[ERROR] | {error.GenerateErrorReport()}");
    }
}