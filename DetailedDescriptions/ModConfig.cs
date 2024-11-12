﻿public sealed class ModConfig
{
    /// <summary>Quantidade de sementes para o calculo de lucro por ciclo de plantio</summary>
    public int TotalSeeds { get; set; } = 8;

    /// <summary>Exibir a descrição original do item.</summary>
    public bool ShowOriginalDescription { get; set; } = true;

    /// <summary>Exibir a descrição das estações de plantio.</summary>
    public bool ShowSeasonDescription { get; set; } = true;

    /// <summary>Exibir a descrição dos dias necessários para colheita.</summary>
    public bool ShowHarvestInDaysDescription { get; set; } = true;

    /// <summary>Show how many crops are harvested per harvest</summary>
    public bool ShowHarvestAverageStackDescription { get; set; } = true;

    /// <summary>Exibir a descrição sobre a necessidade de replantio ou o tempo de regeneração.</summary>
    public bool ShowRegrowDescription { get; set; } = true;

    /// <summary>Exibir a descrição do preço de compra das sementes.</summary>
    public bool ShowBuyPriceDescription { get; set; } = true;

    /// <summary>Exibir a descrição do preço de venda da colheita.</summary>
    public bool ShowSellPriceDescription { get; set; } = true;

    /// <summary>Exibir a descrição do lucro por ciclo.</summary>
    public bool ShowProfitPerCycleDescription { get; set; } = true;

    /// <summary>Exibir a descrição da rentabilidade (taxa de lucro) por ciclo.</summary>
    public bool ShowProfitabilityDescription { get; set; } = true;

    /// <summary>Exibir a descrição sobre crescimento em uma treliça</summary>
    public bool ShowIsRaised { get; set; } = true;
}
