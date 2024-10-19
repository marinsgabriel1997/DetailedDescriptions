# Better Detailed Descriptions - Stardew Valley Mod

## About the Mod

*Better Detailed Descriptions* is a mod designed to provide more comprehensive and informative descriptions for seeds and crops in [Stardew Valley](https://www.nexusmods.com/stardewvalley). The goal is to help players make informed decisions about which crops to plant by offering detailed insights into aspects such as profitability, growing seasons, seed purchase costs, and whether crops require replanting or regrow after harvest. With this information, players can optimize their farm’s efficiency and better understand the costs and benefits associated with each seed.

Additionally, the mod is compatible with other mods that modify seed descriptions, ensuring that all seed descriptions—including those from custom mods—are enhanced with the same level of detail. Compatibility has been validated with popular mods such as [Cornucopia](https://www.nexusmods.com/stardewvalley/mods/19508) and [Stardew Valley Expanded](https://www.nexusmods.com/stardewvalley/mods/3753).

## Features

- **In-Depth Information**: Every seed in the game will now feature a detailed description, including:
  - The seasons in which the crop can be planted.
  - The number of days until the initial harvest.
  - Whether the crop requires replanting or regrows after each harvest.
  - Purchase price of seeds and sell price of the harvested crop.
  - Estimated number of harvests within the full growing season (including across consecutive seasons, such as a 56-day span for crops that grow across Spring and Summer).
  - Profit per cycle, factoring in seed costs, selling prices, and workload.
- **Profitability Index**: A detailed profitability calculation for each planting cycle, allowing players to gauge the efficiency of their crop selection.
- **Automatic Translations**: All season information and other relevant data are automatically translated using SMAPI’s built-in translation system, adapting to the language set in the game. The translations are pulled from the files located in the `Mods/BetterDetailedDescriptions/i18n` folder.
- **Customization Options**: Through compatibility with the [Generic Mod Config Menu](https://www.nexusmods.com/stardewvalley/mods/5098), players can customize which details are shown in the descriptions, including:
  - Displaying original item descriptions.
  - Showing planting season information.
  - Displaying seed prices, harvest prices, and profitability per planting cycle.
  - Whether to include information about trellis requirements for crops that need support.

## How It Works

This mod utilizes SMAPI events to intercept and dynamically modify data files from *Stardew Valley*, such as *Data/Crops*, *Data/Objects*, and *Strings/Objects*. It then calculates various statistics about each crop, such as:

1. The time it takes for the plant to reach harvest stage.
2. The total number of harvests possible in a given season.
3. Profit per planting cycle, including costs for seeds and profits from selling the crops.

The descriptions are dynamically generated based on this data, providing players with a clear and informative overview of each crop's profitability and characteristics.

## How to Install

### Requirements:
- Stardew Valley 1.6+
- SMAPI 4.0.8+
- Generic Mod Config Menu

### Installation:
1. Download the *Better Detailed Descriptions* mod from [NexusMods](https://www.nexusmods.com/stardewvalley/mods/28472).
2. Extract the content of the zip file into your Stardew Valley Mods folder, or use the Nexus Mod Manager for easy installation.
3. Launch the game through SMAPI to enable the mod.

## How to Use

Once installed, the mod automatically updates all seed descriptions in the game. Simply access your inventory or browse the seed catalog at a shop, and you will see the new detailed descriptions for each crop, helping you make informed choices about what to plant.

## Translation

The mod’s descriptions, including season and crop details, are automatically translated into the language of the game you are playing. Translation files are located in the `Mods/BetterDetailedDescriptions/i18n` folder. While the translations are generated using AI, contributions to improve them are always welcome!

Supported languages:
- German (de.json)
- English (default.json)
- Spanish (es.json)
- French (fr.json)
- Hungarian (hu.json)
- Italian (it.json)
- Japanese (ja.json)
- Korean (ko.json)
- Portuguese (pt.json)
- Russian (ru.json)
- Turkish (tr.json)
- Chinese (zh.json)

## Contribution and Feedback

- **Bugs and Suggestions**: If you find any bugs or have suggestions for improvement, feel free to open an issue on the mod’s GitHub repository.
- **Contributions**: Pull requests are welcome! Whether you want to improve the code or add new features, your contributions are encouraged.

## Credits

- **Creator**: Gabriel de Marins
- **Framework**: SMAPI by Pathoschild  
- **Mod Configuration Menu**: *Generic Mod Config Menu* by spacechase0

## License

This project is licensed under the GPL-3.0 License. This ensures that any modification or redistribution of the code remains open-source, keeping it free and available to the community.
