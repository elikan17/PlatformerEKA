using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefactorEnemy : MonoBehaviour
{
    public Stats enemyStats;

    [Tooltip("The transform that will lock onto the player once the enemy has spotted them.")]
    public Transform sight;

    [Tooltip("Blue explosion particles")]
    public GameObject enemyExplosionParticles;

    public PatrolRefractor patrolScript;

    private bool slipping = false;

    private GameObject player;

    /// <summary>
    /// Contains tunable parameters to tweak the enemy's movement and behavior.
    /// </summary>
    [System.Serializable]
    public struct Stats
    {
        [Tooltip("How fast the enemy moves.")]
        public float walkSpeed;

        [Tooltip("Whether the enemy should move or not")]
        public bool idle;

        [Tooltip("How fast the enemy runs after the player (only when idle is false).")]
        public float chaseSpeed;

        [Tooltip("How close the enemy needs to be to explode")]
        public float explodeDist;

    }

    private void Update()
    {
        // changes the enemy's behavior: pacing in circles or chasing the player
        if (enemyStats.idle == true)
        {
            patrolScript.Patrol(enemyStats.walkSpeed);
        }
        else
        {
            Chase(player.transform);
        }
        
        //stops enemy from following player up steep slopes
        if (slipping == true)
        {
            transform.Translate(Vector3.back * 20 * Time.deltaTime, Space.World);
        }
    }

    private void Chase(Transform toChase)
    {
        sight.position = new Vector3(toChase.transform.position.x, transform.position.y, toChase.transform.position.z);
        transform.LookAt(sight);
        transform.position = Vector3.MoveTowards(transform.position, toChase.transform.position, Time.deltaTime * enemyStats.chaseSpeed);
                   
        //Explode if we get within the enemyStats.explodeDist
        if (Vector3.Distance(transform.position, player.transform.position) < enemyStats.explodeDist)
        {
            StartCoroutine("Explode");
            enemyStats.idle = true;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 9)
        {
            slipping = true;
        }
        else
        {
            slipping = false;
        }
    }


   private void OnTriggerEnter(Collider other)
    {
        //start chasing if the player gets close enough
        if (other.gameObject.tag == "Player")
        {
            player = other.gameObject;
            enemyStats.idle = false;
        }
    }

   private void OnTriggerExit(Collider other)
    {
        //stop chasing if the player gets far enough away
        if (other.gameObject.tag == "Player")
        {
            enemyStats.idle = true;      
        }
    }

    private IEnumerator Explode()
    {
        GameObject particles = Instantiate(enemyExplosionParticles, transform.position, new Quaternion());
        yield return new WaitForSeconds(0.2f);
        Destroy(transform.parent.gameObject);
    }


}