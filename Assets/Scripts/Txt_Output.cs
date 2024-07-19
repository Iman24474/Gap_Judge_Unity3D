using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class Txt_Output : MonoBehaviour
{
    string myFilePath;
	public string ParticipantId;
    public int age;
    public string gender;
    string date;
    int counter;
    int duplicate=1;

    Headset headSet;
    CarSpawner carEntitySpawner;
	StreamWriter fileWriter;


    void Awake()
    {
        headSet = GameObject.Find("XR Origin (XR Rig)").GetComponent<Headset>();
        carEntitySpawner = GameObject.Find("Car Spawner").GetComponent<CarSpawner>();
    
    }

	private void Start()
	{

        date = DateTime.Now.ToString();


        //internal storage/android/data/yourApp/files/testFile.txt
        myFilePath = Application.dataPath + "/RawData/TXT_Format/" + ParticipantId + "_GJ.txt";

		while(File.Exists(myFilePath))
        {
            myFilePath = Application.dataPath + "/RawData/TXT_Format/" + ParticipantId + "_GJ_" + duplicate + ".txt";
            duplicate++;
        }              
                

        WriteToFile("Gap Judge Project\n" + "\nDate: "
        + date + "\nParticipant ID: " + ParticipantId + "\nAge: " + age 
        + "\nGender: " + gender + "\n\n\nTime\tHead_X_Pos\tHead_Y_Pos"
        + "\tHead_Z_Pos\tHead_X_Rot\tHead_Y_Rot\tHead_Z_Rot\tCarCreationTime\t" 
        + "CarId\tCarSize\tCar_X_Pos\tCar_Y_Pos\tCar_Z_Pos\tGapSize\n");
	}

    void Update()
    {

        counter = headSet.HeadXPos.Count;
        
    }

	
    
    private void OnApplicationQuit()
    {

        StringBuilder stringBuilder = new StringBuilder();
        for(int i=0; i < counter; i++)
        {
            stringBuilder.Append(headSet.TotalTime[i].ToString("F4") + "\t"
            + headSet.HeadXPos[i].ToString("F4") + "\t"
            + headSet.HeadYPos[i].ToString("F4") + "\t"
            + headSet.HeadZPos[i].ToString("F4") + "\t"
            + headSet.HeadXRot[i].ToString("F4") + "\t"
            + headSet.HeadYRot[i].ToString("F4") + "\t"
            + headSet.HeadZRot[i].ToString("F4") + "\t"
            + carEntitySpawner.carCreationTime[i].ToString("F4") + "\t" 
            + carEntitySpawner.carEntityId[i].ToString("F4") + "\t"
            + carEntitySpawner.carSize[i].ToString("F4") + "\t"
            + carEntitySpawner.carPosition[i].x.ToString("F4") + "\t"
            + carEntitySpawner.carPosition[i].y.ToString("F4") + "\t"
            + carEntitySpawner.carPosition[i].z.ToString("F4") + "\t"
            + carEntitySpawner.gapTime[i].ToString("F4") + "\t"
            + "\n");
        }
        
        File.AppendAllText(myFilePath, stringBuilder.ToString());


    }
    
    
    
    
    
    public void WriteToFile(string message)
    {
        try
        {
            fileWriter = new StreamWriter(myFilePath, true);
            fileWriter.Write(message);
            fileWriter.Close();
        }
        catch(System.Exception e)
        {
            Debug.LogError("Cannot write into the file");
        }
    }

}
