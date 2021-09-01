using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;


public class ParticleEngine
{
    private Random random;
    private List<Particle> particles;
    private List<Texture2D> textures;

    //vars
    public Vector2 EmitterLocation { get; set; }
    private Color colour;
    private int colourChannel;
    private int total;
    private float lifeSpan;
    private float scale;
    private float xScale;
    private float yScale;
    private int startAngle;
    private int endAngle;
    private bool mirror;
    private int rotation;
    private int rotationSpeed = 360;
    //
    public int count;

    public ParticleEngine(List<Texture2D> textures, Vector2 location, Color col, int colChannel = 1, int _total = 10, float _lifeSpan = 1f, float _scale = 1f, float _xScale = 1f, float _yScale = 1f, int _startAngle = 0, int _endAngle = 360, bool _mirror = false, int _rotation = 0, int _rotationSpeed = 0)
    {
        EmitterLocation = location;
        this.textures = textures;
        this.particles = new List<Particle>();
        random = new Random();

        colour = col;
        colourChannel = colChannel;
        total = _total;
        lifeSpan = _lifeSpan;
        scale = _scale;
        xScale = _xScale;
        yScale = _yScale;
        startAngle = _startAngle;
        endAngle = _endAngle;
        mirror = _mirror;
        rotationSpeed *= _rotationSpeed;
        rotation = _rotation + rotationSpeed;
        

        //total = 5;
        //lifeSpan = 1f;
        //scale = 0.1f;
        //xScale = 5f;
        //yScale = 5f;
        //startAngle = 0;
        //endAngle = 1;
        //mirror = true;
        //rotationSpeed *= 0;
        //rotation = 180 + rotationSpeed;
    }

    public void Update(bool addNew)
    {

        if (addNew) {
            for (int i = 0; i < total; i++)
            {
                count++;
                particles.Add(GenerateNewParticle());
            }
            
        }

        for (int particle = 0; particle < particles.Count; particle++)
            {
            particles[particle].Update();
            if (particles[particle].TTL <= 0)
            {
                count--;
                particles.RemoveAt(particle);
                particle--;
            }
        }

    }

    private Particle GenerateNewParticle() {

        Texture2D texture = textures[random.Next(textures.Count)];
        Vector2 position = EmitterLocation;

        startAngle -= rotation;
        endAngle -= rotation;

        float direction = (float)random.Next(startAngle, endAngle);
        direction = direction * (float)(3.14 / 180);

        Vector2 velocity = new Vector2(
             xScale * (float)(Math.Cos(direction)) * scale,
             yScale * (float)(Math.Sin(direction)) * scale);

        if (mirror) {
            if (random.Next(0, 2) == 0) {
                Vector2 mirroring = new Vector2(
                    velocity.X * 2,
                    velocity.Y * 2);
                velocity -= mirroring;
            }
        }


        float angle = 0;
        float angularVelocity = 0;


        Color color = new Color();

        switch (colourChannel) {
            case 1:
                color = new Color((float)random.NextDouble(), colour.G, colour.B);
                break;
            case 2:
                color = new Color(colour.R, (float)random.NextDouble(), colour.B);
                break;
            case 3:
                color = new Color(colour.R, colour.G, (float)random.NextDouble());
                break;
            case 4:
                color = new Color(colour.R, colour.G, colour.B, (float)random.NextDouble());
                break;
        }




        float size = (float)random.NextDouble() * scale;
        int ttl = (int)(20 + random.Next(40) * lifeSpan);

        return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
    }

    public void move(Vector2 displace) {
        //EmitterLocation += displace;
        EmitterLocation = displace;
    }

    public void Draw(SpriteBatch spriteBatch, float layerDepth = 0.7f)
    {
        for (int index = 0; index < particles.Count; index++) {
            particles[index].Draw(spriteBatch, layerDepth);
        }
    }

}


