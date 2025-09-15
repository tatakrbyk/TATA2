using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class WorldUtilityManager : MonoBehaviour
    {
        private static WorldUtilityManager instance; public static WorldUtilityManager Instance { get { return instance; } }

        [SerializeField] private LayerMask characterLayer;
        [SerializeField] private LayerMask environmentLayers;
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public LayerMask GetCharacterLayer()
        {
            return characterLayer;
        }
        public LayerMask GetEnvironmentLayers()
        {
            return environmentLayers;
        }

        public bool CanIDamageThisTarget(CharacterGroup attackingCharacter, CharacterGroup targetCharacter)
        {
            if (attackingCharacter == CharacterGroup.Team01)
            {
                switch (targetCharacter)
                {
                    case CharacterGroup.Team01: return false;
                    case CharacterGroup.Team02: return true;
                    default:
                        break;
                }
            }
            else if (attackingCharacter == CharacterGroup.Team02)
            {
                switch (targetCharacter)
                {
                    case CharacterGroup.Team01: return true;
                    case CharacterGroup.Team02: return false;
                    default:
                        break;
                }
            }

            return false;
        }
        public float GetAngleOfTarget(Transform characterTransform, Vector3 targetDirection)
        {
            targetDirection.y = 0;
            float viewableAngle = Vector3.Angle(characterTransform.forward, targetDirection);
            Vector3 cross = Vector3.Cross(characterTransform.forward, targetDirection);

            if(cross.y < 0) viewableAngle = -viewableAngle;
            return viewableAngle;
        }

        public DamageIntensity GetDamageIntensityBasedOnPoiseDamage(float poiseDamage)
        {
            DamageIntensity damageIntensity = DamageIntensity.Ping;

            if(poiseDamage >= 10)
                damageIntensity = DamageIntensity.Light;
            if (poiseDamage >= 30)
                damageIntensity = DamageIntensity.Medium;
            if (poiseDamage >= 70)
                damageIntensity = DamageIntensity.Heavy;
            if (poiseDamage >= 120)
                damageIntensity = DamageIntensity.Colossal;
            
            return damageIntensity;
        }

        public Vector3 GetRipostingPositionBasedOnWeaponClass(WeaponClass weaponClass)
        {
            Vector3 position = new Vector3(0.11f, 0, 0.7f);

            switch(weaponClass)
            {
                case WeaponClass.StraightSword:      // Change position here if you desire
                    break;
                case WeaponClass.Spear:              // Change position here if you desire
                    break;
                case WeaponClass.MediumShield:       // Change position here if you desire
                    break;
                case WeaponClass.Fist:               // Change position here if you desire
                    break;
                default:
                    break;
            }

            return position;
        }
    }

}
