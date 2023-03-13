using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserTest : MonoBehaviour
{
    public LineRenderer laserLineRenderer;
    public float laserWidth = 0.1f;
    public float laserMaxLength = 1500f;
    public int maxReflectionCount = 5;
    public float maxStepDistance = 200f;
    private Vector3[] positions;
    // Start is called before the first frame update
    void Start()
    {
        Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
        laserLineRenderer.SetPosition(0,Vector3.zero);
        laserLineRenderer.SetPosition(1, Vector3.zero);
        laserLineRenderer.SetWidth(laserWidth, laserWidth);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        RaycastHit hit;
        Vector3 directionLa = this.transform.forward;
        if(directionLa == null)
        {
            Debug.Log("Direction Null");
        }else if(this.transform.position == null)
        {
            Debug.Log("Position Null");
        }
        DrawReflectionPattern(this.transform.position,directionLa,maxReflectionCount, 0);
        /*if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, directionLa * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            if (hit.collider)
            {
                Debug.Log("reflect");
                directionLa = Vector3.Reflect(directionLa, hit.normal);
            }
            ShootLaserFromTargetPosition(transform.position, directionLa, hit.distance);
            
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }*/
    }
    void ShootLaserFromTargetPosition( Vector3 targetPosition, Vector3 direction, float length,int step )
     {
         Ray ray = new Ray( targetPosition, direction );
         RaycastHit raycastHit;
         Vector3 endPosition = targetPosition + ( length * direction );
 
         if( Physics.Raycast( ray, out raycastHit, length ) ) {
             endPosition = raycastHit.point;
         }
 
         laserLineRenderer.SetPosition( step, targetPosition );
         laserLineRenderer.SetPosition( step+1, endPosition );
     }
    private void DrawReflectionPattern(Vector3 position, Vector3 direction, int reflectionsRemaining, int step)
    {
        if (reflectionsRemaining == 0)
        {
            return;
        }

        Vector3 startingPosition = position;

        Ray ray = new Ray(position, direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, maxStepDistance))
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.tag == "Mirror") { 
                direction = Vector3.Reflect(direction, hit.normal);
                position = hit.point; }
            if (hit.collider.tag == "Receiver")
            {
                //GM.setHasWon(true);
                Debug.Log("win");
                position = hit.point;
                direction = new Vector3(0, 0, 0);
                //reflectionsRemaining = 0;
                //return;
            }
            position = hit.point;
        }
        else if(hit.Equals(null))
        {
            position += direction * maxStepDistance;
        }
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawLine(startingPosition, position);

        Debug.DrawLine(startingPosition, position, Color.blue);
        laserLineRenderer.SetPosition(step, startingPosition);
        laserLineRenderer.SetPosition(step + 1, position);
        step = step+1;
        //Debug.Log("Step:" + step);
        //Debug.Log("Reflections Remaining" + reflectionsRemaining);
        //Debug.Log("Position" + position);
        //Debug.Log("Direction" + direction);
        DrawReflectionPattern(position, direction, reflectionsRemaining - 1, step);


    }
}
