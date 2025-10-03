using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class SpellManager : MonoBehaviour
    {
        [Header("Spell Target")]
        [SerializeField] protected CharacterManager spellTarget;

        [Header("VFX")]
        [SerializeField] protected GameObject impactParticle;
        [SerializeField] protected GameObject impactParticleFullCharge;

        protected virtual void Awake()
        {
        }
        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
        }
    }
}
