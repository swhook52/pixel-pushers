using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Game : MonoBehaviour
{
    public float holep;
    public int w, h, x, y;
    public bool[,] hwalls, vwalls;
    public Transform Level, Player, Goal;
    public GameObject Floor, Wall;
    public CinemachineVirtualCamera cam;
    public Vector3 tempScale;
    List<string> floors = new List<string>();
    Animator anim;

    void Start()
    {

        if(floors.Count > 0){
            floors.Clear();
        }

        foreach (Transform child in Level)
            Destroy(child.gameObject);

        hwalls = new bool[w + 1, h];
        vwalls = new bool[w, h + 1];
        var st = new int[w, h];

        void dfs(int x, int y)
        {
            st[x, y] = 1;

            if(!floors.Contains(x+"|"+y)){
                floors.Add(x+"|"+y);
            }

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

        foreach (var floor in floors)
        {
            var floorX = float.Parse(floor.Split('|')[0]);
            var floorY = float.Parse(floor.Split('|')[1]);
            Instantiate(Floor, new Vector3(floorX, floorY), Quaternion.identity, Level);
        }

        foreach (Transform child in Level) {

            if (child.gameObject.name.Contains("wall")) { 
                //var tempPos = child.transform.position;
                //tempPos.z = 1;
                //child.transform.position = tempPos;
                child.transform.SetSiblingIndex(1);
            }

            if (child.gameObject.name.Contains("floor")) { 
                //var tempPos = child.transform.position;
                //tempPos.z = 0;
                //child.transform.position = tempPos;
                child.transform.SetSiblingIndex(2);
            }

        }

        x = Random.Range(0, w);
        y = Random.Range(0, h);
        Player.position = new Vector3(x, y);
        do Goal.position = new Vector3(Random.Range(0, w), Random.Range(0, h));
        while (Vector3.Distance(Player.position, Goal.position) < (w + h) / 4);
        cam.m_Lens.OrthographicSize = Mathf.Pow(w / 3 + h / 2, 0.7f) + 1;
    }

    void Update()
    {
        anim = Player.GetComponent<Animator>();

        var dirs = new[]
        {
            (x - 1, y, hwalls, x, y, Vector3.right, 90, KeyCode.A),
            (x + 1, y, hwalls, x + 1, y, Vector3.right, 90, KeyCode.D),
            (x, y - 1, vwalls, x, y, Vector3.up, 0, KeyCode.S),
            (x, y + 1, vwalls, x, y + 1, Vector3.up, 0, KeyCode.W),
        };
        var rand = Random.value;
        foreach (var (nx, ny, wall, wx, wy, sh, ang, k) in dirs.OrderBy(d => rand))
            if (Input.GetKey(k))
                if (wall[wx, wy]){
                    Player.position = Vector3.Lerp(Player.position, new Vector3(nx, ny), 0.1f);
                } else {(x, y) = (nx, ny); }

        if(Input.GetKeyDown(KeyCode.A)){
            tempScale = Player.transform.localScale;
            tempScale.x = -1;
            Player.transform.localScale = tempScale;
        }

        if(Input.GetKeyDown(KeyCode.D)){
            tempScale = Player.transform.localScale;
            tempScale.x = 1;
            Player.transform.localScale = tempScale;
        }

        // START animatin / key pressed
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)){
            anim.SetBool("isWalking", true);
        } else {
            anim.SetBool("isWalking", false);
        }

        if (Input.GetKey(KeyCode.Mouse0)){
            anim.SetBool("isShooting", true);
        } else {
            anim.SetBool("isShooting", false);
        }
        // END animatin / key pressed

        //Player.position = Vector3.Lerp(Player.position, new Vector3(x, y), Time.deltaTime * 12);
        if (Vector3.Distance(Player.position, Goal.position) < 0.12f && Input.GetKeyDown(KeyCode.F))
        {
            if (Random.Range(0, 5) < 3) w++;
            else h++;
            Start();
        }
    }
}
