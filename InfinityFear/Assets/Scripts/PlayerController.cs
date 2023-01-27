using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using Photon.Pun;
public class PlayerController : MonoBehaviourPun, IPunObservable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetKey(KeyCode.W))
            {
                gameObject.transform.Translate(Vector3.forward * 50 * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                gameObject.transform.Translate(Vector3.back * 50 * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                gameObject.transform.Translate(Vector3.left * 50 * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                gameObject.transform.Translate(Vector3.right * 50 * Time.deltaTime);
            }
        }
    }
    
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {

        }
        else if (stream.IsReading)
        {

        }
    }
}
