using System.IO;
using Source.FightModule.Scripts.Configs;
using UnityEditor;
using UnityEngine;

namespace Source.FightModule.Scripts.Utils.Editor
{
    public class DiceCreator : UnityEditor.Editor
    {
        [MenuItem("DiceCreator/CreateDice", false, -1)]
        private static void CreateDice()
        {
            DiceConfig diceConfig = CreateInstance<DiceConfig>();
            diceConfig.InitByDefault();

            const string directoryPath = "Assets/Source/FightModule/Configs/Dices/";
            const string fileName = "DiceConfig.asset";
            string assetPath = $"{directoryPath}{fileName}";

            if (File.Exists(assetPath))
            {
                int fileIndex = 1;
                do
                {
                    string newFileName = $"DiceConfig{fileIndex}.asset";
                    assetPath = $"{directoryPath}{newFileName}";
                    fileIndex++;
                } while (File.Exists(assetPath));

                AssetDatabase.CreateAsset(diceConfig, assetPath);
                AssetDatabase.SaveAssets();
                Selection.activeObject = diceConfig;

                Debug.Log($"New DiceConfig has been created and saved in: {assetPath}");
            }
            else
            {
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                AssetDatabase.CreateAsset(diceConfig, assetPath);
                AssetDatabase.SaveAssets();
                Selection.activeObject = diceConfig;

                Debug.Log($"New DiceConfig has been created and saved in: {assetPath}");
            }
        }
    }
}