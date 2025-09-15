using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XD
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        PlayerManager player;

        [Header("Weapon Model Instantiation Slots")]
        [HideInInspector] public WeaponModelInstantiationSlot rightHandWeaponSlot;
        [HideInInspector] public WeaponModelInstantiationSlot leftHandWeaponSlot;
        [HideInInspector] public WeaponModelInstantiationSlot leftHandShieldSlot;
        [HideInInspector] public WeaponModelInstantiationSlot backSlot;

        [Header("Weapon Managers")]
        public WeaponManager rightWeaponManager;
        public WeaponManager leftWeaponManager;

        [Header("Weapon Models")]
        [HideInInspector] public GameObject rightHandWeaponModel;
        [HideInInspector] public GameObject leftHandWeaponModel;

        [SerializeField] bool DebugEquipNewItems = false;

        [Header("General Equipment  Models")]
        public GameObject hatsObject;
        [HideInInspector] public GameObject[] hats;
        public GameObject hoodsObject;   
        [HideInInspector] public GameObject[] hoods;
        public GameObject faceCoversObject;
        [HideInInspector] public GameObject[] faceCovers;
        public GameObject helmetAccessoriesObject;
        [HideInInspector] public GameObject[] helmetAccessories;
        public GameObject backAccessoriesObject;
        [HideInInspector] public GameObject[] backAccessories;
        public GameObject hipAccessoriesObject;
        [HideInInspector] public GameObject[] hipAccessories;
        public GameObject rightShoulderObject;
        [HideInInspector] public GameObject[] rightShoulders;
        public GameObject rightElbowObject;
        [HideInInspector] public GameObject[] rightElbows;
        public GameObject rightKneeObject;
        [HideInInspector] public GameObject[] rightKnees;
        public GameObject leftShoulderObject;
        [HideInInspector] public GameObject[] leftShoulders;
        public GameObject leftElbowObject;
        [HideInInspector] public GameObject[] leftElbows;
        public GameObject leftKneeObject;
        [HideInInspector] public GameObject[] leftKnees;

        [Header("Male Equipment Models")]
        public GameObject maleFullHelmetObject;
        [HideInInspector] public GameObject[] maleHeadFullHelmets;
        public GameObject maleFullBodyObject;
        [HideInInspector] public GameObject[] maleBodies;
        public GameObject maleRightUpperArmObject;
        [HideInInspector] public GameObject[] maleRightUpperArms;
        public GameObject maleRightLowerArmObject;
        [HideInInspector] public GameObject[] maleRightLowerArms;
        public GameObject maleRightHandObject;
        [HideInInspector] public GameObject[] maleRightHands;
        public GameObject maleLeftUpperArmObject;
        [HideInInspector] public GameObject[] maleLeftUpperArms;
        public GameObject maleLeftLowerArmObject;   
        [HideInInspector] public GameObject[] maleLeftLowerArms;
        public GameObject maleLeftHandObject;
        [HideInInspector] public GameObject[] maleLeftHands;
        public GameObject maleHipsObject;
        [HideInInspector] public GameObject[] maleHips;
        public GameObject maleRightLegObject;
        [HideInInspector] public GameObject[] maleRightLegs;
        public GameObject maleLeftLegObject;
        [HideInInspector] public GameObject[] maleLeftLegs;

        [Header("Female Equipment Models")]
        public GameObject femaleFullHelmetObject;
        [HideInInspector] public GameObject[] femaleHeadFullHelmets;
        public GameObject femaleFullBodyObject;
        [HideInInspector] public GameObject[] femaleBodies;
        public GameObject femaleRightUpperArmObject;
        [HideInInspector] public GameObject[] femaleRightUpperArms;
        public GameObject femaleRightLowerArmObject;
        [HideInInspector] public GameObject[] femaleRightLowerArms;
        public GameObject femaleRightHandObject;
        [HideInInspector] public GameObject[] femaleRightHands;
        public GameObject femaleLeftUpperArmObject;
        [HideInInspector] public GameObject[] femaleLeftUpperArms;
        public GameObject femaleLeftLowerArmObject;
        [HideInInspector] public GameObject[] femaleLeftLowerArms;
        public GameObject femaleLeftHandObject;
        [HideInInspector] public GameObject[] femaleLeftHands;
        public GameObject femaleHipsObject;
        [HideInInspector] public GameObject[] femaleHips;
        public GameObject femaleRightLegObject;
        [HideInInspector] public GameObject[] femaleRightLegs;
        public GameObject femaleLeftLegObject;
        [HideInInspector] public GameObject[] femaleLeftLegs;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

            InitializeWeaponSlots();
            InitializeArmorModels();
        }

        protected override void Start()
        {
            base.Start();

            EquipWeapons();
        }
        private void Update()
        {
            if (DebugEquipNewItems)
            {
                DebugEquipNewItems = false;
                EquipArmors();
            }
        }
        public void EquipArmors()
        {
            LoadHeadEquipment(player.playerInventoryManager.headEquipment);
            LoadBodyEquipment(player.playerInventoryManager.bodyEquipment);
            LoadLegEquipment(player.playerInventoryManager.legEquipment);
            LoadHandEquipment(player.playerInventoryManager.handEquipment);
        }

        private void InitializeArmorModels()
        {
            #region UNI-SEX Models
            // HATS
            List<GameObject> hatsList = new List<GameObject>();
            foreach (Transform child in hatsObject.transform)
            {
                hatsList.Add(child.gameObject);
            }
            hats = hatsList.ToArray();

            // HOODS
            List<GameObject> hoodsList = new List<GameObject>();
            foreach (Transform child in hoodsObject.transform)
            {
                hoodsList.Add(child.gameObject);
            }
            hoods = hoodsList.ToArray();

            // FACE COVERS
            List<GameObject> faceCoversList = new List<GameObject>();
            foreach (Transform child in faceCoversObject.transform)
            {
                faceCoversList.Add(child.gameObject);
            }
            faceCovers = faceCoversList.ToArray();

            // HELMET ACCESSORIES
            List<GameObject> helmetAccessoriesList = new List<GameObject>();
            foreach (Transform child in helmetAccessoriesObject.transform)
            {
                helmetAccessoriesList.Add(child.gameObject);
            }
            helmetAccessories = helmetAccessoriesList.ToArray();

            // BACK ACCESSORIES
            List<GameObject> backAccessoriesList = new List<GameObject>();
            foreach (Transform child in backAccessoriesObject.transform)
            {
                backAccessoriesList.Add(child.gameObject);
            }
            backAccessories = backAccessoriesList.ToArray();

            // HIP ACCESSORIES
            List<GameObject> hipAccessoriesList = new List<GameObject>();
            foreach (Transform child in hipAccessoriesObject.transform)
            {
                hipAccessoriesList.Add(child.gameObject);
            }
            hipAccessories = hipAccessoriesList.ToArray();

            // RIGHT SHOULDER
            List<GameObject> rightShouldersList = new List<GameObject>();
            foreach (Transform child in rightShoulderObject.transform)
            {
                rightShouldersList.Add(child.gameObject);
            }
            rightShoulders = rightShouldersList.ToArray();

            // RIGHT ELBOW
            List<GameObject> rightElbowsList = new List<GameObject>();
            foreach (Transform child in rightElbowObject.transform)
            {
                rightElbowsList.Add(child.gameObject);
            }
            rightElbows = rightElbowsList.ToArray();

            // RIGHT KNEE
            List<GameObject> rightKneesList = new List<GameObject>();
            foreach (Transform child in rightKneeObject.transform)
            {
                rightKneesList.Add(child.gameObject);
            }
            rightKnees = rightKneesList.ToArray();

            // LEFT SHOULDER
            List<GameObject> leftShouldersList = new List<GameObject>();
            foreach (Transform child in leftShoulderObject.transform)
            {
                leftShouldersList.Add(child.gameObject);
            }
            leftShoulders = leftShouldersList.ToArray();
            // LEFT ELBOW
            List<GameObject> leftElbowsList = new List<GameObject>();
            foreach (Transform child in leftElbowObject.transform)
            {
                leftElbowsList.Add(child.gameObject);
            }
            leftElbows = leftElbowsList.ToArray();
            
            // LEFT KNEE
            List<GameObject> leftKneesList = new List<GameObject>();
            foreach (Transform child in leftKneeObject.transform)
            {
                leftKneesList.Add(child.gameObject);
            }
            leftKnees = leftKneesList.ToArray();
            #endregion
            #region Male Models
            // Male Equipment
            List<GameObject> maleFullHelmetList = new List<GameObject>();
            foreach (Transform child in maleFullHelmetObject.transform)
            {
                maleFullHelmetList.Add(child.gameObject);
            }
            maleHeadFullHelmets = maleFullHelmetList.ToArray();

            List<GameObject> maleBodiesList = new List<GameObject>();
            foreach (Transform child in maleFullBodyObject.transform)
            {
                maleBodiesList.Add(child.gameObject);
            }
            maleBodies = maleBodiesList.ToArray();

            // Male Right Upper Arm
            List<GameObject> maleRightUpperArmsList = new List<GameObject>();
            foreach(Transform child in maleRightUpperArmObject.transform)
            {
                maleRightUpperArmsList.Add(child.gameObject);
            }
            maleRightUpperArms = maleRightUpperArmsList.ToArray();

            // Male Right Lower Arm
            List<GameObject> maleRightLowerArmsList = new List<GameObject>();
            foreach(Transform child in maleRightLowerArmObject.transform)
            {
                maleRightLowerArmsList.Add(child.gameObject);
            }
            maleRightLowerArms = maleRightLowerArmsList.ToArray();

            // Male Right Hand
            List<GameObject> maleRightHandsList = new List<GameObject>();
            foreach(Transform child in maleRightHandObject.transform)
            {
                maleRightHandsList.Add(child.gameObject);
            }
            maleRightHands = maleRightHandsList.ToArray();

            // Male Left Upper Arm
            List<GameObject> maleLeftUpperArmsList = new List<GameObject>();
            foreach(Transform child in maleLeftUpperArmObject.transform)
            {
                maleLeftUpperArmsList.Add(child.gameObject);
            }
            maleLeftUpperArms = maleLeftUpperArmsList.ToArray();

            // Male Left Lower Arm
            List<GameObject> maleLeftLowerArmsList = new List<GameObject>();
            foreach(Transform child in maleLeftLowerArmObject.transform)
            {
                maleLeftLowerArmsList.Add(child.gameObject);
            }
            maleLeftLowerArms = maleLeftLowerArmsList.ToArray();

            // Male Left Hand
            List<GameObject> maleLeftHandsList = new List<GameObject>();
            foreach(Transform child in maleLeftHandObject.transform)
            {
                maleLeftHandsList.Add(child.gameObject);
            }
            maleLeftHands = maleLeftHandsList.ToArray();

            // Male Hip
            List<GameObject> maleHipsList = new List<GameObject>();
            foreach (Transform child in maleHipsObject.transform)
            {
                maleHipsList.Add(child.gameObject);
            }
            maleHips = maleHipsList.ToArray();

            // Male Right Leg
            List<GameObject> maleRightLegsList = new List<GameObject>();
            foreach(Transform child in maleRightLegObject.transform)
            {
                maleRightLegsList.Add(child.gameObject);
            }
            maleRightLegs = maleRightLegsList.ToArray();

            // Male Left Leg
            List<GameObject> maleLeftLegsList = new List<GameObject>();
            foreach(Transform child in maleLeftLegObject.transform)
            {
                maleLeftLegsList.Add(child.gameObject);
            }
            maleLeftLegs = maleLeftLegsList.ToArray();
            #endregion
            #region Female Models

            // Female Full Helmet
            List<GameObject> femaleFullHelmetsList = new List<GameObject>();
            foreach(Transform child in femaleFullHelmetObject.transform)
            {
                femaleFullHelmetsList.Add(child.gameObject);
            }
            femaleHeadFullHelmets = femaleFullHelmetsList.ToArray();

            // Female Body
            List<GameObject> femaleBodiesList = new List<GameObject>();
            foreach(Transform child in femaleFullBodyObject.transform)
            {
                femaleBodiesList.Add(child.gameObject);
            }
            femaleBodies = femaleBodiesList.ToArray();

            // Female Right Upper Arm
            List<GameObject> femaleRightUpperArmsList = new List<GameObject>();
            foreach (Transform child in femaleRightUpperArmObject.transform)
            {
                femaleRightUpperArmsList.Add(child.gameObject);
            }
            femaleRightUpperArms = femaleRightUpperArmsList.ToArray();

            // Female Right Lower Arm
            List<GameObject> femaleRightLowerArmsList = new List<GameObject>();
            foreach (Transform child in femaleRightLowerArmObject.transform)
            {
                femaleRightLowerArmsList.Add(child.gameObject);
            }
            femaleRightLowerArms = femaleRightLowerArmsList.ToArray();

            // Female Right Hand
            List<GameObject> femaleRightHandsList = new List<GameObject>();
            foreach(Transform child in femaleRightHandObject.transform)
            {
                femaleRightHandsList.Add(child.gameObject);
            }
            femaleRightHands = femaleRightHandsList.ToArray();

            // Female Left Hand
            List<GameObject> femaleLeftHandsList = new List<GameObject>();
            foreach(Transform child in femaleLeftHandObject.transform)
            {
                femaleLeftHandsList.Add(child.gameObject);
            }
            femaleLeftHands = femaleLeftHandsList.ToArray();

            // Female Left Upper Arm
            List<GameObject> femaleLeftUpperArmsList = new List<GameObject>();
            foreach (Transform child in femaleLeftUpperArmObject.transform)
            {
                femaleLeftUpperArmsList.Add(child.gameObject);
            }
            femaleLeftUpperArms = femaleLeftUpperArmsList.ToArray();

            // Female Left Lower Arm
            List<GameObject> femaleLeftLowerArmsList = new List<GameObject>();
            foreach (Transform child in femaleLeftLowerArmObject.transform)
            {
                femaleLeftLowerArmsList.Add(child.gameObject);
            }
            femaleLeftLowerArms = femaleLeftLowerArmsList.ToArray();
            
            // Female Hip
            List<GameObject> femaleHipsList = new List<GameObject>();
            foreach(Transform child in femaleHipsObject.transform)
            {
                femaleHipsList.Add(child.gameObject);
            }
            femaleHips = femaleHipsList.ToArray();

            // Female Right Leg
            List<GameObject> femaleRightLegsList = new List<GameObject>();
            foreach(Transform child in femaleRightLegObject.transform)
            {
                femaleRightLegsList.Add(child.gameObject);
            }
            femaleRightLegs = femaleRightLegsList.ToArray();

            // Female Left Leg
            List<GameObject> femaleLeftLegsList = new List<GameObject>();
            foreach(Transform child in femaleLeftLegObject.transform)
            {
                femaleLeftLegsList.Add(child.gameObject);
            }
            femaleLeftLegs = femaleLeftLegsList.ToArray();

            #endregion

        }
        public void LoadHeadEquipment(HeadEquipmentItem equipment)
        {
            UnloadHeadEquipmentModels();
            
            if(equipment == null)
            {
                if (player.IsOwner)
                {
                    player.playerNetworkManager.headEquipmentID.Value = -1; // -1 Will never be an item ID, So it will always be null
                }

                player.playerInventoryManager.headEquipment = null;
                return;
            }

            player.playerInventoryManager.headEquipment = equipment;

            switch(equipment.headEquipmentType)
            {
                case HeadEquipmentType.FullHelmet:
                    player.playerBodyManager.DisableHair();
                    player.playerBodyManager.DisableHead();
                    break;
                case HeadEquipmentType.Hat:
                    break;
                case HeadEquipmentType.Hood:
                    player.playerBodyManager.DisableHair();
                    break;
                case HeadEquipmentType.FaceCover:
                    player.playerBodyManager.DisableFacialHair();
                    break;
                default:
                    break;
            }

            foreach(var model in equipment.equipmentModels)
            {
                model.LoadModel(player, player.playerNetworkManager.isMale.Value);
            }

            player.playerStatsManager.CalculateTotalArmorAbsorption();

            if (player.IsOwner)
            {
                player.playerNetworkManager.headEquipmentID.Value = equipment.itemID;
            }
        }

        private void UnloadHeadEquipmentModels()
        {
            foreach(var model in maleHeadFullHelmets)
            {
                model.SetActive(false);
            }

            foreach (var model in femaleHeadFullHelmets)
            {
                model.SetActive(false);
            }

            foreach (var model in hats)
            {
                model.SetActive(false);
            }
            foreach (var model in hoods)
            {
                model.SetActive(false);
            }
            foreach (var model in faceCovers)
            {
                model.SetActive(false);
            }
            foreach (var model in helmetAccessories)
            {
                model.SetActive(false);
            }

            player.playerBodyManager.EnableHead();
            player.playerBodyManager.EnableHair();

            // RE-Enable Hair and Head
        }
        public void LoadBodyEquipment(BodyEquipmentItem equipment)
        {
            UnloadBodyEquipmentModel();
            if (equipment == null)
            {
                if (player.IsOwner)
                {
                    player.playerNetworkManager.bodyEquipmentID.Value = -1; // -1 Will never be an item ID, So it will always be null
                }
                player.playerInventoryManager.bodyEquipment = null;
                return;
            }
            player.playerInventoryManager.bodyEquipment = equipment;
            player.playerBodyManager.DisableBody();
            foreach (var model in equipment.equipmentModels)
            {
                model.LoadModel(player, player.playerNetworkManager.isMale.Value);
            }

            player.playerStatsManager.CalculateTotalArmorAbsorption();
            
            if (player.IsOwner)
            {
                player.playerNetworkManager.bodyEquipmentID.Value = equipment.itemID;
            }

        }
        public void UnloadBodyEquipmentModel()
        {
             foreach(var model in rightShoulders)
             {
                model.SetActive(false);
             }
            foreach (var model in rightElbows)
            {
                model.SetActive(false);
            }
            
            foreach (var model in leftShoulders)
            {
                model.SetActive(false);
            }
            foreach (var model in leftElbows)
            {
                model.SetActive(false);
            }
            foreach(var model in backAccessories)
            {
                model.SetActive(false);
            }

            // Male 
            foreach(var model in maleBodies)
            {
                model.SetActive(false);
            }
            foreach (var model in maleRightUpperArms)
            {
                model.SetActive(false);
            }
            foreach(var model in maleLeftUpperArms)
            {
                model.SetActive(false);
            }

            // Female
            foreach (var model in femaleBodies)
            {
                model.SetActive(false);
            }
            foreach(var model in femaleRightUpperArms)
            {
                model.SetActive(false);
            }
            foreach(var model in femaleLeftUpperArms)
            {
                model.SetActive(false);
            }

            player.playerBodyManager.EnableBody();
        }

        public void LoadLegEquipment(LegEquipmentItem legEquipment)
        {
            UnloadLegEquipmentModel();
            if (legEquipment == null)
            {
                if (player.IsOwner)
                {
                    player.playerNetworkManager.legEquipmentID.Value = -1; // -1 Will never be an item ID, So it will always be null
                }
                player.playerInventoryManager.legEquipment = null;
                return;
            }
            player.playerInventoryManager.legEquipment = legEquipment;
            player.playerBodyManager.DisableLowerBody();

            foreach (var model in legEquipment.equipmentModels)
            {
                model.LoadModel(player, player.playerNetworkManager.isMale.Value);
            }
            player.playerStatsManager.CalculateTotalArmorAbsorption();
            if (player.IsOwner)
            {
                player.playerNetworkManager.legEquipmentID.Value = legEquipment.itemID;
            }
        }

        private void UnloadLegEquipmentModel()
        {
            foreach (var model in maleHips)
            {
                model.SetActive(false);
            }
            foreach (var model in femaleHips)
            {
                model.SetActive(false);
            }
            foreach (var model in maleRightLegs)
            {
                model.SetActive(false);
            }
            foreach(var model in maleLeftLegs)
            {
                model.SetActive(false);
            }
            foreach (var model in femaleRightLegs)
            {
                model.SetActive(false);
            }
            foreach (var model in femaleLeftLegs)
            {
                model.SetActive(false);
            }
            foreach (var model in rightKnees)
            {
                model.SetActive(false);
            }
            foreach(var model in leftKnees)
            { 
                model.SetActive(false); 
            }
            player.playerBodyManager.EnableLowerBody();
        }

        public void LoadHandEquipment(HandEquipmentItem handEquipment)
        {
            UnloadHandEquipmentModel();
            if(handEquipment == null)
            {
                if (player.IsOwner)
                {
                    player.playerNetworkManager.handEquipmentID.Value = -1; // -1 Will never be an item ID, So it will always be null
                }
                player.playerInventoryManager.handEquipment = null;
                return;
            }
            
            player.playerInventoryManager.handEquipment = handEquipment;
            player.playerBodyManager.DisableArms();

            foreach (var model in handEquipment.equipmentModels)
            {
                model.LoadModel(player, player.playerNetworkManager.isMale.Value);
            }

            player.playerStatsManager.CalculateTotalArmorAbsorption();
            if (player.IsOwner)
            {
                player.playerNetworkManager.handEquipmentID.Value = handEquipment.itemID;
            }
        }

        private void UnloadHandEquipmentModel()
        {
            foreach(var model in maleLeftLowerArms)
            {
                model.SetActive(false);
            }
            foreach(var model in maleRightLowerArms)
            {
                model.SetActive(false);
            }
            foreach (var model in femaleLeftLowerArms)
            {
                model.SetActive(false);
            }
            foreach (var model in femaleRightLowerArms)
            {
                model.SetActive(false);
            }
            foreach (var model in maleLeftHands)
            {
                model.SetActive(false);
            }
            foreach (var model in maleRightHands)
            {
                model.SetActive(false);
            }
            foreach (var model in femaleLeftHands)
            {
                model.SetActive(false);
            }
            foreach (var model in femaleRightHands)
            {
                model.SetActive(false);
            }

            player.playerBodyManager.EnableArms();
        }
        private void InitializeWeaponSlots()
        {
            WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

            foreach (var weaponSlot in weaponSlots)
            {
                if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
                {
                    rightHandWeaponSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHandWeaponSlot)
                {
                    leftHandWeaponSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHandShieldSlot)
                {
                    leftHandShieldSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.BackSlot)
                {
                    backSlot = weaponSlot;
                }
            }
        }
        public void EquipWeapons()
        {
            LoadRightWeapon();
            LoadLeftWeapon();
        }

        #region Right Hand Weapon

        public void SwitchRightWeapon()
        {
            if(!player.IsOwner) { return; }

            player.playerAnimatorManager.PlayActionAnimation("Swap_Right_Weapon_01", false, false, true, true);

            WeaponItem selectedWeapon = null;

            player.playerInventoryManager.rightHandWeaponIndex += 1;

            if(player.playerInventoryManager.rightHandWeaponIndex < 0 || player.playerInventoryManager.rightHandWeaponIndex > 2)
            {
                player.playerInventoryManager.rightHandWeaponIndex = 0;

                // Check if holding more than one weapon
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;
                for (int i = 0; i < player.playerInventoryManager.weaponsInRightHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInRightHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;
                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInRightHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }

                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.rightHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.rightHandWeaponIndex = firstWeaponPosition;
                    player.playerNetworkManager.currentRightHandWeaponID.Value = firstWeapon.itemID;
                }
                return;
            }

            foreach(WeaponItem weapon in player.playerInventoryManager.weaponsInRightHandSlots)
            {
                if (player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex];  
                    
                    // Assign the network weapon ID so it swithes for all connected clients
                    player.playerNetworkManager.currentRightHandWeaponID.Value = player.playerInventoryManager.weaponsInRightHandSlots[player.playerInventoryManager.rightHandWeaponIndex].itemID;
                    return;
                }
            }

            if(selectedWeapon == null && player.playerInventoryManager.rightHandWeaponIndex <= 2)
            {
                SwitchRightWeapon(); 
            }


        }
        public void LoadRightWeapon()
        {
            if (player.playerInventoryManager.currentRightHandWeapon != null)
            {
                rightHandWeaponSlot.UnloadWeapon();

                rightHandWeaponModel = Instantiate(player.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandWeaponSlot.PlaceWeaponModelIntoSlot(rightHandWeaponModel);
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
                player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);
            }
        }

        #endregion
        #region Left Hand Weapon
        public void SwitchLeftWeapon()
        {
            if (!player.IsOwner) { return; }
            


            player.playerAnimatorManager.PlayActionAnimation("Swap_Left_Weapon_01", false, false, true, true);

            WeaponItem selectedWeapon = null;

            player.playerInventoryManager.leftHandWeaponIndex += 1;

            if (player.playerInventoryManager.leftHandWeaponIndex < 0 || player.playerInventoryManager.leftHandWeaponIndex > 2)
            {
                player.playerInventoryManager.leftHandWeaponIndex = 0;

                // Check if holding more than one weapon
                float weaponCount = 0;
                WeaponItem firstWeapon = null;
                int firstWeaponPosition = 0;
                for (int i = 0; i < player.playerInventoryManager.weaponsInLeftHandSlots.Length; i++)
                {
                    if (player.playerInventoryManager.weaponsInLeftHandSlots[i].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                    {
                        weaponCount += 1;
                        if (firstWeapon == null)
                        {
                            firstWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[i];
                            firstWeaponPosition = i;
                        }
                    }
                }
                Debug.Log("XD 7");

                if (weaponCount <= 1)
                {
                    player.playerInventoryManager.leftHandWeaponIndex = -1;
                    selectedWeapon = WorldItemDatabase.Instance.unarmedWeapon;
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = selectedWeapon.itemID;
                }
                else
                {
                    player.playerInventoryManager.leftHandWeaponIndex = firstWeaponPosition;
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = firstWeapon.itemID;
                }
                return;
            }
            Debug.Log("XD 5");

            foreach (WeaponItem weapon in player.playerInventoryManager.weaponsInLeftHandSlots)
            {
                if (player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID != WorldItemDatabase.Instance.unarmedWeapon.itemID)
                {
                    selectedWeapon = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex];

                    // Assign the network weapon ID so it swithes for all connected clients
                    player.playerNetworkManager.currentLeftHandWeaponID.Value = player.playerInventoryManager.weaponsInLeftHandSlots[player.playerInventoryManager.leftHandWeaponIndex].itemID;
                    return;
                }
            }
            Debug.Log("XD");

            if (selectedWeapon == null && player.playerInventoryManager.leftHandWeaponIndex <= 2)
            {
                SwitchLeftWeapon();
            }
        }
        public void LoadLeftWeapon()
        {
            if (player.playerInventoryManager.currentLeftHandWeapon != null)
            {
                if(leftHandWeaponSlot.currentWeaponModel != null)
                {
                    leftHandWeaponSlot.UnloadWeapon();
                }

                if(leftHandShieldSlot.currentWeaponModel != null)
                {
                    leftHandShieldSlot.UnloadWeapon();
                }
                
                leftHandWeaponModel = Instantiate(player.playerInventoryManager.currentLeftHandWeapon.weaponModel);

                switch (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType)
                {
                    case WeaponModelType.Shield:
                        leftHandShieldSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
                        break;
                    case WeaponModelType.Weapon:
                        leftHandWeaponSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
                        break;
                    default:
                        break;
                }

                leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
                leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
            }
        }
        #endregion

        #region Two Handing Weapon
        public void UnTwoHandWeapon()
        {
            player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);

            if(player.playerInventoryManager.currentLeftHandWeapon.weaponModelType == WeaponModelType.Weapon)
            {
                leftHandWeaponSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
            }
            else if (player.playerInventoryManager.currentLeftHandWeapon.weaponModelType == WeaponModelType.Shield)
            {
                leftHandShieldSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);
            }

            rightHandWeaponSlot.PlaceWeaponModelIntoSlot(rightHandWeaponModel);

            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }

        public void TwoHandRightWeapon()
        { 
            if(player.playerInventoryManager.currentRightHandWeapon == WorldItemDatabase.Instance.unarmedWeapon)
            {
                if(player.IsOwner)
                {
                    player.playerNetworkManager.IsTwoHandingRightWeapon.Value = false;
                    player.playerNetworkManager.IsTwoHandingWeapon.Value = false;
                }

                return;
            }

            player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentRightHandWeapon.weaponAnimator);

            backSlot.PlaceWeaponModelInUnequippedSlot(leftHandWeaponModel, player.playerInventoryManager.currentLeftHandWeapon.weaponClass, player);

            // Place the two handed weapon model in the main (right hand)
            rightHandWeaponSlot.PlaceWeaponModelIntoSlot(rightHandWeaponModel);

            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }
        public void TwoHandLeftWeapon()
        {
            if (player.playerInventoryManager.currentLeftHandWeapon == WorldItemDatabase.Instance.unarmedWeapon)
            {
                if (player.IsOwner)
                {
                    player.playerNetworkManager.IsTwoHandingLeftWeapon.Value = false;
                    player.playerNetworkManager.IsTwoHandingWeapon.Value = false;
                }

                return;
            }

            player.playerAnimatorManager.UpdateAnimatorController(player.playerInventoryManager.currentLeftHandWeapon.weaponAnimator);

            backSlot.PlaceWeaponModelInUnequippedSlot(rightHandWeaponModel, player.playerInventoryManager.currentRightHandWeapon.weaponClass, player);

            // Place the two handed weapon model in the main (right hand)
            rightHandWeaponSlot.PlaceWeaponModelIntoSlot(leftHandWeaponModel);

            rightWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentRightHandWeapon);
            leftWeaponManager.SetWeaponDamage(player, player.playerInventoryManager.currentLeftHandWeapon);
        }
        #endregion
        #region Damage Colliders
        // Call: Main_Light_Attack_01 Events
        public void OpenDamageCollider()
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.meleeDamageCollider.EnableDamageCollider();
                player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentRightHandWeapon.whooshes));
            }
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManager.meleeDamageCollider.EnableDamageCollider();
                player.characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(player.playerInventoryManager.currentLeftHandWeapon.whooshes));

            }
        }
        
        // Call: Main_Light_Attack_01 Events
        public void CloseDamageCollider()
        {
            if (player.playerNetworkManager.isUsingRightHand.Value)
            {
                rightWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }
            else if (player.playerNetworkManager.isUsingLeftHand.Value)
            {
                leftWeaponManager.meleeDamageCollider.DisableDamageCollider();
            }
        }
        #endregion
    }

}
