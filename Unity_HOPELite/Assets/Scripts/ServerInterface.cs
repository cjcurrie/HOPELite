using UnityEngine;
using System.Collections;
//using SocketIOClient;
using SocketIO;
using System;
using System.Collections.Generic;

public class ServerInterface
{
  //Client client;
  string url = "localhost:2358/";
  SocketIOComponent socket;

  public void Initialize()
  {
    GameObject go = GameObject.Find("SocketIO");
    socket = go.GetComponent<SocketIOComponent>();

    socket.Connect();

    // Register callbacks
    socket.On("message", OnMessage);

    // Send first contact to server
    socket.Emit("connection");

    // Log in
    Dictionary<string,string> login = new Dictionary<string,string>();
    login["username"] = "cjcurrie";
    socket.Emit("login", new JSONObject(login));

    /*
    client = new Client(url);

    client.Opened += OnSocketOpened;
    client.Message += SocketMessage;
    client.SocketConnectionClosed += SocketConnectionClosed;
    client.Error +=SocketError;

    client.Connect();
    */
  }

  public void OnMessage(SocketIOEvent e)
  {
    Debug.Log(string.Format("[{0}: {1}]", e.data["source"], e.data["text"]));
  }

  /*
  public void SendMessage(string message)
  {
    client.Emit("message", message);
  }

  public void DeInitialize()
  {
    client.Close();
  }

  // Callbacks
  public void OnSocketOpened(object sender, EventArgs e)
  {
    Debug.Log("Socket opened: "+e);
  }

  void SocketMessage (object sender, MessageEventArgs e)
  {
    if ( e!= null)
    {
      if (e.Message.Event == "message") {
        string msg = e.Message.MessageText;
        Debug.Log("Received message: "+msg);
      }
      else
        Debug.Log("Got non-message event from server.");
    }
  }

  void SocketConnectionClosed(object sender, EventArgs e)
  {
    Debug.Log("Socket connection closed: "+e.ToString());
  }

  void SocketError(object sender, ErrorEventArgs e)
  {
    Debug.LogError("Socket error: "+e.Message);
  }
  */
}
