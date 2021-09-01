using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

// AnimatedSprite.  This class  handles the animation and drawing of a 2D animation
// based on a single imported texture, which is a single horizontal strip of 
// sequential images (cells). AnimatedSprite expects the cells of the sprite strip to be square

// added in a seperate x,y position, darwing depth and spriteEffect setting methods, rather than it being in the draw method.
// this is so they can be set by the game input and logic before drawing


class AnimatedSpriteStripManager
{

    private AnimatedSpriteStrip[] myAnimatedSpriteStrips;
    private int actionsAddedCount = 0;
    private int currentAction = 0;
    public string currentDirection = "right";

    public string previousAction="none";

    public bool isFinished() { return myAnimatedSpriteStrips[currentAction].isFinished(); }

    public Vector2 Origin() {
        return myAnimatedSpriteStrips[currentAction].Origin();
    }

    public Rectangle boundingBox() {
        return myAnimatedSpriteStrips[currentAction].boundingBox();
    }

    public string currentActionName;

    public AnimatedSpriteStripManager(int numActions)
    {
        myAnimatedSpriteStrips = new AnimatedSpriteStrip[numActions];
    }

    public void addAnimatedSpriteStrip(AnimatedSpriteStrip thisAnim)
    {
        if (actionsAddedCount > myAnimatedSpriteStrips.Length)
        {
            Console.WriteLine("adding too many actions for your actions manager");
        }
        else
        {
            myAnimatedSpriteStrips[actionsAddedCount] = thisAnim;
            actionsAddedCount = actionsAddedCount + 1;
        }
    }


    public void setCurrentAction(string actionName)
    {

        for (int n = 0; n < actionsAddedCount; n++)
        {
            if (actionName == myAnimatedSpriteStrips[n].myName)
            {

                currentAction = n;
                if (previousAction != actionName)
                {

                    myAnimatedSpriteStrips[n].reset();

                }
                previousAction = currentActionName;
                currentActionName = actionName;
                setCurrentDirection(currentDirection);
                return;
            }
        }
        Console.WriteLine("Cannot find this action in action list");
    }


    public void setCurrentDirection(string dir)
    {
        // assumes all actions drawn facing to the LEFT
        if (actionsAddedCount == 0) return;

        if (dir == "left")
        {
            currentDirection = "left";
            myAnimatedSpriteStrips[currentAction].setSpriteEffect(SpriteEffects.FlipHorizontally);
        }
        if (dir == "right")
        {
            currentDirection = "right";
            myAnimatedSpriteStrips[currentAction].setSpriteEffect(SpriteEffects.None);
        }
    }

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch, float XPos, float YPos, Color colour, float scale = 1f, float rotation = 0f)
    {
        if (actionsAddedCount == 0) return;
        myAnimatedSpriteStrips[currentAction].Draw(gameTime, spriteBatch, XPos, YPos, colour, scale, rotation);
    }

}

