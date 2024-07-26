using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResponseAnalyzer : MonoBehaviour
{
    float leadGapRT;
    float timeToSpare;
    float btnPressed;
    float btnReleased;
    CarSpawner carSpawner;
    [HideInInspector] public bool tagged;
    [HideInInspector] public bool timingInitiated;
    [HideInInspector] public bool timingEnded;
    [HideInInspector] public List<float> gapsGeneratedRounded = new List<float>();   
    [HideInInspector] public List<float> gapsGeneratedActual = new List<float>();  
    [HideInInspector] public Dictionary<float, List<float>> ResponseAnalysis = new Dictionary<float,List<float>>();
    [HideInInspector] public Dictionary<float, List<float>> GapsSeenRounded = new Dictionary<float,List<float>>();
    [HideInInspector] public Dictionary<float, List<float>> GapsSeenActual = new Dictionary<float,List<float>>(); 
    // Start is called before the first frame update
    void Start()
    {
        carSpawner = GameObject.Find("Car Spawner").GetComponent<CarSpawner>();
    }

    // Update is called once per frame
    void Update()
    {

        TagCars();

        GameObject leadCar = GameObject.FindWithTag("LeadCar");
        GameObject tailCar = GameObject.FindWithTag("TailCar");
        float headZ = Camera.main.transform.position.z;
        float headX = Camera.main.transform.position.x;

        if (tagged && !timingInitiated && Input.GetKeyDown(KeyCode.P))
        {

            float leadCarX = leadCar.transform.position.x;
            float tailCarX = tailCar.transform.position.x;
            float leadCarSize = leadCar.GetComponent<CarEntity>().carSize;
            float tailCarSize = tailCar.GetComponent<CarEntity>().carSize;

            leadGapRT = LeadGapReactionTimeCalculation();
            btnPressed = carSpawner.elapsedTime;

            ResponseAnalysis.Add(carSpawner.trialNum, new List<float>()
            {
                btnPressed,
                headX,
                headZ,
                leadCarX,
                leadCarSize,
                tailCarX,
                tailCarSize,
                leadGapRT
            });

            GapsSeenRounded.Add(carSpawner.trialNum, gapsGeneratedRounded);
            ActualGapsSeenCalculator();
            GapsSeenActual.Add(carSpawner.trialNum, gapsGeneratedActual);

            gapsGeneratedRounded = new List<float>();
            gapsGeneratedActual = new List<float>();
            timingInitiated = true;
        }

        if(tagged && !timingEnded && Input.GetKeyUp(KeyCode.P))
        {
            float leadCarX = leadCar.transform.position.x;
            float tailCarX = tailCar.transform.position.x;
            float leadCarSize = leadCar.GetComponent<CarEntity>().carSize;
            float tailCarSize = tailCar.GetComponent<CarEntity>().carSize;

            timeToSpare = TimeToSpareCalculation();
            btnReleased = carSpawner.elapsedTime;
            float btnHoldTime = btnReleased - btnPressed;

            ResponseAnalysis[carSpawner.trialNum].Add(btnReleased);
            ResponseAnalysis[carSpawner.trialNum].Add(headX);
            ResponseAnalysis[carSpawner.trialNum].Add(headZ);
            ResponseAnalysis[carSpawner.trialNum].Add(leadCarX);
            ResponseAnalysis[carSpawner.trialNum].Add(leadCarSize);
            ResponseAnalysis[carSpawner.trialNum].Add(tailCarX);
            ResponseAnalysis[carSpawner.trialNum].Add(tailCarSize);
            ResponseAnalysis[carSpawner.trialNum].Add(timeToSpare);
            ResponseAnalysis[carSpawner.trialNum].Add(btnHoldTime);

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

    void ActualGapsSeenCalculator()
    {
        for(int i=1; i < gapsGeneratedActual.Count; i++)
        {
            gapsGeneratedActual[i-1] = gapsGeneratedActual[i] - gapsGeneratedActual[i-1];
        }
        gapsGeneratedActual.RemoveRange(gapsGeneratedRounded.Count, gapsGeneratedActual.Count - gapsGeneratedRounded.Count);
    }


}

