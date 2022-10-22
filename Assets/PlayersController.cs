using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NativeWebSocket;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;
using Mono.Cecil.Cil;
using System;

public class IncomingMessageDto
{
    public string code { get; set; }
}

public class GameDataDto
{
    public string gui { get; set; }
    public int max_players { get; set; }
}

public class RoomCodeDto
{
    public string code { get; set; }
}

public class PlayersController : MonoBehaviour
{
    [SerializeField]
    Player player;

    int arrowUp = 0b00000000;
    int arrowLeft = 0b00000110;
    int arrowRight = 0b00000100;
    int arrowDown = 0b00000010;

    int keyUp = 0b00000000;
    int keyDown = 0b00000001;


    WebSocket websocket;
    readonly HttpClient client = new HttpClient();
    // Start is called before the first frame update
    async void Start()
    {
        var gameData = new GameDataDto() { gui = "CrossArrows", max_players = 6};
        var serializedData = JsonConvert.SerializeObject(gameData);
        var data = new StringContent(serializedData);
        try
        {
            // Getting room code from the server
            var res = await client.PostAsync("http://localhost:8081/create", data);
            var resData = await res.Content.ReadAsStringAsync();
            var roomCode = JsonConvert.DeserializeObject<IncomingMessageDto>(resData).code;

            print(roomCode);

            // Connecting to WS
            websocket = new WebSocket("ws://127.0.0.1:8081/room/socket");

            websocket.OnOpen += async () =>
            {
                print("connected");
                var roomCodeData = new RoomCodeDto() { code = roomCode};
                var returnCode = JsonConvert.SerializeObject(roomCodeData);
                print(returnCode);
                await websocket.SendText(returnCode);
            };

            websocket.OnMessage += (bytes) =>
            {
                print("Message Received");
                if(bytes.Length == 2)
                {
                    var userId = bytes[0];
                    var action = bytes[1];
                    if (player == null)
                    {
                        return;
                    }

                    if (action == (arrowUp | keyDown))
                    {
                        player.controls.right = true;
                    }
                    else if(action == (arrowUp | keyUp))
                    {
                        player.controls.right = false;
                    }
                }
                else
                {
                    print(Encoding.UTF8.GetString(bytes));
                }
            };

            websocket.OnError += (error) =>
            {
                print(error);
            };

            await websocket.Connect();
        }
        catch (HttpRequestException e)
        {
            print(e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {
        #if !UNITY_WEBGL || UNITY_EDITOR
                if(websocket != null)
        {
            websocket.DispatchMessageQueue();
        }
        #endif
    }

    private async void OnApplicationQuit()
    {
        if(websocket != null) {
            await websocket.Close();
        }
    }
}
