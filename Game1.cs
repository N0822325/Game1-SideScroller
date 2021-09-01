using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;


public static class global {
    public static Texture2D block;
    public static List<Texture2D> textures;
    public static Color baseColour;
}





public class Game1 : Game {

    Random r;

    GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    
    private Camera2D _camera;
    private UI userInterface;
    private Levels level;
    private Player player;
    private ParticleEngine P;

    int cNum = 11;
    bool cUp = true;
    bool itt = false;
    int count = 0;


    public Game1() {
        graphics = new GraphicsDeviceManager(this);
        graphics.PreferredBackBufferWidth = 1280;
        graphics.PreferredBackBufferHeight = 720;
        graphics.IsFullScreen = false;
        Content.RootDirectory = "Content";
    }


    protected override void Initialize() {

        global.block = Content.Load<Texture2D>("Block");
        global.textures = new List<Texture2D>();
        global.textures.Add(global.block);
        global.baseColour = new Color(173, 255, 47);


        // Level init
        Texture2D greenFloor = Content.Load<Texture2D>("GreenFloor");
        Texture2D wall = Content.Load<Texture2D>("wall1");
        Texture2D background1 = Content.Load<Texture2D>("Background1");
        Texture2D background2 = Content.Load<Texture2D>("Background2");
        Texture2D background3 = Content.Load<Texture2D>("Background3");
        Texture2D background4 = Content.Load<Texture2D>("Background4");


        List<Texture2D> floors = new List<Texture2D>();
        floors.Add(greenFloor);

        List<Texture2D> walls = new List<Texture2D>();
        walls.Add(wall);

        List<Texture2D> backgrounds = new List<Texture2D>();
        backgrounds.Add(background1);
        backgrounds.Add(background2);
        backgrounds.Add(background3);
        backgrounds.Add(background4);


        //

        r = new Random(0);
        player = new Player(Content);
        P = new ParticleEngine(global.textures, player.getPosition(), global.baseColour, 1, 1, 1, 0.1f);
        _camera = new Camera2D(GraphicsDevice.Viewport, graphics);
        userInterface = new UI(graphics);
        level = new Levels(floors, walls, backgrounds);

        level.loadLevel(new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight));

        base.Initialize();

    }

    protected override void LoadContent() {
        spriteBatch = new SpriteBatch(GraphicsDevice);
    
    }

    protected override void Update(GameTime gameTime) {

        userInterface.Update();

        level.Update(new Rectangle(0,0,graphics.PreferredBackBufferWidth,graphics.PreferredBackBufferHeight));

        player.update(gameTime);


        int[] collisions = { int.MinValue, int.MinValue, int.MinValue, int.MinValue };
        Rectangle[] DebugBoxes = { new Rectangle(), new Rectangle(), new Rectangle(), new Rectangle() };

        Rectangle start = player.getHitbox();        
        Rectangle next = player.getNextHitbox();

        Rectangle current = start;

        int topSpeed = player.topSpeed;

        // Quarterstep
        int[] displace = { (next.X - current.X)/ topSpeed, (next.Y - current.Y)/ topSpeed };

        for (int i = 1; i <= topSpeed+1; i++) {

            if (i > topSpeed) {
                // Bc rect is int
                // diving by quarterstep can result in displace being 0
                // So final check at end if distance covered is small enough to result in 0 by diving by quarterstep
                displace[0] = next.X - current.X;
                displace[1] = next.Y - current.Y;
                current = start;
            }

            current.Offset(displace[0], displace[1]);

            if (i <= topSpeed) {
                DebugBoxes[i - 1] = current;
            }
           

            foreach (int[] collision in level.checkCollisions(current)) {

                for (int j = 0; j < 4; j++) {
                    if (collision[j] != int.MinValue) {
                        collisions[j] = collision[j];
                    }
                }

                

                if (collisions[0] != int.MinValue || collisions[2] != int.MinValue) {
                    displace[1] = 0;
                    
                }
                if (collisions[1] != int.MinValue || collisions[3] != int.MinValue) {
                    displace[0] = 0;
                    
                }
            }

        }

        player.DebugBoxes = DebugBoxes;

        bool isGrounded = level.checkCollision(player.getBottom());


        player.lateUpdate(gameTime, collisions, isGrounded);

        
        P.EmitterLocation = new Vector2(player.getBottom().X + player.getBottom().Width/2,player.getBottom().Y);
        P.Update(true);
    
        int[] res = { graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight };
        _camera.update(gameTime, player.getPosition(), res);

        base.Update(gameTime);

    }



    protected override void Draw(GameTime gameTime) {
        count++;

        if (cNum >= 30) {
            cUp = false; itt = false; }
        if (cNum <= 10) {
            cUp = true; itt = false; }


        if (itt) {
            if (count % 4 == 1) {

                if (cUp) cNum++;
                if (!cUp) cNum--;

            }
        } else {
            if(r.Next(0,900) == 1) {
                itt = true;
                if (cUp) cNum++;
                else cNum--;
            }
        }


        Color c = new Color(cNum, cNum, cNum);

        GraphicsDevice.Clear(c);

        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, _camera.GetViewMatrix());
            P.Draw(spriteBatch,0.5f);
        spriteBatch.End();

        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, _camera.getParticalViewMatrix());
            level.DrawParticals(gameTime, spriteBatch);
        spriteBatch.End();

        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, _camera.GetBackViewMatrix());
            level.DrawBackground(gameTime, spriteBatch);
        spriteBatch.End();

        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, _camera.GetViewMatrix());
            level.DrawLevel(gameTime, spriteBatch);
            player.Draw(gameTime, spriteBatch);          
        spriteBatch.End();





        base.Draw(gameTime);

    }


}



