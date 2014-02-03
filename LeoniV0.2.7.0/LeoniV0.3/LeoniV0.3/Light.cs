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



public class Light
{
    public Vector3 lightPos;
    public float lightPower;
    public float ambientPower;
    public Matrix lightsViewProjectionMatrix;

    public void Update()
    {
        ambientPower = 0.5f;

        lightPos = new Vector3(-30, 12, -30);
        lightPower = 1f;

        Matrix lightsView = Matrix.CreateLookAt(lightPos, new Vector3(25, 0, 25), new Vector3(0, 1, 0));
        Matrix lightsProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver2, 2f ,5f, 1000f);

        lightsViewProjectionMatrix = lightsView * lightsProjection;
    }
}

