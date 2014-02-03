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


/*This is designed to activate current commands that could be used in-game*/
namespace LeoniV0_3
{
    

    public class Actions
    {

        public float Health = 1.0f;
        private Game1 game1;
        public float Score = 0f;

        public Actions(Game1 game1)
        {
            this.game1 = game1;
        }

        public void doFunction(int t, int lasti)
        {
            switch (t)
            {
                case 0:
                    //(game1 as Game1).TotalBlur = 3;
                    Health -= 0.2f;
                    (game1 as Game1).actions.Score -= 1;
                    break;
                case 1:
                    (game1 as Game1).userObjects.objects.Add((game1 as Game1).ivoltmeter);                         
                    break;
                case 2:
                    if((game1 as Game1).level < 4){
                      (game1 as Game1).level = 4;
                      (game1 as Game1).actions.Score += 2;
                    }
                    (game1 as Game1).userObjects.objects.Add((game1 as Game1).ibattery);
                    (game1 as Game1).userObjects.objects[(game1 as Game1).userObjects.objects.Count - 1].data[0] = (game1 as Game1).SObjects[lasti].voltage;
                    break;
                case 3:
                    (game1 as Game1).userObjects.objects.Add((game1 as Game1).iplasmagun);
                    (game1 as Game1).level = 3.1f;
                    break;
                case 4:
                   if((game1 as Game1).level < 4){
                      (game1 as Game1).level = 4.01f;
                      (game1 as Game1).actions.Score += 10;
                    }
                    (game1 as Game1).userObjects.objects.Add((game1 as Game1).iairbattery);
                    (game1 as Game1).userObjects.objects[(game1 as Game1).userObjects.objects.Count - 1].data[0] = (game1 as Game1).SObjects[lasti].voltage;
                    break;
                default:
                    break;

            }
        }

        /*Searches for the position of an object in an array*/
        private int searchFor(string tag)
        {
            for (int i = 0; i < (game1 as Game1).SObjects.Count; i++)            
                if ((game1 as Game1).SObjects[i].comp.id == tag)                
                    return i;                
          
            return -1;
        }   

        public void update()
        {
            bool inrange = false;

            /*Spin Monster Eyes*/
            for (int i = 0; i < (game1 as Game1).SObjects.Count; i++)
            {
                if ((game1 as Game1).SObjects[i].comp.id == "testmonster_eye")
                {

                    if ((game1 as Game1).SObjects[i].inRange((game1 as Game1).cameraCol.Position))
                    {
                        float XDistance = (game1 as Game1).cameraCol.Position.X - (game1 as Game1).SObjects[i].pos.X;
                        float YDistance = (game1 as Game1).cameraCol.Position.Z - (game1 as Game1).SObjects[i].pos.Z;

                        float rotation = (float)Math.Atan2(YDistance, XDistance);

                        (game1 as Game1).SObjects[i].comp.rot.Y = -rotation;

                        (game1 as Game1).SObjects[i].tint = new Vector4(1, 0.8f, 0.8f, 1);
                        Health -= 0.0007f;
                        inrange = true;
                    }
                    else
                    {
                        (game1 as Game1).SObjects[i].comp.rot.Y += 0.01f;
                    }
                }
                else if ((game1 as Game1).SObjects[i].comp.id == "testmonster_head")
                {

                    if ((game1 as Game1).SObjects[i].inRange((game1 as Game1).cameraCol.Position))
                    {
                        float XDistance = (game1 as Game1).cameraCol.Position.X - (game1 as Game1).SObjects[i].pos.X;
                        float YDistance = (game1 as Game1).cameraCol.Position.Z - (game1 as Game1).SObjects[i].pos.Z;

                        float rotation = (float)Math.Atan2(YDistance, XDistance);

                        (game1 as Game1).SObjects[i].comp.rot.Y = -rotation;
                        (game1 as Game1).SObjects[i].tint = new Vector4(1, 0.8f, 0.8f, 1);
                        inrange = true;
                    }
                    else
                    {
                        (game1 as Game1).SObjects[i].tint = new Vector4(1);
                        (game1 as Game1).SObjects[i].comp.rot.Y += 0.01f;
                    }
                }         
            }

            if (inrange == true)
            {
                (game1 as Game1).overlay_amount += 0.0025f;
                if ((game1 as Game1).overlay_amount > 0.8f)
                    (game1 as Game1).overlay_amount = 0.8f;
            }
            else
            {
                (game1 as Game1).overlay_amount -= 0.0025f;
                if((game1 as Game1).overlay_amount<0)
                    (game1 as Game1).overlay_amount = 0;
            }
            /*Health*/
            Health -= 0.0001f;

            if (Health < 0)
            {
                (game1 as Game1).level = -8f;
                Health = 0;
            }

            /*Adjust voltage for solar panel dependent on light*/
            for (int i = 0; i < (game1 as Game1).SObjects.Count; i++)
            {
                if ((game1 as Game1).cameraCol.Position.X < (game1 as Game1).SObjects[i].pos.X && (game1 as Game1).SObjects[i].comp.id.Equals("SolarPanel"))
                {
                    float random = (float)(game1 as Game1).ran.NextDouble()/1000;
                    (game1 as Game1).SObjects[i].voltage = 25*(float)Math.Sin(  (Math.PI/35)*((game1 as Game1).SObjects[i].pos.X - (game1 as Game1).cameraCol.Position.X)-17.5f)+30+random;
                }
            }
        }
        private bool x_click=false;

        public void drawUpdate()
        {
            /*Action "X"*/
            if (GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.X))
            {
                if (x_click == false)//Make sure we arn't adding multiple things at once
                {
                    int todo = -1;
                    int lasti = -1;
                    for (int i = 0; i < (game1 as Game1).SObjects.Count; i++)
                    {
                        if (Vector3.Distance((game1 as Game1).cameraCol.Position, (game1 as Game1).SObjects[i].pos) < (game1 as Game1).SObjects[i].range && !(game1 as Game1).SObjects[i].action.Equals(""))
                        {
                            todo = (game1 as Game1).SObjects[i].todo;
                            lasti = i;
                            break;
                        }
                    }

                    if (todo == 0)
                    {
                        (game1 as Game1).SObjects[lasti].resetData();
                        (game1 as Game1).objectSelection = 0;
                        doFunction(0, lasti);
                    }
                    else if (todo == 1)
                    {
                        doFunction(1, lasti);
                        (game1 as Game1).SObjects.RemoveAt(lasti);
                        (game1 as Game1).objectSelection = 0;
                    }
                    else if (todo == 2)
                    {
                        doFunction(2, lasti);
                        (game1 as Game1).SObjects.RemoveAt(lasti);
                        (game1 as Game1).objectSelection = 0;
                    }
                    else if (todo == 3)
                    {
                        doFunction(3, lasti);
                        (game1 as Game1).SObjects.RemoveAt(lasti);
                        (game1 as Game1).objectSelection = 0;
                    }
                    else if (todo == 4)
                    {
                        doFunction(4, lasti);
                        (game1 as Game1).SObjects.RemoveAt(lasti);
                        (game1 as Game1).objectSelection = 0;
                    }
                }
                x_click = true;
            }
            else
                x_click = false;
        }
        
        private float convert_radian_to_smallest_value(float radian)
        {
            while (true)
            {
                if (radian > Math.PI * 2)
                {
                    radian -= (float)Math.PI * 2;
                }
                else if (radian < -Math.PI * 2)
                {
                    radian += (float)Math.PI * 2;
                }
                else
                    break;
            }
            return radian;
        }

        public void draw()
        {
            //int closest = -1;
            //float closet_value = -1;

            /*Look for objects within range of the camera*/
            for (int i = 0; i < (game1 as Game1).SObjects.Count; i++)
            {
                if ((game1 as Game1).SObjects[i].inRange((game1 as Game1).cameraCol.Position) && !(game1 as Game1).SObjects[i].action.Equals(""))
                {
                    Vector2 length = (game1 as Game1).SegoeUI.MeasureString((game1 as Game1).SObjects[i].data) / 2;
                    (game1 as Game1).drawString((game1 as Game1).SObjects[i].data, new Vector2((game1 as Game1).screenWidth / 2 - length.X / 2, (game1 as Game1).screenHeight - 100), 0.5f);
                    length = (game1 as Game1).SegoeUI.MeasureString((game1 as Game1).SObjects[i].action) / 2;
                    (game1 as Game1).drawString((game1 as Game1).SObjects[i].action, new Vector2((game1 as Game1).screenWidth / 2 - length.X / 2, (game1 as Game1).screenHeight - 50), 0.5f);
                    (game1 as Game1).objectSelection = i;

                    drawUpdate();
                    //spriteBatch.DrawString(SegoeUI, "i", new Vector2(screenWidth / 2, screenHeight - 100), Color.White);
                  
                    break;
                }
            }
            //if(closest!=-1)
            //{
                
            //}





            
            /*Search and charge batteries*/            
            for (int k = 0; k < (game1 as Game1).SObjects.Count; k++)
            { 
                if ((game1 as Game1).SObjects[k].comp.id == "battery")
                {
                    (game1 as Game1).SObjects[k].tint = new Vector4(1);
                    //Look for anything that could charge batteries
                    for (int i = 0; i < (game1 as Game1).SObjects.Count; i++)
                    {                        
                        if (!(game1 as Game1).SObjects[i].comp.id.Equals("battery") && (game1 as Game1).SObjects[i].voltage != 0 && (game1 as Game1).SObjects[k].inRange((game1 as Game1).SObjects[i].pos))
                        {
                            if ((game1 as Game1).level == 4.1f)
                            {
                                (game1 as Game1).level = 5;
                            }
                            (game1 as Game1).SObjects[k].voltage += 0.0005f;
                            float voltage = (game1 as Game1).SObjects[k].voltage;
                            if (voltage < 0.5f)
                            {
                                (game1 as Game1).SObjects[k].tint = new Vector4(1f, 0.5f+(voltage), 0.5f, 1);
                            }
                            else if (voltage < 1.0f)
                            {
                                (game1 as Game1).SObjects[k].tint = new Vector4(1f - (voltage-0.5f), 1f, 0.5f - (voltage - 0.5f), 1);
                            }
                            else 
                                (game1 as Game1).SObjects[k].tint = new Vector4(0.5f, 1f, 0.5f, 1);
                            

                        }
                    }
                    
                }                
            } 
                  
        }

    }
}