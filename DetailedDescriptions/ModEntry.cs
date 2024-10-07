using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.Objects;
using Newtonsoft.Json;
using System.Reflection;

namespace DetailedDescriptions
{
    internal sealed class ModEntry : Mod
    {
        public override void Entry(IModHelper helper)
        {
            helper.Events.Content.AssetRequested += OnAssetRequested;
        }

        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.IsEquivalentTo("Data/Objects"))
            {                
                e.Edit(asset =>
                {
                    var data = asset.AsDictionary<string, ObjectData>().Data;

                    int count = 0;

                    this.Monitor.Log("Iterando sobre Data/Objects", LogLevel.Debug);

                    foreach (var entry in data)
                    {
                        count++;

                        if (count > 2)
                        {
                            break; // Interrompe o loop após processar o segundo item
                        }

                        this.Monitor.Log($"=== Propriedades do item com chave: {entry.Key}:", LogLevel.Debug);

                        var objData = entry.Value;

                        this.Monitor.Log($"Name: {objData.Name}", LogLevel.Debug);
                        this.Monitor.Log($"DisplayName: {objData.DisplayName}", LogLevel.Debug);
                        this.Monitor.Log($"Description: {objData.Description}", LogLevel.Debug);
                        this.Monitor.Log($"Type: {objData.Type}", LogLevel.Debug);
                        this.Monitor.Log($"Category: {objData.Category}", LogLevel.Debug);
                        this.Monitor.Log($"Price: {objData.Price}", LogLevel.Debug);
                        this.Monitor.Log($"SpriteIndex: {objData.SpriteIndex}", LogLevel.Debug);
                        this.Monitor.Log($"Edibility: {objData.Edibility}", LogLevel.Debug);
                        this.Monitor.Log($"CanBeGivenAsGift: {objData.CanBeGivenAsGift}", LogLevel.Debug);
                    }

                    
                    if (data.ContainsKey("472"))
                    {
                        this.Monitor.Log("Alterando a descrição de 'Parsnip Seeds' (ID 472)", LogLevel.Debug);
                        this.Monitor.Log($"Descrição original de 'Parsnip Seeds' (ID 472): {data["472"].Description}", LogLevel.Debug);

                        data["472"].Description = "Nova descrição para as sementes de Parsnip.";

                        this.Monitor.Log($"Nova descrição de 'Parsnip Seeds' (ID 472): {data["472"].Description}", LogLevel.Debug);
                    }
                    else
                    {
                        this.Monitor.Log("Item com ID 472 (Parsnip Seeds) não encontrado em Data/Objects.", LogLevel.Warn);
                    }
                });
            }
        }
    }
}
