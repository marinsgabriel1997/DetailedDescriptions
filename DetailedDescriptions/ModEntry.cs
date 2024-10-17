using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.Objects;
using StardewValley.GameData.Crops;
using GenericModConfigMenu;
using StardewModdingAPI.Framework.ModLoading.Rewriters.StardewValley_1_6;
using StardewValley;

namespace DetailedDescriptions
{
    internal sealed class ModEntry : Mod
    {
        private ModConfig Config;

        private IDictionary<string, CropData> cropsData;
        private IDictionary<string, ObjectData> objectsData;

        public override void Entry(IModHelper helper)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();

            int numberOfCrops = Config.NumberOfCrops;
            bool Profit = Config.Profit;

            //helper.Events.Content.AssetRequested += OnAssetRequested;
            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            helper.Events.GameLoop.DayStarted += OnDayStarted;

        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            configMenu.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () => this.Helper.WriteConfig(this.Config)
            );

            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => "Habilitar lucro?",
                tooltip: () => "Exibir o Lucro?",
                getValue: () => this.Config.Profit,
                setValue: value => this.Config.Profit = value
            );
        }
        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            this.Helper.GameContent.InvalidateCache("Data/Objects");
            TryUpdateDescriptions();
        }

        private void TryUpdateDescriptions()
        {
            cropsData = Game1.content.Load<Dictionary<string, CropData>>("Data/Crops");
            objectsData = Game1.content.Load<Dictionary<string, ObjectData>>("Data/Objects");

            foreach (var cropEntry in cropsData)
            {
                string cropKey = cropEntry.Key; // Chave (ID da semente)
                CropData cropData = cropEntry.Value; // Dados da semente (CropData)                
                string harvestKey = cropData.HarvestItemId.ToString(); // Obtém o harvestKey, que é o ID do item colhido

                // Verifica se a colheita e a semente estão presentes em objectsData
                if (objectsData.ContainsKey(cropKey) && objectsData.ContainsKey(harvestKey))
                {
                    ObjectData seedObject = objectsData[cropKey]; // Dados do item da semente
                    ObjectData harvestObject = objectsData[harvestKey]; // Dados do item colhido
                   
                    string originalDescription = seedObject.Description; // Verifica a descrição original da semente e extrai a chave

                    // Cálculos para a nova descrição
                    int seed_daysinphase_sum = cropData.DaysInPhase.Sum();
                    int total_cycle_days = cropData.Seasons.Count * 28;
                    int harvests = cropData.RegrowDays == -1 ? total_cycle_days / seed_daysinphase_sum : 1 + ((28 - seed_daysinphase_sum) / cropData.RegrowDays);
                    int seed_buy_value = seedObject.Price * 2;
                    int profit_cycle = (harvests * harvestObject.Price) - seed_buy_value;                    
                    int total_work = cropData.RegrowDays == -1 ? harvests * 2 : harvests; // Cálculo do trabalho total (colheitas e replantios)                    
                    double profitability = total_work > 0 ? (double)profit_cycle / total_work : 0; // Calcular rentabilidade                   
                    List<StardewValley.Season> seasons = cropData.Seasons; // Obtém as estações do cultivo (ex.: "Spring", "Summer", etc.)                                     
                    string[] seasonNames = seasons
                        .Select(season => season.ToString()) // Usa ToString para obter o nome da estação
                        .ToArray(); // Converte a lista para array de strings                   
                    string translatedSeasonsString = string.Join(", ", seasonNames.Select(name => this.Helper.Translation.Get($"season.{name}"))); // Traduz cada nome de estação usando o sistema de tradução do SMAPI

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
                        harvestInDaysDescription + "\n" + 
                        regrowDescription + "\n" +
                        buyPriceDescription + "\n" +
                        sellPriceDescription + "\n\n" +
                        profitPerCycleDescription + "\n" +
                        harvestsDescription + "\n" +
                        profitDescription + "\n\n" +
                        profitabilityDescription
                    );

                    seedObject.Description = newDescription;

                    this.Monitor.Log(newDescription);
                }
            }
        }
    }
}
