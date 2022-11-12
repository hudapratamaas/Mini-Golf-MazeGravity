using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCameraScreenSpace : MonoBehaviour
{
    [SerializeField] RawImage background;
    Coroutine cameraStarter;
    WebCamTexture backCamera;

    
    private void OnEnable()
    {
        if (cameraStarter == null)
        cameraStarter = StartCoroutine(Startcamera());
    }

    private void OnDisable()
    {
        if(backCamera != null && backCamera.isPlaying)
        backCamera.Stop();

        if(cameraStarter != null)
        StopCoroutine(cameraStarter);
    }

    // Update is called once per frame
    void Update()
    {
        if(cameraStarter == null && (backCamera == null || backCamera.isPlaying == false))
        {
            StartCoroutine(Startcamera());
            return;
        }

    }

    IEnumerator Startcamera()
    {
        #if UNITY_EDITOR
        while (UnityEditor.EditorApplication.isRemoteConnected == false)
            yield return new WaitForEndOfFrame();
        #endif

        WebCamDevice[] devices = WebCamTexture.devices;
        foreach (var device in devices)
        {
            if(device.isFrontFacing == false)
            {
                backCamera = new WebCamTexture( device.name, 1080, 2400);

            }
        }
        if(backCamera == null)
        {
            Debug.Log("Back Camera Not Found");
            yield break;
        }

        background.texture = backCamera;
        backCamera.Play();

        while(backCamera.isPlaying == false)
            yield return new WaitForEndOfFrame();

            
        while (backCamera.width < 100)
            yield return new WaitForEndOfFrame();

        int flipY = backCamera.videoVerticallyMirrored ? -1 : 1;
        background.transform.localScale = new Vector3(1, flipY, 1);

        int orient = -backCamera.videoRotationAngle;
        background.transform.rotation = Quaternion.Euler(0, 0, orient);

            background.rectTransform.sizeDelta = new Vector2(
            backCamera.width,
            backCamera.height
        );
    }
}
