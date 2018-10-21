﻿using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BetterCrafting.CategoryManager;

namespace BetterCrafting
{
    public class ModEntry : Mod
    {
        private static bool oldMenu = false;
        private CategoryData categoryData;
        public Nullable<ItemCategory> lastCategory;

        public override void Entry(IModHelper helper)
        {
            this.categoryData = this.Helper.ReadJsonFile<CategoryData>("categories.json");
            if (this.categoryData == null)
            {
                this.Monitor.Log("Failed to read category data from 'categories.json'", LogLevel.Error);
                this.Monitor.Log("All crafting recipes will be in the 'Misc' tab", LogLevel.Warn);

                this.categoryData = new CategoryData();
            }

            MenuEvents.MenuChanged += MenuChanged;
        }

        private void MenuChanged(object sender, EventArgsClickableMenuChanged e)
        {
            if (e.NewMenu is GameMenu gameMenu && !oldMenu)
            {
                var craftingTabNum = gameMenu.getTabNumberFromName("crafting");
                var pages = this.Helper.Reflection.GetFieldValue<List<IClickableMenu>>(gameMenu, "pages");
                pages[craftingTabNum] = new BetterCraftingPage(this, this.categoryData, this.lastCategory);
            }
            else
                oldMenu = false;
        }

        public static void setOldMenu(bool oldMenuActive = false)
        {
            oldMenu = oldMenuActive;
        }
    }
}
