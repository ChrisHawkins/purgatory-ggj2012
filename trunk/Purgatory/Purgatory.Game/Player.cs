
namespace Purgatory.Game
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Audio;
    using Microsoft.Xna.Framework.Graphics;
    using Purgatory.Game.Animation;
    using Purgatory.Game.Controls;
    using Purgatory.Game.Graphics;
    using Purgatory.Game.Physics;
    using Purgatory.Game.PowerUps;

    public class Player : IMoveable
    {
        public static bool InputFrozen = false;
        private const int ShieldMaxHealth = 5;
        public const int MaxHealth = 12;
        public const float MaxEnergy = 10;
        private const float EnergyRegenChargeTime = 3f;
        private const float MinEnergyPerSecond = 2f;
        private const float MaxEnergyPerSecond = 10f;
        public const float MaxSpeed = 350;
        public const int MaxBounce = 10;
        public const float BulletSpeed = 600;

        private const float EnergyPerShot = 1f;
        public float Speed { get; set; }

        private Vector2 direction;
        public Vector2 MovementDirection { get; set; }
        private DirectionalSprite sprite;
        public PlayerNumber PlayerNumber { get; private set; }
        public Vector2 BulletDirection { get; set; }
        private List<float> xPenetrations;
        private List<float> yPenetrations;
        public float ShootCooldown { get; set; }
        public float ShootTimer { get; set; }

        public Vector2 DashVelocity { get; set; }

        public const float DashCooldownTime = 1;
        public float TimeSinceLastDash { get; set; }

        public const float SpiralShotTime = 5;
        public float TimeSinceSpiralBegan { get; set; }

        private List<DashSprite> dashPath;
        private Vector2 lastDashSprite;

        public Sprite BulletSprite { get; set; }
        private InputController inputController;

        public int Health { get; set; }
        public float Energy { get; set; }

        public int BulletBounce { get; set; }
        
        public int ShieldHealth { get; set; }
        private Embellishment shield;

        public List<Bullet> BulletList { get; set; }
        private List<Bullet> dyingBullets { get; set; }

        public TimeSpan NoClipTime { get; set; }

        private GrowShrinkEffect effect;

        private Rectangle collisionRectangle;

        public Cue ShootSFX;
        public Cue DamageSFX;
        public Cue DeathSFX;
        public Cue DashSFX;

        public Player(PlayerNumber playerNumber)
        {
            this.TimeSinceSpiralBegan = 100;
            this.BulletBounce = 0;
            dashPath = new List<DashSprite>();
            lastDashSprite = new Vector2(float.PositiveInfinity);

            this.dyingBullets = new List<Bullet>();

            this.TimeSinceLastDash = 100;
            this.Speed = Player.MaxSpeed;
            this.PlayerNumber = playerNumber;
            this.Health = Player.MaxHealth;
            this.Energy = Player.MaxEnergy;
            this.BulletList = new List<Bullet>();
            this.direction = new Vector2(0, 1);
            this.effect = new GrowShrinkEffect(1000f, 0.02f);

            this.xPenetrations = new List<float>();
            this.yPenetrations = new List<float>();

            this.ShootCooldown = 0.2f;
            this.NoClipTime = NoClipPowerUp.Duration;
             
            if (this.PlayerNumber == PlayerNumber.PlayerOne)
            {
                this.ShootSFX = AudioManager.Instance.LoadCue("Purgatory_HaloThrow");
                this.DamageSFX = AudioManager.Instance.LoadCue("Purgatory_LifeDamageScream");
                this.DeathSFX = AudioManager.Instance.LoadCue("Purgatory_LifeDyingScream");
            }
            else
            {
                this.ShootSFX = AudioManager.Instance.LoadCue("Purgatory_ScytheThrow");
                this.DamageSFX = AudioManager.Instance.LoadCue("Purgatory_DeathDamageScream");
                this.DeathSFX = AudioManager.Instance.LoadCue("Purgatory_DeathDyingScream");
            }
            
            this.DashSFX = AudioManager.Instance.LoadCue("Purgatory_PlayerDash");
        }

        private void MakeShield()
        {
            string asset = this.PlayerNumber == PlayerNumber.PlayerOne ? "LifeShield" : "DeathShield";

            this.shield = new Embellishment()
            {
                EmbellishmentSprite = new Sprite(BigEvilStatic.Content.Load<Texture2D>(asset), 64, 64),
                Entrance = new PopInEffect(750f, 0.2f),
                Exit = new ExpandDeathEffect(3000f, 200f),
                Persists = true
            };

            this.shield.EmbellishmentSprite.Effects.Add(new PulsateEffect(2000f, 0.10f));
            this.sprite.AddEmbellishment(this.shield);
        }

        public void Initialize(InputController controller, DirectionalSprite sprite, Sprite bulletSprite)
        {
            this.inputController = controller;
            this.sprite = sprite;
            this.BulletSprite = bulletSprite;
            this.collisionRectangle = new Rectangle(0, 0, sprite.Width, sprite.Height);
            this.Spawn();
            
            //if (this.playerNumber == PlayerNumber.PlayerOne)
            //{
            //    this.position = new Vector2(Level.TileWidth) * 55;
            //}
            //else if (this.playerNumber == PlayerNumber.PlayerTwo)
            //{
            //    this.position = new Vector2(Level.TileWidth * 127, Level.TileWidth * 141);
            //}
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
        }

        public Vector2 LastPosition { get; set; }

        private Vector2 position;

        public void EnterPurgatory(PlayerNumber playerNumber, Level purgatory, Portal portal)
        {
            if (playerNumber == this.PlayerNumber)
            {
                this.Speed = Player.MaxSpeed * 3 / 4;
                this.Health = Player.MaxHealth;
                this.Energy = 0;
                this.BulletBounce = 0;
                this.Level = purgatory;
                this.Level.ClearPickups();

                this.Level.AddToPickups(portal, this.position, 50 * 32, true);
            }
            
            this.BulletList.Clear();
        }

        public void Spawn()
        {
            this.position = this.Level.FindSpawnPoint(true);
            this.LastPosition = this.position;
            this.Health = Player.MaxHealth;
            this.Energy = Player.MaxEnergy;
            this.Speed = Player.MaxSpeed;
        }

        public void Update(GameTime gameTime)
        {
            if (NoClipTime < NoClipPowerUp.Duration)
            {
                NoClipTime += gameTime.ElapsedGameTime;
            }

            this.Level.Update(gameTime);

            this.BulletBounce = (int)MathHelper.Clamp(this.BulletBounce, 0, Player.MaxBounce);
            this.Health = (int)MathHelper.Clamp(this.Health, 0, Player.MaxHealth);

            if (!InputFrozen)
            {
                this.TimeSinceSpiralBegan += (float)gameTime.ElapsedGameTime.TotalSeconds;
                TimeSinceLastDash += (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.UpdateMovement(gameTime);

                // Update player direction. Dont change if movement direction has no length
                if (MovementDirection.LengthSquared() != 0)
                {
                    this.direction = MovementDirection;
                    this.sprite.PlayAnimation = true;

                    if (this.Health > 0)
                    {
                        this.sprite.AddEffect(this.effect);
                    }
                }
                else
                {
                    this.sprite.RemoveEffect(this.effect);
                    this.sprite.PlayAnimation = false;
                }

                this.RegenEnergy(gameTime);

                if (!(this.Level is PurgatoryLevel))
                {
                    if (this.TimeSinceSpiralBegan <= SpiralShotTime)
                    {
                        this.Energy = 0;
                        if (this.ShootTimer > this.ShootCooldown)
                        {
                            for (int i = 0; i < 5; ++i)
                            {
                                Vector2 bulletPos = this.Position;
                                Bullet b = new Bullet(bulletPos, Vector2.Normalize(new Vector2((float)Math.Cos(MathHelper.WrapAngle((MathHelper.TwoPi / 5) * (i + 1) + this.TimeSinceSpiralBegan * 2)), (float)Math.Sin(MathHelper.WrapAngle((MathHelper.TwoPi / 5) * (i + 1) + this.TimeSinceSpiralBegan * 2)))), this.BulletBounce, Player.BulletSpeed, new Sprite(this.BulletSprite), this.Level, this.NoClipTime < NoClipPowerUp.Duration);
                                this.BulletList.Add(b);
                                this.ShootTimer = 0.0f;
                                AudioManager.Instance.PlayCue(ref this.ShootSFX, true);
                            }
                        }
                    }

                    this.inputController.UpdateShoot(this, gameTime);

                    for (int i = this.BulletList.Count - 1; i >= 0; --i)
                    {
                        this.BulletList[i].Update(gameTime);

                        if (this.BulletList[i].RemoveFromList)
                        {
                            AddDyingBullet(this.BulletList[i]);
                            this.BulletList.RemoveAt(i);
                        }
                    }
                }
            }

            if (this.ShieldHealth > 0)
            {
                if (this.shield == null)
                {
                    this.MakeShield();
                }

                this.shield.EmbellishmentSprite.Alpha = (float)this.ShieldHealth / (float)ShieldMaxHealth + 0.2f;
            }
            else
            {
                if (this.shield != null)
                {
                    this.shield.Destroy();
                    this.shield = null;
                }
            }

            this.sprite.UpdateAnimation(gameTime);
            this.Level.CheckPickUpCollisions(this);

            if (Microsoft.Xna.Framework.Input.Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.K))
            {
                //this.Health -= 1;
                //this.ShieldHealth = 10;
                //this.sprite.AddEffect(new PopInEffect(1000f, 0.25f));
                //this.sprite.AddEffect(new PurgatoryEffect());
                //PlayPurgatoryAnimation();
                //this.sprite.AddEffect(new PopInEffect(1000f, 0.25f, true));

                //this.sprite.AddEffect(new SpinEffect(1250f));
                //this.sprite.AddEffect(new PopInEffect(3000f, 0.2f, true));
            }

            List<Bullet> finallyDeleteAlltogether = new List<Bullet>();

            foreach (var bullet in this.dyingBullets)
            {
                bullet.Sprite.UpdateAnimation(gameTime);
                bullet.Sprite.UpdateEffects(gameTime);

                if (bullet.Sprite.Effects.Count == 0)
                {
                    finallyDeleteAlltogether.Add(bullet);
                }
            }

            foreach (var bullet in finallyDeleteAlltogether)
            {
                this.dyingBullets.Remove(bullet);
            }
        }

        private void AddDyingBullet(Bullet bullet)
        {
            bullet.Sprite.Effects.Add(new PopInEffect(150f, 0f, true));
            this.dyingBullets.Add(bullet);
        }

        private void RegenEnergy(GameTime gameTime)
        {
            if (this.Energy < Player.MaxEnergy && !(this.Level is PurgatoryLevel))
            {
                float regenRate;

                if (this.ShootTimer <= this.ShootCooldown)
                {
                    regenRate = Player.MinEnergyPerSecond;
                }
                else
                {
                    float lerp = (ShootTimer - this.ShootCooldown) / Player.EnergyRegenChargeTime;
                    regenRate = Player.MinEnergyPerSecond + lerp * (Player.MaxEnergyPerSecond - Player.MinEnergyPerSecond);
                }
                
                this.Energy += regenRate * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.Energy = Math.Min(this.Energy, Player.MaxEnergy);
            }
        }

        public void Draw(SpriteBatch batch, Bounds bounds)
        {
            this.sprite.Draw(this.direction, batch, bounds.AdjustPoint(this.Position));

            foreach (var bullet in BulletList)
            {
                bullet.Draw(batch, bounds);
            }

            foreach (var bullet in this.dyingBullets)
            {
                bullet.Draw(batch, bounds);
            }

            foreach (var dash in dashPath)
            {
                dash.Draw(batch, bounds);
            }
        }

        private void UpdateMovement(GameTime gameTime)
        {
            this.inputController.UpdateMovement(this, gameTime);

            if (this.DashVelocity != Vector2.Zero)
            {
                this.LastPosition = this.Position;
                this.position += DashVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.DashVelocity -= 30 * this.Speed * this.MovementDirection * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (DashVelocity.LengthSquared() <= Speed * Speed)
                {
                    this.DashVelocity = Vector2.Zero;
                }
            }
            else
            {
                this.lastDashSprite = new Vector2(float.PositiveInfinity);
                this.LastPosition = this.Position;
                this.position += MovementDirection * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            this.CheckForCollisions();

            // Update dash path transparency.
            for (int i = dashPath.Count - 1; i >= 0; --i )
            {
                dashPath[i].update(gameTime);
                if (dashPath[i].RemoveFromList)
                {
                    dashPath.RemoveAt(i);
                }
            }

            if(this.DashVelocity != Vector2.Zero)
            {   
                float distanceCheck = 20;
                if(Vector2.DistanceSquared(lastDashSprite, this.position) > distanceCheck * distanceCheck)
                {
                    if (float.IsInfinity(this.lastDashSprite.X))
                    {
                        this.lastDashSprite = this.position;
                    }
                    else
                    {
                        Vector2 posChange = (this.position - lastDashSprite);
                        posChange.Normalize();
                        lastDashSprite += (posChange * distanceCheck);
                    }

                    DashSprite dashSprite = new DashSprite(this.sprite.CreateSprite(this.MovementDirection), lastDashSprite);
                    this.dashPath.Add(dashSprite);
                }
            }
        }

        private void CheckForCollisions()
        {
            if (this.Position != this.LastPosition)
            {
                //Vector2 tempPosition = this.position;
                //int tempHeight = collisionRectangle.Height;
                //collisionRectangle.Height = tempHeight / 2;
                //this.position.Y += (float)tempHeight / 4;

                this.xPenetrations.Clear();
                this.yPenetrations.Clear();
                List<Rectangle> possibleRectangles = Level.GetPossibleRectangles(Position, LastPosition);

                foreach (Rectangle r in possibleRectangles)
                {
                    Vector2 penetration = CollisionSolver.SolveCollision(this, r);
                    if (penetration.X != 0)
                    {
                        this.xPenetrations.Add(penetration.X);
                    }
                    if (penetration.Y != 0)
                    {
                        this.yPenetrations.Add(penetration.Y);
                    }
                }

                //this.position = tempPosition;

                if (xPenetrations.Count != 0 || yPenetrations.Count != 0)
                {
                    if (xPenetrations.Count >= yPenetrations.Count)
                    {
                        this.xPenetrations.Sort();
                        this.position.X -= xPenetrations[0];
                    }
                    if (yPenetrations.Count >= xPenetrations.Count)
                    {
                        this.yPenetrations.Sort();
                        this.position.Y -= yPenetrations[0];
                    }
                }

                //collisionRectangle.Height = tempHeight;
            }
        }

        public Level Level { get; set; }

        public Rectangle CollisionRectangle
        {
            get { return GeometryUtility.GetAdjustedRectangle(this.Position, this.collisionRectangle); }
        }

        public void CheckBulletCollisions(List<Bullet> list)
        {
            for(int b = 0; b < list.Count; ++b)
            {
                if (this.CollisionRectangle.Intersects(list[b].CollisionRectangle))
                {
                    
                    if (this.ShieldHealth > 0)
                    {
                        this.ShieldHealth--;
                        list[b].SwitchOwner(this);

                        Vector2 displacement = this.position - list[b].Position;
                        Vector2 normal = Vector2.Normalize(displacement);
                        Vector2 projection = Vector2.Dot(list[b].Direction, normal) * normal;
                        Vector2 rejection = direction - projection;
                        list[b].Direction = Vector2.Normalize(rejection - projection);

                        this.BulletList.Add(list[b]);
                        list.RemoveAt(b);
                        --b;
                    }
                    else
                    {
                        this.Health --;

                        if (this.Health > 0)
                        {
                            this.sprite.AddEffect(new PainEffect());
                            AudioManager.Instance.PlayCue(ref this.DamageSFX, false);
                        }
                        else if (Level is PurgatoryLevel)
                        {
                            this.sprite.AddEffect(new SpinEffect(1250f));
                            this.sprite.AddEffect(new PopInEffect(3000f, 0.2f, true));

                            AudioManager.Instance.PlayCue(ref this.DeathSFX, false);
                            Player.InputFrozen = true;
                        }

                        list[b].RemoveFromList = true;
                    }
                }
            }
        }

        internal void SetBulletDirection(Vector2 targetPosition)
        {
            this.BulletDirection = targetPosition - this.Position;
            this.BulletDirection.Normalize();
        }
    }
}
