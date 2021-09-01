using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;


class Player {

    // Init

    private Random r = new Random();

    private AnimatedSpriteStripManager sprites = new AnimatedSpriteStripManager(20);

    private InputHandler input;

    // Static hitbox value
    public static Rectangle hitBox;

    private float rotation = 0f;

    private float xPos;
    private float yPos;

    private float xSpeed;
    private float ySpeed;
    private float storedX;
    private float storedY;
    private float slidingXSpeed = 1;
    private float slidingYSpeed = 1;

    public int topSpeed = 4; // 1 = width of hitbox

    private string nextAction;

    private float scale = 1f;
    private Color colour = Color.White;

    private string currentAction;
    private List<String> ActionQueue = new List<string>();

    private bool canMove = true;
    private int lagFrames = 0;
    private Keys[] buffer;
    private bool isGrounded = false;



    public Player(ContentManager content) {

        input = new InputHandler();

        LoadContent(content);

    }

    protected void LoadContent(ContentManager content) {

        Texture2D baseHitbox = content.Load<Texture2D>("Base Hitbox");
        int w = (int)((float)baseHitbox.Width * scale);
        int h = (int)((float)baseHitbox.Height * scale);
        hitBox = new Rectangle(new Point(0, 0), new Point(w, h));
        baseHitbox.Dispose();


        LoadSprites("idle", "Idle", 1, 1f, true, content);
        LoadSprites("run", "Run", 8, 0.1f, true, content);
        LoadSprites("walk", "Walk", 8, 0.1f, true, content);
        LoadSprites("sprint", "Sprint", 6, 0.1f, true, content);
        LoadSprites("cJump", "Charge Jump", 6, 0.1f, false, content);
        LoadSprites("jump", "Jump", 5, 0.1f, false, content);
        LoadSprites("fall", "Fall", 3, 0.1f, true, content);

        LoadSprites("AA1", "Air Attack 1", 4, 0.1f, false, content);
        LoadSprites("AA2S", "Air Attack 2 Short", 4, 0.1f, false, content);
        LoadSprites("AA2", "Air Attack 2", 6, 0.1f, true, content);

        LoadSprites("sprint", "Sprint", 6, 0.1f, true, content);
        LoadSprites("Grapple", "Turn Around", 4, 0.1f, false, content);


        xPos = 50;
        yPos = 50;

    }

    private void LoadSprites(string name, string filename, int numFrames, float frameTime, bool isLooping, ContentManager content) {

        Texture2D texture = content.Load<Texture2D>(filename);
        AnimatedSpriteStrip strip = new AnimatedSpriteStrip(texture, numFrames, frameTime, isLooping);

        strip.setName(name);
        sprites.addAnimatedSpriteStrip(strip);

    }

    public Rectangle[] DebugBoxes;

    public void Draw(GameTime gameTime, SpriteBatch spriteBatch) {

        Rectangle spriteBounds = getSpriteBox();

        //Rectangle uc = getHitbox();
        //Rectangle next = getNextHitbox();

        ////spriteBatch.Draw(global.block, next, Color.White);
        //spriteBatch.Draw(global.block, uc, Color.Black);
        //spriteBatch.Draw(global.block, getBottom(), Color.Red);

        //Random r = new Random(0);
        //for (int i = 3; i >= 0; i--) {
        //    Color c = new Color(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
        //    spriteBatch.Draw(global.block, DebugBoxes[i], c);
        //}


        sprites.Draw(gameTime, spriteBatch, spriteBounds.X, spriteBounds.Y, colour, scale, rotation);

    }



    public Vector2 getPosition() {
        return new Vector2(xPos, yPos);
    }

    // Returns bottom of rectangle
    public Rectangle getBottom() {
        Rectangle output = getHitbox();

        output.Y += output.Height -2;
        output.Height = 3;


        return output;
    }

    // Returns the hitbox of the player
    public Rectangle getHitbox() {

        Rectangle b = getSpriteBox();

        Rectangle output = hitBox;

        output.Offset(b.Center.X - hitBox.Width / 2, b.Center.Y - hitBox.Height / 2);

        return output;
    }

    // Returns the Box containing the player sprite
    public Rectangle getSpriteBox() {
        Rectangle output = sprites.boundingBox();

        float h = (float)output.Height * scale;
        float w = (float)output.Width * scale;

        output.Height = (int)h;
        output.Width = (int)w;

        output.Offset(xPos - output.Width / 2, yPos - output.Height / 2);

        return output;
    }

    // Returns the Box containing the next POSSIBLE player position
    public Rectangle getNextHitbox() {

        Rectangle output = getHitbox();

        output.Offset(xSpeed, ySpeed);

        return output;

    }


    public void changeAction(string action) {

        //ActionQueue.Capacity == 0

        //if (canMove) {
        sprites.setCurrentAction(action);
        //}



    }

    private string direction() {
        return sprites.currentDirection;
    }

    private void changeDirection(string dir) {
        if (dir == "left") {
            sprites.setCurrentDirection("left");
        } else {
            sprites.setCurrentDirection("right");
        }
    }

    private void playQueue() {
        if (ActionQueue.Count != 0) { changeAction(ActionQueue[0]); }

        if (sprites.isFinished()) { ActionQueue.RemoveAt(0); }

        if (ActionQueue.Count == 0) { canMove = true; }
    }

    public void update(GameTime gameTime) {

        input.Update(); getBottom();



        //Debug.WriteLine(storedX);

        nextAction = "idle";
        
        if (input.IsKeyDown(Keys.D)) { changeDirection("right"); } else if (input.IsKeyDown(Keys.A)) { changeDirection("left"); }


        if (!canMove) {

            if (!sprites.isFinished())
                return;

            canMove = true;
            return;

        }


        //

        if (isGrounded) {
            ySpeed = 0;

            if (input.IsKeyDown(Keys.Space)) {
                changeAction("idle");
                Charge();
            } 
            else { 

                if (input.IsKeyDown(Keys.D) || input.IsKeyDown(Keys.A)) {

                    if (input.IsHoldingKey(Keys.LeftAlt))
                        Walk();
                    else
                        Run();

                }

                if (input.IsKeyDown(Keys.LeftControl)) {
                    Grapple();
                }

                if (input.IsKeyDown(Keys.LeftShift)) {
                    Sprint();
                }

                if (input.IsKeyDown(Keys.W)) {
                    Jump();
                }

                if (input.isMouseButtonDown()) {
                    //Attack();
                }

            }

        } else {



            if (input.IsKeyDown(Keys.LeftControl)) {
                Grapple();
                if (!input.IsHoldingKey(Keys.LeftControl)) {
                    storedX /= (float)gameTime.ElapsedGameTime.TotalSeconds;
                }
                //
            } 
            else {
                Fall();
                rotation = 0f;

                //if (input.IsKeyDown(Keys.W)) {
                //    Jump();
                //}

                if (input.isMouseButtonClick()) {
                    Attack();
                } else if (input.isRightMouseButtonClick()) {
                    if (sprites.currentActionName == "AA1") {
                        Attack2S();
                    } else {
                        Attack2();
                    }
                }

                var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Air Sprint

                if (input.IsKeyDown(Keys.LeftShift)) {
                    Sprint();
                    ySpeed += 100;
                    xSpeed = xSpeed * deltaTime * scale * slidingXSpeed;

                    if (direction() == "left") {
                        //rotation = -45f;
                        if (xSpeed > 0) xSpeed *= -1;
                    } else if (direction() == "right") {
                        //rotation = 45f;
                        if (xSpeed < 0) xSpeed *= -1;
                    }
                }

                changeAction(nextAction);


                //ySpeed = ySpeed * deltaTime * scale * slidingYSpeed;
                ySpeed = ySpeed * deltaTime * scale;

                return;
            }
        }
        rotation = 0f;


        if (sprites.isFinished()) {
            if (sprites.currentActionName == "cJump") {

                if (input.IsKeyDown(Keys.D) || input.IsKeyDown(Keys.A)) {

                    Sprint();

                }

                if (input.IsKeyDown(Keys.W)) {
                    Jump();
                }

                //if ( !(input.IsKeyDown(Keys.D) || input.IsKeyDown(Keys.A)) && !(input.IsKeyDown(Keys.W))) {
                //    //nextAction = "idle";
                //}

            }

            if (sprites.currentActionName == "Grapple") {

                if (input.IsKeyDown(Keys.W)) {
                    Jump();
                    xSpeed = storedX;
                    storedX = 0;
                }

            }
        }


        changeAction(nextAction);

        if (sprites.currentActionName == "idle") {
            slidingXSpeed = 1;
            slidingYSpeed = 1;
        }



        normaliseSpeed(gameTime);
    }

    public void lateUpdate(GameTime gameTime, int[] collisions, bool _isGrounded) {

        /// top,right,bottom,left -> 0,1,2,3
        // EG collisions[0] relates the the yPos of the top of the geometry

        // Player hitting Right
        if (xSpeed > 0 && collisions[3] != int.MinValue) {
            xSpeed = 0;
            xPos = collisions[3] - (float)hitBox.Width / 2;
        }
        // Player hitting Left
        if (xSpeed < 0 & collisions[1] != int.MinValue) {
            xSpeed = 0;
            xPos = collisions[1] + (float)hitBox.Width / 2;
        }
        // Player hitting Head
        if (ySpeed < 0 && collisions[2] != int.MinValue) {
            ySpeed = 0;
            yPos = collisions[2] + (float)hitBox.Height / 2;
        }
        // Player hitting Feet
        if (ySpeed > 0 & collisions[0] != int.MinValue) {
            ySpeed = 0;
            yPos = collisions[0] - (float)hitBox.Height / 2;
            isGrounded = true;
        }


        isGrounded = _isGrounded;

        if(!(xPos+xSpeed > xPos)) {
            //slidingXSpeed = 1;
            //slidingYSpeed = 1;
        }

        xPos += xSpeed;
        yPos += ySpeed;
    }

    private void normaliseSpeed(GameTime gameTime) {
        if (direction() == "left" && xSpeed > 0) xSpeed *= -1;
        else if (direction() == "right" && xSpeed < 0) xSpeed *= -1;


        // Debug
        if (input.IsKeyDown(Keys.Up)) { ySpeed = -5500000; }
        if (input.IsKeyDown(Keys.Down)) { ySpeed = 35000000; }
        if (input.IsKeyDown(Keys.Left)) { xSpeed = -120000000; }
        if (input.IsKeyDown(Keys.Right)) { xSpeed = 3500000000; }


        // Normalise
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        xSpeed = xSpeed * deltaTime * scale * slidingXSpeed;
        ySpeed = ySpeed * deltaTime * scale * slidingYSpeed;


        // Limit speed
        if (xSpeed > hitBox.Width * topSpeed) {
            xSpeed = hitBox.Width * topSpeed - 1;
        }
        if (-xSpeed > hitBox.Width * topSpeed) {
            xSpeed = -hitBox.Width * topSpeed + 1;
        }
        if (ySpeed > hitBox.Width * topSpeed) {
            ySpeed = hitBox.Width * topSpeed - 5;
        }
        if (-ySpeed > hitBox.Width * topSpeed) {
            ySpeed = -hitBox.Width * topSpeed + 5;
        }

    }


    /// private small functions

    // moves
    private void Run() {

        Debug.WriteLine("run");
        nextAction = "run";

        xSpeed = 100;
        
    }

    private void Walk() {

        Debug.WriteLine("walk");
        nextAction = "walk";

        xSpeed = 50;
        
    }

    private void Charge() {

        Debug.WriteLine("cJump");
        nextAction = "cJump";
        //canMove = false;

        slidingXSpeed = 4;
        slidingYSpeed = 4;

        xSpeed = 0;
        ySpeed = 0;

        

    }

    private void Jump() {

        Debug.WriteLine("Jump");
        nextAction = "jump";
        canMove = false;

        ySpeed = -200;        

    }

    private void Fall() {

        Debug.WriteLine("fall");
        nextAction = "fall";

        ySpeed = 100;

    }

    private void Grapple() {

        Debug.WriteLine("grapple");
        nextAction = "Grapple";

        storedX = xSpeed;
        //storedX = xSpeed / scale / slidingXSpeed;
        storedY = ySpeed;

        xSpeed = 0;
        ySpeed = 0;

    }

    private void Sprint() {

        Debug.WriteLine("sprint");
        nextAction = "sprint";
        canMove = false;

        if (slidingXSpeed < 2) slidingXSpeed = 2;
        if (slidingYSpeed < 2) slidingYSpeed = 2;      

        xSpeed = 150;
        
    }

    private void Attack() {

        Debug.WriteLine("attack");
        nextAction = "AA1";
        canMove = false;

        lagFrames = 10;

    }

    private void Attack2() {

        Debug.WriteLine("attack 2");
        nextAction = "AA2";
        canMove = false;       

    }

    private void Attack2S() {

        Debug.WriteLine("attack 2 short");
        nextAction = "AA2S";
        canMove = false;

    }

    private void Spin() {

        Debug.WriteLine("Air Spin");
        nextAction = "ASpin";
        canMove = false;
        
    }

}