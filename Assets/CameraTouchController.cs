using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTouchController : MonoBehaviour
{
    [SerializeField, Range(0, 20)] float filterFactor = 1;
    [SerializeField, Range(0,10) ] float dragFactor = 1;
    [SerializeField, Range(0,2)] float zoomFactor = 1; 
    [Tooltip("equel camera y position")]
    [SerializeField] float minCamPos = 70;
    [SerializeField] float maxCamPos = 170;
    [SerializeField] Collider topCollider;

    float distace;
    // Start is called before the first frame update
    void Start()
    {
        distace = this.transform.position.y;
    }

    Vector3 touchBeganWorldPos;
    Vector3 cameraBeganWolrdPos;
    void Update()
    {
        if(Input.touchCount == 0)
        return;

        var touch0 = Input.GetTouch(0);

        if(touch0.phase == TouchPhase.Began)
        {
            touchBeganWorldPos = Camera.main.ScreenToWorldPoint(
                new Vector3( touch0.position.x, touch0.position.y, distace)); 
                cameraBeganWolrdPos = this.transform.position;
        }
        if(Input.touchCount == 1 && touch0.phase ==  TouchPhase.Moved)
        {
            var touchMovedWorldPos = Camera.main.ScreenToWorldPoint(
                new Vector3( touch0.position.x, touch0.position.y, distace)); 

            var delta = touchMovedWorldPos - touchBeganWorldPos;

            var targetPos = cameraBeganWolrdPos - delta*dragFactor;

            
            targetPos = new Vector3(
                Mathf.Clamp(targetPos.x, topCollider.bounds.min.x,topCollider.bounds.max.x ),
                targetPos.y,
                Mathf.Clamp(targetPos.z, topCollider.bounds.min.z, topCollider.bounds.max.z)
            );

            this.transform.position = Vector3.Lerp(
                this.transform.position,
                targetPos,
                Time.deltaTime*filterFactor
            );
        }

            if(Input.touchCount < 2)
            return;

            var touch1 = Input.GetTouch(1);

            if(touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                var touch0PresPos = touch0.position - touch0.deltaPosition;
                var touch1PresPos = touch1.position - touch1.deltaPosition;
                var preDistance = Vector3.Distance(touch0PresPos, touch1PresPos);
                var currDistance = Vector3.Distance(touch0.position, touch1.position);
                var delta = currDistance - preDistance;

                this.transform.position -= new Vector3(0,delta*zoomFactor,0);
                this.transform.position = new Vector3(
                    this.transform.position.x,
                    Mathf.Clamp(this.transform.position.y, minCamPos, maxCamPos),
                    this.transform.position.z

                );
                distace = this.transform.position.y; 
                
            }
    }
}
