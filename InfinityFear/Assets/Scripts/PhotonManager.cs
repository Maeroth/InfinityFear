using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class PhotonManager : MonoBehaviourPunCallbacks
{
    public InputField RoomName;
    public GameObject[] DisableOnConnect;
    public GameObject[] EnableOnConnect;
    public GameObject[] DisableOnRoom;
    public static void ConnectToPhoton()
    {   //Lanzamos la conexion con Photon mediante la configuración de los ficheros
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom()
    {
        string roomId = "RoomID#" + Random.Range(0, 10000).ToString();
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { IsOpen = true, MaxPlayers = 4, IsVisible = true },TypedLobby.Default, null);
        
        
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("Room");
    }

    public override void OnJoinedRoom()
    {
        foreach(GameObject g in DisableOnRoom)
        {
            g.SetActive(false);
        }

        GameObject player = PhotonNetwork.Instantiate("Player", new Vector3(0, 2, 0), Quaternion.identity, 0, null);
    }

    public override void OnConnectedToMaster()
    {
        Debug.LogWarning("We have authed with photon and CONNECTED!");
        //Se eliminan de la pantalla aquellos objetos que no queramos mostrar, por ejemplo, los menús
        foreach(GameObject g in DisableOnConnect)
        {
            g.SetActive(false);
        }

        //Se agregan a la pantalla aquellos objetos que queramos mostrar, por ejemplo, los botones
        foreach (GameObject g in EnableOnConnect)
        {
            g.SetActive(true);
        }
    }
}
