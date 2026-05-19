using UnityEngine;
using UnityEngine.AI;
public class BasicEnemy : MonoBehaviour
{
    // From the following tutorial: https://www.youtube.com/watch?v=UjkSFoLxesw
    public NavMeshAgent agent;
    public Transform player;
    // A layer mask is an int under the hood that for which each bit refers to a layer in a Unity scene (on ay given object look below the name and to the right of the tag to find its dropdown)
    // Layers themselves are groupings of objects relating to physics and render calculations
    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange, minWalkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //Animation Data
    Animator animator;
    private readonly int speedHash = Animator.StringToHash("Speed");
    protected float CurrentSpeed;
    [SerializeField] private float MaxSpeed = 3.5f;
    protected float SpeedLimit;
    protected float WalkingSpeed;

    protected bool wasChasing = false;
    
    /// <summary>
    /// Speed progress is a value between 0 and 1 representing how fast the player is moving for animation purposes.
    /// </summary>
    protected float SpeedProgress; // Calculate speed as a percentage of max speed


    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

        animator = GetComponentInChildren<Animator>();
        WalkingSpeed = MaxSpeed * 0.381549f;
        SpeedLimit = WalkingSpeed;
        agent.speed = SpeedLimit;
    }

    private void Update()
    {
        //Check for sight and attack range

        if (player != null)
        {
            // Calculate clean flat distance (ignoring height differences)
            Vector3 displacement = player.position - transform.position;
            displacement.y = 0; 
            float distanceToPlayer = displacement.magnitude;

            // Directly assign your states based on the raw math!
            playerInSightRange = distanceToPlayer <= sightRange;
            playerInAttackRange = distanceToPlayer <= attackRange;
        }

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

        //Patroling Animator Control
        agent.speed = SpeedLimit;

        CurrentSpeed = agent.velocity.magnitude;
    
        /// <summary>
        /// Speed progress is a value between 0 and 1 representing how fast the player is moving for animation purposes.
        /// </summary>
        SpeedProgress = CurrentSpeed / MaxSpeed; // Calculate speed as a percentage of max speed
        
        animator.SetFloat(speedHash, SpeedProgress);
    }

    private void Patroling()
    {
        SpeedLimit = WalkingSpeed;

        if(!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            Debug.Log("Walkpoint Set");
            agent.SetDestination(walkPoint);
        }

        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;
        Debug.Log("distanceToWalkPoint = " + distanceToWalkPoint);

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude <= 2f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // 1. Get a completely random angle in radians (0 to 2 * PI)
        float randomAngle = Random.Range(0f, Mathf.PI * 2f);

        // 2. Get a random distance strictly between your Min and Max ranges
        float randomDistance = Random.Range(minWalkPointRange, walkPointRange);

        // 3. Convert Polar Coordinates (Angle & Distance) to Cartesian Coordinates (X & Z)
        float randomX = Mathf.Cos(randomAngle) * randomDistance;
        float randomZ = Mathf.Sin(randomAngle) * randomDistance;

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        // Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround) Casts a ray from walkPoint in the direction -transform.up (which is straight down) for the length of 2 units (meters) checking if it hits anything considered the ground (things that pass the whatIsGround LayerMask)
        if (Physics.Raycast(walkPoint, -transform.up, 3f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        //Debug.Log("Chasing Player!");
        agent.isStopped = false;
        SpeedLimit = MaxSpeed;
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Halts the enemy
        //agent.SetDestination(transform.position);
        agent.isStopped = true;

        Vector3 flatTarget = new Vector3(player.position.x, transform.position.y, player.position.z);

        transform.LookAt(flatTarget);

        if(!alreadyAttacked)
        {
            // Attack Code here:
            Debug.Log("Enemy Attacked!");
            //animator.SetTrigger("InRange");
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
