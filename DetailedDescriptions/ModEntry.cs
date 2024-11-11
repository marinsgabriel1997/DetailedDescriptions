using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.Objects;
using StardewValley.GameData.Crops;
using GenericModConfigMenu;
using StardewValley;

namespace BetterDetailedDescriptions
{
    internal sealed class ModEntry : Mod
    {
        private ModConfig Config;

        // Dicionario "Data/Crops"
        private IDictionary<string, CropData> cropsData;
        // Dicionario "Data/Objects"
        private IDictionary<string, ObjectData> objectsData;

        public override void Entry(IModHelper helper)
        {
            this.Config = this.Helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.GameLaunched += OnGameLaunched;
            //helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
        }

        private void OnGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
                return;

            configMenu.Register(
                mod: this.ModManifest,
                reset: () => this.Config = new ModConfig(),
                save: () =>
                {
                    this.Helper.WriteConfig(this.Config);
                    UpdateDescriptions();
                }
            );

            // Exibir a descrição original 
            configMenu.AddNumberOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("option.totalSeeds.name"),
                tooltip: () => this.Helper.Translation.Get("option.totalSeeds.tooltip"),
                getValue: () => this.Config.TotalSeeds,
                setValue: value => this.Config.TotalSeeds = value
            );

            // Exibir a descrição original 
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("option.showOriginalDescription.name"),
                tooltip: () => this.Helper.Translation.Get("option.showOriginalDescription.tooltip"),
                getValue: () => this.Config.ShowOriginalDescription,
                setValue: value => this.Config.ShowOriginalDescription = value
            );

            // Exibir a descrição das estações de plantio
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("option.showSeasonDescription.name"),
                tooltip: () => this.Helper.Translation.Get("option.showSeasonDescription.tooltip"),
                getValue: () => this.Config.ShowSeasonDescription,
                setValue: value => this.Config.ShowSeasonDescription = value
            );

            // Exibir a descrição dos dias necessários para colheita
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("option.showHarvestInDaysDescription.name"),
                tooltip: () => this.Helper.Translation.Get("option.showHarvestInDaysDescription.tooltip"),
                getValue: () => this.Config.ShowHarvestInDaysDescription,
                setValue: value => this.Config.ShowHarvestInDaysDescription = value
            );

            // Exibir a descrição sobre a necessidade de replantio ou tempo de regeneração
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("option.showRegrowDescription.name"),
                tooltip: () => this.Helper.Translation.Get("option.showRegrowDescription.tooltip"),
                getValue: () => this.Config.ShowRegrowDescription,
                setValue: value => this.Config.ShowRegrowDescription = value
            );

            // Exibir a descrição sobre crescimento em uma treliça
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("option.showIsRaised.name"),
                tooltip: () => this.Helper.Translation.Get("option.showIsRaised.tooltip"),
                getValue: () => this.Config.ShowIsRaised,
                setValue: value => this.Config.ShowIsRaised = value
            );

            // Exibir a descrição do preço de compra das sementes
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("option.showBuyPriceDescription.name"),
                tooltip: () => this.Helper.Translation.Get("option.showBuyPriceDescription.tooltip"),
                getValue: () => this.Config.ShowBuyPriceDescription,
                setValue: value => this.Config.ShowBuyPriceDescription = value
            );

            // Exibir a descrição do preço de venda da colheita
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("option.showSellPriceDescription.name"),
                tooltip: () => this.Helper.Translation.Get("option.showSellPriceDescription.tooltip"),
                getValue: () => this.Config.ShowSellPriceDescription,
                setValue: value => this.Config.ShowSellPriceDescription = value
            );

            // Exibir a descrição do lucro por ciclo
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("option.showProfitPerCycleDescription.name"),
                tooltip: () => this.Helper.Translation.Get("option.showProfitPerCycleDescription.tooltip"),
                getValue: () => this.Config.ShowProfitPerCycleDescription,
                setValue: value => this.Config.ShowProfitPerCycleDescription = value
            );

            // Exibir a descrição da rentabilidade (taxa de lucro) por ciclo
            configMenu.AddBoolOption(
                mod: this.ModManifest,
                name: () => this.Helper.Translation.Get("option.showProfitabilityDescription.name"),
                tooltip: () => this.Helper.Translation.Get("option.showProfitabilityDescription.tooltip"),
                getValue: () => this.Config.ShowProfitabilityDescription,
                setValue: value => this.Config.ShowProfitabilityDescription = value
            );
        }



        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            UpdateDescriptions();
        }

        private void UpdateDescriptions()
        {
            this.Helper.GameContent.InvalidateCache("Data/Objects");
            this.Helper.GameContent.InvalidateCache("Data/Crops");

            cropsData = Game1.content.Load<Dictionary<string, CropData>>("Data/Crops");
            objectsData = Game1.content.Load<Dictionary<string, ObjectData>>("Data/Objects");

            foreach (var cropEntry in cropsData)
            {
                string cropKey = cropEntry.Key; // Chave (ID da semente)
                CropData cropData = cropEntry.Value; // Dados da semente (CropData)                
                string harvestKey = cropData.HarvestItemId.ToString(); // Obtém o harvestKey, que é o ID do item colhido

                if (objectsData.ContainsKey(cropKey) && objectsData.ContainsKey(harvestKey))
                {
                    ObjectData seedObject = objectsData[cropKey]; // Dados da semente
                    ObjectData harvestObject = objectsData[harvestKey]; // Dados do item colhido

                    // Cálculos para a nova descrição
                    int totalSeedsConfig = Math.Max(1, Config.TotalSeeds);
                    int seed_daysinphase_sum = cropData.DaysInPhase.Sum();
                    int total_cycle_days = cropData.Seasons.Count * 28;
                    int harvests = cropData.RegrowDays == -1 ? total_cycle_days / seed_daysinphase_sum : 1 + ((total_cycle_days - seed_daysinphase_sum) / cropData.RegrowDays);
                    int seed_buy_value = seedObject.Price * 2;
                    //int profit_cycle = (harvests * harvestObject.Price * totalSeedsConfig) - (seed_buy_value * totalSeedsConfig);
                    int seed_cost_total = cropData.RegrowDays == -1 ? harvests * seed_buy_value * totalSeedsConfig : seed_buy_value * totalSeedsConfig;
                    int profit_cycle = (harvests * harvestObject.Price * totalSeedsConfig) - seed_cost_total;
                    int total_work = cropData.RegrowDays == -1 ? harvests * 2 : harvests; // Cálculo do trabalho total (colheitas e replantios)
                    double profitability = total_work > 0 ? ((double)profit_cycle / totalSeedsConfig) / total_work : 0; // Calcular rentabilidade
                    List<StardewValley.Season> seasons = cropData.Seasons; // Obtém as estações do cultivo (ex.: "Spring", "Summer", etc.)
                    string[] seasonNames = seasons
                        .Select(season => season.ToString()) // Usa ToString para obter o nome da estação
                        .ToArray(); // Converte a lista para array de strings                   
                    string translatedSeasonsString = string.Join(", ", seasonNames.Select(name => this.Helper.Translation.Get($"season.{name}"))); // Traduz cada nome de estação usando o sistema de tradução do SMAPI

                    List<string> newDescription = new List<string>();

                    // Textos para a descrição da semente
                    if (Config.ShowOriginalDescription)
                    {
                        newDescription.Add(seedObject.Description);
                        newDescription.Add("");
                    }

                    if (Config.ShowSeasonDescription)
                    {
                        string seasonDescription = $"{this.Helper.Translation.Get("season")}: {translatedSeasonsString}";
                        newDescription.Add(seasonDescription);
                    }

                    if (Config.ShowHarvestInDaysDescription)
                    {
                        string harvestInDaysDescription = this.Helper.Translation.Get("harvestInDays", new { days = seed_daysinphase_sum });
                        newDescription.Add(harvestInDaysDescription);
                    }

                    if (Config.ShowRegrowDescription)
                    {
                        string regrowDescription = (cropData.RegrowDays == -1
                        ? this.Helper.Translation.Get("needsReplanting")
                        : this.Helper.Translation.Get("regrowInDays", new { days = cropData.RegrowDays }));
                        newDescription.Add(regrowDescription);
                    }

                    if (Config.ShowIsRaised && cropData.IsRaised)
                    {
                        string isRaisedDescription = this.Helper.Translation.Get("isRaised");
                        newDescription.Add(isRaisedDescription);
                    }

                    if (Config.ShowBuyPriceDescription)
                    {
                        string buyPriceDescription = this.Helper.Translation.Get("buyPrice", new { price = seed_buy_value });
                        newDescription.Add(buyPriceDescription);
                    }

                    if (Config.ShowSellPriceDescription)
                    {
                        string sellPriceDescription = this.Helper.Translation.Get("sellPrice", new { price = harvestObject.Price });
                        newDescription.Add(sellPriceDescription);
                    }

                    // Informações de lucro por ciclo
                    if (Config.ShowProfitPerCycleDescription)
                    {
                        newDescription.Add("");
                        string profitPerCycleDescription = this.Helper.Translation.Get("profitPerCycle", new { totalDays = total_cycle_days, totalSeeds = totalSeedsConfig });
                        string harvestsDescription = this.Helper.Translation.Get("harvests", new { count = harvests });
                        string profitDescription = this.Helper.Translation.Get("profit", new { value = profit_cycle });
                        newDescription.Add(profitPerCycleDescription);
                        newDescription.Add(harvestsDescription);
                        newDescription.Add(profitDescription);
                    }

                    // Rentabilidade
                    if (Config.ShowProfitabilityDescription)
                    {
                        newDescription.Add("");
                        string profitabilityDescription = this.Helper.Translation.Get("profitability", new { rate = profitability.ToString("F2") });
                        newDescription.Add(profitabilityDescription);
                    }

                    // Atualiza a descrição da semente
                    LocalizedContentManager.LanguageCode currentLanguage = LocalizedContentManager.CurrentLanguageCode;

                    string localizationFile = currentLanguage switch
                    {
                        LocalizedContentManager.LanguageCode.en => "Strings/Objects",
                        LocalizedContentManager.LanguageCode.pt => "Strings/Objects.pt-BR",
                        LocalizedContentManager.LanguageCode.ko => "Strings/Objects.ko-KR",
                        LocalizedContentManager.LanguageCode.ja => "Strings/Objects.ja-JP",
                        LocalizedContentManager.LanguageCode.zh => "Strings/Objects.zh-CN",
                        LocalizedContentManager.LanguageCode.de => "Strings/Objects.de-DE",
                        LocalizedContentManager.LanguageCode.fr => "Strings/Objects.fr-FR",
                        LocalizedContentManager.LanguageCode.it => "Strings/Objects.it-IT",
                        LocalizedContentManager.LanguageCode.es => "Strings/Objects.es-ES",
                        LocalizedContentManager.LanguageCode.ru => "Strings/Objects.ru-RU",
                        LocalizedContentManager.LanguageCode.hu => "Strings/Objects.hu-HU",
                        LocalizedContentManager.LanguageCode.tr => "Strings/Objects.tr-TR",
                        _ => "Strings/Objects"
                    };

                    IDictionary<string, string> objectStrings = Game1.content.Load<Dictionary<string, string>>(localizationFile);

                    string seedNameKey = seedObject.DisplayName.Split(':').Last().TrimEnd(']');

                    string localizedSeedName = objectStrings.ContainsKey(seedNameKey) ? objectStrings[seedNameKey] : seedObject.DisplayName;

                    seedObject.DisplayName = localizedSeedName.PadRight(35, ' ').ToString();
                    seedObject.Description = string.Join(Environment.NewLine, newDescription);
                }
            }
        }
    }
}
