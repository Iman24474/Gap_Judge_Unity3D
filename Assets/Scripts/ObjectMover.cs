using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    const float startPoint = -125f;
    const float speed = 11.176f;
    const float endPoint = 125f;
    const float midPoint = 0f;
    float waitTime;
    float carXPos;
    float carStatus;
    float btnStatus;
    bool passedCenterPoint;
    bool dataRecording = true;
    Color color1;
    Color color2;
    CarEntity carEntity;
    CarSpawner carSpawner;
    ResponseAnalyzer responseAnalyzer;
    List<float> CarRecord = new List<float>();
    // Start is called before the first frame update
    void Start()
    {
        carSpawner = GameObject.Find("Car Spawner").GetComponent<CarSpawner>();
        responseAnalyzer = GameObject.Find("XR Origin (XR Rig)").GetComponent<ResponseAnalyzer>();
        carEntity = GetComponent<CarEntity>();
        color1 = GetComponent<MeshRenderer>().materials[0].color;
        color2 = GetComponent<MeshRenderer>().materials[1].color;
        waitTime = carSpawner.waitTime;
        responseAnalyzer.gapsGeneratedActual.Add(carSpawner.elapsedTime);
        Debug.Log("Gap: " + waitTime);
    }

    // Update is called once per frame
    void Update()
    {
         
        carXPos = gameObject.transform.position.x;

        if(gameObject.tag != "TailCar" && carXPos >= midPoint && !passedCenterPoint)
        {
            responseAnalyzer.gapsGeneratedRounded.Add(waitTime);
            passedCenterPoint = true;
        }

        // Car data recording
        if(carXPos == startPoint)
        {
            CreatedCar();
        }

        else if(gameObject.tag == "DestroyedCar" && dataRecording)
        {
            DestroyCarAfterBtnPress();
            dataRecording = false;
        }

        else if(carXPos < endPoint)
        {
            MovingCar();
        }

        else if(dataRecording)
        {
            DestroyCar();
            dataRecording = false;
        }

        if(gameObject.tag == "TailCar" && carXPos >= midPoint)
        {
            responseAnalyzer.timingEnded = false;
            responseAnalyzer.timingInitiated = false;
            responseAnalyzer.tagged = false;
            carSpawner.rightTriggerPressed = false;
        }

        if(carSpawner.trialNum == carSpawner.trialCount)
        {
            carSpawner.newTrial = false;
        }
        else
        {
            // start a new trial
            carSpawner.newTrial = true;
        }

        // Move the gameobject forward along the x-axis
        gameObject.transform.position += Vector3.right * speed * Time.deltaTime;

    }


    void CreatedCar()
    {
        btnStatus = responseAnalyzer.btnState;
        carStatus = 1; // Car is created
        float pink = 0;

        if(color1 == Color.magenta || color2 == Color.magenta) 
        {
            pink = 1;
        }

        CarRecord = new List<float>() 
        {
            carStatus, // Car status
            carEntity.entityID, // Car Id
            carEntity.carSize, // Car Size
            startPoint, // Car X-Pos
            waitTime, // Car gap size
            pink, // is the car pink?
            btnStatus // controller btn status
        };

        if(!carSpawner.CarInfo.ContainsKey(carSpawner.elapsedTime))
        {
            carSpawner.CarInfo.Add(carSpawner.elapsedTime, new List<List<float>> {CarRecord});
        }
        else
        {
            carSpawner.CarInfo[carSpawner.elapsedTime].Add(CarRecord);
        }
        
    }


    void MovingCar()
    {
        btnStatus = responseAnalyzer.btnState;
        carStatus = 2; // Car is moving
        float pink = 0;

        if(color1 == Color.magenta || color2 == Color.magenta) 
        {
            pink = 1;
        }

        // Car data recording
        CarRecord = new List<float>() 
        {
            carStatus, // Car status
            carEntity.entityID, // Car Id
            carEntity.carSize, // Car Size
            carXPos, // Car X-Pos
            waitTime, // Car gap size
            pink, // is the car pink?
            btnStatus // controller btn status
        };

        if(!carSpawner.CarInfo.ContainsKey(carSpawner.elapsedTime))
        {
            carSpawner.CarInfo.Add(carSpawner.elapsedTime, new List<List<float>> {CarRecord});
        }
        else
        {
            carSpawner.CarInfo[carSpawner.elapsedTime].Add(CarRecord);
        }

    }


    void DestroyCarAfterBtnPress()
    {
        btnStatus = responseAnalyzer.btnState;
        carStatus = 3; // Car is destroyed
        float pink = 0;

        if(color1 == Color.magenta || color2 == Color.magenta) 
        {
            pink = 1;
        }

        // Car data recording
        CarRecord = new List<float>() 
        {
            carStatus, // Car status
            carEntity.entityID, // Car Id
            carEntity.carSize, // Car Size
            carXPos, // Car X-Pos
            waitTime, // Car gap size
            pink, // is the car pink?
            btnStatus // controller btn status
        };

        if(!carSpawner.CarInfo.ContainsKey(carSpawner.elapsedTime))
        {
            carSpawner.CarInfo.Add(carSpawner.elapsedTime, new List<List<float>> {CarRecord});
        }
        else
        {
            carSpawner.CarInfo[carSpawner.elapsedTime].Add(CarRecord);
        }


        Destroy(gameObject);

    }

    void DestroyCar()
    {
        btnStatus = responseAnalyzer.btnState;
        carStatus = 3; // Car is destroyed
        float pink = 0;

        if(color1 == Color.magenta || color2 == Color.magenta) 
        {
            pink = 1;
        }

        // Car data recording
        CarRecord = new List<float>() 
        {
            carStatus, // Car status
            carEntity.entityID, // Car Id
            carEntity.carSize, // Car Size
            endPoint, // Car X-Pos
            waitTime, // Car gap size
            pink, // is the car pink?
            btnStatus // controller btn status
        };

        carSpawner.CarInfo.Add(carSpawner.elapsedTime, new List<List<float>> {CarRecord});

        // Once the car reaches the endpoint, it will be destroyed
        if(carXPos >= endPoint)
        {
            Destroy(gameObject);
        }
    }



}
