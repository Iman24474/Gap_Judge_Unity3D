using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    const float speed = 11.176f;
    const float endPoint = 125f;
    const float midPoint = 0f;
    CarSpawner carSpawner;
    ResponseAnalyzer responseAnalyzer;
    // Start is called before the first frame update
    void Start()
    {
        carSpawner = GameObject.Find("Car Spawner").GetComponent<CarSpawner>();
        responseAnalyzer = GameObject.Find("XR Origin (XR Rig)").GetComponent<ResponseAnalyzer>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += Vector3.right * speed * Time.deltaTime; 

        if(gameObject.transform.position.x >= endPoint)
        {
            Destroy(gameObject);
        }

        else if(gameObject.tag == "DestroyedCar")
        {
            Destroy(gameObject);
        }

        else if(gameObject.tag == "TailCar" && gameObject.transform.position.x >= midPoint)
        {
            gameObject.tag = "ClonedCar";
            responseAnalyzer.timingEnded = false;
            responseAnalyzer.timingInitiated = false;
            responseAnalyzer.tagged = false;
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

    }
}
