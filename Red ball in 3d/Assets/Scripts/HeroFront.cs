using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroFront : MonoBehaviour
{
    public bool onTheLadder = false;
    private GameObject hero;
  
    // Start is called before the first frame update
    void Start()
    {
        onTheLadder = false;
        //hero = FindFirstObjectByType<HeroMoving>().gameObject;
        hero = FindFirstObjectByType<HeroMovingForCamera3>().gameObject;

        //FindFirstObjectByType - функция ищет первый попавшийся объект на котором есть определеннвй компонент
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.GetComponent<Ladder>() != null)
        {
            SetLadderStatus(true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(col.gameObject.GetComponent<Ladder>() != null)
        {
           SetLadderStatus(false);
        }
    }

    public void SetLadderStatus(bool status)
    {
        onTheLadder = status; // менять значение onTheLAdder юудем через отдельную функцию
        hero.GetComponent<Rigidbody>().useGravity = !status;
        // !status - противоположное значение переменной статус
    }
}
