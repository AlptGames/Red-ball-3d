using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera3 : MonoBehaviour
{

    public GameObject hero;
    private GameObject heroHead;
    public float distanceToHero = 4;

    public float sensivityHorizontal = 3;

    public float sensivityVertical = 3;

    float cameraYRotation;

    float maxVertical = 45;
    float minVertical = -50;
    float cameraXRotation;
    private Vector3 startPosition;
    public float offsetX = 0;
    public float offsetY = 2;
    public float offsetZ = -5;

    private float speed = 15;

    private GameObject cameraDirectionPoint; // тояка которачя бдует направлена вперед тоносительно камеры,но паралельно земле

    // Start is called before the first frame update
    void Start()
    {
      SetUpCamera();

    }

    public void SetUpCamera() //функция для настройки местоположения и поворота камеры от 3 лиц
    {
      if(heroHead == null)
      {
           heroHead = new GameObject();
           heroHead.name = "New Hero Head";

             cameraDirectionPoint = new GameObject();
        cameraDirectionPoint.name = "Camera direction point";

      }

       
        heroHead.transform.forward = hero.transform.forward;
       
        transform.position = heroHead.transform.position;
        transform.parent = heroHead.transform;
        transform.forward = heroHead.transform.forward;
        transform.position -= transform.forward * distanceToHero;

           Cursor.lockState = CursorLockMode.Locked;
       Cursor.visible = false; 

      // heroHead.transform.parent = null; // отвязали оюъект головы от героя

      //убедимся что у героя есть нужный скрипт
      if(hero.GetComponent<HeroMovingForCamera3>() != null)
      {
        hero.GetComponent<HeroMovingForCamera3>().directionPoint = cameraDirectionPoint;
      }
    }

    // Update is called once per frame
    void Update()
    {

        heroHead.transform.position = hero.transform.position + new Vector3(0, 0.5f, 0);

        cameraDirectionPoint.transform.position = transform.position; // таскаем эту точку за камерой

        float deltaX;
             deltaX = Input.GetAxis("Mouse X"); 
             deltaX = deltaX * Time.deltaTime;
             deltaX = deltaX * sensivityHorizontal * 100;

             cameraYRotation = heroHead.transform.localEulerAngles.y + deltaX;
             //hero.transform.localEulerAngles = new Vector3(0, heroYRotation, 0);

               float deltaY;
             deltaY = Input.GetAxis("Mouse Y"); 
             deltaY = deltaY * Time.deltaTime;
             deltaY = deltaY * sensivityVertical * 100;

             cameraXRotation = cameraXRotation - deltaY;
             cameraXRotation = Mathf.Clamp(cameraXRotation, minVertical, maxVertical);


            // heroHead.transform.localRotation = Quaternion.Euler(cameraXRotation, 0, 0);

            heroHead.transform.localEulerAngles = new Vector3(cameraXRotation, cameraYRotation, 0);

            cameraDirectionPoint.transform.localEulerAngles = new Vector3(0, cameraYRotation, 0);
            // бросим луч из головы героя к камере героя

            RaycastHit hit; 
             if(Physics.Linecast(heroHead.transform.position, transform.position, out hit) == true) 
             {
                transform.position += transform.forward * Time.deltaTime * speed;

             }
             else 
             {

                if(Physics.Linecast(heroHead.transform.position, transform.position-transform.forward*0.5f, out hit) == false) // убрали дрожание камеры
                {
                       //Vecto3.Distance - вычисление расстояния  между 2 объектами
                if(Vector3.Distance(heroHead.transform.position, transform.position) < distanceToHero)
                {
                    transform.position -= transform.forward * Time.deltaTime * speed;
                }
                }

             
             }
    }
}
