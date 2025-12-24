using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace XD
{
    public class AICharacterManager : CharacterManager
    {
        [Header("Character Name")]
        public string characterName = "";   
        [HideInInspector] public AICharacterNetworkManager aiCharacterNetworkManager;
        [HideInInspector] public AICharacterCombatManager aiCharacterCombatManager;
        [HideInInspector] public AICharacterLocomotionManager aICharacterLocomotionManager;
        [HideInInspector] public AICharacterInventoryManager aiCharacterInventoryManager;

        [Header("Navmesh Agent")]
        public NavMeshAgent navMeshAgent;

        [Header("Current State")]
        public AIState currentState;

        [Header("States")]
        public IdleState idle;
        public PursueTargetState pursueTarget;
        public CombatStanceState combatStance;
        public AttackState attack;
        public InvestigateSoundState investigateSoundState;

        [Header("Activation Beacon")]
        [SerializeField] protected AIActivationBeacon beacon;

        protected override void Awake()
        {
            base.Awake();
            aiCharacterCombatManager = GetComponent<AICharacterCombatManager>();
            aiCharacterNetworkManager = GetComponent<AICharacterNetworkManager>();
            aICharacterLocomotionManager = GetComponent<AICharacterLocomotionManager>();
            aiCharacterInventoryManager = GetComponent<AICharacterInventoryManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
        }

        protected override void Start()
        {
            base.Start();
            // If the animator or Gameobject becomes disabl, we will keep our current animaton when re-enabled
            // This is especially useful for disabling enemies that are far away, and re-enabling them later keeping them in spesific states (like sleep or dead)
            animator.keepAnimatorStateOnDisable = true;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            if (beacon != null) Destroy(beacon);
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            if (IsOwner)
            {
                // NOTE(Taha): Using of SO multiple AI, So we need to copy SO per AI
                // Use a copy of the scriptable objects, so the originals are not modified
                idle = Instantiate(idle);
                pursueTarget = Instantiate(pursueTarget);
                combatStance = Instantiate(combatStance);
                attack = Instantiate(attack);
                investigateSoundState = Instantiate(investigateSoundState);
                currentState = idle;
            }

            aiCharacterNetworkManager.currentHealth.OnValueChanged += aiCharacterNetworkManager.OnHPChanged;
            
            if(!aiCharacterNetworkManager.isAwake.Value)
                animator.Play(aiCharacterNetworkManager.sleepingAnimation.Value.ToString());

            if(isDead.Value)
                animator.Play("Dead_01");

            CreateActivationBeacon();
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            aiCharacterNetworkManager.currentHealth.OnValueChanged -= aiCharacterNetworkManager.OnHPChanged;    
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            if(characterUIManager.hasFloatingHPBar)
            {
                characterNetworkManager.currentHealth.OnValueChanged += characterUIManager.OnHPChanged;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            if (characterUIManager.hasFloatingHPBar)
            {
                characterNetworkManager.currentHealth.OnValueChanged -= characterUIManager.OnHPChanged;
            }
        }

        protected override void Update()
        {
            base.Update();

            aiCharacterCombatManager.HandleActionRecovery(this);

            if (navMeshAgent == null) return;

            if (IsOwner)
            {
                ProcessStateMachine();
            }

            if (!navMeshAgent.enabled) return;

            Vector3 positionDifference = navMeshAgent.transform.position - transform.position;

            if(positionDifference.magnitude > 0.2f)
            {
                navMeshAgent.transform.localPosition = Vector3.zero;
            }


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
                    aiCharacterNetworkManager.isMoving.Value = true;
                }
                else
                {
                    aiCharacterNetworkManager.isMoving.Value = false;
                }
            }
            else
            {
                aiCharacterNetworkManager.isMoving.Value = false;
            }
        }

        // Activation 
        public void ActivateCharacter(PlayerManager player)
        {
            aiCharacterCombatManager.AddPlayerToPlayersWithinRange(player);

            if(player.IsLocalPlayer)
            {
                // Enable Renderers (Optionally)
                // Renderers can be disabled for other players Not near this ai, this will save on memory
            }

            if (!NetworkManager.Singleton.IsHost) return;
            
            if(aiCharacterCombatManager.playerWithinActivationRange.Count > 0)
            {
                aiCharacterNetworkManager.isActive.Value = true;
            }
            else
            {
                aiCharacterNetworkManager.isActive.Value = false;
            }
        }

        public void DeactivateCharacter(PlayerManager player)
        {
            aiCharacterCombatManager.RemovePlayerFromPlayersWithinRange(player);

            if (player.IsLocalPlayer)
            {
                // Disable Renderers (Optionally)
                // Renderers can be disabled for other players Not near this ai, this will save on memory
            }

            if(beacon != null)
            {
                beacon.gameObject.transform.position = transform.position;
                beacon.gameObject.SetActive(true);
            }
            // Drop a beacon on this transform (when coming into contanct with it, it will re-enable the ai)
            if (!NetworkManager.Singleton.IsHost) return;

            if (aiCharacterCombatManager.playerWithinActivationRange.Count > 0)
            {
                aiCharacterNetworkManager.isActive.Value = true;
            }
            else
            {
                aiCharacterCombatManager.SetTarget(null);
                aiCharacterNetworkManager.isActive.Value = false;
            }
        }

        public void CreateActivationBeacon()
        {
            if(beacon == null)
            {
                GameObject beaconObject = Instantiate(WorldAIManager.Instance.beaconGameobejct);
                beacon.transform.position = transform.position;

                beacon = beaconObject.GetComponent<AIActivationBeacon>();
                beacon.SetOwnerOfBeacon(this);
            }
            else
            {
                beacon.transform.position = transform.position;
                beacon.gameObject.SetActive(true);
            }
        }
    }

}
