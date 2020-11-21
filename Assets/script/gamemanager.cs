


using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class gamemanager : MonoBehaviour
{
    private static gamemanager _instance;

    public static gamemanager Instance
    {
        get
        {
            return _instance;
        }
    }

    public GameObject pacman;
    public GameObject blink;
    public GameObject clyde;
    public GameObject inky;
    public GameObject pinky;

    public GameObject startpanel;
    public GameObject gamepanel;

    public GameObject startcountdownprefab;
    public GameObject gameoverprefab;
    public GameObject winprefab;
    public AudioClip startclip;
    public Text remainText;
    public Text nowText;
    public Text scoreText;

    private int pacdotNum = 0;
    private int nowEat = 0;
    public int score = 0;

    public bool issuperpacman=false;

    public List<int> usingindex = new List<int>();
    public List<int> rawindex = new List<int>{0,1,2,3};

    private List<GameObject> pacdotgos = new List<GameObject>();

    private void Awake() 
    {
         _instance=this;
         Screen.SetResolution(1024,768,false);
         int tempcount=rawindex.Count;
         for (int i = 0; i < tempcount; i++)
         {
             int tempindex = Random.Range(0,rawindex.Count);
             usingindex.Add(rawindex[tempindex]);
             rawindex.RemoveAt(tempindex);
         }
         foreach (Transform t in GameObject.Find("Maze").transform)
         {
             pacdotgos.Add(t.gameObject);
         }
         pacdotNum=GameObject.Find("Maze").transform.childCount;   
    }

    private void Start() 
    {
        setGameState(false);
        
    }
    private void Update() 
    {
        if (nowEat==pacdotNum&&pacman.GetComponent<move>().enabled !=false)
        {
            gamepanel.SetActive(false);
            Instantiate(winprefab);
            StopAllCoroutines();
            setGameState(false);
        }
        if (nowEat==pacdotNum)
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(0);
            }
        }
        
        if (gamepanel.activeInHierarchy)
        {
            remainText.text = "Remain:\n\n"+(pacdotNum-nowEat);
            nowText.text = "Eaten:\n\n"+nowEat;
            scoreText.text = "Score:\n\n"+score;

        }    
    }
    public void onStartButton()
    {
        StartCoroutine(playStartCountDown());
        AudioSource.PlayClipAtPoint(startclip,new Vector3(0,0,-5));
        startpanel.SetActive(false);
    }
    public void onExitButton()
    {
        Application.Quit();
    }
    
    IEnumerator playStartCountDown()
    {
        GameObject go = Instantiate(startcountdownprefab);
        yield return new WaitForSeconds(4f);
        Destroy(go);
        setGameState(true);
        Invoke("creatSuperPacdot",1f);
        gamepanel.SetActive(true);
        GetComponent<AudioSource>().Play();

    }
   
    public void oneatpacdot(GameObject go)
    {
        nowEat++;
        score +=100;
        pacdotgos.Remove(go);
    }

    public void oneatsuperpacdot()
    {
        score+=200;
        Invoke("creatSuperPacdot",10f);
        issuperpacman=true;
        Freezeenemy();
        StartCoroutine(RecoveryEnemy());
    }

    IEnumerator RecoveryEnemy()
    {
        yield return new WaitForSeconds(3f);
        disFreezeenemy();
        issuperpacman=false;
        

    }
    

    private void creatSuperPacdot()
    {
        if (pacdotgos.Count<10)
        {
            return;
        }
        int tempIndex = Random.Range(0,pacdotgos.Count);
        pacdotgos[tempIndex].transform.localScale = new Vector3(3,3,3);
        pacdotgos[tempIndex].GetComponent<pacdot>().isSuperPacdot = true;
    }
    private void Freezeenemy()
    {
        blink.GetComponent<ghostmove>().enabled=false;
        clyde.GetComponent<ghostmove>().enabled=false;
        inky.GetComponent<ghostmove>().enabled=false;
        pinky.GetComponent<ghostmove>().enabled=false;
        blink.GetComponent<SpriteRenderer>().color=new Color(0.7f,0.7f,0.7f,0.7f);
        clyde.GetComponent<SpriteRenderer>().color=new Color(0.7f,0.7f,0.7f,0.7f);
        inky.GetComponent<SpriteRenderer>().color=new Color(0.7f,0.7f,0.7f,0.7f);
        pinky.GetComponent<SpriteRenderer>().color=new Color(0.7f,0.7f,0.7f,0.7f);

    }
    private void disFreezeenemy()
    {
        blink.GetComponent<ghostmove>().enabled=true;
        clyde.GetComponent<ghostmove>().enabled=true;
        inky.GetComponent<ghostmove>().enabled=true;
        pinky.GetComponent<ghostmove>().enabled=true;
        blink.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f,1f);
        clyde.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f,1f);
        inky.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f,1f);
        pinky.GetComponent<SpriteRenderer>().color=new Color(1f,1f,1f,1f);

    }
    private void setGameState(bool state)
    {
        pacman.GetComponent<move>().enabled=state;
        blink.GetComponent<ghostmove>().enabled=state;
        clyde.GetComponent<ghostmove>().enabled=state;
        inky.GetComponent<ghostmove>().enabled=state;
        pinky.GetComponent<ghostmove>().enabled=state;
    }
}
