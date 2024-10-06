using System;
using System.Linq;
using System.Collections.Generic;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.Crops; // Necessário para o tipo CropData

namespace DetailedDescriptions
{
    internal sealed class ModEntry : Mod
    {
        // Variável para armazenar os dados dos cultivos
        private Dictionary<string, CropData> cropData;

        public override void Entry(IModHelper helper)
        {
            // Inscreve-se no evento SaveLoaded, que é chamado quando um jogo salvo é carregado
            helper.Events.GameLoop.SaveLoaded += GameLoop_SaveLoaded;
        }

        // Método chamado quando o jogo salvo é carregado
        private void GameLoop_SaveLoaded(object? sender, SaveLoadedEventArgs e)
        {
            // Carregar os dados dos cultivos usando o tipo correto (CropData)
            cropData = Game1.content.Load<Dictionary<string, CropData>>("Data/Crops");

            if (cropData != null && cropData.Count > 0)
            {
                this.Monitor.Log("Iniciando leitura de crops.", LogLevel.Debug);

                // Exibe as informações de cada cultivo
                foreach (var crop in cropData)
                {
                    string cropId = crop.Key;
                    CropData cropInfo = crop.Value;

                    // Exibir as informações do cultivo no console do SMAPI
                    this.Monitor.Log($"Crop ID: {cropId}, Nome: {cropInfo.HarvestItemId}, Estações: {string.Join(", ", cropInfo.Seasons)}", LogLevel.Info);
                }
            }
            else
            {
                this.Monitor.Log("Nenhum dado de cultivo encontrado!", LogLevel.Warn);
            }
        }
    }
}
