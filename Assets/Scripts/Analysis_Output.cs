using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class Analysis_Output : MonoBehaviour
{
    string myFilePath;
	public string ParticipantId;
    public int age;
    public string gender;
    string date;
    int duplicate=1;

    ResponseAnalyzer responseAnalyzer;
	StreamWriter fileWriter;

    void Awake()
    {
        responseAnalyzer = GameObject.Find("XR Origin (XR Rig)").GetComponent<ResponseAnalyzer>();

    }
    // Start is called before the first frame update
    void Start()
    {
        date = DateTime.Now.ToString();


        //internal storage/android/data/yourApp/files/testFile.txt
        myFilePath = Application.dataPath + "/RawData/TXT_Format/" + ParticipantId + "_GJ_Analysis.txt";

		while(File.Exists(myFilePath))
        {
            myFilePath = Application.dataPath + "/RawData/TXT_Format/" + ParticipantId + "_GJ_Analysis_" + duplicate + ".txt";
            duplicate++;
        }              
                

        WriteToFile("Gap Judge Project\n" + "\nDate: "
        + date + "\nParticipant ID: " + ParticipantId + "\nAge: " + age 
        + "\nGender: " + gender + "\n\n\n");
        
    }

    private void OnApplicationQuit()
    {

        StringBuilder stringBuilder = new StringBuilder();

        foreach(float key in responseAnalyzer.ResponseAnalysis.Keys)
        {
            stringBuilder.Append(
                "==================== Trial " + key + " ====================" + "\n" +
                "Btn Press (s): " + "\t\t\t\t\t\t\t" + responseAnalyzer.ResponseAnalysis[key][0].ToString("F4") + "\n" +
                "Ped X (m): " + "\t\t\t\t\t\t\t\t" + responseAnalyzer.ResponseAnalysis[key][1].ToString("F4") + "\n" +
                "Ped Z (m): " + "\t\t\t\t\t\t\t\t" + responseAnalyzer.ResponseAnalysis[key][2].ToString("F4") + "\n" +
                "Lead Car X (m): " + "\t\t\t\t\t\t" + responseAnalyzer.ResponseAnalysis[key][3].ToString("F4") + "\n" +
                "Lead Car Size (m): " + "\t\t\t\t\t\t" + responseAnalyzer.ResponseAnalysis[key][4].ToString("F4") + "\n" +
                "Tail Car X (m): " + "\t\t\t\t\t\t" + responseAnalyzer.ResponseAnalysis[key][5].ToString("F4") + "\n" +
                "Tail Car Size (m): " + "\t\t\t\t\t\t" + responseAnalyzer.ResponseAnalysis[key][6].ToString("F4") + "\n\n" +
                "Btn Release (s): " + "\t\t\t\t\t\t" + responseAnalyzer.ResponseAnalysis[key][8].ToString("F4") + "\n" +
                "Ped X (m): " + "\t\t\t\t\t\t\t\t" + responseAnalyzer.ResponseAnalysis[key][9].ToString("F4") + "\n" +
                "Ped Z (m): " + "\t\t\t\t\t\t\t\t" + responseAnalyzer.ResponseAnalysis[key][10].ToString("F4") + "\n" +
                "Lead Car X (m): " + "\t\t\t\t\t\t" + responseAnalyzer.ResponseAnalysis[key][11].ToString("F4") + "\n" +
                "Lead Car Size (m): " + "\t\t\t\t\t\t" + responseAnalyzer.ResponseAnalysis[key][12].ToString("F4") + "\n" +
                "Tail Car X (m): " + "\t\t\t\t\t\t" + responseAnalyzer.ResponseAnalysis[key][13].ToString("F4") + "\n" +
                "Tail Car Size (m): " + "\t\t\t\t\t\t" + responseAnalyzer.ResponseAnalysis[key][14].ToString("F4") + "\n\n" +
                "Btn Hold Time (s): " + "\t\t\t\t\t\t" + responseAnalyzer.ResponseAnalysis[key][16].ToString("F4") + "\n" +
                "Lead Gap RT (s): " + "\t\t\t\t\t\t" + responseAnalyzer.ResponseAnalysis[key][7].ToString("F4") + "\n" +
                "Time to Spare (s): " + "\t\t\t\t\t\t" + responseAnalyzer.ResponseAnalysis[key][15].ToString("F4") + "\n\n" +
                "Gaps Seen (Actual): " + "\t");

                foreach(float value in responseAnalyzer.GapsSeenActual[key])
                {
                    stringBuilder.Append(
                        value.ToString("F4") + " | "
                    );
                }
                stringBuilder.Append("\n" +
                "Gaps Seen (Rounded): " + "\t");

                foreach(float value in responseAnalyzer.GapsSeenRounded[key])
                {
                    stringBuilder.Append(
                        value.ToString("F1") + " | "
                    );
                }
                stringBuilder.Append("\n\n");

        }

        File.AppendAllText(myFilePath, stringBuilder.ToString());


    }

    void WriteToFile(string message)
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
