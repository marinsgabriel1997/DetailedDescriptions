using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.Objects;
using Newtonsoft.Json;

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
                // Verifica se o SpriteIndex do item é 472 (Parsnip Seeds)
                if (entry.Value.SpriteIndex == 472)
                {
                    this.Monitor.Log($"Item encontrado: {entry.Key}", LogLevel.Debug);

                    // Converte o objeto ObjectData para JSON e imprime os dados brutos antes da alteração
                    string jsonDataAntes = JsonConvert.SerializeObject(entry.Value, Formatting.Indented);
                    this.Monitor.Log($"Dados brutos do item (antes da alteração): {jsonDataAntes}", LogLevel.Debug);

                    // === Alterando a descrição do item ===
                    entry.Value.Description = "Testando nova descrição no objeto";

                    // Converte o objeto alterado para JSON e imprime os dados brutos após a alteração
                    string jsonDataDepois = JsonConvert.SerializeObject(entry.Value, Formatting.Indented);
                    this.Monitor.Log($"Dados brutos do item (após a alteração): {jsonDataDepois}", LogLevel.Debug);

                    break;
                }
            }

            // Registra o evento para interceptar o carregamento do arquivo Strings/Objects
            helper.Events.Content.AssetRequested += OnAssetRequested;
        }

        // Método chamado quando o arquivo Strings/Objects é solicitado
        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            // Verifica se o arquivo solicitado é Strings/Objects
            if (e.NameWithoutLocale.IsEquivalentTo("Strings/Objects"))
            {
                e.Edit(asset =>
                {
                    // Carrega os dados do arquivo Strings/Objects
                    var data = asset.AsDictionary<string, string>().Data;

                    // Verifica se a chave "ParsnipSeeds_Description" existe
                    if (data.ContainsKey("ParsnipSeeds_Description"))
                    {
                        // Altera a descrição localizada para "Testando nova descrição"
                        data["ParsnipSeeds_Description"] = "brasil";
                        this.Monitor.Log("Descrição de Parsnip Seeds alterada para 'Testando nova descrição localizada'", LogLevel.Debug);
                    }
                    else
                    {
                        this.Monitor.Log("Chave 'ParsnipSeeds_Description' não encontrada.", LogLevel.Warn);
                    }
                });
            }
        }
    }
}
