using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AIState
{
    Idle,
    Path,
    Attacking
}
public class EnemyScript : MonoBehaviour, IDamageInfo
{
    UnityEngine.AI.NavMeshAgent navMeshAgent;
    float pathFindTimer = .5f;
    [SerializeField] float pathFindUpdateTime = .5f;
    [SerializeField] GameObject target;
    AIState state = AIState.Idle;
    [SerializeField] GameObject pathParent;
    Rigidbody rb;
    Transform[] path;
    int currentPathPoint = 0;
    EnemyHands notFriendlyHands;
    [SerializeField] FootstepSouds soundSteps;
    [SerializeField] AudioClip soundTest;
    [SerializeField] AudioSource mouthAudio;
    [SerializeField] GameObject head;
    [SerializeField] LayerMask viewMask;
    [SerializeField] float viewAngle = 25;
    [SerializeField] float losePlayerTime = 3.0f;
    float sawLastTimer = 0;

    float attackTimer = 0;
    [SerializeField] float attackTime = 1f;
    [SerializeField] Transform attackCenter;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (pathParent !=null)
        {
            path = pathParent.GetComponentsInChildren<Transform>();
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        soundSteps.UpdateFootstep(navMeshAgent.velocity.magnitude);
        UpdateLookingForStuff();
        switch (state)
        {
            case AIState.Idle:
                break;
            case AIState.Path:
                UpdatePath();
                break;
            case AIState.Attacking:
                UpdateAttacking();
                break;
            default:
                break;
        }
    }
    private void UpdateAttacking()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0 && (target.transform.position - transform.position).magnitude < 3 )
        {
            RaycastHit[] rhits =Physics.SphereCastAll(attackCenter.transform.position,1, Vector3.up);
            for (int i = 0; i < rhits.Length; i++)
            {
                IDamageInfo dmg = rhits[i].transform.GetComponent<IDamageInfo>();
                if (dmg != null && dmg != this)
                {
                    dmg.DamageTaken(20f, Vector3.zero, transform.forward * 80);
                }
            }
            attackTimer = attackTime;
        }
        pathFindTimer -= Time.deltaTime;
        if (pathFindTimer <= 0)
        {
            pathFindTimer = pathFindUpdateTime;
            if (target != null)
            {
                if (navMeshAgent.remainingDistance < 2)
                {
                    
                }
                navMeshAgent.SetDestination(target.transform.position);
            }
        }
    }
    private void UpdateLookingForStuff()
    {
        sawLastTimer += Time.deltaTime;
        if (sawLastTimer >= losePlayerTime && state == AIState.Attacking)
        {
            SetPathState();
        }

        Vector3 direction = target.transform.position - head.transform.position;
        float angularDifference = Vector3.Angle(direction.normalized, head.transform.forward);
        if (angularDifference > viewAngle) return;
        if (Physics.Raycast(new Ray(head.transform.position, direction.normalized), out RaycastHit h, 20))
        {
            Debug.DrawRay(head.transform.position, direction.normalized * h.distance);

            Player pla = h.transform.GetComponent<Player>();
            if (pla != null)
            {
                sawLastTimer = 0;
                state = AIState.Attacking;
                navMeshAgent.stoppingDistance = 1.0f;
            }
        }
    }
    private void SetPathState()
    {
        state = AIState.Path;
        float d = 500000;
        int closest = 0;

        for (int i = 0; i < path.Length; i++)
        {
            float dist = (transform.position - path[i].position).magnitude;
            if (dist < d)
            {
                closest = i;
                d = dist;
            }
        }
        currentPathPoint = closest;
        navMeshAgent.SetDestination(path[currentPathPoint].position);
    }
    private void UpdatePath()
    {
        if (navMeshAgent.remainingDistance < 1)
        {
            NextPath();
            navMeshAgent.SetDestination(path[currentPathPoint].position);
        }
    }
    private void NextPath()
    {
        currentPathPoint++;
        if (currentPathPoint == path.Length)
        {
            currentPathPoint = 0;
        }
    }

    public void DamageTaken(float damage, Vector3 position, Vector3 force)
    {
        transform.position -= force;
    }
}
