using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static JsonSerializerSettings settings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        Converters = new List<JsonConverter>
                {
                    new ResourceHolderConverter(),
                    new CoordinateConverter(),
                    new FeatureConverter(),
                    new BiomeConverter(),
                    new AbstractEntityConverter(),

                    new BuildingVariantDictionaryConverter(),
                    new DeckSetConverter(),
                    new ScoutEntityDictionaryConverter(),
                }
    };

    public static void SavePlayerCharacter(int slot)
    {
        SaveData saveData = new(true);

        string json = JsonConvert.SerializeObject(saveData, settings);
        File.WriteAllText(GetSavePath(slot), json);

        Debug.Log($"Saved to {GetSavePath(slot)}");
    }

    public static void LoadPlayerCharacter(int slot)
    {
        if (File.Exists(GetSavePath(slot)))
        {
            string json = File.ReadAllText(GetSavePath(slot));

            SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json, settings);

            PlayerCharacter.SetPlayer(
                saveData.Name,
                saveData.PlayerMap,
                saveData.PlayerBase,
                saveData.Inventory,
                saveData.Income,
                saveData.Experience,
                saveData.Stats,
                saveData.Clock,
                saveData.Decks,
                saveData.Scouts,
                saveData.CurrentBase,
                saveData.OldBase,
                saveData.InGame
            );
        }
        else
        {
            PlayerCharacter.CreatePlayer($"Player {slot}");
        }
    }

    private static string GetSavePath(int slot)
    {
        return Path.Combine(Application.persistentDataPath, $"save_{slot}.json");
    }

    public static void DeleteSlot(int slot)
    {
        File.Delete(GetSavePath(slot));
    }
}
