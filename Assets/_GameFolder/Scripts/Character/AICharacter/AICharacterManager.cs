using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace XD
{
    public class AICharacterManager : CharacterManager
    {
        [HideInInspector] public AICharacterNetworkManager aICharacterNetworkManager;
        [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
        [HideInInspector] public AICharacterLocomotionManager aICharacterLocomotionManager;

        [Header("Navmesh Agent")]
        public NavMeshAgent navMeshAgent;

        [Header("Current State")]
        [SerializeField] private AIState currentState;

        [Header("States")]
        public IdleState idle;
        public PursueTargetState pursueTarget;
        public CombatStanceState combbatStance;
        public AttackState attack;



        protected override void Awake()
        {
            base.Awake();
            aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
            aICharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
            aICharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();

            // NOTE(Taha): Using of SO multiple AI, So we need to copy SO per AI
            // Use a copy of the scriptable objects, so the originals are not modified
            idle = Instantiate(idle);
            pursueTarget = Instantiate(pursueTarget);

            currentState = idle;
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if(IsOwner)
            {
                ProcessStateMachine();

            }
        }

        protected override void Update()
        {
            base.Update();

            aiCharacterCombatManager.HandleActionRecovery(this);
        }
        private void ProcessStateMachine()
        {
            AIState nextState = currentState?.Tick(this);
            if(nextState != null)
            {
                currentState = nextState;
            }

            // The Position/Rotation should be reset only after the state machine has processed it's tick
            navMeshAgent.transform.localPosition = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;

            if(aiCharacterCombatManager.currentTarget != null)
            {
                aiCharacterCombatManager.targetsDirection = aiCharacterCombatManager.currentTarget.transform.position - transform.position;
                aiCharacterCombatManager.viewableAngle = WorldUtilityManager.Instance.GetAngleOfTarget(transform, aiCharacterCombatManager.targetsDirection);
                aiCharacterCombatManager.distanceFromTarget = Vector3.Distance(transform.position, aiCharacterCombatManager.currentTarget.transform.position);
            }
            if (navMeshAgent.enabled)
            {
                Vector3 agentDestination = navMeshAgent.destination;
                float remainingDistance = Vector3.Distance(agentDestination, transform.position);

                if(remainingDistance > navMeshAgent.stoppingDistance)
                {
                    aICharacterNetworkManager.isMoving.Value = true;
                }
                else
                {
                    aICharacterNetworkManager.isMoving.Value = false;
                }
            }
            else
            {
                aICharacterNetworkManager.isMoving.Value = false;
            }
        }
    }

}
