using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;



public class Levels {

    private Random r;

    private List<Texture2D> floors;
    private List<Texture2D> walls;
    private List<Texture2D> backgrounds;

    private List<Geometry> level = new List<Geometry>();
    private List<Geometry> back = new List<Geometry>();

    int s = 2;

    private ParticalEntityManager particals = new ParticalEntityManager();

    
    public Levels(List<Texture2D> _floors, List<Texture2D> _walls, List<Texture2D> _backgrounds) {

        r = new Random();

        floors = _floors;
        walls = _walls;
        backgrounds = _backgrounds;

    }

    public void Update(Rectangle _r) {

        if (r.Next(0, 200) == 1 && s < 4) {
            particals.Add("jelly", global.textures, _r, 0.1f, s);
            s++;
        }

        particals.Update();
    }

    public void DrawParticals(GameTime gameTime, SpriteBatch spriteBatch) {
        particals.Draw(spriteBatch);
    }
    public void DrawBackground(GameTime gameTime, SpriteBatch spriteBatch) {
        foreach (Geometry g in back) {
            g.Draw(gameTime, spriteBatch, 0.6f);
        }
    }
    public void DrawLevel(GameTime gameTime, SpriteBatch spriteBatch) {
        foreach (Geometry g in level) {
            g.Draw(gameTime, spriteBatch, 0.4f);
        }
    }
    //

    public void loadLevel(Rectangle r) {

        level.Clear();
        particals.Clear();


        int height = 700;
        int size = 100;
        Color c = new Color(40, 40, 40);


        addBack(size * floors[0].Width, height, c);
        addWalls(size * floors[0].Width, height, new Color(255, 255, 255));
        addFlooring(size, height, c);


        particals.Add("jelly", global.textures, r, 0.1f, 1);

        
        particals.Add("fallBlock", global.textures, r);
        

    }

    private void addFlooring(int size, int yStart, Color colour, int xStart = 0, int textureNumber = 0) {
        Geometry floor = new Geometry(floors[textureNumber]);

        for (int i = 0; i < size; i++) {
            floor.addGeometry(new Rectangle(
                floors[textureNumber].Width * i, yStart,
                floors[textureNumber].Width, floors[textureNumber].Height),

                false, colour);
        }

        floor.addGeometry(new Rectangle(
            xStart, yStart,
            floors[textureNumber].Width * size, floors[textureNumber].Height),

            true,
            new Color(0, 0, 0, 0)
        );

        level.Add(floor);
    }

    private void addWalls(int size, int yStart, Color colour, int xStart = 0, int textureNumber = 0) {
        Geometry wall = new Geometry(walls[textureNumber]);


        wall.addGeometry(new Rectangle(-1, 0, 1, 1000));
        wall.addGeometry(new Rectangle(0, -1, int.MaxValue, 1));


        for (int i = 800; i < size; i += r.Next(walls[textureNumber].Width* 5, size/10) ) {

            int h = walls[textureNumber].Height - r.Next(-walls[textureNumber].Height / 2, walls[textureNumber].Height / 2);

            wall.addGeometry(new Rectangle(
                i, yStart - h + 4,
                walls[textureNumber].Width, h),

                true, colour);
        }


        level.Add(wall);
    }

    private void addBack(int size, int yStart, Color colour, int xStart = 0) {
        List<Geometry> _back = new List<Geometry>();
        _back.Add(new Geometry(backgrounds[0]));
        _back.Add(new Geometry(backgrounds[1]));
        _back.Add(new Geometry(backgrounds[2]));

        int i = 800;

        while(i < size) {
            int c = r.Next(0, 2);
            int h = backgrounds[c].Height - r.Next(-backgrounds[c].Height / 2, backgrounds[c].Height / 2);

            _back[c].addGeometry(new Rectangle(
                i, yStart - h + 40,
                backgrounds[c].Width, h),

                false, colour);

            i += r.Next(backgrounds[c].Width * 5, size / 10);
        }


        foreach (Geometry g in _back) {
            back.Add(g);
        }
       
    }

    public List<int[]> checkCollisions(Rectangle playerRect) {

        List<int[]> output = new List<int[]>();

        foreach (Geometry g in level) {
            output.Add(g.checkCollisions(playerRect));
        }

        return output;
    }

    public bool checkCollision(Rectangle playerRect) {

        foreach (Geometry g in level) {
            if (g.checkCollision(playerRect)) {
                return true;
            }
        }

        return false;
    }

}

