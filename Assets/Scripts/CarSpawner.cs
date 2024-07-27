using System.Collections;
using System.Collections.Generic;
using Oculus.Haptics;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{

    public Color[] colors;
    public float[] gaps;
    public GameObject[] carPool;
    HapticClipPlayer playerRight;
    public HapticClip hapticClip;
    public int trialCount;
    public float carCreationDelay;
    float delay;
    bool firstCar = true;
    [HideInInspector] public bool newTrial = false;
    bool activeResponse;
    [HideInInspector] public bool rightTriggerPressed;
    [HideInInspector] public float waitTime;
    float customTime;
    float timeReset;
    [HideInInspector] public bool isPink;
    [HideInInspector] public float elapsedTime;
    [HideInInspector] public GameObject pinkCar;
    [HideInInspector] public float trialNum;

    [HideInInspector] public Dictionary<float, List<List<float>>> CarInfo = new Dictionary<float, List<List<float>>>();
    ResponseAnalyzer responseAnalyzer;


    // Start is called before the first frame update
    void Start()
    {
        playerRight = new HapticClipPlayer(hapticClip);
        responseAnalyzer = GameObject.Find("XR Origin (XR Rig)").GetComponent<ResponseAnalyzer>();
        delay = carCreationDelay;

    }

    // Update is called once per frame
    void Update()
    {
        OVRInput.Update();
        customTime += Time.deltaTime;
        delay -= Time.deltaTime;
        elapsedTime += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.S) || newTrial)
        {
            if(delay <= 0)
            {
                carCreation();
            }
            

            if(pinkCar == null)
            {
                if((OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || Input.GetKeyDown(KeyCode.P)) && !rightTriggerPressed)
                {
                    //haptic feedback
                    playerRight.Play(Controller.Right);

                    trialNum++;

                    rightTriggerPressed = true;
                    firstCar = true;
                    delay = carCreationDelay;
                }
            }
            else if(pinkCar != null && pinkCar.transform.position.x >= 0)
            {
                if((OVRInput.Get(OVRInput.RawButton.RIndexTrigger) || Input.GetKeyDown(KeyCode.P)) && !rightTriggerPressed)
                {
                    //haptic feedback
                    playerRight.Play(Controller.Right);

                    trialNum++;

                    rightTriggerPressed = true;
                    firstCar = true;
                    delay = carCreationDelay;
                }
            }

        }

    }


    void carCreation()
    {
        // Randomly select a prefab from the pool
        GameObject selectedPrefab = carPool[Random.Range(0, carPool.Length)];

        if(firstCar)
        {
            // Clone the prefab, adjust its color.
            pinkCar = Instantiate(selectedPrefab, new Vector3(-125f, 0, 0), Quaternion.Euler(-90, 90, 0));

            if(selectedPrefab.tag == "VW")
            {
                pinkCar.GetComponent<MeshRenderer>().materials[1].color = Color.magenta;
            }

            else
            {
                pinkCar.GetComponent<MeshRenderer>().materials[0].color = Color.magenta;
            }            

            // Resest the timer
            timeReset = customTime;
            customTime -= timeReset;

            // Select the next gap size
            waitTime = gaps[Random.Range(0, gaps.Length)];

            firstCar = false;
            newTrial = true;
            rightTriggerPressed = false;

            // Increment the car entity ID
            selectedPrefab.GetComponent<CarEntity>().entityID++;
            //Set the intantiated car's tag to "ClonedCar"
            pinkCar.tag = "ClonedCar";

        }

        else if(customTime >= waitTime)
        {
            GameObject obj = Instantiate(selectedPrefab, new Vector3(-125f, 0, 0), Quaternion.Euler(-90, 90, 0));

            // Resest the timer
            timeReset = customTime;
            customTime -= timeReset;

            // Select the next gap size
            waitTime = gaps[Random.Range(0, gaps.Length)];

            // Increment the car entity ID
            selectedPrefab.GetComponent<CarEntity>().entityID++;  
            //Set the intantiated car's tag to "ClonedCar" 
            obj.tag = "ClonedCar";

        } 
       
    }



}


