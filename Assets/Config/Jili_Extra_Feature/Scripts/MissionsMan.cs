using UnityEngine;
using System;
public class MissionsMan : MonoBehaviour
{
    public float MaxTime = 43200;
    public float TheTime = 7200;
    public float[] MissionGoals;
    float savetimestamp;
    private void Start()
    {
        TheTime = GetSavedTime();
    }
    [ContextMenu("AddMissionPoints")]
    void TestAddMissionPoints()
    {
        AddMissionPoints(MissionGoals[PlayerPrefs.GetInt("ActiveMissionLevel")] / 5);
    }
    [ContextMenu("Reset")]
    public void ResetPoints()
    {
        for(int i = 0; i < MissionGoals.Length; i++)
        {
            PlayerPrefs.SetFloat("TotalMissionPoints_" + i.ToString(), 0);
            PlayerPrefs.SetInt("MissionUnlocked" + i.ToString(), 0);
        }
        PlayerPrefs.SetInt("ActiveMissionLevel",0);
        Refresh();
    }
    public void AddMissionPoints(float Amount = 500)
    {
        int TheActiveLevel = PlayerPrefs.GetInt("ActiveMissionLevel");
        float NewAmount = PlayerPrefs.GetFloat("TotalMissionPoints_" + TheActiveLevel.ToString()) + Amount;
        if (NewAmount >= MissionGoals[TheActiveLevel])
        {
            NewAmount = MissionGoals[TheActiveLevel];
            PlayerPrefs.SetInt("MissionUnlocked" + TheActiveLevel.ToString(), 1);

            //NextLevel
            GoToNextLevel();

        }
        PlayerPrefs.SetFloat("TotalMissionPoints_" + TheActiveLevel.ToString(), NewAmount);

        Refresh();
    }
    void Refresh()
    {
        MissionStat[] btns = FindObjectsOfType<MissionStat>();
        for (int i = 0; i < btns.Length; i++)
        {
            btns[i].Refresh();
        }
    }
    public float GetMissionPoints(int TheLevel)
    {
        return PlayerPrefs.GetFloat("TotalMissionPoints_" + TheLevel.ToString());
    }
    public bool IsMissionUnlocked(int TheLevel)
    {
        if (PlayerPrefs.GetInt("ActiveMissionLevel") >= TheLevel)
        {
            return true;
        }
        /*if (PlayerPrefs.GetInt("MissionUnlocked" + TheLevel.ToString()) == 1)
        {
            return true;
        }*/
        return false;
    }
    public void GoToNextLevel()
    {
        int TheActiveLevel = PlayerPrefs.GetInt("ActiveMissionLevel");
        if (TheActiveLevel < MissionGoals.Length - 1)
        {
            PlayerPrefs.SetInt("ActiveMissionLevel", TheActiveLevel + 1);
        }
        Refresh();
    }
    public int GetMissionLevel()
    {
        int TheActiveLevel = PlayerPrefs.GetInt("ActiveMissionLevel");
        /*if (TheActiveLevel >= MissionGoals.Length - 1 && PlayerPrefs.GetInt("ClaimMissionPoints_" + TheActiveLevel.ToString()) == 1
            )
        {
            TheActiveLevel = MissionGoals.Length;
        }*/
        return TheActiveLevel;

    }
    public string GetTime()
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(TheTime);

        //string thetime=timeSpan.ToString(@"hh\:mm\:ss");
        string thetime = timeSpan.Hours.ToString() + " H " + timeSpan.Minutes.ToString() + " M " + timeSpan.Seconds.ToString() + " S";
        return thetime;
    }
    private void Update()
    {
        if (TheTime > 0)
        {
            TheTime -= Time.deltaTime;
            SaveTime(TheTime);
        }
        else
        {
            ResetTime();
        }
    }
    void ResetTime()
    {
        SaveSession();
        PlayerPrefs.SetFloat("ElapsedTime", MaxTime);
        TheTime = MaxTime;
        ResetPoints();
    }
    void SaveTime(float TheT)
    {
        if (savetimestamp > Time.time)
            return;
        savetimestamp = Time.time + 1;
        PlayerPrefs.SetFloat("ElapsedTime",TheT);

        SaveSession();
    }
    float GetSavedTime()
    {
        float elapsedtime = GetTimeDifferenceBtwSession_seconds();
       // Debug.Log(elapsedtime);
        float thet = PlayerPrefs.GetFloat("ElapsedTime")- elapsedtime;
        //float thet = GetTimeDifferenceBtwSession_seconds();
        if (thet <= 0)
        {
            thet = MaxTime;
            ResetTime();
        }
        return thet;
    }
    void SaveSession()
    {
        string prefname = "LAST_SESSION";
        DateTime _new = DateTime.Now;
        PlayerPrefs.SetString(prefname, _new.ToString());
    }
    public float GetTimeDifferenceBtwSession_seconds()
    {
        string prefname ="LAST_SESSION";
        string last_session = PlayerPrefs.GetString(prefname);
        if (string.IsNullOrEmpty(last_session))
        {
            PlayerPrefs.SetString(prefname, DateTime.Now.ToString());
            return 0;
        }
        float Diff = (float)DateTime.Now.Subtract(DateTime.Parse(last_session)).TotalSeconds;
        //Diff = Diff / 60;
        return Diff;
    }

}
