using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

namespace Spes.UI
{
    public class MainMenuScript : MonoBehaviour
    {
        [SerializeField] protected TMP_InputField playerNameIF;
        [Inject] [SerializeField] protected GameBase gameBaseRef;


        protected string playerName;

        public void OnStartClientClicked()
        {
            gameBaseRef.playerSettings.playerName = playerNameIF.text;
            gameBaseRef.SetupAsClient();
        }

        public void OnStartServerClicked()
        {
            gameBaseRef.SetupAsServer();
        }

        public void OnStartHostClicked()
        {
            gameBaseRef.playerSettings.playerName = playerNameIF.text;
            gameBaseRef.SetupAsHost();
        }
    }
}
