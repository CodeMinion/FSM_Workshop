/*************************************************************************
 * SIG-Games Workshop
 * 
 * Main Topic: Finite State Machines
 * Sub Topics: Singleton Design Pattern
 *             State Design Pattern
 * 
 * Purpose: The main goal of this workshop is to introduce finite state 
 *          machines (FSM) and demonstrated how they can be applied in 
 *          the context of games. In the case of this example we use
 *          an FSM to control our goblin lumberjack as he goes on the 
 *          world and attempts to gather wood.
 
 *          As an alternate goal this workshops attempts to introduce
 *          some design patterns along with their implementation and
 *          ussage. 
 *          State Design Pattern: Used in order to create or FSM.
 *          Singlton Design Patter: Used to maintain only one
 *                                  one instance of each state at
 *                                  any point in time. Since all the 
 *                                  state information is held by the 
 *                                  owner of the state there is no
 *                                  need to create new instance of the 
 *                                  states every time.
 *                                  
 * Author: Frank E. Hernandez
 * A.K.A.: CodeMinion
 * Site: http://www.cs.fiu.edu/~fhern006
 * 
 
 * Art:
 * 
 * Author: Yar
 * Filename: iso-64x64-outside
 * Link: http://opengameart.org/content/isometric-64x64-outside-tileset
 * 
 * Author: Blarumyrran
 * Filename: human-city
 * Link: http://opengameart.org/content/old-stone-buildings
 * 
 * 
 * Author: Clint Bellanger
 * Filename: goblin_lumberjack_black.png
 * Filename: goblin_lumberjack_blue.png
 * Filename: goblin_lumberjack_cyan.png
 * Filename: goblin_lumberjack_green.png
 * Filename: goblin_lumberjack_orange.png
 * Filename: goblin_lumberjack_pink.png
 * Filename: goblin_lumberjack_purple.png
 * Filename: goblin_lumberjack_red.png
 * Filename: goblin_lumberjack_white.png
 * Filename: goblin_lumberjack_yellow.png
 * Link: http://opengameart.org/content/goblin-lumberjack 
 * 
 * Author: qubodup
 * Filename: pointer
 * Link: http://opengameart.org/content/bw-ornamental-cursor-19x19
 * 
 * 
 * Audio:
 * Author: qubodup
 * Filename: click-click-mono
 * Link: http://opengameart.org/content/click-ui-menu-sfx-yesnoselect
 * 
 * 
 * ***********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using IsometricMap.Map;
using IsometricMap.Entities;

using IsometricMap.Logic.States.Goblin.Lumberjack;
namespace IsometricMap
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region These aren't the droids you are looking for.

        MapTile m_TestTile;
        MapLayer m_TestLayer;

        Texture2D mapTexture;

        float nextUpdate = 0;
        float updateDelay = 500;

        
        KeyboardState prevState;
        MouseState prevMouse;

        List<List<int>> m_mapList;

        List<TreeEntity> m_TreeList;

        List<LumberMill> m_LumberMills;

        GameEntitity m_Gobling;

        Vector2 m_mousePostion;
        Texture2D m_mouseTexture;
        Rectangle m_mouseSource;

        int m_LayerWidth = 40;
        int m_DrawWidth = 10;
        int m_LayerHeight = 40;
        int m_DrawHeight = 25;

        SpriteFont m_testFont;
        int m_nTreeAmount = 4;

        Texture2D m_BorderTexture;
        Vector2 m_BorderPosition = new Vector2(-5, -40);

        SoundEffectInstance m_sMouseClick;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            #region These aren't the droids you are looking for.

            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";


            m_TreeList = new List<TreeEntity>();
            m_TreeList.Add(new TreeEntity(new Vector2(2 * 64, 4 * 24)));
            m_TreeList.Add(new TreeEntity(new Vector2(9 * 64 + 32, 7 * 24)));
            m_TreeList.Add(new TreeEntity(new Vector2(0 * 64, 0 * 24)));
            m_TreeList.Add(new TreeEntity(new Vector2(1 * 64, 10 * 24)));

            m_LumberMills = new List<LumberMill>();
            m_LumberMills.Add(new LumberMill(new Vector2(8 * 64, 8 * 24)));

            m_mouseSource = new Rectangle(0, 0, 19, 19);
            
            m_TestLayer = new MapLayer(new Vector2(m_LayerWidth, m_LayerHeight), new Vector2(64, 64));
        
            m_TestTile = new MapTile(new Vector2(0, 0), 0, new Vector2(64,64));

#endregion
            
            m_Gobling = new GoblinLumberjack(ENTITY_COLOR.RED, new Vector2(64 * 8, 0), DIRECTION.DOWN);
            
            #region These aren't the droids you are looking for.

            ((GoblinLumberjack)m_Gobling).SetTargetLumberMill(m_LumberMills[0]);
            
            m_Gobling.SetEntitySpeed(new Vector2(1, 1));
            m_mapList = new List<List<int>>();
            m_mapList.Add(new List<int>() { });
            #endregion
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            #region These aren't the droids you are looking for.

            m_BorderTexture = Content.Load<Texture2D>("SIG_Games_Utils/Border");

            mapTexture = Content.Load<Texture2D>("Map_Tileset/iso-64x64-outside");

            m_mouseTexture = Content.Load<Texture2D>("Sprites/Mouse/pointer");

            m_testFont = Content.Load<SpriteFont>("Fonts/DisplayFont");
            //mapTexture = Content.Load<Texture2D>("Map_Tileset/32_flagstone_tiles");
            m_TestTile.SetTileSet(mapTexture);

            m_TestLayer.SetTileSetRef(mapTexture);

            m_TestLayer.SetDrawViewEnd(new Vector2(m_DrawWidth, m_DrawHeight));
            GenerateRandomMap();

            m_Gobling.SetSpriteTexture(Content.Load<Texture2D>(((GoblinLumberjack)m_Gobling).GetResroucePath()));
            for (int i = 0; i < m_nTreeAmount; i++)
                m_TreeList[i].SetSpriteTexture(mapTexture);

            Texture2D lumberMillText = Content.Load<Texture2D>(m_LumberMills[0].GetResourceFile());

            m_LumberMills[0].SetSpriteTexture(lumberMillText);

            SoundEffect mouseEffect = Content.Load<SoundEffect>("SoundEffects/Mouse/click-click-mono");
            m_sMouseClick = mouseEffect.CreateInstance();
            
            #endregion

        }
        #region These aren't the droids you are looking for.

        public void GenerateRandomMap()
        {
            if (m_TestLayer == null)
                return;

            Vector2 tiles = MapHandler.GetInstance().GetTileSetSizeByTiles(m_TestLayer.GetTileSetRef(), new Vector2(64,64));
            int totalTiles = (int)(tiles.X * tiles.Y);
            Random rand = new Random();
            for (int i = 0; i < m_TestLayer.GetMapSize().Y; i++)
            {
                for (int j = 0; j < m_TestLayer.GetMapSize().X; j++)
                {
                    int tileID = 0;//134;//34;//rand.Next(0, totalTiles);
                    m_TestLayer.AddTile(tileID, new Vector2(i, j));
                }
            }
        }

        /// <summary>
        /// Used to reset the map. 
        /// </summary>
        public void ResetMap()
        {
            m_TestLayer = new MapLayer(new Vector2(40, 40), new Vector2(64, 64));
            m_TestLayer.SetTileSetRef(mapTexture);
            GenerateRandomMap();

        }
        #endregion

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            #region These aren't the droids you are looking for.

            KeyboardState currState = Keyboard.GetState();

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // Exit the game if the escape key is pressed.
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //if (Keyboard.GetState().IsKeyDown(Keys.F5))
            //    ResetMap();

            if (!prevState.Equals(currState))
            {
                /*
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    m_TestLayer.ScrollLeft();
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    m_TestLayer.ScrollRight();
                if (currState.IsKeyDown(Keys.Up))
                    m_TestLayer.ScrollUp();
                if (currState.IsKeyDown(Keys.Down))
                    m_TestLayer.ScrollDown();
                 */
            }

            prevState = currState;
           
            MouseState currMouse = Mouse.GetState();
            if (!currMouse.Equals(prevMouse))
            {

                m_mousePostion.X = currMouse.X;
                m_mousePostion.Y = currMouse.Y;

                if (currMouse.LeftButton == ButtonState.Pressed)
                {
                    m_sMouseClick.Play();
                    bool bClicked = false;
                    // Check if a tree was clicked.
                    for (int i = 0; i < m_nTreeAmount; i++)
                    {
                        if (m_TreeList[i].IsTreeClicked(m_mousePostion))
                        {
                            m_Gobling.MoveDistanceInFrontOfEntity(m_TreeList[i], 0);
                            bClicked = true;
                            break;
                        }
                    }
                    // Check if a lumber mill was clicked.
                    if (m_LumberMills[0].IsClicked(m_mousePostion))
                    {
                        m_Gobling.MoveDistanceInFrontOfEntity(m_LumberMills[0], 0);
                        bClicked = true;
                    }
                    // If neither a lumber mill or a tree was clicked and the
                    // click occurred inside the clickable area then it must have
                    // been a tile click. 
                    if (!bClicked && !IsClickOutsideClickArea(m_mousePostion))
                    {
            #endregion
                        // Set the state of the gobling to moving if we click anywhere else
                        // that is not a tree or a lumber mill.
                        ((GoblinLumberjack)m_Gobling).GetStateMachine().ChangeState(GLumberjack_MOVE.GetInstance());

                        #region These aren't the droids you are looking for.

                        ((GoblinLumberjack)m_Gobling).SetTargetTree(null);
                        int x;
                        int y = currMouse.Y / (int)(64 * 0.25);
                        if (y % 2 == 1)
                        {
                            x = (currMouse.X - 32) / 64;
                        }
                        else
                        {
                            x = (int)currMouse.X / 64;
                        }
                        Vector2 dest = new Vector2();
                        if (y % 2 == 1)
                        {
                            dest.X = x * 64 + 32;
                        }
                        else
                        {
                            dest.X = x * 64;
                        }
                        dest.Y = y * (int)(64 * 0.25) - (int)(64 * 0.50);
                        if (dest.X >= 0 && dest.X < m_DrawWidth * 64
                            && dest.Y >= 0 && dest.Y < m_DrawHeight * (int)(64 * 0.25))
                            m_Gobling.MoveTo(dest);
                    }

                }
            }
            prevMouse = currMouse;

            // TODO: Add your update logic here
            float currentUpdate =  (float)gameTime.TotalGameTime.TotalMilliseconds;
            if (nextUpdate <= currentUpdate)
            {

                //tileCounter = (tileCounter+1) % (mapTexture.Width / 64 * mapTexture.Height / 64);
                //m_TestTile.SetTileID(tileCounter);
                nextUpdate = currentUpdate + updateDelay;
            }

            m_Gobling.Update(gameTime);

            m_LumberMills[0].Update(gameTime);

                        #endregion
            base.Update(gameTime);
        }
        #region These aren't the droids you are looking for.

        public bool IsClickOutsideClickArea(Vector2 mousePosition)
        {
            if (mousePosition.X < 0)
                return true;

            if (mousePosition.X > m_LayerWidth * 64)
                return true;

            if (mousePosition.Y < 12)
                return true;

            if (mousePosition.Y > m_LayerHeight * 64 * 0.75)
                return true;

            return false;
        }
        #endregion
        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            #region These aren't the droids you are looking for.

            spriteBatch.Begin();
            // Draw the map layer we are walking on.
            m_TestLayer.Draw(spriteBatch, new Vector2(0, 0 ));

            // Draw the goblin lumberjack.
            m_Gobling.Draw(spriteBatch);
            
            // Draw all the trees we have.
            for (int i = 0; i < m_nTreeAmount; i++)
                m_TreeList[i].Draw(spriteBatch);

            // Draw the lumber mill.
            m_LumberMills[0].Draw(spriteBatch);

            spriteBatch.End();

            spriteBatch.Begin();
            // Draw Border.
            spriteBatch.Draw(m_BorderTexture, m_BorderPosition, Color.White);
            // Draw Workshop Info
            spriteBatch.DrawString(m_testFont, "Current State:", new Vector2(20, 450), Color.White);
#endregion

            if (((GoblinLumberjack)m_Gobling).GetStateMachine().GetCurrentState() != null)
            {
                spriteBatch.DrawString(m_testFont, ((GoblinLumberjack)m_Gobling).GetStateMachine().GetCurrentState().GetStateName(), new Vector2(20, 470), Color.White);

            }

            #region These aren't the droids you are looking for.

            spriteBatch.DrawString(m_testFont, "Logs Carried: " + ((GoblinLumberjack)m_Gobling).GetCarriedWood() + " / " + ((GoblinLumberjack)m_Gobling).GetLogMaxCapacity(), new Vector2(20, 500), Color.White);
            spriteBatch.DrawString(m_testFont, "Tree Wood Left: " + ((GoblinLumberjack)m_Gobling).GetTargetTreeWoodLeft(), new Vector2(20, 520), Color.White);

            // Draw the mouse
            spriteBatch.Draw(m_mouseTexture, m_mousePostion, m_mouseSource, Color.White);

            
            spriteBatch.End();
            #endregion

            base.Draw(gameTime);
        }
    }
}
