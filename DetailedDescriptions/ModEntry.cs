using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.Objects;
using Newtonsoft.Json; // Certifique-se de que você tenha o Newtonsoft.Json disponível no projeto

namespace DetailedDescriptions
{
    internal sealed class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            // Carrega os dados de objetos no Entry para iterar e encontrar o item com SpriteIndex 472
            Dictionary<string, ObjectData> objectData = this.Helper.GameContent.Load<Dictionary<string, ObjectData>>("Data/Objects");

            this.Monitor.Log("Iterando sobre os objetos para encontrar o item com SpriteIndex 472...", LogLevel.Debug);

            foreach (var entry in objectData)
            {
                // Verifica se o SpriteIndex do item é 472
                if (entry.Value.SpriteIndex == 472)
                {
                    this.Monitor.Log($"Item encontrado: {entry.Key}", LogLevel.Debug);

                    // Converte o objeto ObjectData para JSON e imprime os dados brutos
                    string jsonData = JsonConvert.SerializeObject(entry.Value, Formatting.Indented);
                    this.Monitor.Log($"Dados brutos do item (JSON): {jsonData}", LogLevel.Debug);

                    return;
                }
            }
        }
    }
}
