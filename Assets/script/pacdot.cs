
using UnityEngine;

public class pacdot : MonoBehaviour
{
    public bool isSuperPacdot= false;
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.name =="Pacman")
        {
            if(isSuperPacdot)
            {
                gamemanager.Instance.oneatpacdot(gameObject);
                gamemanager.Instance.oneatsuperpacdot();
                Destroy(gameObject);
            }
            else
            {
                gamemanager.Instance.oneatpacdot(gameObject);
                Destroy(gameObject);
            }
            
        }
    }
}
