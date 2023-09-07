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
using TMPro;
using UnityEngine.SceneManagement;

public class PlayersController : MonoBehaviour
{
    [SerializeField]
    List<Player> playerPrefab;

    [SerializeField]
    TextMeshProUGUI codeText;

    bool wasStarted = false;

    public List<Player> players = new();
    public static PlayersController instance;

    // Start is called before the first frame update
    async void Start()
    {
        //var gameData = new GameDataDto() { gui = "CrossArrows", max_players = 6};
        //var serializedData = JsonConvert.SerializeObject(gameData);
        //var data = new StringContent(serializedData);
        //try
        //{
        //    // Getting room code from the server
        //    var res = await client.PostAsync("http://localhost:8081/create", data);
        //    var resData = await res.Content.ReadAsStringAsync();
        //    var roomCode = JsonConvert.DeserializeObject<IncomingMessageDto>(resData).code;

        //    print(roomCode);

        //    codeText.text = "Code: " + roomCode;

        //    // Connecting to WS
        //    websocket = new WebSocket("ws://127.0.0.1:8081/room/socket");

        //    websocket.OnOpen += async () =>
        //    {
        //        print("connected");
        //        var roomCodeData = new RoomCodeDto() { code = roomCode};
        //        var returnCode = JsonConvert.SerializeObject(roomCodeData);
        //        print(returnCode);
        //        await websocket.SendText(returnCode);
        //    };

        //    websocket.OnMessage += (bytes) =>
        //    {
        //        print("Message Received");
        //        if(bytes.Length == 2)
        //        {
        //            var userId = bytes[0];
        //            var action = bytes[1];

        //            var player = players.Find(player => player.id == userId);

        //            if (playerPrefab == null || player == null)
        //            {
        //                return;
        //            }

        //            // UP movement
        //            if (action == (arrowUp | keyDown))
        //            {
        //                player.controls.top = true;
        //            }
        //            else if(action == (arrowUp | keyUp))
        //            {
        //                player.controls.top = false;
        //            }

        //            // Bottom movement
        //            if (action == (arrowDown | keyDown))
        //            {
        //                player.controls.bottom = true;
        //            }
        //            else if (action == (arrowDown | keyUp))
        //            {
        //                player.controls.bottom = false;
        //            }

        //            // Left movement
        //            if (action == (arrowLeft | keyDown))
        //            {
        //                player.controls.left = true;
        //            }
        //            else if (action == (arrowLeft | keyUp))
        //            {
        //                player.controls.left = false;
        //            }

        //            // Right movement
        //            if (action == (arrowRight | keyDown))
        //            {
        //                player.controls.right = true;
        //            }
        //            else if (action == (arrowRight | keyUp))
        //            {
        //                player.controls.right = false;
        //            }
        //        }
        //        else
        //        {
        //            var wsEvent = JsonConvert.DeserializeObject<EventDto>(Encoding.UTF8.GetString(bytes));
        //            print(wsEvent.event_name);
        //            if(wsEvent.event_name == "player_added")
        //            {
        //                int randomIndex = UnityEngine.Random.Range(0, 3);
        //                var addedPlayer = Instantiate(playerPrefab[randomIndex], Vector3.zero, Quaternion.identity);
        //                addedPlayer.id = wsEvent.id;
        //                addedPlayer.userName = wsEvent.nickname;
        //                addedPlayer.GetComponentInChildren<TextMeshProUGUI>().text = addedPlayer.userName;
        //                players.Add(addedPlayer);
        //            }
        //            else if(wsEvent.event_name == "player_removed")
        //            {
        //                var player = players.Find(player => player.id == wsEvent.id);
        //                Destroy(player.gameObject);
        //                players.Remove(player);
        //            }
        //        }
        //    };

        //    websocket.OnError += (error) =>
        //    {
        //        print(error);
        //    };

        //    await websocket.Connect();
        //}
        //catch (HttpRequestException e)
        //{
        //    print(e.Message);
        //}
        Joystick.onError += (error) =>
        {
            print(error);
        };

        Joystick.onPlayerRemoved += (id, nickname) =>
        {
            var player = players.Find(player => player.id == id);
            Destroy(player.gameObject);
            players.Remove(player);
        };

        Joystick.onPlayerJoined += (id, nickname) =>
        {
            int randomIndex = UnityEngine.Random.Range(0, 3);
            var addedPlayer = Instantiate(playerPrefab[randomIndex], Vector3.zero, Quaternion.identity);
            addedPlayer.id = id;
            addedPlayer.userName = nickname;
            addedPlayer.GetComponentInChildren<TextMeshProUGUI>().text = addedPlayer.userName;
            players.Add(addedPlayer);
        };

        Joystick.onCodeAcquired += (code) => {
            codeText.text = "Code: " + code;
            print(code);
        };

        await Joystick.Begin(new JoystickConfig() { 
            gui = "CrossArrows", 
            domain = "server-spxv4kchhq-lm.a.run.app", 
            isSecure = true, 
            maxPlayers = 6, 
            port = "" });
    }

    // Update is called once per frame
    void Update()
    {
        Joystick.Update();
    }

    private async void OnApplicationQuit()
    {
        await Joystick.GameOver();
    }

    IEnumerator FinishGame()
    {
        yield return new WaitForSecondsRealtime(180);
        players.Sort((p1, p2) =>
        {
            return p1.score.CompareTo(p2.score);
        });
        if(players.Count== 0)
        {
            PlayerPrefs.SetString("Winner", "None");
        }
        else
        {
            PlayerPrefs.SetString("Winner", players[0].userName);
        }
        SceneManager.LoadScene("Scores");
    }


    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
}
