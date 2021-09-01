using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;



class CollisionRectangle {

    public CollisionRectangle(Rectangle _rect, bool _collision, Color? _colour = null) {
        rect = _rect;
        collision = _collision;

        colour = _colour ?? Color.White;
        
    }

    public Rectangle rect = new Rectangle();
    public Color colour = Color.White;
    public bool collision;

    public int IsTouchingLeft(Rectangle playerRect) {
        if (playerRect.Right > rect.Left &&
            playerRect.Left < rect.Left &&
            playerRect.Bottom > rect.Top &&
            playerRect.Top < rect.Bottom) 
        {
            return rect.Left;
        }
        return int.MinValue;
    }
    public int IsTouchingRight(Rectangle playerRect) {
        if (playerRect.Left < rect.Right &&
            playerRect.Right > rect.Right &&
            playerRect.Bottom > rect.Top &&
            playerRect.Top < rect.Bottom) 
        {
            return rect.Right;
        }
        return int.MinValue;
    }
    public int IsTouchingTop(Rectangle playerRect) {
        if (playerRect.Bottom > rect.Top &&
            playerRect.Top < rect.Top &&
            playerRect.Right > rect.Left &&
            playerRect.Left < rect.Right) 
        {
            return rect.Top;
        }
        return int.MinValue;
    }
    public int IsTouchingBottom(Rectangle playerRect) {
        if (playerRect.Top < rect.Bottom &&
            playerRect.Bottom > rect.Bottom &&
            playerRect.Right > rect.Left &&
            playerRect.Left < rect.Right) 
        {
            return rect.Bottom;
        }
        return int.MinValue;
    }

}


class Geometry {

    private Texture2D texture;
    private List<CollisionRectangle> objects = new List<CollisionRectangle>();

    public Geometry(Texture2D _texture) {
        texture = _texture;
    }



    public void addGeometry(Rectangle rect, bool collision = true, Color? colour = null) {
        colour = colour ?? Color.White;
        objects.Add(new CollisionRectangle(rect, collision, colour));
    }
    public void addGeometry(Point point1, Point point2, bool collision = true, Color? colour = null) {
        colour = colour ?? Color.White;
        Rectangle rect = new Rectangle(point1, point2);
        objects.Add(new CollisionRectangle(rect, collision, colour));
    }
    public void addGeometry(int x1, int y1, int x2, int y2, bool collision = true, Color? colour = null) {
        colour = colour ?? Color.White;

        Point point1 = new Point(x1, y1);
        Point point2 = new Point(x2, y2);

        Rectangle rect = new Rectangle(point1, point2);
        objects.Add(new CollisionRectangle(rect, collision, colour));
    }

    public void clear() {
        objects.Clear();
    }

    public bool checkCollision(Rectangle playerRect) {
        foreach (CollisionRectangle current in objects) {
            if (current.collision) {
                if (playerRect.Intersects(current.rect)) {
                    return true;
                }
            }
        }
        return false;
    }
    public bool checkCollision(Point playerPoint) {
        foreach (CollisionRectangle current in objects) {
            if (current.collision) {
                if (current.rect.Contains(playerPoint)) {
                    return true;
                }
            }
        }
        return false;
    }


    public int[] checkCollisions(Rectangle playerRect) {

        int[] output = { int.MinValue, int.MinValue, int.MinValue, int.MinValue };
        

        foreach (CollisionRectangle current in objects) {

            if (current.collision) {
                if (current.IsTouchingTop(playerRect) != int.MinValue) {
                    output[0] = current.IsTouchingTop(playerRect);
                }
                if (current.IsTouchingRight(playerRect) != int.MinValue) {
                    output[1] = current.IsTouchingRight(playerRect);
                }
                if (current.IsTouchingBottom(playerRect) != int.MinValue) {
                    output[2] = current.IsTouchingBottom(playerRect);
                }
                if (current.IsTouchingLeft(playerRect) != int.MinValue) {
                    output[3] = current.IsTouchingLeft(playerRect);
                }
            }           
        }

        return output;
    }


    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, float layerDepth = 0.5f) {
        foreach (CollisionRectangle current in objects) {
            spriteBatch.Draw(texture, current.rect, null, current.colour, 0f, new Vector2(0,0), SpriteEffects.None, layerDepth);
        }
    }

}

