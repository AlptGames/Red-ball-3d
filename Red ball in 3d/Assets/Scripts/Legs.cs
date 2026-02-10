using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : MonoBehaviour
{
   // HeroMoving heroScript;

    HeroMovingForCamera3 heroScript2;

    private HeroFront frontObject;
    // Start is called before the first frame update
    void Start()
    {
       // heroScript = GameObject.Find("Hero").GetComponent<HeroMoving>();
             heroScript2 = GameObject.Find("Hero").GetComponent<HeroMovingForCamera3>();

             frontObject = GetComponent<HeroFront>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider collider)
    {
         if(collider.isTrigger == false)
        {
            heroScript2.onGround = true;
             //frontObject.SetLadderStatus(false);
        }

    }
}
