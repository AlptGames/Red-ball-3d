using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMovingForCamera3 : MonoBehaviour
{
     Rigidbody rb;

    private float speed;

    private Vector3 direction;

    public float speedWalk = 3;

    public float speedRun = 6;

    public GameObject directionPoint; // точка ткоторую скрипт камеры утсановит для героя чтобы герой мог понять какое направление для него бдует являться передом а ккакое задом

    private GameObject heroDirectionPoint; // точка которая бюудет поворачиваться туда куда герой должен повернуться когда мы нажали на какую то кнопку

        private float rotationSpeed = 10;

        
    public bool onGround = true;

    public float jump = 10;

   
     private HeroFront frontObject;


        

     
    // Start is called before the first frame update
    void Start()
    {
        rb=GetComponent<Rigidbody>();

        heroDirectionPoint = new GameObject();
        heroDirectionPoint.name = "hero direction point";

        frontObject = FindFirstObjectByType<HeroFront>();
        if(frontObject == null)
        {
            print("error Cant find the front object");
        }

    }

    // Update is called once per frame
    void Update()
    {
        

        heroDirectionPoint.transform.position = transform.position;

      direction = new Vector3(0, 0, 0);
      speed = 0;
        if(frontObject.onTheLadder == false)
        {
             //Vector3.forward - это направление вперед в глобальное системе координат

        if(Input.GetKey(KeyCode.W))
        {
            //direction = direction + Vector3.forward; - глобальная система координат
             direction = direction + directionPoint.transform.forward;
            speed = 1;
        }

          if(Input.GetKey(KeyCode.S))
        {
          //direction = direction - Vector3.forward;
           direction = direction - directionPoint.transform.forward;
          speed = 1;
        }

            if(Input.GetKey(KeyCode.D))
        {
            //direction =  direction + Vector3.right;
            direction =  direction + directionPoint.transform.right;
            
            
            speed = 1;
        }

               if(Input.GetKey(KeyCode.A))
        {
            //direction = direction - Vector3.right;
            direction =  direction - directionPoint.transform.right;
            speed = 1;
        }

       //   rb.AddForce(Vector3.down * extraGravity,  ForceMode.Acceleration);

        if(Input.GetKeyDown(KeyCode.Space) && onGround == true)
        {
            rb.AddForce(Vector3.up * 200 * jump);
            onGround = false;
        }
        rb.AddForce(-Vector3.up * 2);

     if(frontObject.onTheLadder == true)
                {
                    if(Input.GetKeyDown(KeyCode.Space))
                    {
                        frontObject.SetLadderStatus(false);
                    }
                }


        direction = direction.normalized;

         if(Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
        {
            speed = speedRun * speed;
        }
        else
        {
            {
                speed = speedWalk * speed;
            }
        }
    // будем поворачивать героя только если какие то кнопки нажаты
    
    if(direction != new Vector3(0, 0, 0))
    {
        // transform.forward = direction -  мгновенный поворот
        heroDirectionPoint.transform.forward = direction;
        // метод RotateTowards который плавно меняет угол поворота объенкта до тех пор пока он не сравняется с нужным объектом
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, heroDirectionPoint.transform.forward, rotationSpeed * Time.deltaTime, 0);
        transform.rotation = Quaternion.LookRotation(newDirection); 
    }
       
        rb.velocity = transform.forward * speed + new Vector3(0,rb.velocity.y, 0); // устанавливаем веткор скорости
        
        }

         if(frontObject.onTheLadder == true)
        {
            direction = new Vector3(0, 0, 0);

            if(Input.GetKey(KeyCode.W))
            {
                direction = Vector3.up;
            }

             if(Input.GetKey(KeyCode.S))
            {
                direction = Vector3.down;
            }

            rb.velocity = direction * speedWalk;
        }
       
    }
}
