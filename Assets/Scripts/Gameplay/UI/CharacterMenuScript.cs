using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Spes.UI
{
    public class CharacterMenuScript : MonoBehaviour
    {
        #region Variables

        [SerializeField] protected TMP_Text descriptionTxt;
        [SerializeField] protected TMP_Text CharacterNameTxt;
        [SerializeField] protected TMP_Text CharacterTitleTxt;
        [SerializeField] protected TMP_Text CharacterClassTxt;
        [SerializeField] protected TMP_Text HPTxt;
        [SerializeField] protected TMP_Text MPTxt;
        [SerializeField] protected TMP_Text ATKTxt;
        [SerializeField] protected TMP_Text ProtectionTxt;
        [SerializeField] protected TMP_Text EfficiencyTxt;
        [SerializeField] protected TMP_Text EnergyTxt;
        [SerializeField] protected TMP_Text EnergyRecoveryTxt;
        [SerializeField] protected TMP_Text HPRecoveryTxt;
        [SerializeField] protected TMP_Text MPRecoveryTxt;
        [SerializeField] protected TMP_Text AbilityRecoveryTxt;
        [SerializeField] protected TMP_Text ResistanceTxt;
        [SerializeField] protected TMP_Text DexterityTxt;
        [SerializeField] protected TMP_Text CritDamageTxt;
        [SerializeField] protected TMP_Text CritChanceTxt;
        [SerializeField] protected TMP_Text ShieldDexterityTxt;
        [SerializeField] protected Image CharacterImage;

        [Inject] protected GameCore gameInstance;

        #endregion

    }

    public static class UIUtility
    {
        public static string GetLocalizedStatString(CharacterStat stat)
        {
            return "";
        }
    }
}
