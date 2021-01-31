using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
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

        AddEnemies();
        fixZPositions();
    }

    void Awake() {
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

        //UpdateCharacterAnimation(Player.GetComponent<Animator>());

        //Player.position = Vector3.Lerp(Player.position, new Vector3(x, y), Time.deltaTime * 12);
        //if (Vector3.Distance(Player.position, Goal.position) < 0.12f && Input.GetKeyDown(KeyCode.F))
        //{
        //}
    }

    //void UpdateCharacterAnimation(Animator anim) {

    //    //flip char horizontal if left/right direction
    //    Vector3 tempScale = Player.transform.localScale;
    //    if(Input.GetKeyDown(KeyCode.A)){ tempScale.x = -1; }
    //    if(Input.GetKeyDown(KeyCode.D)){ tempScale.x = 1; }
    //    Player.transform.localScale = tempScale;

    //    //Update animation on key press
    //    anim.SetBool("isWalking", (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)));
    //    anim.SetBool("isShooting", Input.GetKey(KeyCode.Mouse0));

    //}

    void fixZPositions()
    {
        // force objects over floors
        foreach (Transform child in Level)
        {
            var tempPos = child.transform.position;
            if (child.name.Contains("wall") || child.name.Contains("enemy"))
            {
                tempPos.z = -1;
            }
            if (child.name.Contains("floor"))
            {
                tempPos.z = 0;
            }
            child.transform.position = tempPos;
        }
    }

    void AddEnemies()
    {
        var nmeMin = lvlCount / 5 > 1 ? lvlCount / 5 : 1;
        var nmeMax = (h * w) / 4;
        var numOfEnemies = Random.Range(nmeMin, nmeMax);
        List<Transform> avoids = new List<Transform> { Player, Goal };
        while (numOfEnemies > 0)
        {
            var nmeV3 = getRandPosition(avoids);
            var newNme = Instantiate(Enemy, nmeV3, Quaternion.identity, Level);
            avoids.Add(newNme.transform);
            numOfEnemies--;
        }
    }

    void AddWeapon(List<Transform> avoids)
    {
        if(lvlCount == 1)
        {
            var nmeV3 = getRandPosition(avoids);
            var newNme = Instantiate(Weapon, nmeV3, Quaternion.identity, Level);
            avoids.Add(newNme.transform);
        }
    }

    void AddHealth(List<Transform> avoids)
    {
        if (lvlCount == 1)
        {
            var nmeV3 = getRandPosition(avoids);
            var newNme = Instantiate(Health, nmeV3, Quaternion.identity, Level);
            avoids.Add(newNme.transform);
        }
    }

    Vector3 getRandPosition(List<Transform> avoidObjects)
    {

        var randX = 0;
        var randY = 0;

        do
        {
            randX = Random.Range(0, w);
            randY = Random.Range(0, h);
        } while (!canSpawn(randX, randY, avoidObjects));

        return new Vector3(randX, randY);

    }

    bool canSpawn(int testX, int testY, List<Transform> avoidObjects)
    {

        bool canSpawn = true;
        int variance = 1;

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
