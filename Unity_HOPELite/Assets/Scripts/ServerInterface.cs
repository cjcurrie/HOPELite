using UnityEngine;
using System.Collections;
using SocketIOClient;

public static class ServerInterface
{
  static Client client;
  static string url = "localhost:2358/";

  public static void Initialize()
  {
    client = new Client(url);

    client.Opened += SocketOpened;
    client.Message += SocketMessage;
    client.SocketConnectionClosed += SocketConnectionClosed;
    client.Error +=SocketError;

    client.Connect();
  }

  public static void SendMessage(string message)
  {
    client.Emit("message", message);
  }

  public static void DeInitialize()
  {
    client.Close();
  }

  // Callbacks
  static void SocketOpened(object sender, SocketIOClient.MessageEventArgs e)
  {
    Debug.Log("Socket opened: "+e);
  }

  static void SocketMessage (object sender, System.EventArgs e)
  {
    if ( e!= null)
    {
      if (e.Message.Event == "message") {
        string msg = e.Message.MessageText;
        process(msg);
      }
      else
        Debug.Log("Got non-message event from server.");
    }
  }

  static void SocketConnectionClosed(object sender, System.EventArgs e)
  {
    Debug.Log("Socket connection closed: "+e);
  }

  static void SocketError(object sender, System.EventArgs e)
  {
    Debug.LogError("Socket error: "+e);
  }
}
