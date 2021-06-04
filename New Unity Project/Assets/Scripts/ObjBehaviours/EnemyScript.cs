using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AIState
{
    Idle,
    Path,
    Attacking,
    GotoInterest

}
interface IListener
{
    abstract void HearSound(float interest, Vector3 position);
}
public class EnemyScript : MonoBehaviour, IDamageInfo, IListener
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
    [SerializeField] float health = 100f;
    [SerializeField] GameObject rotateThisObject;
    [SerializeField] float maxLookRotation = 45;
    float sawLastTimer = 0;
    bool stunned = false;
    [SerializeField] float stunTime = 2;
    float stunTimer;
    [SerializeField] float maxHealth = 100;
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
        if (stunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0) stunned = false;
            else return;
        }
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
            case AIState.GotoInterest:
                UpdateGoto();
                break;
            default:
                break;
        }
    }
    private void UpdateAttacking()
    {

        navMeshAgent.updateRotation = true;
        float dist = (target.transform.position - transform.position).magnitude;
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0 && dist < 3 )
        {
            RaycastHit[] rhits =Physics.SphereCastAll(attackCenter.transform.position,1, Vector3.up);
            for (int i = 0; i < rhits.Length; i++)
            {
                IDamageInfo dmg = rhits[i].transform.GetComponent<IDamageInfo>();
                if (dmg != null && dmg != this)
                {
                    dmg.DamageTaken(20f, Vector3.zero, transform.forward * 20);
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
        if (dist < 1.5)
        {
            navMeshAgent.updateRotation = false;
            Vector3 dir = target.transform.position - transform.position;
            dir = new Vector3(dir.x, 0, dir.z).normalized;
            float angle = Vector3.SignedAngle(rotateThisObject.transform.forward, dir, Vector3.up);
            float rotation = angle * Time.deltaTime * 3;
            if (Mathf.Abs(rotation) > Mathf.Abs(angle)) rotation = angle;
            transform.Rotate(transform.up, rotation);
            
        }
        else if (dist > 2f)
        {
            navMeshAgent.updateRotation = true;
        }
        UpdateMyEyes();
        
    }
    private void UpdateGoto()
    {
        if (navMeshAgent.remainingDistance < .5f && navMeshAgent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathComplete)
        {
            state = AIState.Path;
        }
    }
    private void UpdateMyEyes()
    {
        Vector3 dir = target.transform.position - transform.position;
        dir = new Vector3(dir.x, 0, dir.z).normalized;
        float angle = Vector3.SignedAngle(rotateThisObject.transform.forward, dir, Vector3.up);
        float rotation = angle * Time.deltaTime * 3;
        if (Mathf.Abs(rotation) > Mathf.Abs(angle)) rotation = angle;
        rotateThisObject.transform.Rotate(transform.up, rotation);
        Vector3 resultRotation = rotateThisObject.transform.localEulerAngles;
        if (resultRotation.y > 180 && resultRotation.y < 360 - maxLookRotation)
        {
            resultRotation.y = 360 - maxLookRotation;
        }
        else if (resultRotation.y < 180 && resultRotation.y > maxLookRotation)
        {
            resultRotation.y = maxLookRotation;
        }
        rotateThisObject.transform.localRotation = Quaternion.Euler(resultRotation);
    }
    private void UpdateLookingForStuff()
    {
        sawLastTimer += Time.deltaTime;
        if (sawLastTimer >= losePlayerTime && state == AIState.Attacking)
        {
            SetPathState();
        }

        Vector3 direction = target.transform.position - head.transform.position;
        float angularDifference = Vector3.Angle(direction.normalized, transform.forward);
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
        if (health <= 0)
        {
            health -= damage;
            navMeshAgent.ResetPath();
            stunned = true;
            stunTimer = stunTime;
            health = maxHealth;
        }
    }

    public void HearSound(float interest, Vector3 position)
    {
        if (state != AIState.Attacking)
        {
            state = AIState.GotoInterest;
            navMeshAgent.SetDestination(position);
        }
    }
}
