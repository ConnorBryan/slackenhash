using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using Slackenhash.UI;

namespace Slackenhash
{
	public class Slackenhash : Mod
	{
		public static Slackenhash instance;

		public static int FOUR_SECONDS = 60 /* Frames/Second */ * 4 /* Seconds */;
		public static int FIFTEEN_SECONDS = 60 /* Frames/Second */ * 15 /* Seconds */;
		public static int THIRTY_SECONDS = 60 /* Frames/Second */ * 30 /* Seconds */;
		public static int FORTY_FIVE_SECONDS = 60 /* Frames/Second */ * 45 /* Seconds */;
		public static int FIFTEEN_MINUTES = 60 /* Frames/Second */ * 60 /* Seconds/Minute */ * 15 /* Minutes */;
		public static int DECK_SIZE = 60;
		public static int INITIAL_DEAL_SIZE = 4;
		public static int CARD_WIDTH = 200;
		public static float CARD_HEIGHT_TO_WIDTH_RATIO = 1.6f;
		public static int PLAYER_WIDTH = 225;
		public static float PLAYER_HEIGHT_TO_WIDTH_RATIO = 0.55f;

		public static Dictionary<int, string> ROMAN_NUMERAL_LOOKUP = new Dictionary<int, string>()
		{
			{ 1, "I" },
			{ 2, "II" },
			{ 3, "III" },
			{ 4, "IV" },
			{ 5, "V" },
			{ 6, "VI" },
			{ 7, "VII" },
			{ 8, "VIII" },
			{ 9, "IX" },
			{ 10, "X" },
		};

		public static Dictionary<int, string> LEVEL_NAME_LOOKUP = new Dictionary<int, string>()
		{
			{ 1, "Forest" },
			{ 2, "Desert" },
			{ 3, "Snow" },
			{ 4, "Cavern" },
			{ 5, "Mushroom" },
			{ 6, "Underground Desert" },
			{ 7, "Underground Jungle" },
			{ 8, "Evil" },
			{ 9, "Dungeon" },
			{ 10, "Underworld" },
		};

		public static Dictionary<CardKind, string> CARD_KIND_TO_TITLE = new Dictionary<CardKind, string>()
		{
			{ CardKind.Chamber, "Chamber" },
			{ CardKind.Loot, "Loot" },
		};

		public static Dictionary<CardSubkind, string> CARD_SUBKIND_TO_TITLE = new Dictionary<CardSubkind, string>()
		{
			{ CardSubkind.Foe, "Foe" },
			{ CardSubkind.Bonus, "Bonus" },
			{ CardSubkind.Class, "Class" },
			{ CardSubkind.Curse, "Curse" },
			{ CardSubkind.Effect, "Effect" },
			{ CardSubkind.Equipment, "Equipment" },
			{ CardSubkind.Race, "Race" },
			{ CardSubkind.Trap, "Trap" }
		};

		public static Dictionary<EquipmentSlot, string> EQUIPMENT_SLOT_TO_TITLE = new Dictionary<EquipmentSlot, string>()
		{
			{ EquipmentSlot.None, "" },
			{ EquipmentSlot.Accessory, "Accessory" },
			{ EquipmentSlot.Armor, "Armor" },
			{ EquipmentSlot.Headwear, "Headwear" },
			{ EquipmentSlot.Hand, "Hand" },
			{ EquipmentSlot.Footgear, "Footgear" }
		};

		public Game game;
		public bool showingModal;
		public bool showingPrompt;

		public enum Phase
		{
			Waiting,
			BreachingTheChamber,
			FightingAFoe,
			TakingItIn,
			SheddingWeight
		}

		public enum Race
		{
			Human,
			Dwarf,
			Elf,
			Halfling
		}

		public enum Class
		{
			Apprentice,
			Fighter,
			Ranger,
			Wizard
		}

		public enum CardKind
		{
			Chamber,
			Loot
		}

		public enum CardSubkind
		{
			Foe,
			Race,
			Class,
			Trap,
			Bonus,
			Curse,
			Equipment,
			Effect
		}

		public enum EquipmentSlot
        {
			None,
			Headwear,
			Armor,
			Footgear,
			Hand,
			Accessory
        }

		private UserInterface _modalUI;
		private UserInterface _promptUI;
		private UserInterface _playerUI;
		private UserInterface _historyUI;

		internal CardUI cardUI;
		internal ModalUI modalUI;
		internal PromptUI promptUI;
		internal PlayerUI playerUI;
		internal HistoryUI historyUI;

		public override void Load()
		{
			instance = this;

			game = new Game();

			showingModal = false;
			modalUI = new ModalUI();
			modalUI.Activate();
			_modalUI = new UserInterface();
			_modalUI.SetState(modalUI);

			showingPrompt = false;
			_promptUI = new UserInterface();

			playerUI = new PlayerUI();
			playerUI.Activate();
			_playerUI = new UserInterface();
			_playerUI.SetState(playerUI);

			historyUI = new HistoryUI();
			historyUI.Activate();
			_historyUI = new UserInterface();
			_historyUI.SetState(historyUI);
		}

		public void UpdatePower()
        {
			playerUI.Show();
        }

		public void AddHistoryLog(string entry)
		{
			historyUI.Add(entry);
		}

		public void ShowModal(string title, UIElement element)
		{
			showingModal = true;

			modalUI.SetTitle(title);
			modalUI.SetElement(element);
			modalUI.Build();
		}

		public void HideModal()
		{
			showingModal = false;

			modalUI.Clear();
		}

		public void ShowPrompt(string title, List<(string, UIElement.MouseEvent)> options, int duration = 15)
        {
			showingPrompt = true;

			promptUI = new PromptUI(title, options, duration);
			promptUI.Activate();
			_promptUI.SetState(promptUI);
        }

		public void HidePrompt()
        {
			showingPrompt = false;

			promptUI.Remove();
        }

		public void StartGame()
		{
			game.Start();
			playerUI.Show();
		}

		public override void UpdateUI(GameTime gameTime)
		{
			_modalUI?.Update(gameTime);
			_promptUI?.Update(gameTime);
			_playerUI?.Update(gameTime);
			_historyUI?.Update(gameTime);
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{
            List<string> disabledLayers = new List<string> { "Vanilla: Hotbar", "Vanilla: Inventory", "Vanilla: Info Accessories Bar", "Vanilla: Builder Accessories Bar", "Vanilla: Map / Minimap", "Vanilla: Resource Bars" };
			int inventoryLayerIndex = -1;
			int i = 0;
			foreach (GameInterfaceLayer layer in layers)
            {
                if (disabledLayers.Contains(layer.Name))
                {
                    layer.Active = false;
                }

				if (layer.Name == "Vanilla: Mouse Text")
                {
					inventoryLayerIndex = i;
                }

				i++;
            }

			if (inventoryLayerIndex != -1)
			{
				layers.Insert(inventoryLayerIndex, new LegacyGameInterfaceLayer(
					"Slackenhash: Prompt",
					delegate
					{
						if (showingPrompt)
						{
							_promptUI.Draw(Main.spriteBatch, new GameTime());
						}
						return true;
					},
					InterfaceScaleType.UI
				));
				layers.Insert(inventoryLayerIndex, new LegacyGameInterfaceLayer(
					"Slackenhash: History",
					delegate
					{
						_historyUI.Draw(Main.spriteBatch, new GameTime());
						return true;
					},
					InterfaceScaleType.UI
				));

				layers.Insert(inventoryLayerIndex, new LegacyGameInterfaceLayer(
						"Slackenhash: Players",
						delegate
						{
							_playerUI.Draw(Main.spriteBatch, new GameTime());
							return true;
						},
						InterfaceScaleType.UI
					)
				);

				layers.Insert(inventoryLayerIndex, new LegacyGameInterfaceLayer(
					"Slackenhash: Modal",
					delegate
					{
						if (showingModal && modalUI != null)
						{
							_modalUI.Draw(Main.spriteBatch, new GameTime());
						}
						return true;
					},
					InterfaceScaleType.UI
				));
			}
		}
	}
}