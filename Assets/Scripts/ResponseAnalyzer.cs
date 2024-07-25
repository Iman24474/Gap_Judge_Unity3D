using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseAnalyzer : MonoBehaviour
{
    CarSpawner carSpawner;
    [HideInInspector] public bool tagged;
    [HideInInspector] public bool timingInitiated;
    [HideInInspector] public bool timingEnded;
    // Start is called before the first frame update
    void Start()
    {
        carSpawner = GameObject.Find("Car Spawner").GetComponent<CarSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        TagCars();
        Debug.Log("Tagged: " + tagged);
        Debug.Log("Initiated: " + timingInitiated);
        Debug.Log("Ended: " + timingEnded);
        if (tagged && !timingInitiated && Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Time of Entry: " + LeadGapReactionTimeCalculation());
            timingInitiated = true;
        }

        if(tagged && !timingEnded && Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Time to Spare: " + TimeToSpareCalculation());
            timingEnded = true;
        }
    }

    void TagCars()
    {
        float headZ = Camera.main.transform.position.z;
        float headX = Camera.main.transform.position.x;
        // Find all game objects with the tag "ClonedCar"
        GameObject[] clonedCars = GameObject.FindGameObjectsWithTag("ClonedCar");
        // Initial distance for comparison, set to a high value
        float tailCarDist = 2000; // All cars are instantiated at a distance of 125 
        float leadCarDist = -2000;

        if(!tagged && carSpawner.rightTriggerPressed)
        {
            foreach(GameObject car in clonedCars)
            {
                float headAndCarDist = headX - car.transform.position.x;
                // Find the tail car distance from subject at the time of entering the road
                if(headAndCarDist > 0 && headAndCarDist <= tailCarDist)
                {
                    tailCarDist = headAndCarDist;
                }

                // Find the lead car distance from subject at the time of entering the road
                if(headAndCarDist <= 0 && headAndCarDist >= leadCarDist)
                {
                    leadCarDist = headAndCarDist;
                }
            }

            // Give tags to lead and tail cars
            foreach(GameObject car in clonedCars)
            {
                float headAndCarDist = headX - car.transform.position.x;
                if(headAndCarDist == tailCarDist)
                {
                    car.tag = "TailCar";
                }
                if(headAndCarDist == leadCarDist)
                {
                    car.tag = "LeadCar";
                }
            }

            // Give tags to cars that are behind the closest tail car
            foreach(GameObject car in clonedCars)
            {
                float dist = headX - car.transform.position.x;
                if(dist > tailCarDist)
                {
                    car.tag = "DestroyedCar" ;
                }
            }

            tagged = true;
        }
    }

    float LeadGapReactionTimeCalculation()
    {
        const float carSpeed = 11.176f;
        float headX = Camera.main.transform.position.x;
        GameObject leadCar = GameObject.FindWithTag("LeadCar");

        float carSize = leadCar.GetComponent<CarEntity>().carSize;
        float leadCarRearBumper = leadCar.transform.position.x - carSize / 2;
        float headToLeadDist = leadCarRearBumper - headX;
        float distTime = headToLeadDist / carSpeed;                
        return distTime;         
    }

    float TimeToSpareCalculation()
    {
        float headX = Camera.main.transform.position.x;
        const float carSpeed = 11.176f;
        GameObject tailCar = GameObject.FindWithTag("TailCar");

        float carSize = tailCar.GetComponent<CarEntity>().carSize;
        float tailCarFrontBumper = tailCar.transform.position.x + carSize / 2;
        float headToTailDist = headX - tailCarFrontBumper;
        float distTime = headToTailDist / carSpeed;
        return distTime;
    }


}

