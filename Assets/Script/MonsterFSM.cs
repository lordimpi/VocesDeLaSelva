using UnityEngine;
using UnityEngine.AI;

public class MonsterFSM : MonoBehaviour
{
    public MonsterState currentState;

    public Transform objetivo;
    public Animator animator;
    public NavMeshAgent agent;

    [Header("Rangos")]
    public float visionRange = 15f;
    public float attackRange = 2f;

    [Header("Velocidades")]
    public float patrolSpeed = 2f;
    public float chaseSpeed = 6f;

    [Header("Patrullaje")]
    public Transform[] patrolPoints;
    private int patrolIndex = 0;

    [Header("Ataque")]
    private float attackCooldown = 1.5f;
    private float lastAttackTime = -999f;
    private int intentosFallidos = 0;
    public int maxIntentosFallidos = 3;

    [Header("Jugador")]
    public bool jugadorMuerto = false;

    private bool isDead = false;

    private void Start()
    {
        ChangeState(MonsterState.Patrol);
    }

    private void Update()
    {
        // Simular muerte del jugador para pruebas
        if (Input.GetKeyDown(KeyCode.K))
        {
            jugadorMuerto = true;
            Debug.Log("Jugador muerto");
        }

        if (isDead) return;

        float distance = Vector3.Distance(transform.position, objetivo.position);

        switch (currentState)
        {
            case MonsterState.Idle:
                if (jugadorMuerto) return;

                if (distance < visionRange)
                    ChangeState(MonsterState.Chase);
                break;

            case MonsterState.Patrol:
                if (jugadorMuerto) return;

                Patrol();

                if (distance < visionRange)
                    ChangeState(MonsterState.Chase);
                break;

            case MonsterState.Chase:
                if (jugadorMuerto)
                {
                    ChangeState(MonsterState.Idle);
                    return;
                }

                agent.SetDestination(objetivo.position);

                if (distance <= attackRange)
                    ChangeState(MonsterState.Attack);
                else if (distance > visionRange + 3)
                    ChangeState(MonsterState.Patrol);
                break;

            case MonsterState.Attack:
                if (jugadorMuerto)
                {
                    ChangeState(MonsterState.Idle);
                    return;
                }

                FaceTarget();

                if (distance > attackRange)
                {
                    ChangeState(MonsterState.Chase);
                    return;
                }

                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    animator.SetTrigger("attack10");
                    lastAttackTime = Time.time;
                    intentosFallidos++;

                    if (intentosFallidos >= maxIntentosFallidos)
                    {
                        ChangeState(MonsterState.Idle);
                        return;
                    }
                }
                break;

            case MonsterState.Dead:
                break;
        }
    }

    void ChangeState(MonsterState newState)
    {
        currentState = newState;
        intentosFallidos = 0;

        switch (newState)
        {
            case MonsterState.Idle:
                agent.speed = 0;
                animator.ResetTrigger("walk2");
                animator.ResetTrigger("run3");
                animator.ResetTrigger("attack10");
                animator.SetTrigger("Idle1");
                break;

            case MonsterState.Patrol:
                agent.speed = patrolSpeed;
                agent.SetDestination(patrolPoints[patrolIndex].position);
                animator.ResetTrigger("Idle1");
                animator.ResetTrigger("run3");
                animator.ResetTrigger("attack10");
                animator.SetTrigger("walk2");
                break;

            case MonsterState.Chase:
                agent.speed = chaseSpeed;
                animator.ResetTrigger("Idle1");
                animator.ResetTrigger("walk2");
                animator.ResetTrigger("attack10");
                animator.SetTrigger("run3");
                break;

            case MonsterState.Attack:
                agent.speed = 0;
                animator.ResetTrigger("run3");
                animator.ResetTrigger("Idle1");
                animator.ResetTrigger("walk2");
                // El trigger "attack10" se lanza con cooldown dentro de Update
                break;

            case MonsterState.Dead:
                agent.isStopped = true;
                animator.SetTrigger("death");
                isDead = true;
                break;
        }
    }

    void Patrol()
    {
        if (agent.remainingDistance < 0.5f)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[patrolIndex].position);
        }
    }

    void FaceTarget()
    {
        Vector3 dir = (objetivo.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}

public enum MonsterState
{
    Idle,
    Patrol,
    Chase,
    Attack,
    Dead
}