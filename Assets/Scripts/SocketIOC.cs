using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleJson;
using SocketIOClient;
using SocketIOClient.Messages;
using UnityEngine;

public class SocketIOC : MonoBehaviour
{
    public Client socket;

    public string serverAddress;

    private Queue<IMessage> messageQueue;

    void Start()
    {
        messageQueue = new Queue<IMessage>();

        socket = new Client(serverAddress);
        socket.On("connect", (fn) => { socket.Emit("add-user", "{\"userId\":\"game\", \"userName\":\"game\"}"); });
        socket.On("user-joined", (data) => { messageQueue.Enqueue(data); });
        socket.On("user-left", (data) => { messageQueue.Enqueue(data); });
        socket.On("set-vector2D", (data) => { messageQueue.Enqueue(data); });
        socket.Error += (sender, e) => { Debug.Log("socket Error: " + e.Message.ToString()); };

        socket.Connect();
        Debug.Log("Connected to socket");
    }

    void Update()
    {
        for (int i = 0; i < messageQueue.Count; i++)
        {
            IMessage message = messageQueue.Dequeue();

            JsonObject args = (JsonObject) message.Json.args[0];

            Debug.Log(message.Json.name);
            
            switch (message.Json.name)
            {
                case "user-joined":
                    if (args.Values.Count > 1)
                    {
                        Debug.Log((string) args.Values.ElementAt(1));
                        GetComponent<GameManager>().spawnPlayer((string) args.Values.ElementAt(0));
                    }

                    break;
                case "user-left":
                    if (args.Values.Count > 0)
                    {
                        GetComponent<GameManager>().deSpawnPlayer((string) args.Values.ElementAt(0));
                    }

                    break;
                case "set-vector2D":
                    if (args.Values.Count > 2)
                    {
                        Vector2 direction = new Vector2(Convert.ToSingle(args.Values.ElementAt(0)),
                            Convert.ToSingle(args.Values.ElementAt(1)));
                        GetComponent<GameManager>().movePlayer((string) args.Values.ElementAt(2), direction);
                    }

                    break;
            }
        }
    }

    void OnDestroy()
    {
        socket.Close();
        Debug.Log("Disconnected socket");
    }
}

//If a player disconnects he will be despawned but his trails won't.