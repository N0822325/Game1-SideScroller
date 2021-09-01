
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;



public class Camera2D {
    private readonly GraphicsDeviceManager graphicsFrame;
    private readonly Viewport _viewport;

    public Camera2D(Viewport viewport, GraphicsDeviceManager g) {

        graphicsFrame = g;

        _viewport = viewport;

        Rotation = 0;
        Zoom = 1;
        Origin = new Vector2(0, 0);
        Position = Vector2.Zero;
    }

    public Vector2 Position;
    public float Rotation { get; set; }
    public float Zoom { get; set; }
    private float ManualZoom = 0f;
    public Vector2 Origin { get; set; }

    public Matrix GetViewMatrix() {
        return
            Matrix.CreateTranslation(new Vector3(-Position, 0.0f)) *
            Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Zoom, Zoom, 1) *
            Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
    }

    public Matrix GetBackViewMatrix() {
        return
            Matrix.CreateTranslation(new Vector3(-new Vector2(Position.X/5,Position.Y/5), 0.0f)) *
            Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Zoom, Zoom, 1) *
            Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
    }

    public Matrix getParticalViewMatrix() {
        return
            Matrix.CreateTranslation(new Vector3(new Vector2(0,0), 0.0f)) *
            Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Zoom, Zoom, 1) *
            Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
    }

    public void update(GameTime gameTime, Vector2 playerPos, int[] resolution) {

        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        var keyboardState = Keyboard.GetState();

        if (keyboardState.IsKeyDown(Keys.Divide))
            Rotation -= deltaTime;

        if (keyboardState.IsKeyDown(Keys.Multiply))
            Rotation += deltaTime;

        if (keyboardState.IsKeyDown(Keys.NumPad8)) {
            Position -= new Vector2(0, 250) * deltaTime;
            //if (Position.Y < 0) {
            //    Position.Y = 0;
            //}
        }

        if (keyboardState.IsKeyDown(Keys.NumPad2)) {
            Position += new Vector2(0, 250) * deltaTime;
        }


        if (keyboardState.IsKeyDown(Keys.NumPad4)) {
            Position -= new Vector2(250, 0) * deltaTime;
            //if (Position.X < 0) {
            //    Position.X = 0;
            //}
        }

        if (keyboardState.IsKeyDown(Keys.NumPad6))
            Position += new Vector2(250, 0) * deltaTime;

        Zoom = (float)resolution[0] / 1280;

        if (keyboardState.IsKeyDown(Keys.Subtract))
            ManualZoom -= 1 * deltaTime;
        if (keyboardState.IsKeyDown(Keys.Add))
            ManualZoom += 1 * deltaTime;

        Zoom += ManualZoom;

        playerPos.X = (int)playerPos.X;
        playerPos.Y = (int)playerPos.Y;

        playerPos.X -= graphicsFrame.PreferredBackBufferWidth / 2 / Zoom;
        playerPos.Y -= (float)(graphicsFrame.PreferredBackBufferHeight / 1.2 / Zoom);

        Position = playerPos;


        if (Position.X < 0) Position.X = 0;
        if (Position.Y < 0) Position.Y = 0;
    }
}