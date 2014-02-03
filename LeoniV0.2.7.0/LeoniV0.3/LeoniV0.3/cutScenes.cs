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
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.DataStructures;
using BEPUphysics.Collidables;
using BEPUphysics.MathExtensions;
using BEPUphysics.Entities;
//using SkinnedModel;

namespace LeoniV0_3
{
    public class cutScenes
    {
        #region introAnimations

        Game1 game;

        public cutScenes(Game1 Game)
        {
            this.game = Game;
        }

        float lastlevel = 0;
        bool enter_click = false;
        public void draw()
        {
            (game as Game1).postEffect.CurrentTechnique = (game as Game1).postEffect.Techniques["DepthOfField"];
            (game as Game1).postEffect.Parameters["blurAmount"].SetValue(0);
            (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(255 / 255f, 255 / 255f, 255 / 255f)); 
            (game as Game1).GraphicsDevice.BlendState = BlendState.AlphaBlend;
            
            //spriteBatch.DrawString(testFont, Camera.Yaw.ToString(), new Vector2(100, 100), Color.White, Camera.Yaw, new Vector2(0), 1, SpriteEffects.None, 0);
            //spriteBatch.Draw(MapArrow, new Rectangle(100, 100, 50, 50), null, Color.White, Camera.Yaw, new Vector2(50,50), SpriteEffects.None, 0); 

            if ((game as Game1).level == -1) { (game as Game1).startTime = DateTime.Now.Ticks / 10000; (game as Game1).level = 0; }
            long nowTime = DateTime.Now.Ticks / 10000;

            if ((game as Game1).level == -21) //Credits
            {
                (game as Game1).startTime = DateTime.Now.Ticks / 10000;
                (game as Game1).level = -20;
            }

            #region Credits
            if ((game as Game1).level == -20)
            {
                (game as Game1).lastPos = new Vector3(0, 3, -5);
                (game as Game1).cameraCol.Position = new Vector3(0, 3, -5);
                (game as Game1).Camera.Position = (game as Game1).cameraCol.Position;
                (game as Game1).Camera.Yaw = (float)Math.PI;
                (game as Game1).Camera.Pitch = -0.5f;

                nowTime = DateTime.Now.Ticks / 10000;
                float alpha = nowTime - (game as Game1).startTime;
                (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);
                if (alpha < 3000)
                {
                    Vector2 text_size = (game as Game1).SegoeUI.MeasureString("Leoni");
                    float curcolor = (alpha - 1000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                    drawString("Leoni", new Vector2(450 - (text_size.X / 2), 250 - (text_size.Y) / 2), 0.8f);
                    (game as Game1).Camera.Pitch = -1;
                }
                else if (alpha < 4000)
                {
                    Vector2 text_size = (game as Game1).SegoeUI.MeasureString("Leoni");
                    float curcolor = 1 - (alpha - 3000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                    drawString("Leoni", new Vector2(450 - (text_size.X / 2), 250 - (text_size.Y) / 2), 0.8f);
                    (game as Game1).Camera.Pitch = -1;
                }
                else if (alpha < 6000)
                {
                    Vector2 text_size = (game as Game1).SegoeUI.MeasureString("Justin Marple");
                    float curcolor = (alpha - 5000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                    drawString("Justin Marple", new Vector2(450 - (text_size.X / 2), 250 - (text_size.Y) / 2), 0.8f);
                    (game as Game1).Camera.Pitch = -1;
                }
                else if (alpha < 7000)
                {
                    Vector2 text_size = (game as Game1).SegoeUI.MeasureString("Justin Marple");
                    float curcolor = 1 - (alpha - 6000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                    drawString("Justin Marple", new Vector2(450 - (text_size.X / 2), 250 - (text_size.Y) / 2), 0.8f);
                    (game as Game1).Camera.Pitch = -1;
                }
                else if (alpha < 8000)
                {
                    Vector2 text_size = (game as Game1).SegoeUI.MeasureString("Bryan Caputo");
                    float curcolor = (alpha - 7000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                    drawString("Bryan Caputo", new Vector2(450 - (text_size.X / 2), 250 - (text_size.Y) / 2), 0.8f);
                    (game as Game1).Camera.Pitch = -1;
                }
                else if (alpha < 9000)
                {
                    Vector2 text_size = (game as Game1).SegoeUI.MeasureString("Bryan Caputo");
                    float curcolor = 1 - (alpha - 8000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                    drawString("Bryan Caputo", new Vector2(450 - (text_size.X / 2), 250 - (text_size.Y) / 2), 0.8f);
                    (game as Game1).Camera.Pitch = -1;
                }
                else
                    (game as Game1).level = -10;
            }
            #endregion

            #region MainMenu

            if ((game as Game1).level == -10)//Main Menu
            {
                (game as Game1).actions.Health = 1;
                (game as Game1).actions.Score = 0;
                (game as Game1).overlay_amount = 0;
                (game as Game1).userObjects.selected = -1;
                (game as Game1).userObjects.objects.RemoveRange(0, (game as Game1).userObjects.objects.Count);
                (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);
                
                (game as Game1).flying_fast = false; (game as Game1).songstart = false;//Some glitches that could occur
                if ((game as Game1).startCamPos == 0)
                {
                    (game as Game1).lastPos = new Vector3(0, 3, -5);
                    (game as Game1).cameraCol.Position = new Vector3(0, 3, -5);
                    (game as Game1).chapters = 0; (game as Game1).variabletochange = 0; //Prevent glitches later on
                    (game as Game1).startCamPos = 1;
                }

                (game as Game1).Camera.Position = (game as Game1).cameraCol.Position;
                (game as Game1).Camera.Yaw = (float)Math.PI;
                (game as Game1).Camera.Pitch = -0.5f;

                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(0, 0, 0));
                drawString("Leoni V" + (game as Game1).mainVersion, new Vector2(750, 475), .35f);

                if ((game as Game1).variabletochange == 0)
                {
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(1.7f, .3f, .6f));
                    drawString("Leoni", new Vector2(100, 25), 1.2f);
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(2, 2, 2));
                    drawString("New Game", new Vector2(150, 200), 0.5f);
                    drawString("Level Select", new Vector2(200, 250), 0.5f);
                    drawString("Quit", new Vector2(250, 300), 0.5f);
                    drawString((game as Game1).arrowstring, new Vector2(100 + (50 * (game as Game1).menu_selection), 200 + (50 * (game as Game1).menu_selection)), 0.5f);
                }
                else if ((game as Game1).variabletochange == 1 && (game as Game1).chapters == 0)
                {
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(1.7f, .3f, .6f));
                    drawString("Chapter Select", new Vector2(75, 25), .8f);
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(2, 2, 2));
                    drawString("Chapter 1", new Vector2(150, 200), .5f);
                    drawString("Chapter 2", new Vector2(200, 250), .5f);
                    drawString("Back", new Vector2(250, 300), .5f);
                    drawString((game as Game1).arrowstring, new Vector2(100 + (50 * (game as Game1).menu_selection), 200 + (50 * (game as Game1).menu_selection)), .5f);
                }
                else if ((game as Game1).variabletochange == 2 && (game as Game1).chapters == 1)
                {
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(1.7f, .3f, .6f));
                    drawString("Level Select", new Vector2(80, 25), .9f);
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(2, 2, 2));
                    drawString("Level 1.1", new Vector2(150, 200), .5f);
                    drawString("Level 1.2", new Vector2(200, 250), .5f);
                    drawString("Back", new Vector2(250, 300), .5f);
                    drawString((game as Game1).arrowstring, new Vector2(100 + (50 * (game as Game1).menu_selection), 200 + (50 * (game as Game1).menu_selection)), .5f);
                }
                bool thumbUp = false; bool thumbDown = false;
                /*Check left or right thumbsticks if the user decides to use that*/
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.2f || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y > 0.2f)
                {
                    thumbUp = true;
                }
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.2f || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y < -0.2f)
                {
                    thumbDown = true;
                }
                //Did the user go up or down?
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.Up) || thumbDown || thumbUp)
                {
                    if ((game as Game1).menu_move == false)
                    {
                        if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || Keyboard.GetState().IsKeyDown(Keys.Down) || thumbDown)
                        {
                            if ((game as Game1).menu_selection <= 2 && (game as Game1).menu_selection >= 0)
                            {
                                (game as Game1).menu_selection++;
                                /*for (int i = 0; i <= 5; i++)
                                {
                                    cameraCol.Position = cameraCol.Position - new Vector3(0, .02f, 0);
                                }*/
                            }
                        }
                        if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || Keyboard.GetState().IsKeyDown(Keys.Up) || thumbUp)
                            if ((game as Game1).menu_selection <= 2 && (game as Game1).menu_selection >= 0)
                            {
                                (game as Game1).menu_selection--;
                                /*for (int i = 0; i <= 5; i++)
                                    cameraCol.Position = cameraCol.Position + new Vector3(0, .02f, 0);*/
                            }


                        if ((game as Game1).menu_selection > 2)
                        {
                            (game as Game1).menu_selection = 0;
                            //for (int i = 0; i <= 15; i++)
                            //cameraCol.Position = cameraCol.Position + new Vector3(0, .02f, 0); //Makes it off by a little bit every time :(
                            //cameraCol.Position = new Vector3(0, 3, -5);
                        }
                        else if ((game as Game1).menu_selection < 0)
                        {
                            (game as Game1).menu_selection = 2;
                            /*for (int i = 0; i <= 15; i++)
                                cameraCol.Position = cameraCol.Position - new Vector3(0, .02f, 0); //Makes it off by a little bit every time :(
                                cameraCol.Position = new Vector3(0, 2.75f, -5);*/
                        }

                    }

                    (game as Game1).menu_move = true;
                }
                else
                    (game as Game1).menu_move = false;


                //If a menu selection was made
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    if ((game as Game1).clicked_button == false && (game as Game1).variabletochange == 0)
                    {
                        if ((game as Game1).menu_selection == 0)//New Game
                        {
                            (game as Game1).level = -10.1f;
                            //(game as Game1).LoadLevelOne(-1);
                            (game as Game1).clicked_button = true;
                            (game as Game1).startCamPos = 0;
                        }
                        else if ((game as Game1).menu_selection == 1)//Level Select
                        {
                            (game as Game1).cameraCol.Position = new Vector3(0, 3, -5);
                            (game as Game1).menu_selection = 0;
                            (game as Game1).variabletochange = 1;
                            (game as Game1).clicked_button = true;
                        }
                        else if ((game as Game1).menu_selection == 2)//Exit game
                            (game as Game1).Exit();
                    }
                    else if ((game as Game1).clicked_button == false && (game as Game1).variabletochange == 1 && (game as Game1).chapters == 0)
                    {
                        if ((game as Game1).menu_selection == 0)//Chapter 1
                        {
                            (game as Game1).chapters = 1;
                            (game as Game1).variabletochange = 2;
                            (game as Game1).clicked_button = true;
                        }
                        else if ((game as Game1).menu_selection == 1)//Chapter 2
                        {
                            (game as Game1).cameraCol.Position = new Vector3(0, 3, -5); //ADDD THEESE LINES
                            (game as Game1).menu_selection = 0;
                            (game as Game1).chapters = 1;
                            (game as Game1).variabletochange = 2;
                            (game as Game1).clicked_button = true;
                        }
                        else if ((game as Game1).menu_selection == 2)//Back
                        {
                            (game as Game1).cameraCol.Position = new Vector3(0, 3, -5);
                            (game as Game1).menu_selection = 0;
                            (game as Game1).variabletochange = (game as Game1).variabletochange - 1;
                            (game as Game1).clicked_button = true;
                        }
                    }
                    else if ((game as Game1).clicked_button == false && (game as Game1).variabletochange == 2 && (game as Game1).chapters == 1)
                    {
                        if ((game as Game1).menu_selection == 0)//Level 1.1
                        {

                            (game as Game1).lastPos = new Vector3(0, 250, 0);
                            (game as Game1).Camera.Pitch = 0;
                            (game as Game1).Camera.Yaw = 0;
                            (game as Game1).cameraCol.Position = new Vector3(0, 250, 0);
                            (game as Game1).Camera.Position = (game as Game1).cameraCol.Position;
                            (game as Game1).level = -10.11f;
                            //(game as Game1).LoadLevelOne(-1);
                            MediaPlayer.Play((game as Game1).music[1]);
                            MediaPlayer.IsRepeating = true;
                        }
                        else if ((game as Game1).menu_selection == 1)//Level 1.2
                        {
                            (game as Game1).lastPos = new Vector3(0, 250, 0);
                            (game as Game1).Camera.Pitch = 0;
                            (game as Game1).Camera.Yaw = 0;
                            (game as Game1).cameraCol.Position = new Vector3(0, 250, 0);
                            (game as Game1).Camera.Position = (game as Game1).cameraCol.Position;
                            (game as Game1).level = -10.11f;
                            //(game as Game1).LoadLevelOne(-1);
                            MediaPlayer.Play((game as Game1).music[1]);
                            MediaPlayer.IsRepeating = true;
                        }
                        else if ((game as Game1).menu_selection == 2)//Back
                        {
                            (game as Game1).cameraCol.Position = new Vector3(0, 3, -5);
                            (game as Game1).menu_selection = 0;
                            (game as Game1).variabletochange = (game as Game1).variabletochange - 1;
                            (game as Game1).chapters = (game as Game1).chapters - 1;
                            (game as Game1).clicked_button = true;
                        }
                    }
                }
                else
                    (game as Game1).clicked_button = false;
            }
            else if ((game as Game1).level == -10.1f)//Load Intro
            {
                loadLevel(-1, nowTime);
            }
            else if ((game as Game1).level == -10.11f)//Load Level 1.1
            {
                loadLevel(2, nowTime);
            }

            #endregion
            #region PauseScreen
            
            if ((game as Game1).level == -5)//Pause Screen
            {
                bool thumbUp = false; bool thumbDown = false;
                /*Check left or right thumbsticks if the user decides to use that*/
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.2f || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y > 0.2f)
                {
                    thumbUp = true;
                }
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.2f || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y < -0.2f)
                {
                    thumbDown = true;
                }
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.Up) || thumbUp || thumbDown)
                {
                    (game as Game1).clicked_button = true;
                    if ((game as Game1).pause_move == false)
                    {
                        if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || Keyboard.GetState().IsKeyDown(Keys.Down) || thumbDown)
                            (game as Game1).pause_selection += 1;
                        else if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || Keyboard.GetState().IsKeyDown(Keys.Up) || thumbUp)
                            (game as Game1).pause_selection -= 1;

                        if ((game as Game1).pause_selection > 2)
                            (game as Game1).pause_selection = 0;
                        if ((game as Game1).pause_selection < 0)
                            (game as Game1).pause_selection = 2;

                    }
                    (game as Game1).pause_move = true;
                }
                else
                    (game as Game1).pause_move = false;

                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    if (enter_click == false)
                    {
                        enter_click = true;
                        if ((game as Game1).pause_selection == 0)//Resume                       
                            (game as Game1).level = (game as Game1).pause_level;
                        if ((game as Game1).pause_selection == 1)//Options
                        {
                            (game as Game1).pause_selection = 0;
                            (game as Game1).level = -5.1f;
                        }
                        if ((game as Game1).pause_selection == 2)
                        {
                            (game as Game1).level = -10;
                            (game as Game1).space.ForceUpdater.Gravity = new Vector3(0, 0, 0);
                            (game as Game1).variabletochange = 0;
                            (game as Game1).menu_selection = 0;
                            (game as Game1).startCamPos = 0;
                            (game as Game1).clicked_button = true;
                        }
                    }
                    enter_click = true;
                }
                else
                    enter_click = false;

                (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(1.7f, .3f, .6f));
                drawString("Paused", new Vector2(50, 50), 1);
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(2, 2, 2));
                drawString("Resume", new Vector2(75, 150), 0.5f);
                drawString("Options", new Vector2(75, 190), 0.5f);
                drawString("Main Menu", new Vector2(75, 230), 0.5f);

                drawString((game as Game1).arrowstring, new Vector2(25, 150 + (30 * (game as Game1).pause_selection)), 0.5f);
            }
            if ((game as Game1).level == -5.1f)//Optoins
            {
                bool thumbUp = false; bool thumbDown = false;
                /*Check left or right thumbsticks if the user decides to use that*/
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.2f || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y > 0.2f)
                {
                    thumbUp = true;
                }
                if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.2f || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y < -0.2f)
                {
                    thumbDown = true;
                }
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.Up) || thumbUp || thumbDown)
                {
                    (game as Game1).clicked_button = true;
                    if ((game as Game1).pause_move == false)
                    {
                        
                        if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || Keyboard.GetState().IsKeyDown(Keys.Down) || thumbDown)
                            (game as Game1).pause_selection += 1;
                        else if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || Keyboard.GetState().IsKeyDown(Keys.Up) || thumbUp)
                            (game as Game1).pause_selection -= 1;

                        if ((game as Game1).pause_selection > 1)
                            (game as Game1).pause_selection = 0;
                        if ((game as Game1).pause_selection < 0)
                            (game as Game1).pause_selection = 1;

                    }
                    (game as Game1).pause_move = true;
                }
                else
                    (game as Game1).pause_move = false;

                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    if (enter_click == false)
                    {
                        if ((game as Game1).pause_selection == 0)//Contast
                        {
                            (game as Game1).level = -5.2f;
                        }
                        if ((game as Game1).pause_selection == 1)//Back
                        {
                            (game as Game1).level = -5f;
                            (game as Game1).pause_selection = 0;
                        }
                    }
                    enter_click = true;
                }
                else
                    enter_click = false;

                (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(1.7f, .3f, .6f));
                drawString("Options", new Vector2(50, 50), 1);
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(2, 2, 2));
                drawString("Change Contrast", new Vector2(75, 150), 0.5f);
                drawString("Back", new Vector2(75, 190), 0.5f);

                drawString((game as Game1).arrowstring, new Vector2(25, 150 + (30 * (game as Game1).pause_selection)), 0.5f);
            }
            if ((game as Game1).level == -5.2f)
            {
                (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(1.7f, .3f, .6f));
                drawString("Adjust Contrast", new Vector2(50, 50), 1);
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(2, 2, 2));
                drawString("Use Up/Down keys to adjust", new Vector2(75, 150), 0.5f);
                float contrast = (game as Game1).contrast;
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(contrast + 0.6f, contrast + 0.6f, contrast + 0.6f));
                drawString("You should easily be able to read this", new Vector2(75, 190), 0.5f);
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(contrast + 0.1f, contrast + 0.1f, contrast + 0.1f));
                drawString("You should kinda be able to read this", new Vector2(75, 230), 0.5f);
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(contrast - 0.05f, contrast - 0.05f, contrast - 0.05f));
                drawString("You shouldn't be able to read this", new Vector2(75, 270), 0.5f);
                
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B) || Keyboard.GetState().IsKeyDown(Keys.B))
                {
                    (game as Game1).level = 5.1f;
                }
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    (game as Game1).contrast += 0.003f;
                }
                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    (game as Game1).contrast -= 0.003f;
                }


            }
            #endregion

            #region Intro

            if ((game as Game1).level == 0)
            {
                MediaPlayer.Stop();
                MediaPlayer.Volume = 1f;
                (game as Game1).cameraCol.Position = new Vector3(0, 250, 0);
                (game as Game1).Camera.Position = (game as Game1).cameraCol.Position;
                (game as Game1).Camera.Pitch = 0; (game as Game1).Camera.Yaw = 0;
                if (!(game as Game1).songstart)//Has the song not started yet?
                {
                    (game as Game1).seIntro = (game as Game1).vIntro.CreateInstance();
                    (game as Game1).seIntro.Play();//Start song               
                    (game as Game1).songstart = true;//Confirm that the song has started
                }

                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) || (Keyboard.GetState().IsKeyDown(Keys.Enter)))
                {
                    if ((game as Game1).clicked_button == false)
                    {
                        (game as Game1).level = 1;
                        (game as Game1).seIntro.Stop();
                        (game as Game1).startTime = DateTime.Now.Ticks / 10000;
                    }

                }
                else
                    (game as Game1).clicked_button = false;
                float alpha = nowTime - (game as Game1).startTime;
                #region Once they said this world was...
                if (alpha < 7500)
                {
                    if (alpha > 7000)
                    {
                        (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(true);
                        (game as Game1).postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 7000) / 500f);

                    }
                    if (alpha > 1300)
                    {
                        float curcolor = (alpha - 1300) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("Once they said this world was", new Vector2(20, 20), 0.5f);

                        curcolor = (alpha - 3200) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("beautiful . . .", new Vector2(490, 20), 1);
                    }

                    if (alpha > 4400)
                    {
                        float curcolor = (alpha - 4900) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString(". . . But I find that", new Vector2(75, 200), 0.5f);

                        curcolor = (alpha - 5600) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("hard", new Vector2(340, 200), 1);

                        curcolor = (alpha - 5900) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("to believe . . .", new Vector2(490, 230), 0.5f);
                    }
                }
                #endregion
                #region These barren islands...

                else if (alpha < 14500)
                {
                    (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);

                    if (alpha > 14000)
                    {
                        (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(true);
                        (game as Game1).postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 14000) / 500f);

                    }

                    if (alpha > 7000)
                    {
                        float curcolor = (alpha - 7500) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("These", new Vector2(20, 20), 0.5f);

                        curcolor = (alpha - 7800) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("barren", new Vector2(140, 20), 1f);

                        curcolor = (alpha - 8100) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("islands,", new Vector2(360, 50), 0.5f);

                        curcolor = (alpha - 9100) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("shattered parts of flowing green hills,", new Vector2(150, 125), 0.5f);
                    }
                    if (alpha > 11000)
                    {
                        float curcolor = (alpha - 11500) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("now lay in desolate", new Vector2(100, 200), 0.5f);

                        curcolor = (alpha - 12500) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("disrepair", new Vector2(410, 200), 1f);
                    }

                }
                #endregion
                #region On Lofty wings soar birds...
                else if (alpha < 31000)
                {
                    (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);

                    if (alpha > 30000)
                    {
                        (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(true);
                        (game as Game1).postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 30000) / 1000f);
                    }

                    if (alpha > 14500)
                    {
                        float curcolor = (alpha - 14500) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("On lofty wings soar birds in a sky, now", new Vector2(20, 20), 0.5f);

                        //curcolor = (alpha - 13500) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        //postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        //drawString("in a sky, ", new Vector2(120, 20), 0.5f);

                        curcolor = (alpha - 17000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("blackened,", new Vector2(310, 50), 1f);

                        curcolor = (alpha - 18000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("with the ashes of our mistakes", new Vector2(150, 125), 0.5f);

                        curcolor = (alpha - 21000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("their downy white wings", new Vector2(50, 170), 0.5f);

                        curcolor = (alpha - 22000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("tinged with the gray", new Vector2(430, 170), 0.5f);

                        curcolor = (alpha - 23100) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("smut", new Vector2(650, 200), 1f);

                        curcolor = (alpha - 25000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("The sun may still rise in the west,", new Vector2(50, 250), 0.5f);

                        curcolor = (alpha - 27500) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("but it feels like it set on this world", new Vector2(150, 300), 0.5f);

                        curcolor = (alpha - 29000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("a long time ago . . .", new Vector2(250, 350), 0.75f);
                    }

                }
                #endregion
                #region For we are all...
                else if (alpha < 38000)
                {
                    (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);

                    if (alpha > 37000)
                    {
                        (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(true);
                        (game as Game1).postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 37000) / 1000f);
                    }


                    float curcolor = (alpha - 31000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    drawString(". . .  For we are all", new Vector2(50, 50), 0.5f);

                    curcolor = (alpha - 32000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    drawString("cloaked in this darkness", new Vector2(150, 100), 0.75f);

                    curcolor = (alpha - 35000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    drawString("Yet still we strive onwards", new Vector2(200, 250), 0.75f);



                }
                #endregion
                #region A bleak almost meaningless existence
                else if (alpha < 53000)
                {
                    (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);

                    if (alpha > 52000)
                    {
                        (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(true);
                        (game as Game1).postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 52000) / 1000f);
                    }

                    float curcolor = (alpha - 38000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    drawString("A bleak almost meaningless existence.", new Vector2(50, 50), 0.5f);

                    curcolor = (alpha - 40000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    drawString("There is no light at the end of the tunnel,", new Vector2(150, 100), 0.5f);

                    curcolor = (alpha - 42500) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    drawString("no silver lining.", new Vector2(450, 140), 0.5f);

                    curcolor = (alpha - 45000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    drawString("We live the lie of happiness", new Vector2(250, 220), 0.5f);

                    curcolor = (alpha - 48000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    drawString("fight these", new Vector2(50, 250), 0.5f);
                    drawString("smoke-stack monsters,", new Vector2(230, 250), 0.95f);

                    curcolor = (alpha - 51000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    drawString("till the silent soil.", new Vector2(450, 350), 0.65f);
                }
                #endregion
                #region Hoping.  Praying...
                else if (alpha < 54500)
                {
                    (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);

                    if (alpha > 54000)
                    {
                        (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(true);
                        (game as Game1).postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 54000) / 500f);
                    }

                    float curcolor = (alpha - 53000) / 500f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    drawString("Hoping.", new Vector2(50, 50), 0.85f);
                }
                else if (alpha < 56000)
                {
                    (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);

                    if (alpha > 55500)
                    {
                        (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(true);
                        (game as Game1).postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 55500) / 500f);
                    }

                    float curcolor = (alpha - 54500) / 500f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    drawString("Praying.", new Vector2(250, 150), 0.85f);
                }


                else if (alpha < 60000)
                {
                    (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);

                    if (alpha > 59000)
                    {
                        (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(true);
                        (game as Game1).postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 59000) / 1000f);
                    }

                    float curcolor = (alpha - 56000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    drawString("That from our efforts", new Vector2(150, 150), 0.55f);

                    curcolor = (alpha - 57000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    drawString("even a single life will blossom", new Vector2(300, 250), 0.55f);

                }
                #endregion
                #region Even a solemn rose petal...
                else if (alpha < 65000)
                {
                    (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);

                    if (alpha > 64000)
                    {
                        (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(true);
                        (game as Game1).postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 64000) / 1000f);
                    }

                    if (alpha > 60000)
                    {
                        float curcolor = (alpha - 60000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("Even a solemn rose petal", new Vector2(50, 50), 0.85f);

                        curcolor = (alpha - 62000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("would lighten these dark hills", new Vector2(150, 200), 0.85f);
                    }
                }
                #endregion
                else
                {
                    (game as Game1).startTime = DateTime.Now.Ticks / 10000;
                    (game as Game1).level = 1;
                    (game as Game1).seIntro.Stop();
                }
            }


            #endregion
            #region Controls
            if ((game as Game1).level == 1)
            {
                (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);
                drawString("Controls", new Vector2(20, 90), 0.7f);
                //drawString("You can view the controls by pausing the game and selecting 'controls'", new Vector2(20, 70), 0.4f);

                drawString("ASDW Keys: Move Around", new Vector2(50, 150), 0.5f);
                drawString("Mouse: Look Around", new Vector2(50, 180), 0.5f);
                drawString("Right Click: Angel Vision", new Vector2(50, 210), 0.5f);
                drawString("Press B to continue", new Vector2(20, 450), 0.5f);

                if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B) || Keyboard.GetState().IsKeyDown(Keys.B))
                {
                    (game as Game1).level = 2;
                    (game as Game1).startTime = DateTime.Now.Ticks / 10000;
                    MediaPlayer.Play((game as Game1).music[1]);
                    MediaPlayer.IsRepeating = true;
                }
            }
            #endregion
            #region Part1.1
            if ((game as Game1).level == 2)
            {
                Mouse.SetPosition(200, 200);
                float alpha = nowTime - (game as Game1).startTime;
                (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);
                if (alpha < 3000)
                {
                    Vector2 text_size = (game as Game1).SegoeUI.MeasureString("Part 1.1 - The Settling");
                    float curcolor = (alpha - 1000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                    drawString("Part 1.1 - The Settling", new Vector2(500 - (text_size.X / 2), 250 - (text_size.Y) / 2), 0.8f);
                    (game as Game1).Camera.Pitch -= curcolor / 200;
                }
                else if (alpha < 4000)
                {
                    Vector2 text_size = (game as Game1).SegoeUI.MeasureString("Part 1.1 - The Settling");
                    float curcolor = 1 - (alpha - 3000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                    (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                    (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                    drawString("Part 1.1 - The Settling", new Vector2(500 - (text_size.X / 2), 250 - (text_size.Y) / 2), 0.8f);
                    (game as Game1).Camera.Pitch -= curcolor / 150;
                }
                else
                {
                    (game as Game1).level = 1.9f;
                    (game as Game1).startTime = DateTime.Now.Ticks / 10000;
                }
            }
            #endregion
            #region intro_instructions
            if ((game as Game1).level == 1.9f)
            {
                Mouse.SetPosition(200, 200);
                float alpha = nowTime - (game as Game1).startTime;
                (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);

                Vector2 text_size = (game as Game1).SegoeUI.MeasureString("Part 1.1 - The Settling");
                float curcolor = (alpha) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                
                //Red
                //1.7f, .3f, .6f
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor * (1.7f / 1.7f), curcolor * (0.7f / 1.7f), curcolor * (0.9f / 1.7f)));
                (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                drawString("Objective: Collect Supplies for your journey.", new Vector2(20, 20), 0.5f);
                //White
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                drawString("You are currently at one of the old abandoned towns,", new Vector2(20, 100), 0.5f);
                drawString("you need to collect 2 things, a battery and an", new Vector2(20, 140), 0.5f);
                drawString("energy source.  Be careful, the humans were very", new Vector2(20, 180), 0.5f);
                drawString("protective of their energy.  Beware of traps.", new Vector2(20, 220), 0.5f);
                drawString("Press Enter or A to continue", new Vector2(20, 300), 0.5f);               
               
                if(Keyboard.GetState().IsKeyDown(Keys.Enter)||Keyboard.GetState().IsKeyDown(Keys.A)||GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
                {
                    (game as Game1).level = 3;
                }
            }
            #endregion
            #region level3
                       
            if ((game as Game1).level == 3)
            {
                lastlevel = 3;
                /*Draw Health Bar*/
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(1, 1, 1));
                (game as Game1).spriteBatch.Draw((game as Game1).healthBar, new Rectangle(20, 20, 26, 300), Color.White);
                int temph = (int)((game as Game1).actions.Health * 202);

                (game as Game1).spriteBatch.Draw((game as Game1).healthBar_Green, new Rectangle(23, 239, 20, 20), Color.White);
                (game as Game1).spriteBatch.Draw((game as Game1).healthBar_Green, new Rectangle(43, 239 - temph, 20, 20), null, Color.White, (float)Math.PI, Vector2.Zero, SpriteEffects.None, 0);

                (game as Game1).spriteBatch.DrawString((game as Game1).SegoeUI, (game as Game1).actions.Score.ToString(), new Vector2(100, 29), Color.White);
                (game as Game1).spriteBatch.Draw((game as Game1).textBox, new Rectangle(75, 25, 125, 125), Color.White);
                (game as Game1).spriteBatch.DrawString((game as Game1).SegoeUI, "Find a Battery to use", new Vector2(400, 5), Color.White, 0, new Vector2(0), 0.5f, SpriteEffects.None, 0);
                (game as Game1).spriteBatch.Draw((game as Game1).health_green, new Rectangle(26, 239 - temph, 14, temph), Color.White);

                (game as Game1).startTime = DateTime.Now.Ticks / 10000;
            }
            else if ((game as Game1).level == 3.1f)
            {
                float alpha = nowTime - (game as Game1).startTime;
                (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);

                Vector2 text_size = (game as Game1).SegoeUI.MeasureString("Part 1.1 - The Settling");
                float curcolor = (alpha) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;

                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor * (1.7f / 1.7f), curcolor * (0.7f / 1.7f), curcolor * (0.9f / 1.7f)));
                (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                drawString("A Plasma Gun", new Vector2(20, 20), 0.5f);
                //White
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                drawString("Good Job! You found a Plasma Gun", new Vector2(20, 100), 0.5f);
                drawString("This can be used to defend against smoke", new Vector2(20, 140), 0.5f);
                drawString("stack monsters.", new Vector2(20, 180), 0.5f);
                drawString("", new Vector2(20, 220), 0.5f);
                drawString("Click the left mouse button to shoot", new Vector2(20, 300), 0.5f);

                if (Keyboard.GetState().IsKeyDown(Keys.Enter) || GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed || GamePad.GetState(PlayerIndex.One).Triggers.Right!=0 || Mouse.GetState().LeftButton == ButtonState.Pressed)
                {
                    (game as Game1).level = lastlevel;
                    (game as Game1).startTime = DateTime.Now.Ticks / 10000;
                }
            }
            else if ((game as Game1).level == 4)
            {
                lastlevel = 4;
                (game as Game1).actions.Health = 1;
                
                float alpha = nowTime - (game as Game1).startTime;
                (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);

                Vector2 text_size = (game as Game1).SegoeUI.MeasureString("Part 1.1 - The Settling");
                float curcolor = (alpha) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;

                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor * (1.7f / 1.7f), curcolor * (0.7f / 1.7f), curcolor * (0.9f / 1.7f)));
                (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                drawString("A battery!", new Vector2(20, 20), 0.5f);
                //White
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                drawString("Good Job! You found a Lithium Battery", new Vector2(20, 100), 0.5f);
                drawString("This is good, but it needs to be charged", new Vector2(20, 140), 0.5f);
                drawString("Find an energy source by using angel vision", new Vector2(20, 180), 0.5f);
                drawString("", new Vector2(20, 220), 0.5f);
                drawString("Click enter to continue", new Vector2(20, 300), 0.5f);
                
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    (game as Game1).level = 4.1f;
                }

            }
            else if ((game as Game1).level == 4.01f)
            {
                lastlevel = 4;
                (game as Game1).actions.Health = 1;
                float alpha = nowTime - (game as Game1).startTime;
                (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);

                Vector2 text_size = (game as Game1).SegoeUI.MeasureString("Part 1.1 - The Settling");
                float curcolor = (alpha) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;

                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor * (1.7f / 1.7f), curcolor * (0.7f / 1.7f), curcolor * (0.9f / 1.7f)));
                (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                drawString("A battery!", new Vector2(20, 20), 0.5f);
                //White
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                drawString("Good Job! You found a Lithium Air Battery", new Vector2(20, 100), 0.5f);
                drawString("This kind of battery runs off of air!", new Vector2(20, 140), 0.5f);
                drawString("You don't need to find an energy source", new Vector2(20, 180), 0.5f);
                drawString("but this battery degrades in humity! Be careful!", new Vector2(20, 220), 0.5f);
                drawString("Click enter to continue", new Vector2(20, 300), 0.5f);

                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    (game as Game1).level = 5f;
                }

            }
            else if ((game as Game1).level == 4.1f)
            {
                lastlevel = 4.1f;
                /*Draw Health Bar*/
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(1, 1, 1));
                (game as Game1).spriteBatch.Draw((game as Game1).healthBar, new Rectangle(20, 20, 26, 300), Color.White);
                int temph = (int)((game as Game1).actions.Health * 202);

                (game as Game1).spriteBatch.Draw((game as Game1).healthBar_Green, new Rectangle(23, 239, 20, 20), Color.White);
                (game as Game1).spriteBatch.Draw((game as Game1).healthBar_Green, new Rectangle(43, 239 - temph, 20, 20), null, Color.White, (float)Math.PI, Vector2.Zero, SpriteEffects.None, 0);
                
                (game as Game1).spriteBatch.DrawString((game as Game1).SegoeUI, (game as Game1).actions.Score.ToString(), new Vector2(100, 29), Color.White);
                (game as Game1).spriteBatch.Draw((game as Game1).textBox, new Rectangle(75, 25, 125, 125), Color.White);

                (game as Game1).spriteBatch.Draw((game as Game1).health_green, new Rectangle(26, 239 - temph, 14, temph), Color.White);

                (game as Game1).spriteBatch.DrawString((game as Game1).SegoeUI, "Find an energy source", new Vector2(400, 5), Color.White, 0, new Vector2(0), 0.5f, SpriteEffects.None, 0);
            }
            else if ((game as Game1).level == 4.2f)
            {
                lastlevel = 4.1f;
                /*Draw Health Bar*/
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(1, 1, 1));
                (game as Game1).spriteBatch.Draw((game as Game1).healthBar, new Rectangle(20, 20, 26, 300), Color.White);
                int temph = (int)((game as Game1).actions.Health * 202);

                (game as Game1).spriteBatch.Draw((game as Game1).healthBar_Green, new Rectangle(23, 239, 20, 20), Color.White);
                (game as Game1).spriteBatch.Draw((game as Game1).healthBar_Green, new Rectangle(43, 239 - temph, 20, 20), null, Color.White, (float)Math.PI, Vector2.Zero, SpriteEffects.None, 0);

                (game as Game1).spriteBatch.DrawString((game as Game1).SegoeUI, (game as Game1).actions.Score.ToString(), new Vector2(100, 29), Color.White);
                (game as Game1).spriteBatch.Draw((game as Game1).textBox, new Rectangle(75, 25, 125, 125), Color.White);

                (game as Game1).spriteBatch.Draw((game as Game1).health_green, new Rectangle(26, 239 - temph, 14, temph), Color.White);

                (game as Game1).spriteBatch.DrawString((game as Game1).SegoeUI, "Find an Air Tank", new Vector2(400, 5), Color.White, 0, new Vector2(0), 0.5f, SpriteEffects.None, 0);
            }
            else if ((game as Game1).level == 5)
            {
                lastlevel = 5;
                (game as Game1).actions.Health = 1;
                float alpha = nowTime - (game as Game1).startTime;
                (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);

                Vector2 text_size = (game as Game1).SegoeUI.MeasureString("Part 1.1 - The Settling");
                float curcolor = (alpha) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;

                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor * (1.7f / 1.7f), curcolor * (0.7f / 1.7f), curcolor * (0.9f / 1.7f)));
                (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                drawString("Your charging your battery!", new Vector2(20, 20), 0.5f);
                //White
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                drawString("Good Job! Powering your battery will be necessary", new Vector2(20, 100), 0.5f);
                drawString("and a challenge through the game.  Without a charged", new Vector2(20, 140), 0.5f);
                drawString("battery, you'll find it difficult to accomplish certain tasks.  ", new Vector2(20, 180), 0.5f);
                drawString("You know your battery is charged when it's green", new Vector2(20, 220), 0.5f);
                drawString("Click enter to continue", new Vector2(20, 300), 0.5f);

                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    (game as Game1).level = 5.1f;
                }

            }
            else if ((game as Game1).level == 5.1f)
            {
                lastlevel = 5.1f;
                /*Draw Health Bar*/
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(1, 1, 1));
                (game as Game1).spriteBatch.Draw((game as Game1).healthBar, new Rectangle(20, 20, 26, 300), Color.White);
                int temph = (int)((game as Game1).actions.Health * 202);

                (game as Game1).spriteBatch.Draw((game as Game1).healthBar_Green, new Rectangle(23, 239, 20, 20), Color.White);
                (game as Game1).spriteBatch.Draw((game as Game1).healthBar_Green, new Rectangle(43, 239 - temph, 20, 20), null, Color.White, (float)Math.PI, Vector2.Zero, SpriteEffects.None, 0);
                (game as Game1).spriteBatch.DrawString((game as Game1).SegoeUI, (game as Game1).actions.Score.ToString(), new Vector2(100, 29), Color.White);
                (game as Game1).spriteBatch.Draw((game as Game1).textBox, new Rectangle(75, 25, 125, 125), Color.White);
                (game as Game1).spriteBatch.Draw((game as Game1).health_green, new Rectangle(26, 239 - temph, 14, temph), Color.White);

                (game as Game1).spriteBatch.DrawString((game as Game1).SegoeUI, "", new Vector2(400, 5), Color.White, 0, new Vector2(0), 0.5f, SpriteEffects.None, 0);
            }
            #endregion

        }


        void drawString(String str, Vector2 Position, float scale)
        {
            (game as Game1).spriteBatch.DrawString((game as Game1).SegoeUI, str, Position, Color.White, 0, new Vector2(0), scale, SpriteEffects.None, 0);
        }
        void loadLevel(float levelGoto, long nowTime)
        {

            (game as Game1).postEffect.Parameters["use_ov_tint"].SetValue(false);
            (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(2, 2, 2));
            (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1);


            int loading = (game as Game1).loaded_;


            if (loading == 7)//Max
            {
                float alpha = nowTime - (game as Game1).startTime;

                float curcolor = 1 - (alpha) / 1000f; //if (curcolor > 1.1f) curcolor = 1.1f;
                curcolor *= 2;
                (game as Game1).postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                (game as Game1).postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);

                if (alpha > 1000)
                {
                    (game as Game1).level = levelGoto;
                }
            }

            drawString("Loading", new Vector2(20, 20), 0.8f);
            float length = (game as Game1).SegoeUI.MeasureString("..........").X;
            for (int i = 0; i < loading; i++)
            {
                drawString("..........", new Vector2(150 + i * length, 300), 1);
            }
            if (loading == 7)
            {

            }
            else
                (game as Game1).LoadLevelOne(loading);
        }
        #endregion
    }
}
