using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using IsometricMap;
using IsometricMap.Entities;
using IsometricMap.Sprites;

//using IsometricMap.Logic;
//using IsometricMap.Logic.Base;
//using IsometricMap.Logic.States.Goblin.Lumberjack;

namespace IsometricMap.Entities
{
    /// <summary>
    /// Enumeration for the different 
    /// colors of this goblin.
    /// </summary>
    public enum ENTITY_COLOR 
    {
        BLACK,
        BLUE,
        CYAN,
        GREEN,
        ORANGE,
        PINK,
        PURPLE,
        RED,
        WHITE,
        YELLOW

    };

    /// <summary>
    /// Class representing a lumberjack gobling.
    /// </summary>
    public class GoblinLumberjack:GameEntitity
    {
        #region Nothing to see here, move along.
        /// <summary>
        /// This contains all the sprite files for all
        /// the possible lumberjack goblin.
        /// </summary>
        List<string> m_ResourceFiles = new List<string>() 
        { 
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_black",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_blue",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_cyan",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_green",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_orange",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_pink",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_purple",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_red",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_white",
            "Sprites/Gobling/Lumberjack/goblin_lumberjack_yellow"
        };

        /// <summary>
        /// This holds the name for all the animations of this
        /// gobling.
        /// </summary>
        public List<string> m_AnimationIDs = new List<string>()
        {
            "IDLE",
            "MOVE",
            "CARRY_LOGS",
            "CHOP",
            "PUT_DOWN",
            "BLOCK",
            "DIE"
        };

        /// <summary>
        /// This holds the size of each animation 
        /// for this goblin.
        /// </summary>
        List<int> m_AnimationSizes = new List<int>()
        {
            4, // IDLE
            8, // MOVE
            8, // CARRY_LOGS
            6, // CHOP
            4, // PUT_DOWN
            2, // BLOCK
            6  // DIE 
        };

        ENTITY_COLOR m_myColor;
        int m_currLogCarried = 0;
        int m_maxLogCapacity = 10;
        int m_chopDelay = 6*150; // This is the amount of time it takes to obtain on log.
        double m_dNextChop = 0;

        int m_depositDelay = 150;
        double m_dNextDeposit = 0;
        bool m_bChoopingWood = false;
        bool m_bDepositingWood = false;

        SoundEffect m_chopSound;
        SoundEffectInstance m_ChopInstance;

        TreeEntity m_TargetTree;
        LumberMill m_TargetLumberMill;


        // Declare the state machine object.
        public GoblinLumberjack(ENTITY_COLOR color)
        {
            m_myColor = color;
            m_SpriteSize = new Vector2(64, 64);
            LoadAnimations("");
            // Create a new state machine.
            // Set the IDLE state as the start state.
            

        }

        public GoblinLumberjack(ENTITY_COLOR color, Vector2 start_postion, DIRECTION dir):this(color)
        {
            m_WorldPosition = start_postion;
            m_DrawPosition = start_postion;
            m_DestinationPosition = start_postion;
            SetCurrentAnimation(dir, "IDLE");
        }

        /// <summary>
        /// This returns the path to the sprite file for
        /// this goblin.
        /// </summary>
        /// <returns></returns>
        public string GetResroucePath()
        {
            return m_ResourceFiles[(int)m_myColor];
        }

        /// <summary>
        /// This returns the path to the sound
        /// rsource file. 
        /// </summary>
        /// <returns></returns>
        public string GetSoundResourcePath()
        {
            return "SoundEffects/Goblin/Lumberjack/wood_chop";
        }

        /// <summary>
        /// This sets the sound effect for the wood chop.
        /// </summary>
        /// <param name="effect"></param>
        public void SetSoundEffect(SoundEffect effect)
        {
            m_chopSound = effect;
            m_ChopInstance = m_chopSound.CreateInstance();
        }

        protected override void DrawEntity(SpriteBatch spriteBatch)
        {
            //throw new NotImplementedException();
        }
        /// <summary>
        /// Handles the logic for the goblin.
        /// </summary>
        /// <param name="time"></param>
        protected override void UpdateEntity(GameTime time)
        {
            // if the goblin is moving an carrying logs then 
            // we play the carry animation, otherwise we play 
            // the default animation.
            if (IsEntityMoving())
            {
                if (m_currLogCarried > 0)
                    SetCurrentAnimation(m_EntityDircetion, m_AnimationIDs[2]);
                else
                    SetCurrentAnimation(m_EntityDircetion, m_AnimationIDs[1]);
            }
        
            // If we are chopping wood then play the wood_chop sound.
            if(m_currentAnimation.Equals(m_Animations[m_EntityDircetion][m_AnimationIDs[3]]))
            {
                PlayChopSound();
            }
            // only gather if we have waited enough.
            if (m_dNextChop < time.TotalGameTime.TotalMilliseconds)
            {
                GatherWood();
                m_dNextChop = time.TotalGameTime.TotalMilliseconds + m_chopDelay;
            }
            // only deposit wood if we have waited enough.
            if (m_dNextDeposit < time.TotalGameTime.TotalMilliseconds)
            {
                RemoveWood();
                m_dNextDeposit = time.TotalGameTime.TotalMilliseconds + m_chopDelay;
            }
            
            // Update statemachine.
            
        }

        protected override void LoadAnimations(string animationFile)
        {
            String animationID = "";
            Animation anim = null;
            DIRECTION dir = DIRECTION.UP;
            int frameDelay = 150;
            int spriteWidth = 64;
            int spriteHeight = 64;

            int animCount =0;
            for(int i = 0; i < m_AnimationSizes.Count; i ++)
            {
                animationID = m_AnimationIDs[i];
                if(i <=0)
                    animCount = 0;
                else
                    animCount += m_AnimationSizes[i-1];

                for(int j =0;  j< 8 ; j++)
                {
                    anim = new Animation(animCount * spriteWidth, j * spriteHeight, m_AnimationSizes[i], spriteWidth, spriteHeight, frameDelay, true);
                    dir = (DIRECTION)j;
                    AddAnimation(dir, animationID, anim);
            
                }
            }
            SetCurrentAnimation(dir, animationID);

        }

        /// <summary>
        /// This method plays the sound of wood chopping.
        /// Not implemented yet.
        /// </summary>
        public void PlayChopSound()
        {
            //if (m_ChopInstance.State == SoundState.Stopped)
            //    m_ChopInstance.Play();
        }

        /// <summary>
        /// If the entity to go to is a tree then we set it as our target tree.
        /// If the rntity to go to is a lumber mill then we set it as our target
        /// mill.
        /// 
        /// Otherwise it delegates to the parent's implementation.
        /// </summary>
        /// <param name="toEntity"></param>
        /// <param name="distance"></param>
        public override void MoveDistanceInFrontOfEntity(GameEntitity toEntity, int distance)
        {
            // If the target is a tree then save it
            // as a target.
            
            // If it is a lumbermill then save it 
            // as a target.
            
            // Perform the normal logic.
            base.MoveDistanceInFrontOfEntity(toEntity, distance);
        }

        /// <summary>
        /// Sets both target mill and tree to null.
        /// Then it refers to the parent.
        /// </summary>
        /// <param name="targetPos"></param>
        public override void MoveTo(Vector2 targetPos)
        {
            m_TargetLumberMill = null;
            m_TargetEntity = null;

            base.MoveTo(targetPos);
        }

        public TreeEntity GetTargetTree()
        {
            return m_TargetTree;
        }
        public void SetTargetTree(TreeEntity tree)
        {
            m_TargetTree = tree;
        }
        public LumberMill GetTargetLumberMill()
        {
            return m_TargetLumberMill;
        }
        public void SetTargetLumberMill(LumberMill mill)
        {
            m_TargetLumberMill = mill;
        }
        // Write a method to return the state machine.
        
        /// <summary>
        /// Sets the goblin ready to chop wood.
        /// </summary>
        public void ChopWood()
        {
            if (m_TargetTree == null)
                return;

            m_bChoopingWood = true;
         
        }
        public void StopChoppingWood()
        {
            m_bChoopingWood = false;
        }
        /// <summary>
        /// Gets the actual wood.
        /// </summary>
        private void GatherWood()
        {
            if (m_bChoopingWood)
            {
                if (m_currLogCarried < 0)
                    m_currLogCarried = 0;

                if (m_TargetTree == null)
                    return; 
                // Chop for as long as you can carry.
                if (m_currLogCarried < m_maxLogCapacity)
                    m_currLogCarried += m_TargetTree.GetWood();

            }  
        }
        public bool IsDoneChoppingWood()
        {
            if (m_maxLogCapacity <= m_currLogCarried || m_TargetTree == null || !m_TargetTree.IsEnabled())
            {
                m_bChoopingWood = false;

                if (!m_TargetTree.IsEnabled())
                    m_TargetTree = null;

                return true;
            }
            return false;
        }

        public void DepositWood()
        {
            if (m_TargetLumberMill == null)
                return;

            m_bDepositingWood = true;

        }
        public void StopDepositingWood()
        {
            m_bDepositingWood = false;
        }
        private void RemoveWood()
        {
            if (m_bDepositingWood)
            {
                // Drop for as long as you carry logs.
                if (m_currLogCarried > 0)
                    m_currLogCarried -= 9;
            }
        }
        public bool IsDoneDropingWood()
        {
            if (m_currLogCarried <= 0 || m_TargetLumberMill == null || !m_TargetLumberMill.IsEnabled())
            {
                if (m_currLogCarried < 0)
                    m_currLogCarried = 0;

                m_bDepositingWood = false;

                return true;
            }
            return false;
        }
        public int GetCarriedWood()
        {
            return m_currLogCarried;
        }
        public int GetTargetTreeWoodLeft()
        {
            if (m_TargetTree == null)
                return -1;

            return m_TargetTree.GetWoodCount();
        }
        public int GetLogMaxCapacity()
        {
            return m_maxLogCapacity;
        }
        #endregion

    }
}
