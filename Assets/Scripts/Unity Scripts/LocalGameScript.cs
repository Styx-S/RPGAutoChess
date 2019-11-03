using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class LocalGameScript : MonoBehaviour
{
    [SerializeField] bool runServerInLocal;
    private Server server;
    private Client client;

    void Awake() {
        if(runServerInLocal) {
            runServer();
        }
        client = new Client();
        client.run(IPAddress.Parse("127.0.0.1"), CommonDefine.kDefaultPort, UnityControllerCenter.getCenter());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void runServer() {
        server = new Server();
        server.run();
    }
}
