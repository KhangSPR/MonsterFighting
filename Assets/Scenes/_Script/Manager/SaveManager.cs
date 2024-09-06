using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace UIGameDataManager
{
    // note: this uses JsonUtility for demo purposes only; for production work, consider a more performant solution like MessagePack (https://msgpack.org/index.html) 
    // or Protocol Buffers (https://developers.google.com/protocol-buffers)
    // 

    [RequireComponent(typeof(GameDataManager))]
    public class SaveManager : MonoBehaviour
    {

        [SerializeField] string m_SaveFilename = "savegame.dat";
        [Tooltip("Show Debug messages.")]
        [SerializeField] bool m_DebugValues;

        public static event Action<GameData> OnGameDataLoaded;

        void Start()
        {
            LoadGame();
        }

        void OnApplicationQuit()
        {
            SaveGame();
        }

        void OnEnable()
        {
            //SettingsScreen.SettingsUpdated += OnSettingsUpdated;

            //GameScreenController.SettingsUpdated += OnSettingsUpdated;

        }

        void OnDisable()
        {
            //SettingsScreen.SettingsShown -= OnSettingsShown;
            //SettingsScreen.SettingsUpdated -= OnSettingsUpdated;

            //GameScreenController.SettingsUpdated -= OnSettingsUpdated;

        }
        public GameData NewGame()
        {
            return new GameData();
        }

        public void LoadGame()
        {
            // load saved data from FileDataHandler

            if (GameDataManager.Instance.GameData == null)
            {
                if (m_DebugValues)
                {
                    Debug.Log("GAME DATA MANAGER LoadGame: Initializing game data.");
                }

                GameDataManager.Instance.GameData = NewGame();
            }
            else if (FileManager.LoadFromFile(m_SaveFilename, out var jsonString))
            {
                GameDataManager.Instance.GameData.LoadJson(jsonString);

                if (m_DebugValues)
                {
                    Debug.Log("SaveManager.LoadGame: " + m_SaveFilename + " json string: " + jsonString);
                }
            }

            // notify other game objects 
            if (GameDataManager.Instance.GameData != null)
            {
                OnGameDataLoaded?.Invoke(GameDataManager.Instance.GameData);

                Debug.Log("GameData != null");
            }
        }

        public void SaveGame()
        {
            string jsonFile = GameDataManager.Instance.GameData.ToJson();

            // save to disk with FileDataHandler
            if (FileManager.WriteToFile(m_SaveFilename, jsonFile) && m_DebugValues)
            {
                Debug.Log("SaveManager.SaveGame: " + m_SaveFilename + " json string: " + jsonFile);
            }
        }
        void OnSettingsUpdated(GameData gameData)
        {
            GameDataManager.Instance.GameData = gameData;
            SaveGame();
        }

        //void OnSettingsShown()
        //{
        //    // pass the GameData to the Settings Screen
        //    if (m_GameDataManager.GameData != null)
        //    {
        //        GameDataLoaded?.Invoke(m_GameDataManager.GameData);
        //    }
        //}

    }
}
