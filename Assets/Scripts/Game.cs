using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance { get; private set; }
    public float holep;
    public int w, h, x, y, lvlCount;
    public bool[,] hwalls, vwalls;
    public Transform Level;
    public Transform Player;
    public Transform Goal;
    public Transform Enemy;
    public Transform Weapon;
    public Transform Health;
    public GameObject Floor, Wall;
    public CinemachineVirtualCamera cam;
    Animator anim;
    private PanelScript keypad;
    public PlayerController playerController;

    void Start()
    {
        lvlCount++;

        foreach (Transform child in Level)
            Destroy(child.gameObject);

        hwalls = new bool[w + 1, h];
        vwalls = new bool[w, h + 1];
        var st = new int[w, h];

        void dfs(int x, int y)
        {
            st[x, y] = 1;
            Instantiate(Floor, new Vector3(x, y), Quaternion.identity, Level);

            var dirs = new[]
            {
                (x - 1, y, hwalls, x, y, Vector3.right, 90, KeyCode.A),
                (x + 1, y, hwalls, x + 1, y, Vector3.right, 90, KeyCode.D),
                (x, y - 1, vwalls, x, y, Vector3.up, 0, KeyCode.S),
                (x, y + 1, vwalls, x, y + 1, Vector3.up, 0, KeyCode.W),
            };
            foreach (var (nx, ny, wall, wx, wy, sh, ang, k) in dirs.OrderBy(d => Random.value))
                if (!(0 <= nx && nx < w && 0 <= ny && ny < h) || (st[nx, ny] == 2 && Random.value > holep))
                {
                    wall[wx, wy] = true;
                    Instantiate(Wall, new Vector3(wx, wy) - sh / 2, Quaternion.Euler(0, 0, ang), Level);
                }
                else if (st[nx, ny] == 0) dfs(nx, ny);
            st[x, y] = 2;
        }
        dfs(0, 0);

        // force walls over floors
        foreach (Transform child in Level) {
            var tempPos = child.transform.position;
            if(child.name.Contains("wall")){
                tempPos.z = -1;
            }
            if(child.name.Contains("floor")){
                tempPos.z = 0;
            }
            child.transform.position = tempPos;
        }

        x = Random.Range(0, w);
        y = Random.Range(0, h);
        Goal.position = new Vector3(Random.Range(0, w), Random.Range(0, h));

        // Spawn item and return their spawn location, 'avoids' spawn overlap
        List<Transform> avoids = AddEnemies();
                     // avoids = AddWeapon(avoids);
                        avoids = AddHealth(avoids);

        fixZPositions();
    }

    void Awake() {
        Instance = this;
        keypad = GetComponent<PanelScript>();
        keypad.SetInactive();
    }

    public void GenerateMaze()
    {
        if (Random.Range(0, 5) < 3)
        {
            w++;
        }
        else
        {
            h++;
        }
        Start();
    }

    public void DisplayKeypad() {
        keypad.SetActive();
    }

    public void RemoveKeypad() {
        keypad.SetInactive();
    }

    void Update()
    {

        var dirs = new[]
        {
            (x - 1, y, hwalls, x, y, Vector3.right, 90, KeyCode.A),
            (x + 1, y, hwalls, x + 1, y, Vector3.right, 90, KeyCode.D),
            (x, y - 1, vwalls, x, y, Vector3.up, 0, KeyCode.S),
            (x, y + 1, vwalls, x, y + 1, Vector3.up, 0, KeyCode.W),
        };
        //var rand = Random.value;
        //foreach (var (nx, ny, wall, wx, wy, sh, ang, k) in dirs.OrderBy(d => rand)){
        //    if (Input.GetKeyDown(k)){
        //        if (wall[wx, wy]){
        //            //Player.position = Vector3.Lerp(Player.position, new Vector3(nx, ny), 0.1f);
        //        } else {
        //            (x, y) = (nx, ny);
        //        }
        //    }
        //}

        //Player.position = Vector3.Lerp(Player.position, new Vector3(x, y), Time.deltaTime * 12);
        //if (Vector3.Distance(Player.position, Goal.position) < 0.12f && Input.GetKeyDown(KeyCode.F))
        //{
        //}
    }

    void fixZPositions()
    {
        // force objects over floors
        foreach (Transform child in Level)
        {
            var tempPos = child.transform.position;
            if (child.name.Contains("floor"))
            {
                tempPos.z = 1;
            }
            child.transform.position = tempPos;
        }
    }

    List<Transform> AddEnemies()
    {
        var nmeMin = lvlCount / 5 > 1 ? lvlCount / 5 : 1;
        var nmeMax = (h * w) / 4;
        var numOfEnemies = Random.Range(nmeMin, nmeMax);
        List<Transform> avoids = new List<Transform> { Player, Goal };
        while (numOfEnemies > 0)
        {
            var nmeV3 = getRandPosition(avoids, 1);
            var newNme = Instantiate(Enemy, nmeV3, Quaternion.identity, Level);
            avoids.Add(newNme.transform);
            numOfEnemies--;
        }
        return avoids;
    }

    List<Transform> AddWeapon(List<Transform> avoids)
    {
        if(lvlCount == 1){
            var weaponV3 = getRandPosition(avoids);
            var newWeapon = Instantiate(Weapon, weaponV3, Quaternion.identity, Level);
            avoids.Add(newWeapon.transform);
        }
        return avoids;
    }

    List<Transform> AddHealth(List<Transform> avoids)
    {
        var numOfHealth = 0;

        if(lvlCount > 4){
            numOfHealth = Random.Range(0, 1);
        }

        if(lvlCount > 9){
            numOfHealth = Random.Range(0, 2);
        }

        if(lvlCount > 11){
            numOfHealth = Random.Range(0, 3);
        }

        for (int i = 0; i <= numOfHealth; i++)
        {
            var healthV3 = getRandPosition(avoids);
            var newHealth = Instantiate(Health, healthV3, Quaternion.identity, Level);
            avoids.Add(newHealth.transform);
        }

        return avoids;
    }

    public void RemoveHealth(int dmg) {
        playerController.RemoveHealth(dmg);
    }

    Vector3 getRandPosition(List<Transform> avoidObjects, int variance = 0)
    {

        var randX = 0;
        var randY = 0;
        var killswitch = 1000; // max spawn search attempts

        do
        {
            randX = Random.Range(0, w);
            randY = Random.Range(0, h);
            killswitch--;
        } while (!canSpawn(randX, randY, avoidObjects, variance) && killswitch > 0);

        return killswitch > 0 ? new Vector3(randX, randY) : new Vector3(1, 1); //if no spawn in available, spawn at 1x1

    }

    bool canSpawn(int testX, int testY, List<Transform> avoidObjects, int variance)
    {

        bool canSpawn = true;

        foreach (var item in avoidObjects)
        {
            if (
                item.position.x == testX && item.position.y == testY ||
                item.position.x == testX + variance && item.position.y == testY ||
                item.position.x == testX - variance && item.position.y == testY ||
                item.position.x == testX && item.position.y == testY + variance ||
                item.position.x == testX && item.position.y == testY - variance
            )
            {
                canSpawn = false;
                break;
            }
        }

        return canSpawn;

    }
}
