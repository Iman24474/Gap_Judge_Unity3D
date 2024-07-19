using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMover : MonoBehaviour
{
    const float speed = 11.176f;
    const float endPoint = 125f;
    const float destroyPoint = -10f;
    CarSpawner carSpawner;
    // Start is called before the first frame update
    void Start()
    {
        carSpawner = GameObject.Find("Car Spawner").GetComponent<CarSpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position += Vector3.right * speed * Time.deltaTime; 

        if(gameObject.transform.position.x >= endPoint)
        {
            Destroy(gameObject);
        }

        else if(gameObject.transform.position.x <= destroyPoint)
        {
            if(carSpawner.rightTriggerPressed)
            {
                Destroy(gameObject);

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

    }
}
