using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


class UI
{
    private GraphicsDeviceManager graphicsFrame;
    private float scale = 1f;

    Rectangle a;

    public UI(GraphicsDeviceManager g) {
        graphicsFrame = g;
    }

    private Point midPoint() {
        Point output = new Point(
            graphicsFrame.PreferredBackBufferWidth/2,
            graphicsFrame.PreferredBackBufferHeight/2);

        return output;
    }


    public void Update() {

        if (Keyboard.GetState().IsKeyDown(Keys.N)) {
            graphicsFrame.PreferredBackBufferWidth = 1920;
            graphicsFrame.PreferredBackBufferHeight = 1080;
            graphicsFrame.ApplyChanges();
        }
        if (Keyboard.GetState().IsKeyDown(Keys.M)) {
            graphicsFrame.PreferredBackBufferWidth = 1280;
            graphicsFrame.PreferredBackBufferHeight = 720;
            graphicsFrame.ApplyChanges();
        }

        if (Keyboard.GetState().IsKeyDown(Keys.B)) {
            graphicsFrame.IsFullScreen = true;
            graphicsFrame.ApplyChanges();
        }
        if (Keyboard.GetState().IsKeyDown(Keys.V)) {
            graphicsFrame.IsFullScreen = false;
            graphicsFrame.ApplyChanges();
        }

        Point abc = new Point(50,50);
        Point ab = midPoint() - abc;

        a = new Rectangle(ab,new Point(100,100));

    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

        spriteBatch.Draw(global.block,a,new Color(255,255,255,100));

    }

}