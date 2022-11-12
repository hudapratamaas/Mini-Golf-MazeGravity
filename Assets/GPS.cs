using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS : MonoBehaviour
{
    [SerializeField] string latitude;
    [SerializeField] string longitude;
    [SerializeField] string altitude;
    [SerializeField] string horizontalAccuracy;
    [SerializeField] string timetamp;


    Coroutine ActivateGPSCoroutine;

    void Update()
    {
        if(Input.location.status != LocationServiceStatus.Running)
        return;

        latitude =  Input.location.lastData.latitude.ToString("F8").Split()[0];
        longitude =  Input.location.lastData.longitude.ToString("F8").Split()[0];
        altitude = Input.location.lastData.altitude.ToString("F8").Split('.')[0];
        horizontalAccuracy = Input.location.lastData.horizontalAccuracy.ToString();
        timetamp =  Input.location.lastData.timestamp.ToString();
        

        this.transform.rotation = Quaternion.Euler(0, -Input.compass.trueHeading,0);
    }

    private void OnEnable()
    {
        if(ActivateGPSCoroutine == null)
        ActivateGPSCoroutine =  StartCoroutine(ActivateGPS());
    }

    private void OnDisable()
    {
        StopCoroutine(ActivateGPSCoroutine);

        if(Input.location.status == LocationServiceStatus.Running)
        Input.location.Stop();
    }

    IEnumerator ActivateGPS()
    {
    #if UNITY_EDITOR
        Debug.Log("Unity Remote Connecting");
        while(UnityEditor.EditorApplication.isRemoteConnected == false)
        yield return new WaitForSecondsRealtime(1);
    #endif
        Debug.Log("Unity Remote Connecting");

        if(Input.location.isEnabledByUser == false)
        {
            Debug.Log("Location Service is not enabled by user");
            yield break;       
        }

        Debug.Log("Start location service");
        Input.location.Start();

        int maxWait = 15;
        while(Input.location.status == LocationServiceStatus.Stopped
        || Input.location.status == LocationServiceStatus.Initializing
        && maxWait > 0)
        {
            Debug.Log("Location service check : " + Input.location.status);
            yield return new WaitForSecondsRealtime(1);
            maxWait -= 1;
        }

        if(maxWait <1)
        { 
            Debug.Log("Location service Time Out");
            yield break;
        }

        if(Input.location.status ==  LocationServiceStatus.Failed)
        {
            Debug.Log("Location ststus failed to start");
            yield break;
        }

        Input.compass.enabled = true;
    } 
}
