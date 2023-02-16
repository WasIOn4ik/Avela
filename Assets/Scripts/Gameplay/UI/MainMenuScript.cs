using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Spes.UI
{
    public class MainMenuScript : MonoBehaviour
    {
        [SerializeField] protected TMP_InputField playerNameIF;
        [Inject] protected GameplayBase gameBaseRef;
        [Inject] protected GameCore gameInstance;

        protected string playerName;

        public void OnStartClientClicked()
        {
            gameInstance.Settings.playerName = playerNameIF.text;
            gameBaseRef.SetupAsClient();
        }

        public void OnStartServerClicked()
        {
            gameBaseRef.SetupAsServer();
        }

        public void OnStartHostClicked()
        {
            gameInstance.Settings.playerName = playerNameIF.text;
            gameBaseRef.SetupAsHost();
        }
    }
}
