using UnityEngine;
using UnityEngine.AI;

public class EnemyRobot : Enemy
{
    private NavMeshAgent agent;
    private GameObject player;

    [Header("Settings")]
    [SerializeField] private int mode;
    [SerializeField] private float distanceToAgro;

    [Header("State")]
    [SerializeField] private Vector3 oldPlayerPosition;
    private Vector3 directionToPlayer;

    private float roamTimer;
    private float searchTimer;

    [Header("Combat")]
    [SerializeField] private Transform missileExit;
    [SerializeField] private GameObject missilePrefab;
    private bool hasShot;

    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource bulletShot;

    private void Start()
    {
        player = FindAnyObjectByType<PlayerMovement>().gameObject;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        directionToPlayer = player.transform.position - transform.position;

        if (mode != 2 && directionToPlayer.magnitude < distanceToAgro)
        {
            oldPlayerPosition = player.transform.position;
            mode = 1;
        }

        switch (mode)
        {
            case 2:
                Searching();
                break;
            case 1:
                Agro();
                break;
            default:
                Roaming();
                break;
        }

        anim.SetBool("Walking", agent.velocity.magnitude > 0.1f);

    }

    private void Roaming()
    {
        roamTimer += Time.deltaTime;

        if (roamTimer >= 2f)
        {
            Vector3 newPos = RandomNavSphere(transform.position, 30f);
            agent.SetDestination(newPos);
            roamTimer = 0f;
        }

        if (directionToPlayer.magnitude < distanceToAgro)
        {
            mode = 1;
        }
    }

    private void Agro()
    {
        agent.destination = transform.position;

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.LookRotation(directionToPlayer),
              Time.deltaTime
        );

        if (directionToPlayer.magnitude > distanceToAgro)
        {
            oldPlayerPosition = GetValidNavMeshPosition(player.transform.position);
            mode = 2;
        }

        if (!hasShot)
        {
            anim.SetBool("Shooting", false);
            hasShot = true;
            Invoke(nameof(Shoot), 2f);
        }
    }

    private void Shoot()
    {
        bulletShot.Play();
        anim.SetBool("Shooting", true);

        Instantiate(
            missilePrefab,
            missileExit.position,
            Quaternion.LookRotation(player.transform.position - missileExit.position)
        );

        hasShot = false;
    }

    private void Searching()
    {
        anim.SetBool("Shooting", false);
        agent.destination = GetValidNavMeshPosition(oldPlayerPosition);

        if (agent.remainingDistance < 1f)
        {
            searchTimer += Time.deltaTime;

            if (searchTimer >= 4f)
            {
                mode = directionToPlayer.magnitude < distanceToAgro ? 1 : 0;
                searchTimer = 0f;
            }
        }

        if (agent.remainingDistance > 1f && directionToPlayer.magnitude < distanceToAgro)
        {
            mode = 1;
        }
    }

    private Vector3 RandomNavSphere(Vector3 origin, float distance, int attempts = 10)
    {
        for (int i = 0; i < attempts; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * distance + origin;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, distance, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }

        return origin;
    }

    private Vector3 GetValidNavMeshPosition(Vector3 targetPosition)
    {
        if (NavMesh.SamplePosition(targetPosition, out NavMeshHit hit, 5f, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position;
    }

    public override void Death()
    {
        mode = 3;
        anim.SetBool("Dead", true);
        Invoke("death", 2);
    }
    private void death()
    {
        Destroy(gameObject);
    }
}