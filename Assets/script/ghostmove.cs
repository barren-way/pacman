

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ghostmove : MonoBehaviour
{
    public GameObject[] waypointsgo;
   
    public float speed = 0.15f;
    private List<Vector3> waypoints=new List<Vector3>();
    private int index = 0;
    private Vector3 startpos;


    private void Start() 
    {
        startpos=transform.position+new Vector3(0,3,0);
        loadpath(waypointsgo[gamemanager.Instance.usingindex[GetComponent<SpriteRenderer>().sortingOrder-2]]);
    }
//移动输出位置
    private void FixedUpdate() 
    {
        if (transform.position !=waypoints[index])
        {
            Vector2 temp = Vector2.MoveTowards(transform.position,waypoints[index], speed);
            GetComponent<Rigidbody2D>().MovePosition(temp); 
        }
        else
        {
            index++;
            if(index >= waypoints.Count)
            {
                index =0;
                loadpath(waypointsgo[Random.Range(0,3)]);
            }
            
        }
        Vector2 dir = waypoints[index]-transform.position;
        GetComponent<Animator>().SetFloat("dirx",dir.x);
        GetComponent<Animator>().SetFloat("diry",dir.y);
         
    }
    private void loadpath(GameObject go)
    {
        waypoints.Clear();
         foreach(Transform t in go.transform)
        {
            waypoints.Add(t.position);
        } 
        waypoints.Insert(0,startpos);
        waypoints.Add(startpos);       
    }
  //检测碰撞  
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.name =="Pacman")
        {
            if(gamemanager.Instance.issuperpacman)
            {
                transform.position=startpos-new Vector3(0,3,0);
                index=0;
                gamemanager.Instance.score+=500;
            }
            else
            {
                collision.gameObject.SetActive(false);
                gamemanager.Instance.gamepanel.SetActive(false);
                Instantiate(gamemanager.Instance.gameoverprefab);
                Invoke("restart",3f);


            }
            
        }
    }

    private void restart()
    {
        SceneManager.LoadScene(0);

    }

}
