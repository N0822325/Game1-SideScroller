using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


    class BoulderClass
    {
        Texture2D boulderImage;
        Vector2 mySpeed = new Vector2();
        Vector2 myLocation = new Vector2();
        Color myColor = new Color();
        GraphicsDeviceManager graphics;
        int MaxX, MaxY;
        float scale = 1.0f;
        float rotation = 0.0f;

        public BoulderClass(Texture2D texIn, int w, int h, Random ranGen)
        {

            boulderImage = texIn;

            MaxX = w;
            MaxY = h;

            mySpeed.X = (float)ranGen.NextDouble() * 30;
            mySpeed.Y = (float)ranGen.NextDouble() * 30;

            myLocation.X = (float)ranGen.NextDouble() * MaxX;
            myLocation.Y = (float)ranGen.NextDouble() * MaxY;

            myColor.R = (byte)ranGen.Next(255);
            myColor.G = (byte)ranGen.Next(255);
            myColor.B = (byte)ranGen.Next(255);
            myColor.A = 255;

            Console.WriteLine("made me");

        }


        public void initialise(GraphicsDeviceManager g)
        {

            graphics = g;
        }

        public void moveMe(GameTime gameTime)
        {
            // Move the sprite by speed, scaled by elapsed time.
            myLocation += mySpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            

            // Check for bounce.
            if (myLocation.X > MaxX)
            {
                mySpeed.X *= -1;
                myLocation.X = MaxX;
            }

            if (myLocation.X < 0)
            {
                mySpeed.X *= -1;
                myLocation.X = 0;
            }

            if (myLocation.Y > MaxY)
            {
                mySpeed.Y *= -1;
                myLocation.Y = MaxY;
            }

            if (myLocation.Y < 0)
            {
                mySpeed.Y *= -1;
                myLocation.Y = 0;

            }

        rotation += 0.01f;
        }

        public Vector2 getLocation()
        {

            return myLocation;
        }

        public Rectangle getBBox()
        {
            return new Rectangle((int)(myLocation.X- boulderImage.Width/2.0f), (int)(myLocation.Y-boulderImage.Height/2.0f), boulderImage.Width, boulderImage.Height);
        }

        public Color getColor()
        {
            return myColor;
        }

        public Texture2D getImage()
        {
            return boulderImage;
        }


    public void drawMe(SpriteBatch spriteBatch)
    {

        //Rectangle r = null;
        
        Vector2 origin = new Vector2(boulderImage.Width/2.0f, boulderImage.Height / 2.0f);
        float layerDepth = 0.5f;

        spriteBatch.Draw(getImage(), getLocation(), null, getColor(), rotation, origin, scale, SpriteEffects.None, layerDepth);
        //public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);

    }

}

