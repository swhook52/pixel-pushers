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
    public GameObject GameOverPanel;

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

        // Spawn item and return their spawn location, 'avoids' spawn overlap
        List<Transform> avoids = AddEnemies();
                     // avoids = AddWeapon(avoids);
                        avoids = AddHealth(avoids);

        var randGoalPos = getRandPosition(avoids);
        Goal.position = (randGoalPos == Vector3.zero) ? new Vector3(1, 1) : randGoalPos;

        fixZPositions();
    }

    void Awake() {
        Instance = this;
        keypad = GetComponent<PanelScript>();
        keypad.SetInactive();
    }

    public void restartMase() {
        w = 6;
        h = 6;
        playerController.currentHealth = 25;
        GenerateMaze();
        GameOverPanel.SetActive(false);
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
        var numOfEnemies = 0;
        if(lvlCount <= 6){
            numOfEnemies = Random.Range(2, 7);
        }

        if(lvlCount > 6){
            numOfEnemies = Random.Range(7, 12);
        }

        if(lvlCount > 9){
            numOfEnemies = Random.Range(12, 17);
        }
        List<Transform> avoids = new List<Transform> { Player, Goal };
        while (numOfEnemies > 0)
        {
            var nmeV3 = getRandPosition(avoids, 1);
            if(nmeV3 != Vector3.zero){
                var newNme = Instantiate(Enemy, nmeV3, Quaternion.identity, Level);
                avoids.Add(newNme.transform);
            }
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

        if(lvlCount <= 5){
            numOfHealth = Random.Range(0, 1);
        }

        if(lvlCount > 5){
            numOfHealth = Random.Range(0, 2);
        }

        if(lvlCount > 10){
            numOfHealth = Random.Range(0, 3);
        }

        for (int i = 0; i <= numOfHealth; i++)
        {
            var healthV3 = getRandPosition(avoids);
            if(healthV3 != Vector3.zero){
                var newHealth = Instantiate(Health, healthV3, Quaternion.identity, Level);
                avoids.Add(newHealth.transform);
            }
            
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

        return killswitch > 0 ? new Vector3(randX, randY) : Vector3.zero; //if no spawn in available, don't spawn

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
