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

                                // Obtém as estações do cultivo (ex.: "Spring", "Summer", etc.)
                                List<StardewValley.Season> seasons = cropData.Seasons;

                                // Converte cada estação em uma string (usando ToString)
                                string[] seasonNames = seasons
                                    .Select(season => season.ToString()) // Usa ToString para obter o nome da estação
                                    .ToArray(); // Converte a lista para array de strings

                                // Traduz cada nome de estação usando o sistema de tradução do SMAPI
                                string translatedSeasonsString = string.Join(", ", seasonNames.Select(name => this.Helper.Translation.Get($"season.{name}")));

                                // Cria a descrição final com as estações traduzidas
                                string seasonDescription = $"{this.Helper.Translation.Get("season")}: {translatedSeasonsString}.";
                                string harvestInDaysDescription = this.Helper.Translation.Get("harvestInDays", new { days = seed_daysinphase_sum });
                                string regrowDescription = (cropData.RegrowDays == -1
                                    ? this.Helper.Translation.Get("needsReplanting")
                                    : this.Helper.Translation.Get("regrowInDays", new { days = cropData.RegrowDays }));
                                string buyPriceDescription = this.Helper.Translation.Get("buyPrice", new { price = seed_buy_value });
                                string sellPriceDescription = this.Helper.Translation.Get("sellPrice", new { price = harvestObject.Price });

                                // Informações de lucro por ciclo
                                string profitPerCycleDescription = this.Helper.Translation.Get("profitPerCycle", new { totalDays = total_cycle_days });
                                string harvestsDescription = this.Helper.Translation.Get("harvests", new { count = harvests });
                                string profitDescription = this.Helper.Translation.Get("profit", new { value = profit_cycle });
                                string profitabilityDescription = this.Helper.Translation.Get("profitability", new { rate = profitability.ToString("F2") });

                                // Combinando tudo em uma única string
                                string newDescription = (
                                    seasonDescription + "\n" +
                                    harvestInDaysDescription + " " + regrowDescription + "\n" +
                                    buyPriceDescription + "\n" +
                                    sellPriceDescription + "\n\n" +
                                    profitPerCycleDescription + "\n" +
                                    harvestsDescription + "\n" +
                                    profitDescription + "\n\n" +
                                    profitabilityDescription
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
