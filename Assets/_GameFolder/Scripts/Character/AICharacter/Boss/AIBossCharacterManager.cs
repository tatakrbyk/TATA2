using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

namespace XD
{
    public class AIBossCharacterManager : AICharacterManager
    {
        public int bossID = 0;

        [Header("Music")]
        [SerializeField] private AudioClip bossIntroClip;
        [SerializeField] private AudioClip bossBattleLoopClip;
        [Header("Status")]
        public NetworkVariable<bool> bossFightIsActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> hasBeenAwakened = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> hasBeenDefeated = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        [SerializeField] private List<FogWallInteractable> fogWalls;
        [SerializeField] private string sleepAnimation;
        [SerializeField] private string awakenAnimation;

        [Header("Phase Shift")]
        public float minHealthPercentageToShift = 50;
        [SerializeField] string phaseShiftAnimation = "Phase_Change_01";
        [SerializeField] CombatStanceState phase02CombatStanceState;

        [Header("States")]
        [SerializeField] BossSleepState sleepState;


        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            bossFightIsActive.OnValueChanged += OnBossFightIsActiveChanged;
            OnBossFightIsActiveChanged(false, bossFightIsActive.Value);

            if (IsOwner)
            {
                sleepState = Instantiate(sleepState);
                currentState = sleepState;

            }
            if (IsServer)
            {
                // IF OUR SAVE DATA DOES NOT CONTAIN INFORMATION ON THIS BOSS, ADD IT NOW
                if (!WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, false);
                    WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated.Add(bossID, false);
                }
                // Otherwise, Load the data that already exist on this boss
                else
                {
                    hasBeenDefeated.Value = WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated[bossID];
                    hasBeenAwakened.Value = WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened[bossID];
                }

                // Set FogWalls
                StartCoroutine(GetFogWallsFromWorldObjectManager());

                if (hasBeenAwakened.Value)
                {
                    for (int i = 0; i < fogWalls.Count; i++)
                    {
                        fogWalls[i].isActive.Value = true;
                    }
                }
                if (hasBeenDefeated.Value)
                {
                    for (int i = 0; i < fogWalls.Count; i++)
                    {
                        fogWalls[i].isActive.Value = false;
                    }
                    aiCharacterNetworkManager.isActive.Value = false;
                }
            }

            if(!hasBeenAwakened.Value)
            {
                animator.Play(sleepAnimation);
            }
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();

            bossFightIsActive.OnValueChanged -= OnBossFightIsActiveChanged;

        }

        private IEnumerator GetFogWallsFromWorldObjectManager()
        {
            while (WorldObjectManager.Instance.fogWalls.Count == 0)
            {
                yield return new WaitForEndOfFrame();
            }

            fogWalls = new List<FogWallInteractable>();
            foreach (var fogWall in WorldObjectManager.Instance.fogWalls)
            {
                if (fogWall.fogWallID == bossID)
                {
                    fogWalls.Add(fogWall);
                }
            }
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            PlayerUIManager.Instance.playerUIPopUpManager.SendBossDefeatedPopUp("Great Foe Felled");
            if (IsOwner)
            {
                characterNetworkManager.currentHealth.Value = 0;
                isDead.Value = true;
                bossFightIsActive.Value = false;

                foreach (var fogWall in fogWalls)
                {
                    fogWall.isActive.Value = false;
                }

                // Reset Any Flags here 

                // if we are not grounded, play an aerial death animation

                if (!manuallySelectDeathAnimation)
                {
                    characterAnimatorManager.PlayActionAnimation("Death_01", true);
                }

                hasBeenDefeated.Value = true;
                // IF OUR SAVE DATA DOES NOT CONTAIN INFORMATION ON THIS BOSS, ADD IT NOW
                if (!WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                    WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated.Add(bossID, true);
                }
                // Otherwise, Load the data that already exist on this boss
                else
                {
                    WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Remove(bossID);
                    WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated.Remove(bossID);
                    WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                    WorldSaveGameManager.Instance.currentCharacterData.bossesDefeated.Add(bossID, true);
                }

                WorldSaveGameManager.Instance.SaveGame();
            }

            // Play Death SFX

            yield return new WaitForSeconds(5f);

            // Award Players With Runes

            // Disable Character
        }

        public void WakeBoss()
        {
            if (IsOwner)
            {
                if (!hasBeenAwakened.Value)
                {
                    characterAnimatorManager.PlayActionAnimation(awakenAnimation, true);
                }
                bossFightIsActive.Value = true;
                hasBeenAwakened.Value = true;
                currentState = idle;
                if (!WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.ContainsKey(bossID))
                {
                    WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                }
                else
                {
                    WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Remove(bossID);
                    WorldSaveGameManager.Instance.currentCharacterData.bossesAwakened.Add(bossID, true);
                }

                for (int i = 0; i < fogWalls.Count; i++)
                {
                    fogWalls[i].isActive.Value = true;
                }
            }
        }

        private void OnBossFightIsActiveChanged(bool oldStatus, bool newStatus)
        {
            if(bossFightIsActive.Value)
            { 
                WorldSoundFXManager.Instance.PlayBossTrack(bossIntroClip, bossBattleLoopClip);

                GameObject bossHealthBar = Instantiate(PlayerUIManager.Instance.playerUIHUDManager.bossHealthBarObject,
                    PlayerUIManager.Instance.playerUIHUDManager.bossHealthBarParent);

                UI_Boss_HP_Bar bossHPBar = bossHealthBar.GetComponentInChildren<UI_Boss_HP_Bar>();
                bossHPBar.EnableBossBar(this);
            }
            else
            {
                WorldSoundFXManager.Instance.StopBossMusic();
            }
        }

        public void PhaseShift()
        {
            characterAnimatorManager.PlayActionAnimation(phaseShiftAnimation, true);
            combatStance = Instantiate(phase02CombatStanceState);
            currentState = combatStance;
        }
    }
}
