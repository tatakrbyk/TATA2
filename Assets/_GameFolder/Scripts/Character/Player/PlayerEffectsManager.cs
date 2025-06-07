using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XD;

namespace XD
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        [Header("DEBUG")]
        [SerializeField] InstantCharacterEffect effectToTest;
        [SerializeField] bool processEffect = false;

        private void Update()
        {
            if (processEffect)
            {
                processEffect = false;
                InstantCharacterEffect effect = Instantiate(effectToTest); 
                ProcessInstantEffect(effect);
            }
        }
    }

}
