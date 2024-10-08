using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.Objects;
using StardewValley.GameData.Crops;

namespace DetailedDescriptions
{
    internal sealed class ModEntry : Mod
    {
        private IDictionary<string, CropData> cropsData;
        private IDictionary<string, ObjectData> objectsData;

        public override void Entry(IModHelper helper)
        {
            helper.Events.Content.AssetRequested += OnAssetRequested;
        }

        private void OnAssetRequested(object sender, AssetRequestedEventArgs e)
        {
            // Intercepta Data/Crops
            if (e.NameWithoutLocale.IsEquivalentTo("Data/Crops"))
            {
                e.Edit(asset =>
                {
                    cropsData = asset.AsDictionary<string, CropData>().Data;
                    this.Monitor.Log("Data/Crops capturado e armazenado.", LogLevel.Debug);
                });
            }

            // Intercepta Data/Objects
            if (e.NameWithoutLocale.IsEquivalentTo("Data/Objects"))
            {
                e.Edit(asset =>
                {
                    objectsData = asset.AsDictionary<string, ObjectData>().Data;
                    this.Monitor.Log("Data/Objects capturado e armazenado.", LogLevel.Debug);
                });
            }

            // Intercepta Strings/Objects (onde você deseja modificar as descrições)
            if (e.NameWithoutLocale.IsEquivalentTo("Strings/Objects"))
            {
                e.Edit(asset =>
                {
                    var stringsData = asset.AsDictionary<string, string>().Data;

                    if (cropsData != null && objectsData != null)
                    {
                        this.Monitor.Log("Modificando Strings/Objects com base nos dados de Crops e Objects.", LogLevel.Debug);

                        // Itera sobre todas as chaves de cropsData
                        foreach (var cropEntry in cropsData)
                        {
                            string cropKey = cropEntry.Key; // Chave (ID da semente)
                            CropData cropData = cropEntry.Value; // Dados da semente (CropData)

                            // Obtém o harvestKey, que é o ID do item colhido
                            string harvestKey = cropData.HarvestItemId.ToString();

                            // Verifica se a colheita e a semente estão presentes em objectsData
                            if (objectsData.ContainsKey(cropKey) && objectsData.ContainsKey(harvestKey))
                            {
                                ObjectData seedObject = objectsData[cropKey]; // Dados do item da semente
                                ObjectData harvestObject = objectsData[harvestKey]; // Dados do item colhido

                                // Verifica a descrição original da semente e extrai a chave
                                string originalDescription = seedObject.Description;
                                string chaveDescricao = ExtractKeyFromDescription(originalDescription);

                                // Cálculos para a nova descrição
                                int seed_daysinphase_sum = cropData.DaysInPhase.Sum();
                                int total_cycle_days = cropData.Seasons.Count * 28;
                                int harvests = cropData.RegrowDays == -1 ? total_cycle_days / seed_daysinphase_sum : 1 + ((28 - seed_daysinphase_sum) / cropData.RegrowDays);
                                int seed_buy_value = seedObject.Price * 2;
                                int profit_cycle = (harvests * harvestObject.Price) - seed_buy_value;

                                // Cálculo do trabalho total (colheitas e replantios)
                                int total_work = cropData.RegrowDays == -1 ? harvests * 2 : harvests;

                                // Calcular rentabilidade
                                double profitability = total_work > 0 ? (double)profit_cycle / total_work : 0;

                                // Converte a lista de estações para uma string
                                string seed_seasons_string = string.Join(", ", cropData.Seasons);

                                // Cria uma nova descrição com base nos cálculos
                                string newDescription = (
                                    $"Season: {seed_seasons_string}.\n" +
                                    $"Harvest in {seed_daysinphase_sum} days. " +
                                    $"{(cropData.RegrowDays == -1 ? "Needs to be replanted." : $"Then every {cropData.RegrowDays} days.")}\n" +
                                    $"Buy: {seed_buy_value}g.\n" +
                                    $"Sell: {harvestObject.Price}g.\n\n" +
                                    $"Profit per planting cycle ({total_cycle_days} days):\n" +
                                    $"Harvests: {harvests}.\n" +
                                    $"Profit: {profit_cycle}g.\n\n" +
                                    $"Profitability (Bigger is better): {profitability:F2}"
                                );

                                // Modifica a descrição no Strings/Objects usando a chave extraída
                                if (!string.IsNullOrEmpty(chaveDescricao) && stringsData.ContainsKey(chaveDescricao))
                                {
                                    stringsData[chaveDescricao] = newDescription;
                                    this.Monitor.Log($"Nova descrição de {seedObject.Name}: {newDescription}", LogLevel.Debug);
                                }
                            }
                        }
                    }
                    else
                    {
                        this.Monitor.Log("Dados de Crops ou Objects não carregados. Não foi possível modificar Strings/Objects.", LogLevel.Warn);
                    }
                });
            }
        }

        // Função para extrair a chave da descrição original
        private string ExtractKeyFromDescription(string originalDescription)
        {
            // Verifica se a descrição original segue o formato esperado "[LocalizedText Strings\Objects:ParsnipSeeds_Description]"
            if (originalDescription.StartsWith("[LocalizedText Strings\\Objects:") && originalDescription.EndsWith("]"))
            {
                // Extrai a chave do meio da string
                int start = originalDescription.IndexOf(":") + 1;
                int end = originalDescription.IndexOf("]");
                return originalDescription.Substring(start, end - start);
            }
            return null; // Retorna nulo se o formato não for válido
        }
    }
}
