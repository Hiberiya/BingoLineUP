using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BingoLineUp.Classes
{
    enum PlayerAction { None, SelectPositionChip, DropChip, Wins, StandOff, TheEnd }
    enum GameState { InGame, InMainMenu, ToCloseGame}
    class UnionClasses
    {
        public event EventHandler CloseGame;
        PlayGround chips;
        Texture2D tex2DBackground, tex2DBasis, tex2DBasis2, tex2DChipP1, tex2DChipP2;
        Rectangle rectChip, rectBackground;
        SpriteFont font;
        SoundEffect soundWin, soundMoveChip;
        Song songRelax;
        Menu menuGame;

        public UnionClasses(int n, int m)
        {
            chips = new PlayGround(n, m);
            rectChip = new Rectangle(10, 6, 60, 60);
            rectBackground = new Rectangle(0,0,500,475);
            currentAction = PlayerAction.None;
            currentGameState = GameState.InMainMenu;
            menuGame = new Menu();
            WriteToAvailableEventsInMenu();
        }
        private void WriteToAvailableEventsInMenu()
        {
            menuGame.PlayerReadyToPlaySingleGame += new EventHandler(menuGame_PlayerReadyToPlayGame);
            menuGame.PlayerReadyToPlayNetGame += new EventHandler(menuGame_PlayerReadyToPlayNetGame);
            menuGame.PlayerWantsToSetUp += new EventHandler(menuGame_PlayerWantsToSetUp);
            menuGame.PlayerDoesntWantToPlay += new EventHandler(menuGame_PlayerDoesntWantToPlay);
        }
        void menuGame_PlayerDoesntWantToPlay(object sender, EventArgs e)
        {
            CloseGame.Invoke(this, new EventArgs());
        }

        void menuGame_PlayerWantsToSetUp(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void menuGame_PlayerReadyToPlayNetGame(object sender, EventArgs e)
        {
            currentGameState = GameState.InGame;
        }

        void menuGame_PlayerReadyToPlayGame(object sender, EventArgs e)
        {
            currentGameState = GameState.InGame;
        }
        public void LoadContent(ContentManager content)
        {
            tex2DBackground = content.Load<Texture2D>(@"Images\Background");
            tex2DBasis = content.Load<Texture2D>(@"Images\PlayGround");
            tex2DBasis2 = content.Load<Texture2D>(@"Images\PlayGround2");
            tex2DChipP1 = content.Load<Texture2D>(@"Images\Player1");
            tex2DChipP2 = content.Load<Texture2D>(@"Images\Player2");
            font = content.Load<SpriteFont>(@"Fonts\Font");
            soundWin = content.Load<SoundEffect>(@"Sounds\Win");
            soundMoveChip = content.Load<SoundEffect>(@"Sounds\MoveChip");
            songRelax = content.Load<Song>(@"Music\RelaxMusic");
            menuGame.LoadContent(content);
            MediaPlayer.Play(songRelax);
        }

        KeyboardState lastKeyboard;
        int indexColumn = 0;
        float currentTime = 0;
        Chip animChip;
        WinnerType winner = WinnerType.None;
        PlayerAction currentAction;
        GameState currentGameState;

        public void Update(GameTime gameTime)
        {
            switch (currentGameState)
            {
                case GameState.InMainMenu: UpdateMainMenu(gameTime); break;
                case GameState.InGame: UpdateGame(gameTime); break;
            }
        }
        private void UpdateGame(GameTime gameTime) 
        {
            switch (currentAction)
            {
                case PlayerAction.None: currentAction = PlayerAction.SelectPositionChip; break;
                case PlayerAction.SelectPositionChip: UpdatePlayerActions(); break;
                case PlayerAction.DropChip: UpdateDropChip(gameTime); break;
                case PlayerAction.Wins: soundWin.Play(); currentAction = PlayerAction.TheEnd; break;
                case PlayerAction.StandOff: currentAction = PlayerAction.TheEnd; break;
                case PlayerAction.TheEnd: break;
            }
        }
        private void UpdateMainMenu(GameTime gameTime) { menuGame.Update(gameTime); }

        private void UpdatePlayerActions()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left) && lastKeyboard.IsKeyUp(Keys.Left))
            { soundMoveChip.Play(); if (indexColumn != 0) { indexColumn--; rectChip.X -= 70; } else { indexColumn = chips.LenghtHorisontal - 1; rectChip.X += indexColumn * 70; } }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right) && lastKeyboard.IsKeyUp(Keys.Right))
            { soundMoveChip.Play(); if (indexColumn != chips.LenghtHorisontal - 1) { indexColumn++; rectChip.X += 70; } else { indexColumn = 0; rectChip.X = 10; } }
            else if (Keyboard.GetState().IsKeyDown(Keys.Space) && lastKeyboard.IsKeyDown(Keys.Space))
            { soundMoveChip.Play(); if (chips.HorisontalWeight[indexColumn] < chips.LenghtVertical) { currentAction = PlayerAction.DropChip; animChip = chips.Add(indexColumn); } }
            else if (Keyboard.GetState().IsKeyDown(Keys.Escape) && lastKeyboard.IsKeyUp(Keys.Escape)) { currentGameState = GameState.InMainMenu; }
            lastKeyboard = Keyboard.GetState();
        }
        private void UpdateDropChip(GameTime gameTime)
        {
            if (currentTime == 0)
            {
                UpdateAnimation();
                currentTime = 0.001f;
            }
            currentTime = MathHelper.Max(0, currentTime - (float)gameTime.ElapsedGameTime.TotalSeconds);
        }
        private void UpdateAnimation()
        {
            animChip.DeposeDown();
            if (animChip.IsFallen)
            {
                winner = chips.HasWinner();
                rectChip = new Rectangle(8, 6, 60, 60);
                indexColumn = 0;
                if (winner == WinnerType.None) currentAction = PlayerAction.SelectPositionChip;
                else if (winner == WinnerType.FirstPlayer || winner == WinnerType.SecondPlayer) currentAction = PlayerAction.Wins;
                else if (winner == WinnerType.StandOff) { } // Сделать ничью
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            switch (currentGameState)
            {
                case GameState.InMainMenu: DrawingMenu(spriteBatch); break;
                case GameState.InGame: DrawingGame(spriteBatch); break;
            }
        }
        private void DrawingMenu(SpriteBatch spriteBatch)
        {
            menuGame.Draw(spriteBatch);
        }
        private void DrawingGame(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex2DBackground, rectBackground, Color.White);

            switch (currentAction)
            {
                case PlayerAction.SelectPositionChip: DrawSelect(spriteBatch);  break;
                case PlayerAction.DropChip:  break;
                case PlayerAction.Wins: DrawWin(spriteBatch);  break;
                case PlayerAction.StandOff: DrawStandOff(spriteBatch); break;
                case PlayerAction.TheEnd: DrawTheEnd(spriteBatch);  break;
            }
            DrawState(spriteBatch);
        }

        private void DrawSelect(SpriteBatch spriteBatch)
        {
            if (chips.IsFirstPlayer) spriteBatch.Draw(tex2DChipP1, rectChip, Color.White);
            else spriteBatch.Draw(tex2DChipP2, rectChip, Color.White);
        }
        private void DrawState(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex2DBasis2, chips.rectBasis, Color.White);
            for (int j = 0; j < chips.LenghtHorisontal; j++)
            {
                for (int i = 0; i < chips.HorisontalWeight[j]; i++)
                {
                    if (chips.Chips[i, j].IsFirstPlayer) spriteBatch.Draw(tex2DChipP1, chips.Chips[i, j].RectChip, Color.White);
                    else spriteBatch.Draw(tex2DChipP2, chips.Chips[i, j].RectChip, Color.White);
                }
            }

            spriteBatch.Draw(tex2DBasis, chips.rectBasis, Color.White);
        }
        private void DrawStandOff(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, "Ничья!", new Vector2(75, 20), Color.Plum); 
        }
        private void DrawWin(SpriteBatch spriteBatch)
        {
            if (winner == WinnerType.FirstPlayer) { spriteBatch.DrawString(font, "Победил первый игрок!", new Vector2(75, 20), Color.Lime); }
            else if (winner == WinnerType.SecondPlayer) { spriteBatch.DrawString(font, "Победил второй игрок!", new Vector2(75, 20), Color.Red);}
        }
        private void DrawTheEnd(SpriteBatch spriteBatch)
        {
            if (winner == WinnerType.FirstPlayer) { spriteBatch.DrawString(font, "Победил первый игрок!", new Vector2(75, 20), Color.Lime); }
            else if (winner == WinnerType.SecondPlayer) {spriteBatch.DrawString(font, "Победил второй игрок!", new Vector2(75, 20), Color.Red); }
            else if (winner == WinnerType.StandOff) { spriteBatch.DrawString(font, "Ничья!", new Vector2(200, 20), Color.Peru); }
        }
    }
}
