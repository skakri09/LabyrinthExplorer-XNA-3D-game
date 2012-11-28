using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace LabyrinthExplorer
{
    public class Menu
    {
        public enum MenuActions { NEXT_ENTRY, PREV_ENTRY, SELECT_ENTRY, BACK }

        private Dictionary<MenuEntry, Vector2> MainMenuEntries = new Dictionary<MenuEntry, Vector2>();
        private Dictionary<MenuEntry, Vector2> PauseMenuEntries = new Dictionary<MenuEntry, Vector2>();

        private Dictionary<MenuActions, Keys> MenuKeys = new Dictionary<MenuActions, Keys>();

        private int currentSelectionIndex = 0;
        private int prevSelectionIndex = 0;
        private GameStates previousGameState;
        private GameStates menuType;

        Texture2D menuScreen;
        private string useHeadphones = "Use headphones if you have some available!";
        private string chooseEasy = "Unless you love ridiculously hard labyrinths, choose Easy";
        
        public Menu(ContentManager content)
        {
            CreateMenuKeys();

            CreateMenuEntries();

            menuScreen = content.Load<Texture2D>(@"Textures\menuscreen");
        }

        private void CreateMenuEntries()
        {
           
            MenuEntry playGameEasy = new MenuEntry("Easy");
            playGameEasy.Selected += StartDiffEasy;
            MainMenuEntries[playGameEasy] = GetPosition(GameStates.MainMenu);

            MenuEntry playGameMedium = new MenuEntry("Medium");
            playGameMedium.Selected += StartDiffMedium;
            MainMenuEntries[playGameMedium] = GetPosition(GameStates.MainMenu);

            MenuEntry playGameHard = new MenuEntry("Hard");
            playGameHard.Selected += StartDiffHard;
            MainMenuEntries[playGameHard] = GetPosition(GameStates.MainMenu);

            MenuEntry quitGameEntry = new MenuEntry("Quit Game");
            quitGameEntry.Selected += QuitGame;
            MainMenuEntries[quitGameEntry] = GetPosition(GameStates.MainMenu);

            MenuEntry resumeGameEntry = new MenuEntry("Resume Game");
            resumeGameEntry.Selected += ResumeGame;
            PauseMenuEntries[resumeGameEntry] = GetPosition(GameStates.PAUSE);

            PauseMenuEntries[quitGameEntry] = GetPosition(GameStates.PAUSE);

        }

        private void CreateMenuKeys()
        {
            AddKey(MenuActions.BACK, Keys.Escape);

            //AddKey(MenuActions.SELECT_ENTRY, Keys.Space);
            AddKey(MenuActions.SELECT_ENTRY, Keys.Enter);

            AddKey(MenuActions.NEXT_ENTRY, Keys.Down);
            AddKey(MenuActions.NEXT_ENTRY, Keys.S);

            AddKey(MenuActions.PREV_ENTRY, Keys.Up);
            AddKey(MenuActions.PREV_ENTRY, Keys.W);
        }

        public void EnterMenu(GameStates stateWhenEntering, GameStates menuTypeToEnter)
        {
            previousGameState = stateWhenEntering;
            currentSelectionIndex = 0;
            prevSelectionIndex = 0;
            menuType = menuTypeToEnter;

            if (menuType == GameStates.MainMenu)
            {
                Game.SoundManager.PlaySong("LOD", true);
                foreach (MenuEntry entry in MainMenuEntries.Keys)
                {
                    entry.SetUnselected();
                }
                MainMenuEntries.ElementAt(currentSelectionIndex).Key.SetSelected();
            }
            else if (menuType == GameStates.PAUSE)
            {
                foreach (MenuEntry entry in PauseMenuEntries.Keys)
                {
                    entry.SetUnselected();
                }
                PauseMenuEntries.ElementAt(currentSelectionIndex).Key.SetSelected();
            }
        }

        public void UpdateMenu(InputManager input)
        {
            if(input.IsKeyDownOnce(MenuKeys[MenuActions.NEXT_ENTRY]))
            {
                ++currentSelectionIndex;
                if (menuType == GameStates.MainMenu)
                {
                    if (currentSelectionIndex >= MainMenuEntries.Count)
                        currentSelectionIndex = 0;
                    MainMenuEntries.ElementAt(currentSelectionIndex).Key.SetSelected();
                    MainMenuEntries.ElementAt(prevSelectionIndex).Key.SetUnselected();
                }
                else if (menuType == GameStates.PAUSE)
                {
                    if (currentSelectionIndex >= PauseMenuEntries.Count)
                        currentSelectionIndex = 0;
                    PauseMenuEntries.ElementAt(currentSelectionIndex).Key.SetSelected();
                    PauseMenuEntries.ElementAt(prevSelectionIndex).Key.SetUnselected();
                    
                }
                prevSelectionIndex = currentSelectionIndex;
            }
            
            if(input.IsKeyDownOnce(MenuKeys[MenuActions.PREV_ENTRY]))
            {
                --currentSelectionIndex;
                if (menuType == GameStates.MainMenu)
                {
                    if (currentSelectionIndex < 0)
                        currentSelectionIndex = MainMenuEntries.Count - 1;
                    MainMenuEntries.ElementAt(currentSelectionIndex).Key.SetSelected();
                    MainMenuEntries.ElementAt(prevSelectionIndex).Key.SetUnselected();
                }
                else if (menuType == GameStates.PAUSE)
                {
                    if (currentSelectionIndex < 0)
                        currentSelectionIndex = PauseMenuEntries.Count-1;
                    PauseMenuEntries.ElementAt(currentSelectionIndex).Key.SetSelected();
                    PauseMenuEntries.ElementAt(prevSelectionIndex).Key.SetUnselected();
                }
                prevSelectionIndex = currentSelectionIndex;
            }
            if(input.IsKeyDownOnce(MenuKeys[MenuActions.SELECT_ENTRY]))
            {
                if (menuType == GameStates.MainMenu)
                    MainMenuEntries.ElementAt(currentSelectionIndex).Key.OnEntrySelected();
                else if (menuType == GameStates.PAUSE)
                    PauseMenuEntries.ElementAt(currentSelectionIndex).Key.OnEntrySelected();
            }
            
            if(input.IsKeyDownOnce(MenuKeys[MenuActions.BACK]))
            {
                if (previousGameState == GameStates.MainMenu)
                {
                    Game.quitGame = true;
                }
                else if (previousGameState == GameStates.GAME)
                {
                    Game.currentGameState = GameStates.GAME;
                }
                else if (previousGameState == GameStates.PAUSE)
                {
                    //it cant really be, can it %))
                    throw new Exception("Ok, what the fuck happened now, time to get some sleep?");
                }
            }
        }

        public void DrawMenu(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            spriteBatch.Draw(menuScreen, Vector2.Zero, Color.White);

            spriteBatch.DrawString(font, chooseEasy, new Vector2(900, 300), Color.Tomato, 
                MathHelper.ToRadians(-30), Vector2.Zero, 0.25f, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, useHeadphones, new Vector2(900, 350), Color.Tomato, 
                MathHelper.ToRadians(-30), Vector2.Zero, 0.25f, SpriteEffects.None, 0);
            if (menuType == GameStates.MainMenu)
            {
                foreach (MenuEntry entry in MainMenuEntries.Keys)
                {
                    entry.Draw(spriteBatch, font, MainMenuEntries[entry], 1);
                }
            }
            else if (menuType == GameStates.PAUSE)
            {
                foreach (MenuEntry entry in PauseMenuEntries.Keys)
                {
                    entry.Draw(spriteBatch, font, PauseMenuEntries[entry], 1);
                }
            }
            else
                throw new Exception("Menu type is not mainMenu or pause menu O.o");

            
            spriteBatch.End();
        }
   
        private void AddKey(MenuActions action, Keys key)
        {
            //MenuKeys.Add(action, key);
            MenuKeys[action] = key;
        }

        private Vector2 GetPosition(GameStates menuType)
        {
            Vector2 retVec;
            if(menuType == GameStates.MainMenu)
            {
                retVec.X = 400;
                retVec.Y = MainMenuEntries.Count * 120 + 200;
            }
            else if(menuType == GameStates.PAUSE)
            {
                retVec.X = 400;
                retVec.Y = PauseMenuEntries.Count * 120 + 200;
            }
            else
                throw new Exception("Menu type is not mainmenu or pause o.O");
            return retVec;
        }

        #region menu events

        void NewGame(object sender, EventArgs e)
        {
            Game.currentGameState = GameStates.GAME;
        }

        void ResumeGame(object sender, EventArgs e)
        {
            Game.currentGameState = GameStates.GAME;
        }

        void QuitGame(object sender, EventArgs e)
        {
            Game.quitGame = true;
        }

        void BackToMainMenu(object sender, EventArgs e)
        {
            Game.currentGameState = GameStates.MainMenu;
        }

        void StartDiffHard(object sender, EventArgs e)
        {
            World.currentDifficulity = Difficulity.HARD;
            Game.SoundManager.FadeSong(0, new TimeSpan(1000));
            //Game.SoundManager.StopSong();
            Game.currentGameState = GameStates.GAME;
        }

        void StartDiffMedium(object sender, EventArgs e)
        {
            World.currentDifficulity = Difficulity.MEDIUM;
            Game.SoundManager.FadeSong(0, new TimeSpan(1000));
            //Game.SoundManager.StopSong();
            Game.currentGameState = GameStates.GAME;
        }

        void StartDiffEasy(object sender, EventArgs e)
        {
            World.currentDifficulity = Difficulity.EASY;
            Game.SoundManager.FadeSong(0, new TimeSpan(1000));
           // Game.SoundManager.StopSong();
            Game.currentGameState = GameStates.GAME;
        }

        #endregion menu events

    }
}
