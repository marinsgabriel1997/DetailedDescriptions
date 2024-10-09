# Detailed Descriptions - Stardew Valley Mod

### About the Mod

**Detailed Descriptions** is a mod created to provide more informative and detailed descriptions for seeds and crops in Stardew Valley. The goal is to help players make more informed decisions about which crops to plant by presenting information such as profitability, growing seasons, and details about replanting or crop regeneration. This way, the player can maximize farm efficiency and have a better understanding of the costs and benefits of each seed.

### Features

- **Detailed Information**: Each seed will receive a detailed description that includes:
  - The seasons in which it can be planted.
  - The number of days it takes to reach the initial harvest.
  - Whether it requires replanting or regenerates after harvest.
  - Purchase price of the seeds and selling price of the crops.
  - Estimated number of harvests during the full season cycle, including consecutive cycles (e.g., 56 days if the seed can be planted in two consecutive seasons).
  - Profitability calculated per cycle, taking into account costs and the amount of work required.
- **Automatic Translations**: Season information and other data are automatically translated, leveraging SMAPI's translation system.

### How It Works

This mod uses SMAPI events to intercept the relevant data files from Stardew Valley, such as `Data/Crops`, `Data/Objects`, and `Strings/Objects`. It then automatically calculates various statistics about each crop, including:

- How many days the plant takes to grow.
- How many times the crop can be harvested during a season.
- The profit per cycle considering the seed cost and the selling price.

The resulting description is generated dynamically, providing a clear view of the crop's profitability and characteristics.

### How to Install

1. **Requirements**:

   - Stardew Valley 1.6.
   - [SMAPI 4.0.8](https://smapi.io/).

2. **Installation**:

   - Download the Detailed Descriptions mod from the [NexusMods page](https://www.nexusmods.com/stardewvalley/mods/28472).
   - Extract the content of the zip file to your Stardew Valley `Mods` folder, or alternatively, use the Nexus Mod Manager to manage the installation.
   - Launch the game through SMAPI.

### How to Use

After installation, the mod will automatically modify seed descriptions in the game. Just access the inventory or purchase catalog, and you will see the new detailed descriptions for each crop item.

### Translation

Season information and other details are automatically translated to the language you are playing in. The translation files are located in the `Mods/DetailedDescriptions/i18n` folder. Please note that the translations were generated using AI and may contain errors. Contributions to improve the translations are welcome! The translation files are located in the `Mods/DetailedDescriptions/i18n` folder. The mod currently supports the following languages:

- German (`de.json`)
- Default (`default.json`)
- Spanish (`es.json`)
- French (`fr.json`)
- Hungarian (`hu.json`)
- Italian (`it.json`)
- Japanese (`ja.json`)
- Korean (`ko.json`)
- Portuguese (`pt.json`)
- Russian (`ru.json`)
- Turkish (`tr.json`)
- Chinese (`zh.json`)

### Contribution and Feedback

- **Bugs and Suggestions**: If you encounter any bugs or have suggestions for improvements, please open a new issue in the [GitHub repository](https://github.com/gbaeriel/DetailedDescriptions/issues).
- **Contributions**: Pull requests are welcome! If you’d like to improve the code or add new features, feel free to contribute.

### Credits

- **Creator**: Gabriel de Marins (nickname for projects: Gbaeriel).
- **Framework**: [SMAPI](https://smapi.io/) by Pathoschild.

### License

This project is licensed under the GPL-3.0 License. This means that any modification or redistribution of this code must always remain open source, ensuring that it stays free and available to everyone. See the LICENSE file for more details.

---

I hope this mod helps optimize your farm and that you can get the most profit from your crops! Enjoy!
