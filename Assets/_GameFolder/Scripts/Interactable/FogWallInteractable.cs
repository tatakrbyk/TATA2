using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

namespace XD
{
    public class FogWallInteractable : Interactable
    {
        [Header("Fog")]
        [SerializeField] GameObject[] fogGameObjects;

        [Header("Collision")]
        [SerializeField] Collider fogWallCollider;

        [Header("ID")]
        public int fogWallID;

        [Header("Sound")]
        private AudioSource fogWallAudioSource;
        [SerializeField] AudioClip fogWallSFX;

        [Header("Active")]
        public NetworkVariable<bool> isActive = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

        protected override void Awake()
        {
            base.Awake();

            fogWallAudioSource = gameObject.GetComponent<AudioSource>();
        }
        public override void Interact(PlayerManager player)
        {
            base.Interact(player);

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.right);
            player.transform.rotation = targetRotation; 

            AllowPlayerThroughFogWallColliderServerRpc(player.NetworkObjectId);
            player.playerAnimatorManager.PlayActionAnimation("Pass_Through_Fog_01", true);
        }
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            OnIsActiveChanged(false, isActive.Value);
            isActive.OnValueChanged += OnIsActiveChanged;
            WorldObjectManager.Instance.AddFogWallToList(this);
            
        }

        public override void OnNetworkDespawn()
        {
            base.OnNetworkDespawn();
            
            isActive.OnValueChanged -= OnIsActiveChanged;
            WorldObjectManager.Instance.RemoveFogWallToList(this);
        }
        private void OnIsActiveChanged(bool oldStatus, bool newStatus)
        {
            if(isActive.Value)
            {
                foreach (var fogObject in fogGameObjects)
                {
                    fogObject.SetActive(true);
                }
            }
            else
            { 
                foreach (var fogObject in fogGameObjects)
                {
                    fogObject.SetActive(false);
                }
            }
        }

        // When a server rpc does not require ownership, a non owner can activate the function (client player does not own fog wall, as they are not the host)
        [ServerRpc(RequireOwnership = false)]
        private void AllowPlayerThroughFogWallColliderServerRpc(ulong playerObjectID)
        {
            if(IsServer)
            {
                AllowPlayerThroughFogWallColliderClientRpc(playerObjectID);
            }
        }
        [ClientRpc]
        private void AllowPlayerThroughFogWallColliderClientRpc(ulong playerObjectID)
        {
            PlayerManager player = NetworkManager.Singleton.SpawnManager.SpawnedObjects[playerObjectID].GetComponent<PlayerManager>();

            fogWallAudioSource.PlayOneShot(fogWallSFX); 
            if (player != null)
            {
                StartCoroutine(DisableCollisionForTime(player));
            }
        }
        private IEnumerator DisableCollisionForTime(PlayerManager player)
        {
            Physics.IgnoreCollision(player.characterController, fogWallCollider, true);
            yield return new WaitForSeconds(3f);
            Physics.IgnoreCollision(player.characterController, fogWallCollider, false);
        }

    }

}
