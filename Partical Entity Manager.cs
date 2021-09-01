using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;


//class ParticalEntity{

//    private ParticleEngine entity;
//    private Random random;
//    private int randomness;

//    public ParticalEntity(List<Texture2D> textures, Rectangle _bounds, int _randomness) {

//        entity = new ParticleEngine(textures, startLocation);
//        random = new Random();

//        randomness = _randomness;

//    }



//}



//class JellyFish : ParticalEntity {

//public JellyFish(List<Texture2D> textures, Vector2 startLocation, int _randomness) 
//    : base(textures,startLocation,_randomness) { }

class JellyFish {
    
    private ParticleEngine jelly;
    private Rectangle bounds;
    private Rectangle box;
    private Vector2 midPoint;

    private Random random;
    private int randomness;

    private int choice;
    private bool canMove = false;
    private bool isIdle = false;

    public JellyFish(List<Texture2D> textures, Rectangle _bounds,float scale = 0.1f, int col = 2, int ran = 0) {

        random = new Random();
        randomness = 2;

        bounds = _bounds;

        midPoint = new Vector2(bounds.X + (bounds.Width / 2), bounds.Y + (bounds.Height / 2));

        box.X = (int)midPoint.X;
        box.Y = (int)midPoint.Y;
        box.Width = bounds.Width / 10;
        box.Height = bounds.Height / 10;


        jelly = new ParticleEngine(textures, midPoint, new Color(10,10,10), col, 10, 1f, scale, 5, 5, 0, 1, false, 90, 1);



        choice = randomness;
    }

    public void Update() {
        jelly.Update(true);
    }

    public void Draw(SpriteBatch spriteBatch) {
        jelly.Draw(spriteBatch);
    }

    private float A;
    private float B;
    private float C;
    private float xItt;
    private Vector2 current;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 parabola;

    private void calcQuadratic (Vector2 one, Vector2 two, Vector2 three) {
        float denom = (one.X - two.X) * (one.X - three.X) * (two.X - three.X);

        A = (three.X * (two.Y - one.Y) + two.X * (one.Y - three.Y) + one.X * (three.Y - two.Y)) / denom;
        B = (three.X * three.X * (one.Y - two.Y) + two.X * two.X * (three.Y - one.Y) + one.X * one.X * (two.Y - three.Y)) / denom;
        C = (two.X * three.X * (two.X - three.X) * one.Y + three.X * one.X * (three.X - one.X) * two.Y + one.X * two.X * (one.X - two.X) * three.Y) / denom;
    }

    private void moov(Rectangle _bounds) {

    }

    public void movement() {

        int a = random.Next(0,100000);

        choice = random.Next(0, randomness + 1);
        if ((choice == randomness || canMove) && !isIdle) {

            if (!canMove) {
                canMove = true;

                startPoint = new Vector2(jelly.EmitterLocation.X, jelly.EmitterLocation.Y);
                endPoint = new Vector2
                    (random.Next(bounds.X, bounds.X + bounds.Width),
                    random.Next(bounds.Y, bounds.Y + bounds.Height));


                box.X = (int)endPoint.X;
                box.Y = (int)endPoint.Y;
                box.Width = bounds.Width / 10;
                box.Height = bounds.Height / 10;


                //bc sometimes this just fails ? idk bro
                if (!bounds.Contains(endPoint)) {
                    jelly.move(startPoint);
                    canMove = false;
                    return;
                }

                parabola = new Vector2((startPoint.X + endPoint.X) / 2, random.Next(bounds.Y, bounds.Y + bounds.Height));

                current = startPoint;

                calcQuadratic(startPoint, parabola, endPoint);

                float speed = endPoint.X - startPoint.X;

                xItt = (float)(endPoint.X - startPoint.X) / 500;

            }

            if (startPoint.X < endPoint.X && current.X < endPoint.X
                    || endPoint.X < startPoint.X && endPoint.X < current.X) {

                current.X = current.X + xItt;
                current.Y = (A * (current.X * current.X) + B * current.X + C);

            } else {
                canMove = false;
            }

            jelly.move(current);

        } 
        else {
            if (!isIdle) {
                isIdle = true;

                startPoint = new Vector2(jelly.EmitterLocation.X, jelly.EmitterLocation.Y);

                endPoint = new Vector2
                    (random.Next(box.X, box.X + box.Width),
                    random.Next(box.Y, box.Y + box.Height));


                //bc sometimes this just fails ? idk bro
                if (!box.Contains(endPoint)) {
                    jelly.move(startPoint);
                    isIdle = false;
                    return;
                }

                parabola = new Vector2((startPoint.X + endPoint.X) / 2, random.Next(box.Y, box.Y + box.Height));

                current = startPoint;

                calcQuadratic(startPoint, parabola, endPoint);

                float speed = endPoint.X - startPoint.X;

                xItt = (float)(endPoint.X - startPoint.X) / 100;

            }

            if (startPoint.X < endPoint.X && current.X < endPoint.X
                    || endPoint.X < startPoint.X && endPoint.X < current.X) {

                current.X = current.X + xItt;
                current.Y = (A * (current.X * current.X) + B * current.X + C);

            } else {
                isIdle = false;
            }

            jelly.move(current);
        }
        
    }

}



class FallingBlocks {

    ParticleEngine Block;
    private Rectangle bounds;

    private Random random;

    private float angle;
    private float angularVelocity;
    private float Color;
    private float size;
    private float ttl;

    private int count;

    public FallingBlocks(List<Texture2D> textures, Rectangle _bounds) {

        bounds = _bounds;

        Vector2 midPoint = new Vector2(bounds.X, bounds.Y);

        random = new Random();

        Block = new ParticleEngine(textures, midPoint, new Color(0,0,0), 4, 1, 1000f, 1f, 0.1f, 0.1f, 25, 65, false, 0, 0);
    }

    private void Add() {

    }

    public void Update(bool add) {
        count++;

        if (count % 50 == 0) {
            add = true;

            Block.EmitterLocation = new Vector2(random.Next(bounds.X, bounds.X + bounds.Width), bounds.Y);
            
        } else {
            add = false;
        }

        Block.Update(add);

    }

    public void Draw(SpriteBatch spriteBatch) {
        Block.Draw(spriteBatch);
    }

}




class ParticalEntityManager {

    private List<JellyFish> jellyList = new List<JellyFish>();
    private List<FallingBlocks> FallingBlocks = new List<FallingBlocks>();

    public ParticalEntityManager() {}

    public void Add(string entity, List<Texture2D> textures, Rectangle _bounds, float scale = 0.1f, int col = 2, int ran = 0) {
        if(entity == "jelly") {
            JellyFish j = new JellyFish(textures, _bounds, scale, col, ran);
            jellyList.Add(j);
        } 
        else if (entity == "fallBlock") {
            FallingBlocks.Add(new FallingBlocks(textures, _bounds));
        }
        else {
            Debug.WriteLine("no such entity exists");
        }
    }

    public void Clear() {
        jellyList.Clear();
        FallingBlocks.Clear();
    }

    public void Update() {
        if (jellyList.Count != 0) {
            foreach (JellyFish jelly in jellyList) {
                jelly.movement();
                jelly.Update();
            }
        }
        if (FallingBlocks.Count != 0) {
            foreach (FallingBlocks block in FallingBlocks) {
                //block.movement();
                block.Update(true);
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        if (jellyList.Count != 0) {
            foreach (JellyFish jelly in jellyList) {
                jelly.Draw(spriteBatch);
            }
        }
        if (FallingBlocks.Count != 0) {
            foreach (FallingBlocks block in FallingBlocks) {
                //block.movement();
                block.Draw(spriteBatch);
            }
        }
    }
}

