using UnityEngine;
using UnityEngine.AI;
public class BasicEnemy : MonoBehaviour
{
    // From the following tutorial: https://www.youtube.com/watch?v=UjkSFoLxesw
    public NavMeshAgent agent;
    public Transform player;
    // A layer mask is an int under the hood that for which each bit refers to a layer in a Unity scene (on ay given object look below the name and to the right of the tag to find its dropdown)
    // Layers themselves are groupings of objects relating to physics and render calculations
    public LayerMask whatisGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight and attack range

        //Physics.CheckSphere() checks in a spherical area with its center at the first parameter, its radius the second and the third parameter is a LayerMask that filters for valid objects. Returns a boolean 
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();
    }

    private void Patroling()
    {
        //Debug.Log("Patroling");

        if(!walkPointSet) SearchWalkPoint();

        if(walkPointSet)
            agent.SetDestination(walkPoint);
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange,walkPointRange);
        float randomX = Random.Range(-walkPointRange,walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Physics.Raycast(walkPoint, -transform.up, 2f, whatisGround) Casts a ray from walkPoint in the direction -transform.up (which is straight down) for the length of 2 units (meters) checking if it hits anything considered the ground (things that pass the whatIsGround LayerMask)
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatisGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        //Debug.Log("Chasing Player!");
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Halts the enemy
        agent.SetDestination(transform.position);

        Vector3 flatTarget = new Vector3(player.position.x, transform.position.y, player.position.z);

        transform.LookAt(flatTarget);

        if(!alreadyAttacked)
        {
            // Attack Code here:
            Debug.Log("Enemy Attacked!");

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }


}
