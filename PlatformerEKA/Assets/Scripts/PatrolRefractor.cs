using UnityEngine;

public class PatrolRefractor : MonoBehaviour
{

    public Transform[] patrolPoints;

    private int currentPatrolPoint = 0;
    

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Patrol(float walkSpeed)
    {
             //Patrol Logic
             Vector3 moveToPoint = patrolPoints[currentPatrolPoint].position;
             transform.position = Vector3.MoveTowards(transform.position, moveToPoint, walkSpeed * Time.deltaTime);
        
             if (Vector3.Distance(transform.position, moveToPoint) < 0.01f)
             {
                 currentPatrolPoint++;
                 if (currentPatrolPoint > patrolPoints.Length - 1)
                 {
                     currentPatrolPoint = 0;
                 } 
             }
    }
}
