using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;
using BEPUphysics.DataStructures;
using BEPUphysics.Collidables;
using BEPUphysics.MathExtensions;
using BEPUphysics.Entities;
using LeoniV0_3;
//using SkinnedModel;


namespace LeoniV0_3
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        public string mainVersion = "0.2.6.2"; //For Main Menu. Change this for every update!

        #region Loading

            /*Default Loading*/
            public Space space = new Space();
            GraphicsDeviceManager graphics;
            public SpriteBatch spriteBatch;
            public Camera Camera;
            public Effect effect;
            public cutScenes CutScenes;
            public Items userObjects;
            public float contrast = 0.05f;
            public Random ran = new Random();

            public string blahText = "";

            public String arrowstring = "->";
            
            /*Menu Objects*/
            public Item ibattery;
            public Item ivoltmeter;
            public Item iplasmagun;
            public Item iairbattery;
           
            public Texture2D object_pointer;

            /*Rasterize state to make sure we're not drawing triangles not shown on the screen*/
            RasterizerState rs = new RasterizerState();               
          
            /*Actions*/
            public Actions actions;

            /*Voice Overs*/
            public bool songstart = false;
            public SoundEffect vIntro; public SoundEffectInstance seIntro;

            /*Music*/
            public Song[] music = new Song[2];
            
            /*Models*/
            public Model terrain;
            public Model tier; public Model tier_supports;
            public Model[] buildings = new Model[15];
            public Model grass;
            //public List<StaticModel> Comps = new List<StaticModel>();
            public List<StaticModel> MenuComps = new List<StaticModel>();
            public int grassAlphaNum;
            public Model[] Racon;
            public Model testPlane;
            public Model road;
            public Model tank;
            public Models solarPanel;
            public Model battery;
            public Model GlowSphere;
            public Model Test_Monster;
            public Model liAirBattery;
            public Model House1;
            public Model InvisableTest;
            public Model DarkenSphere;
            
            /*Render Target*/
            RenderTarget2D renderTarget;
            RenderTarget2D finalTarget;
            RenderTarget2D bloomTarget;
            RenderTarget2D blurTarget;
            RenderTarget2D finalBloomTarget;
            DepthStencilState depthStencilState = new DepthStencilState();

            /*Extra Camera Stuff*/
            public Vector3 lastPos = new Vector3(0, 250, 10);
            public Sphere cameraCol;

            /*Effect Technique*/
            public bool colored = true;
            public int mode = 0;
            public bool bloom = true;
            public float bloomFactor = 3;
            public Effect postEffect;
            public float blur = 0;
            public bool forceBlur = false;
            public float overlay_amount = 0;

            /*Textures*/
            public Texture2D grassHighDef;
            public Texture2D grassMidDef;
            public Texture2D grassLowDef;
            public Texture2D grassUltraLowDef;
            public Texture2D bumpmap;
            public Texture2D MapArrow;
            public Texture2D Invisable;

            public Texture2D plasma;
            public Texture2D controls;
            public Texture2D aim_cross;
            public Texture2D healthBar;
            public Texture2D healthBar_Green;
            public Texture2D health_green;

            public Texture2D battery_tex;
            public Texture2D battery_logo;

            public Texture2D overlay_tex;
            public Texture2D blood_overlay;

            public Texture2D textBox;
            

            /*2D Text*/
            SpriteFont testFont;
            public SpriteFont SegoeUI;
            public SpriteFont LED_REAL;

            /*Selectable Objects*/
            public List<SelectableObject> SObjects = new List<SelectableObject>();
            
        
            /*Collsion*/
            /*Input*/
            #if XBOX360
                public GamePadState GamePadState;
            #else
                public KeyboardState KeyboardState;
                public MouseState MouseState;
            #endif

            public int screenWidth = 900;
            public int screenHeight = 500;

            /*Constructor*/
            public Game1()
            {
                graphics = new GraphicsDeviceManager(this);              
                graphics.PreferredBackBufferWidth = screenWidth;//900
                graphics.PreferredBackBufferHeight = screenHeight;//500
                this.Window.Title = "Leoni";
                Content.RootDirectory = "Content"; 
            }

            /*Initialize Objects*/
            protected override void Initialize()
            {
                Camera = new Camera(this, new Vector3(0), 5);
                actions = new Actions(this);
                rs.CullMode = CullMode.CullCounterClockwiseFace;
                
                base.Initialize();
            }

            /*Load Everything*/
            protected override void LoadContent()
            {              
                if (Environment.ProcessorCount > 1)
                {
                    //On windows, just throw a thread at every processor.  The thread scheduler will take care of where to put them.
                    for (int i = 0; i < Environment.ProcessorCount; i++)
                    {
                        space.ThreadManager.AddThread();                        
                    }
                }

                /*Basics of the game (effects, rendertargets, spritebatches, ect)*/
                spriteBatch = new SpriteBatch(GraphicsDevice);
                effect = Content.Load<Effect>("Effects\\Effect");
                PresentationParameters pp = graphics.GraphicsDevice.PresentationParameters;
                int w = 1024;
                int h = 1024;
                renderTarget = new RenderTarget2D(graphics.GraphicsDevice, w, h, true, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
                finalTarget = new RenderTarget2D(graphics.GraphicsDevice, w, h, true, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
                //bloomTarget = new RenderTarget2D(graphics.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, true, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
                //blurTarget = new RenderTarget2D(graphics.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, true, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
                //finalBloomTarget = new RenderTarget2D(graphics.GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, true, graphics.GraphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
                postEffect = Content.Load<Effect>("Effects\\PostEffects");
                
                userObjects = new Items();
                //userObjects.objects.Add(new Item(Content.Load<Texture2D>("battery_logo"), 0));
                ibattery = new Item(Content.Load<Texture2D>("Logos\\battery_logo"), 0, "battery");
                ivoltmeter = new Item(Content.Load<Texture2D>("Logos\\Voltmeter_logo"), 1, "voltmeter");
                iplasmagun = new Item(Content.Load<Texture2D>("metal_1"), 3, "Plasma");
                iairbattery = new Item(Content.Load<Texture2D>("Logos\\battery_logo"), 2, "airbattery");
                object_pointer = Content.Load<Texture2D>("Logos\\arrow");     
           
                
                CutScenes = new cutScenes(this);

                /*DepthStencil*/
                depthStencilState.DepthBufferFunction = CompareFunction.LessEqual;
                GraphicsDevice.DepthStencilState = depthStencilState;

                /*Text*/
                SegoeUI = Content.Load<SpriteFont>("Fonts\\foglihten_48");
                LED_REAL = Content.Load<SpriteFont>("Fonts\\LED_REAL");

                /*Texures*/
                bumpmap = Content.Load<Texture2D>("BumpMap");
                MapArrow = Content.Load<Texture2D>("MapArrow");              
                plasma = Content.Load<Texture2D>("Level1Terrain\\Plasma");
                healthBar = Content.Load<Texture2D>("HealthBar");
                healthBar_Green = Content.Load<Texture2D>("health_bar_green");
                health_green = Content.Load<Texture2D>("health_green");
                battery_tex = Content.Load<Texture2D>("Battery_tex");
                overlay_tex = Content.Load<Texture2D>("Smoky_Screen");
                aim_cross = Content.Load<Texture2D>("aim_cross");
                blood_overlay = Content.Load<Texture2D>("Blood_Outside");
                textBox = Content.Load<Texture2D>("Text_Box");


                /*Voice Overs*/
                vIntro = Content.Load<SoundEffect>("Voice-Overs\\Intro");

                /*Music*/
                music[0] = Content.Load<Song>("Music\\Elegy");
                music[1] = Content.Load<Song>("Music\\Broken World");
               
                /*Collision Boxes*/                
                LoadStaticMeshMenu(Content.Load<Model>("MainMenu\\MainMenu"), Content.Load<Texture2D>("MainMenu\\Sand1_Grad"));
                LoadStaticMeshMenu(Content.Load<Model>("MainMenu\\Rose"), Content.Load<Texture2D>("MainMenu\\Rose_Texture"));                
                
                /*Models*/
                battery = Content.Load<Model>("battery");
                GlowSphere = Content.Load<Model>("GlowSphere");
                Test_Monster = Content.Load<Model>("smokemonster_staticbase");
                liAirBattery = Content.Load<Model>("LiAirBattery");
                House1 = Content.Load<Model>("Level1Terrain\\house1");
                DarkenSphere = Content.Load<Model>("Dark_Sphere");

                #region for future use
                /*Assign a model to each object*/
                foreach (Entity e in space.Entities)
                {
                    Box box = e as Box;
                    if (box != null) //This won't create any graphics for an entity that isn't a box since the model being used is a box.
                    {
                        //Matrix scaling = Matrix.CreateScale(box.Width, box.Height, box.Length); //Since the cube model is 1x1x1, it needs to be scaled to match the size of each individual box.
                        //EntityModel model = new EntityModel(e, CubeModel, scaling, this);
                        //model.color = new Vector3(ran.Next(255) / 255f, ran.Next(255) / 255f, ran.Next(255) / 255f);
                        //Add the drawable game component for this entity to the game.
                        //Components.Add(model);
                        //e.Tag = model; //set the object tag of this entity to the model so that it's easy to delete the graphics component later if the entity is removed.
                    }
                }
                #endregion
                //cameraCol = new Sphere(new Vector3(0, 250, 10), 4, 0.1f);
                cameraCol = new Sphere(new Vector3(0, 1, -5), 4, 0.1f);
                Camera.Position = cameraCol.Position;
                lastPos = cameraCol.Position;                
                space.Add(cameraCol);
                MediaPlayer.Play(music[0]);
            }

            public int loaded_ = 0;
            public bool doneLoading = false;

            /*Load Objects when it's Level 1*/
            public void LoadLevelOne(int loaded)
            {
                Vector3 tpos = new Vector3(36, 10, -336); Vector3 tscale = new Vector3(1);
                if (loaded == -1)//Load everything at once
                {
                    //space.ForceUpdater.Gravity = new Vector3(0, -20, 0);

                    /*SObjects*/
                    SObjects = new List<SelectableObject>(); 
                    
                    /*Models*/
                    terrain = Content.Load<Model>("Level1Terrain\\carthenge");
                    tank = Content.Load<Model>("Level1Terrain\\truck002");
                    solarPanel = new Models();
                    solarPanel.add(Content.Load<Model>("SolarPanel"));
                    solarPanel.add(Content.Load<Model>("SolarPanel_Legs"));

                    LoadStaticMesh(terrain, Content.Load<Texture2D>("Sand1"), "terrain");
                    LoadStaticMesh(Content.Load<Model>("Level1Terrain\\road"), Content.Load<Texture2D>("Level1Terrain\\Gravel"), "road");
                    LoadStaticMesh(tank, Content.Load<Texture2D>("Level1Terrain\\metal_seamless"), "tank");
                    LoadStaticMesh(Content.Load<Model>("Level1Terrain\\Truck"), Content.Load<Texture2D>("Level1Terrain\\metal_seamless"), "truck");
                    LoadStaticMesh(Content.Load<Model>("Level1Terrain\\power_poles"), Content.Load<Texture2D>("Level1Terrain\\Wood_tex_no_lines"), "power poles");
                    LoadStaticMesh(Content.Load<Model>("Level1Terrain\\power_lines"), Content.Load<Texture2D>("Level1Terrain\\Wire_Rough"), "power lines"); MakeLastEnergy(1);
                    LoadStaticMesh(House1, Content.Load<Texture2D>("Level1Terrain\\Wood_tex"), "house1");
                    

                    LoadStaticMesh(Content.Load<Model>("Level1Terrain\\oil_barrel"), Content.Load<Texture2D>("Level1Terrain\\rust"), "oilbarrel"); MakeLastEnergy(1);
                    SObjects[SObjects.Count-1].setup(new Vector3(200, 40, -196), 30, "Oil Barrels - 10 gallons ", "Press X to collect oil", 0);

                    LoadStaticMesh(solarPanel.get(0), Content.Load<Texture2D>("SolarPanel_tex"), "SolarPanel", new Vector3(318, 1, -78)); MakeLastEnergy(1);
                    SObjects[SObjects.Count - 1].setup(new Vector3(318, 0.7f, -70), 35, "Solar Panel", "", -1); SObjects[SObjects.Count - 1].voltage = 6.04f;

                    LoadStaticMesh(solarPanel.get(1), Content.Load<Texture2D>("Rust1"), "SolarPanel_Legs", new Vector3(318, 1, -78));
                    LoadStaticMesh(solarPanel.get(0), Content.Load<Texture2D>("SolarPanel_tex"), "SolarPanel", new Vector3(318, -2, -40)); MakeLastEnergy(1);
                    LoadStaticMesh(solarPanel.get(1), Content.Load<Texture2D>("Rust1"), "SolarPanel_Legs", new Vector3(318, -2, -40));
                   
                    LoadStaticMeshWithoutCollisionBox(Content.Load<Model>("voltmeter"), Content.Load<Texture2D>("VoltMeter_tex"), "voltmeter", new Vector3(318, 0.7f, -70)); MakeLastEnergy(0.6f);
                    SObjects[SObjects.Count-1].setup(new Vector3(318, 0.7f, -70), 15, "Volt Meter", "Press X to pick up", 1);

                    LoadStaticMeshWithoutCollisionBox(Content.Load<Model>("battery"), battery_tex, "Battery", new Vector3(-64, 26.7f, 16)); MakeLastEnergy(1);
                    SObjects[SObjects.Count-1].setup(new Vector3(-64, 26.7f, 16), 30, "Lithium Car Battery", "Press X to pick up", 2);
                    SObjects[SObjects.Count-1].voltage = 0.15f;
                    
                    LoadStaticMeshWithoutCollisionBox(Content.Load<Model>("LiAirBattery"), Content.Load<Texture2D>("Metal1"),"LiAirBattery", new Vector3(-64, 30f, 16)); MakeLastEnergy(1);
                    SObjects[SObjects.Count - 1].setup(new Vector3(-64, 30, 16), 30, "Lithium Air Battery", "Press X to pick up", 2);
                    SObjects[SObjects.Count - 1].voltage = 0.15f;
                    //LoadStaticMeshWithoutCollisionBox(Content.Load<Model>("battery"), battery_tex, "Battery", new Vector3(-64, 31, 16));
                }
                else if (loaded == 0)
                {
                    //space.ForceUpdater.Gravity = new Vector3(0, -20, 0);

                    /*SObjects*/
                    SObjects = new List<SelectableObject>();
                    loaded_ = 1;

                    /*Smoke Stack Monsters*/                 
                    LoadStaticMesh(Content.Load<Model>("smokemonster_staticbase"), Content.Load<Texture2D>("metal_1"), "testmonster", tpos);
                    SObjects[SObjects.Count - 1].setup(tpos, 100, "", "", -1);
                    SObjects[SObjects.Count - 1].comp.scale = tscale;
                    LoadStaticMesh(Content.Load<Model>("smokemonster_staticeye"), Content.Load<Texture2D>("eye_tex"), "testmonster_eye", tpos);
                    SObjects[SObjects.Count - 1].comp.scale = tscale;
                    SObjects[SObjects.Count - 1].setup(tpos, 100, "", "", -1);
                    LoadStaticMesh(Content.Load<Model>("smokemonster_statichead"), Content.Load<Texture2D>("metal_1"), "testmonster_head", tpos);
                    SObjects[SObjects.Count - 1].setup(tpos, 100, "", "", -1);
                    SObjects[SObjects.Count - 1].comp.scale = tscale;
                    MediaPlayer.Volume -= 0.2f;
                }
                else if (loaded == 1)
                {
                    /*Models*/
                    terrain = Content.Load<Model>("Level1Terrain\\carthenge");
                    tank = Content.Load<Model>("Level1Terrain\\truck002");
                    solarPanel = new Models();
                    solarPanel.add(Content.Load<Model>("SolarPanel"));
                    solarPanel.add(Content.Load<Model>("SolarPanel_Legs"));

                    LoadStaticMeshWithoutCollisionBox(Content.Load<Model>("LiAirBattery"), Content.Load<Texture2D>("Metal_1"), "LiAirBattery", new Vector3(273, -3, 71)); MakeLastEnergy(1);
                    SObjects[SObjects.Count - 1].setup(new Vector3(273, -3, 71), 30, "Lithium Air Battery", "Press X to pick up", 4);
                    SObjects[SObjects.Count - 1].voltage = 0.15f;
                    MediaPlayer.Volume -= 0.2f;
                    loaded_ = 2;
                }
                else if (loaded == 2)
                {
                    LoadStaticMesh(terrain, Content.Load<Texture2D>("Sand1"), "terrain");
                    LoadStaticMesh(Content.Load<Model>("Level1Terrain\\road"), Content.Load<Texture2D>("Level1Terrain\\Gravel"), "road");
                    LoadStaticMesh(tank, Content.Load<Texture2D>("Level1Terrain\\metal_seamless"), "tank");
                    LoadStaticMeshWithoutCollisionBox(DarkenSphere, Content.Load<Texture2D>("Dark_Sphere_tex"), "sphere", new Vector3(0, 50, 0));
                    SObjects[SObjects.Count - 1].comp.scale = new Vector3(3);
                    MediaPlayer.Volume -= 0.2f;
                    loaded_ = 3;
                }
                else if (loaded == 3)
                {
                    LoadStaticMesh(Content.Load<Model>("Level1Terrain\\Truck"), Content.Load<Texture2D>("Level1Terrain\\metal_seamless"), "truck");
                    LoadStaticMesh(Content.Load<Model>("Level1Terrain\\power_poles"), Content.Load<Texture2D>("Level1Terrain\\Wood_tex_no_lines"), "power poles");
                    LoadStaticMesh(Content.Load<Model>("Level1Terrain\\power_lines"), Content.Load<Texture2D>("Level1Terrain\\Wire_Rough"), "power lines"); MakeLastEnergy(1);
                    LoadStaticMesh(Content.Load<Model>("Level1Terrain\\house1"), Content.Load<Texture2D>("Level1Terrain\\Wood_tex"), "house1");
                    LoadStaticMesh(House1, Content.Load<Texture2D>("Level1Terrain\\Wood_tex"), "house2", new Vector3(0, 0, -100));
                    LoadStaticMesh(House1, Content.Load<Texture2D>("Level1Terrain\\Wood_tex"), "house3", new Vector3(10, 0, 100));
                    //LoadStaticMesh(Content.Load<Model>("Level1Terrain\\Jail_House1"), Content.Load<Texture2D>("Gravel"), "House_Jail1");
                    //SObjects[SObjects.Count - 1].comp.Transform.Translation -= new Vector3(0, 15, 0);                    
                   
                    //LoadStaticMesh(Content.Load<Model>("moon"), Content.Load<Texture2D>("the_moon"), "moon");
                    //LoadStaticMeshWithoutCollisionBox(Content.Load<Model>("Cloud1"), Content.Load<Texture2D>("Cloud_1"), "Cloud1", tpos);
                   // SObjects[SObjects.Count - 1].setup(tpos, 0, "", "", -1);
                    //SObjects[SObjects.Count - 1].technique = "__AlphaBlending__";
                   // SObjects[SObjects.Count - 1].comp.scale = new Vector3(5);
                    MediaPlayer.Volume -= 0.2f;
                    loaded_ = 4;
                }
                else if (loaded == 4)
                {
                    LoadStaticMesh(Content.Load<Model>("Level1Terrain\\oil_barrel"), Content.Load<Texture2D>("Level1Terrain\\rust"), "oilbarrel"); MakeLastEnergy(1);
                    SObjects[SObjects.Count - 1].setup(new Vector3(200, 40, -196), 30, "Oil Barrals - 10 gallons ", "Press X to collect oil", 0);

                    LoadStaticMesh(solarPanel.get(0), Content.Load<Texture2D>("SolarPanel_tex"), "SolarPanel", new Vector3(318, 1, -78)); MakeLastEnergy(1);
                    SObjects[SObjects.Count - 1].setup(new Vector3(318, 0.7f, -70), 35, "Solar Panel", "", -1); SObjects[SObjects.Count - 1].voltage = 6.04f;

                    LoadStaticMeshWithoutCollisionBox(Content.Load<Model>("plasma_gun"), Content.Load<Texture2D>("metal_1"), "plasma_gun", new Vector3(-14, 2, -155));
                    SObjects[SObjects.Count - 1].setup(new Vector3(-14, 2, -155), 10, "Plasma Gun", "Press X to pick up", 3);
                    MediaPlayer.Volume -= 0.2f;
                    loaded_ = 5;
                }
                else if (loaded == 5)
                {
                    LoadStaticMesh(solarPanel.get(1), Content.Load<Texture2D>("Rust1"), "SolarPanel_Legs", new Vector3(318, 1, -78));
                    LoadStaticMesh(solarPanel.get(0), Content.Load<Texture2D>("SolarPanel_tex"), "SolarPanel", new Vector3(318, -2, -40)); MakeLastEnergy(1);
                    LoadStaticMesh(solarPanel.get(1), Content.Load<Texture2D>("Rust1"), "SolarPanel_Legs", new Vector3(318, -2, -40));
                    LoadStaticMeshWithoutCollisionBox(Content.Load<Model>("voltmeter"), Content.Load<Texture2D>("VoltMeter_tex"), "voltmeter", new Vector3(318, 0.7f, -70)); MakeLastEnergy(0.6f);
                    SObjects[SObjects.Count - 1].setup(new Vector3(318, 0.7f, -70), 15, "Volt Meter", "Press X to pick up", 1);
                    MediaPlayer.Volume -= 0.2f;
                    loaded_ = 6;
                }
                else if (loaded == 6)
                {
                    LoadStaticMeshWithoutCollisionBox(Content.Load<Model>("battery"), battery_tex, "Battery", new Vector3(-64, 26.7f, 16)); MakeLastEnergy(1);
                    SObjects[SObjects.Count - 1].setup(new Vector3(-64, 26.7f, 16), 30, "Lithium Car Battery", "Press X to pick up", 2);
                    SObjects[SObjects.Count - 1].voltage = 0.35f;
                    //LoadStaticMeshWithoutCollisionBox(Content.Load<Model>("battery"), battery_tex, "Battery", new Vector3(-64, 31, 16));
                    loaded_ = 7;
                    startTime = DateTime.Now.Ticks / 10000;
                    MediaPlayer.Volume -= 0.2f;
                    doneLoading = true;
                }
            }

            /*This sets the last created component energy level used for angel vision*/
            public void MakeLastEnergy(float energy)
            {
                SObjects[SObjects.Count - 1].comp.energy = energy;
            }
            /*Load Mesh for Main Menu*/
            private void LoadStaticMeshMenu(Model model, Texture2D tex)
            {
                Vector3[] vertices;
                int[] indices;

                TriangleMesh.GetVerticesAndIndicesFromModel(model, out vertices, out indices);
                var mesh = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0, 0, 0)));
                
                StaticModel mod = new StaticModel(model, mesh.WorldTransform.Matrix, this);
                mod.tex = tex;
                MenuComps.Add(mod);
            }
            //This function is a way to get vertice collision, rather then box collision
            public void LoadStaticMesh(Model model, Texture2D tex, string id)
            {
                Vector3[] vertices;
                int[] indices;

                TriangleMesh.GetVerticesAndIndicesFromModel(model, out vertices, out indices);
                var mesh = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0, 0, 0)));
                
                StaticModel mod = new StaticModel(model, mesh.WorldTransform.Matrix, this);
                
                mod.tex = tex;
                mod.id = id;
                mesh.Tag = mod;
                
                SObjects.Add(new SelectableObject(mod));
                space.Add(mesh);                
            }


            public void LoadStaticMesh(Model model, Texture2D tex, string id, Vector3 pos)
            {
                Vector3[] vertices;
                int[] indices;

                TriangleMesh.GetVerticesAndIndicesFromModel(model, out vertices, out indices);
                StaticMesh mesh = new StaticMesh(vertices, indices, new AffineTransform(pos));                

                StaticModel mod = new StaticModel(model, mesh.WorldTransform.Matrix, this);
                mod.tex = tex;
                mod.id = id;
                mesh.Tag = mod;

                space.Add(mesh);
                SObjects.Add(new SelectableObject(mod));
            }
            
            /*Load Model without collision*/
            public void LoadStaticMeshWithoutCollisionBox(Model model, Texture2D tex, string id)
            {
                Vector3[] vertices;
                int[] indices;

                TriangleMesh.GetVerticesAndIndicesFromModel(model, out vertices, out indices);
                var mesh = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0)));   
                
                StaticModel mod = new StaticModel(model, mesh.WorldTransform.Matrix, this);
                mod.tex = tex;
                
                mod.id = id;
                SObjects.Add(new SelectableObject(mod));
            }

            private void LoadStaticMeshWithoutCollisionBox(Model model, Texture2D tex, string id, Vector3 pos)
            {
                Vector3[] vertices;
                int[] indices;

                TriangleMesh.GetVerticesAndIndicesFromModel(model, out vertices, out indices);
                var mesh = new StaticMesh(vertices, indices, new AffineTransform(pos));

                StaticModel mod = new StaticModel(model, mesh.WorldTransform.Matrix, this);
                mod.tex = tex;
                mod.id = id;
                SObjects.Add(new SelectableObject(mod));
            }
            
            //Doesn't work yet.. going to be used for shooting spheres
            /*private void ShootModel(Model model, Texture2D tex)
            {
                Box toAdd = new Box(Camera.Position, 1, 1, 1, 1);
                //Set the velocity of the new box to fly in the direction the camera is pointing.
                //Entities have a whole bunch of properties that can be read from and written to.
                //Try looking around in the entity's available properties to get an idea of what is available.
                toAdd.LinearVelocity = Camera.WorldMatrix.Forward * 100;
                //Add the new box to the simulation.
                space.Add(toAdd);

                Vector3[] vertices;
                int[] indices;

                TriangleMesh.GetVerticesAndIndicesFromModel(model, out vertices, out indices);
                var mesh = new StaticMesh(vertices, indices, new AffineTransform(new Vector3(0, 0, 0)));
                StaticModel mod = new StaticModel(model, mesh.WorldTransform.Matrix, this);
                mod.tex = tex;
                toAdd.Tag = mod;
                Comps.Add(mod);
            }*/

            protected override void UnloadContent(){}
        
        #endregion

        #region Update

            /*Various flags*/
            public bool flying_fast = false;

            public long startFlyTime;
            public long endFlyTime;
            public float blurFactor=0;


            bool pause_click = false; public int pause_level;

            protected override void Update(GameTime gameTime)
            {
                /*Get most recent input states*/
#if !XBOX
                KeyboardState = Keyboard.GetState();
                MouseState = Mouse.GetState();
#endif
                //Exit Game if necessary (F8 Button)
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.F8))
                    this.Exit();

                /*Camera Collision Updates*/
                var tempPos = cameraCol.Position;

                blahText = "";
                int v = 0;
                int q = 0;

                /*Initiate Pause Menu?*/
                #region pause Menu
                if ((GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start)||Keyboard.GetState().IsKeyDown(Keys.Escape))&&(level>2||level==-5))
                {
                    if (pause_click == false)
                    {
                        pause_click = true;
                        if (level == -5)
                        {
                            level = pause_level;
                        }
                        else
                        {
                            pause_level = (int)level;
                            level = -5;
                        }
                    }
                }
                else
                {
                    pause_click = false;
                }
                #endregion

                /*Reset THe Camera Position so that it starts at the start position, not the main_menu postiion*/
                if (level == 1)
                    lastPos = new Vector3(0, 250, 10);

                /*Main Flying Controls*/
                if (level>2)
                {
                    actions.update();
                    userObjects.update(this);

                    //Get x and y of thumbstick
                    float tbX = -GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X;
                    float tbY = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y;

                    /*Get Keyboard movement*/
                    if (Keyboard.GetState().IsKeyDown(Keys.A))
                        tbX += 1;

                    if (Keyboard.GetState().IsKeyDown(Keys.D))
                        tbX -= 1;

                    if (Keyboard.GetState().IsKeyDown(Keys.W))
                        tbY += 1;

                    if (Keyboard.GetState().IsKeyDown(Keys.S))
                        tbY -= 1;

                    float yvel = 0;
                    /*Vertical rise/drop? (Left/Right Shoulders)*/
                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftShoulder))
                        yvel += 30;
                    else if (Keyboard.GetState().IsKeyDown(Keys.Q) || Keyboard.GetState().IsKeyDown(Keys.Space))
                        yvel += 30;

                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.RightShoulder) || Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                        yvel -= 30;
                    else if (Keyboard.GetState().IsKeyDown(Keys.Z))
                        yvel -= 30;

                    

                    #region AngelVision

                    int RightMouseButton = 0;
#if !XBOX
 
                    if(MouseState.RightButton == ButtonState.Pressed)
                    {
                        RightMouseButton = 1;
                    }

#endif


                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.LeftTrigger) || RightMouseButton==1)
                    {                        
                        if (flying_fast == false)
                        {                           
                            startFlyTime = DateTime.Now.Ticks / 10000;
                        }
                        else
                        {
                            endFlyTime = DateTime.Now.Ticks / 10000;
                            blurFactor = (endFlyTime - startFlyTime) / 1000f;
                            if (blurFactor > 3)
                            {
                                blurFactor = 3;
                            }   
                        }
                        /*Boost Speed*/
                        tbX *= 4;
                        tbY *= 4;
                        flying_fast = true;
                    }
                    else
                    {
                        if (flying_fast == true)
                        {
                            /*Un-Blur to a clearer screen*/
                            startFlyTime = (long)(DateTime.Now.Ticks / 10000);           
                            if (blurFactor > 0.1)
                            {
                                blurFactor -= 0.02f * (blurFactor);
                            }
                            else
                            {
                                flying_fast = false;
                                blurFactor = 0;
                            }
                        }

                    }
                    #endregion
                    
                    cameraCol.LinearVelocity = Camera.WorldMatrix.Left * tbX * 30 + Camera.WorldMatrix.Up * yvel + Camera.WorldMatrix.Forward * tbY * 30;

                    /*Final Controls Updates*/

                    //Lets assume not everybodies controler is perfectly set a 0.. so see if the controler is within 0.1 of 0
                    if (Math.Abs(tbX) < 0.1f && Math.Abs(tbY) < 0.1f && yvel == 0)
                    {
                        cameraCol.LinearVelocity = new Vector3(0, 0, 0);
                        cameraCol.Position = new Vector3(lastPos.X, lastPos.Y, lastPos.Z);
                        tempPos = lastPos;
                    }

                    lastPos = tempPos;

                    cameraCol.Orientation = new Quaternion(new Vector3(0), 1);
                    Camera.Position = new Vector3(cameraCol.Position.X, cameraCol.Position.Y + 2, cameraCol.Position.Z);

                }
                /*Major Camera Updates*/
                float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
                Camera.Update(dt);

                /*Major Updates*/  
                space.Update();
               // animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
                base.Update(gameTime);
            }

        #endregion

        #region Draw

            Vector2 overlay_pos = new Vector2(0, 0);
            /*Maps that can be drawn to then given back to pixel shaders*/
            //Texture2D normalMap;
            Texture2D finalMap;
            //Texture2D bloomMap;
            Texture2D blurMap;
            //Texture2D finalBloomMap;

            public float TotalBlur = 1;

            protected override void Draw(GameTime gameTime)
            {

                //long nowTime = DateTime.Now.Ticks / 100;

                GraphicsDevice.RasterizerState = rs;

                GraphicsDevice.Clear(Color.CornflowerBlue);//Background Color 

                /*Default Drawing States.. if this wasn't here, we would have issues with spritebatch making things transparent*/
                GraphicsDevice.BlendState = BlendState.Opaque;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                /*Set Render Target*/
                graphics.GraphicsDevice.SetRenderTarget(finalTarget);
                graphics.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);//Clear buffer

                DrawScene("Simple", gameTime);//Draw everything!         

                graphics.GraphicsDevice.SetRenderTarget(null);//Re-set render target to null
                finalMap = (Texture2D)finalTarget;//Save final image to finalMap

                GraphicsDevice.Clear(Color.Black);//Clear Screen


                /*Post Processing Effects*/
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque,
                    SamplerState.LinearClamp, DepthStencilState.Default,
                    RasterizerState.CullNone, postEffect);

                    postEffect.Parameters["overlay_amount"].SetValue(overlay_amount);
                    postEffect.Parameters["overlay"].SetValue(overlay_tex);
                    postEffect.Parameters["bloodTexture"].SetValue(blood_overlay);
                    postEffect.Parameters["blood_level"].SetValue(1f-actions.Health);
                    overlay_pos += new Vector2(0, 0.001f);
                    postEffect.Parameters["overlay_pos"].SetValue(overlay_pos);

                    /*FinalDrawing*/
                    if (flying_fast)//If flying fast, we're going to want to blur the screen
                    {
                        postEffect.CurrentTechnique = postEffect.Techniques["Blur"];
                        postEffect.Parameters["blur_factor"].SetValue(blurFactor);
                    }
                    else
                        postEffect.CurrentTechnique = postEffect.Techniques["Plain"]; 

                    
                        postEffect.Parameters["blurAmount"].SetValue(blur);

                    graphics.GraphicsDevice.SetRenderTarget(blurTarget);
                    GraphicsDevice.Clear(Color.Black);//Clear Screen

                    spriteBatch.Draw(finalMap, new Rectangle(0, 0, (int)(screenWidth / (TotalBlur*0.9)), (int)(screenHeight / (TotalBlur*0.9))), Color.White);//Reduce scren resolution by a factor of TotalBlur

                    graphics.GraphicsDevice.SetRenderTarget(null);

                    blurMap = (Texture2D)blurTarget;

                    spriteBatch.Draw(finalMap, new Rectangle(0, 0, (int)(screenWidth), (int)(screenHeight)), Color.White);
                   

                  

                   // spriteBatch.Draw(normalMap, new Rectangle(0, 0, 200, 150), Color.Black);
                   // spriteBatch.Draw(blurMap, new Rectangle(0, 150, 200, 150), Color.Black);

                    //drawSpriteAnimations(gameTime);                

                    

                    CutScenes.draw();                    
                    actions.draw();

                    
                    userObjects.draw(spriteBatch, this);



                    //spriteBatch.DrawString(SegoeUI, Camera.Yaw.ToString(), new Vector2(0, 0), Color.White);

                   
                    /*for (int i = 0; i < SObjects.Count; i++)
                    {
                       
                        if (SObjects[i].comp.id == "testmonster_eye")
                        {
                            if (SObjects[i].inRange(cameraCol.Position))
                            {
                                spriteBatch.DrawString(SegoeUI, SObjects[i].comp.rot.Y.ToString(), new Vector2(0, 0), Color.White);
                            }
                        }
                    }*/
                    
                spriteBatch.End();              
                
                base.Draw(gameTime);

                //long oldTime = DateTime.Now.Ticks / 100;
                //dif_time = oldTime - nowTime;
            }

            float shift = 0;
            void DrawScene(String tech, GameTime gameTime)
            {
                effect.Parameters["Bump"].SetValue(bumpmap);
                effect.Parameters["useBump"].SetValue(true);
                effect.Parameters["cameraPos"].SetValue(Camera.Position);
                effect.Parameters["tex_shift"].SetValue(shift);
                shift += 0.001f;//Shifts 'angel vision' plasma clouds to make them look like their moving
                if (SObjects.Count > 14)
                {

                }
                if (level > 1||(int)level==-5)//!=-10
                {
                    for (int i = 0; i < SObjects.Count; i++)
                    {
                        effect.Parameters["Tint"].SetValue(SObjects[i].tint);
                        if (SObjects[i].technique != null)//Differnt Drawing Technique then the rest of the models?
                        {
                            if (SObjects[i].comp != null)
                            {
                                if (SObjects[i].technique == "__AlphaBlending__")
                                {
                                    if (SObjects[i].comp == null)
                                    {
                                        graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                                        SObjects[i].ecomp.Draw(gameTime, "Alpha");
                                        graphics.GraphicsDevice.BlendState = BlendState.Opaque;
                                    }
                                    else
                                    {
                                        graphics.GraphicsDevice.BlendState = BlendState.AlphaBlend;
                                        SObjects[i].comp.Draw(gameTime, "Alpha");
                                        graphics.GraphicsDevice.BlendState = BlendState.Opaque;

                                    }
                                }
                                else
                                {
                                    if (SObjects[i].comp == null)
                                    {
                                        SObjects[i].ecomp.Draw(gameTime, SObjects[i].technique);
                                    }
                                    else
                                        SObjects[i].comp.Draw(gameTime, SObjects[i].technique);

                                }
                            }
                        }
                        else
                        {
                            if (SObjects[i].comp != null)
                                SObjects[i].comp.Draw(gameTime, tech);
                        }
                    }
                        
                }
                if(level==-10)
                {
                    MenuComps[0].Draw(gameTime, tech);
                    MenuComps[1].Draw(gameTime, tech);
                }

                bool temp = bloom; bloom = false;

                bloom = temp;
                
            }

            float DistanceFromObject(Vector3 a, Vector3 b)
            {
                
                float xd = a.X - b.X;
                float yd = a.Y - b.Y;
                float zd = a.Z - b.Z;

                return (float)Math.Sqrt(xd * xd + yd * yd + zd * zd);
            }
        #endregion

            #region introAnimations(no longer used)

            public int objectSelection = 0;

            public long dif_time;
            public float level = -10;
            public long startTime = 0;

            public int pause_selection = 0;
            public int menu_selection = 0;

            public bool pause_move = false;
            public bool menu_move = false;
            public bool clicked_button = false;
            public int variabletochange = 0; 
            public int chapters = 0;
            public int startCamPos = 0; //For camera effects (By Bryan)

            public String pick_up_object = "";//This string will be drawn to let a user pickup something

            void drawSpriteAnimations(GameTime gameTime)
            {

                postEffect.CurrentTechnique = postEffect.Techniques["DepthOfField"];
                postEffect.Parameters["blurAmount"].SetValue(0);
                postEffect.Parameters["TintColor"].SetValue(new Vector3(255 / 255f, 255 / 255f, 255 / 255f));
                GraphicsDevice.BlendState = BlendState.AlphaBlend;

                objectSelection = 0;

                //int fps = (int)Math.Round(1.0 / gameTime.ElapsedGameTime.TotalSeconds); 
                //spriteBatch.DrawString(SegoeUI, cameraCol.Position.ToString(), new Vector2(100, 0), Color.White);
                //spriteBatch.DrawString(SegoeUI, fps.ToString(), new Vector2(100, 0), Color.White);

                actions.draw();

                //spriteBatch.DrawString(SegoeUI, cameraCol.Position.ToString(), new Vector2(0, 50), Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);

                //spriteBatch.DrawString(testFont, Camera.Yaw.ToString(), new Vector2(100, 100), Color.White, Camera.Yaw, new Vector2(0), 1, SpriteEffects.None, 0);
                //spriteBatch.Draw(MapArrow, new Rectangle(100, 100, 50, 50), null, Color.White, Camera.Yaw, new Vector2(50,50), SpriteEffects.None, 0); 

                if (level == -1) { startTime = DateTime.Now.Ticks / 10000; level = 0; }
                long nowTime = DateTime.Now.Ticks / 10000;

                #region MainMenu

                if (level == -10)//Main Menu
                {

                   // MediaPlayer.Stop(); 
                    flying_fast = false; songstart = false;//Some glitches that could occur

                    cameraCol.Position = new Vector3(0, 3, -5);

                    Camera.Position = cameraCol.Position;
                    Camera.Yaw = (float)Math.PI;
                    Camera.Pitch = -0.5f;

                    drawString("Leoni", new Vector2(100, 25), 1);

                    drawString("New Game", new Vector2(150, 200), 0.5f);
                    drawString("Load Game", new Vector2(150, 250), 0.5f);
                    drawString("Quit", new Vector2(150, 300), 0.5f);
                    drawString("->", new Vector2(100, 200 + (50 * menu_selection)), 0.5f);

                    bool thumbUp = false; bool thumbDown = false;
                    /*Check left or right thumbsticks if the user decides to use that*/
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.2f || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y > 0.2f)
                    {
                        thumbUp = true;
                    }
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.2f || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y < -0.2f)
                    {
                        thumbDown = true;
                    }
                    //DId the user go up or down?
                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.Up) || thumbDown || thumbUp)
                    {
                        if (menu_move == false)
                        {
                            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || Keyboard.GetState().IsKeyDown(Keys.Down) || thumbDown)
                                menu_selection++;
                            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || Keyboard.GetState().IsKeyDown(Keys.Up) || thumbUp)
                                menu_selection--;


                            if (menu_selection > 2)
                                menu_selection = 0;
                            else if (menu_selection < 0)
                                menu_selection = 2;

                        }
                        menu_move = true;
                    }
                    else
                        menu_move = false;

                    //If a menu selection was made
                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) || Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        if (clicked_button == false)
                        {
                            if (menu_selection == 0)//New Game
                            {
                                level = -1;
                                LoadLevelOne(-1);
                                clicked_button = true;
                            }
                            else if (menu_selection == 1)//Load Game
                            {

                            }
                            else if (menu_selection == 2)//Exit game
                                Exit();
                        }
                    }
                    else
                        clicked_button = false;
                }

                #endregion
                #region PauseScreen
                if (level == -5)//Pause Screen
                {
                    bool thumbUp = false; bool thumbDown = false;
                    /*Check left or right thumbsticks if the user decides to use that*/
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.2f || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y > 0.2f)
                    {
                        thumbUp = true;
                    }
                    if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.2f || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y < -0.2f)
                    {
                        thumbDown = true;
                    }
                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.Up) || thumbUp || thumbDown)
                    {
                        clicked_button = true;
                        if (pause_move == false)
                        {
                            if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadDown) || Keyboard.GetState().IsKeyDown(Keys.Down) || thumbDown)
                                pause_selection += 1;
                            else if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.DPadUp) || Keyboard.GetState().IsKeyDown(Keys.Up) || thumbUp)
                                pause_selection -= 1;

                            if (pause_selection > 1)
                                pause_selection = 0;
                            if (pause_selection < 0)
                                pause_selection = 1;

                        }
                        pause_move = true;
                    }
                    else
                        pause_move = false;

                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) || Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        if (pause_selection == 0)//Resume                       
                            level = pause_level;
                        if (pause_selection == 1)
                            level = -10;
                    }

                    postEffect.Parameters["use_ov_tint"].SetValue(false);
                    postEffect.Parameters["TintColor"].SetValue(new Vector3(1, 1, 1));

                    drawString("Paused", new Vector2(50, 50), 1);
                    drawString("Resume", new Vector2(75, 150), 0.5f);
                    drawString("Main Menu", new Vector2(75, 180), 0.5f);

                    drawString("->", new Vector2(25, 150 + (30 * pause_selection)), 0.5f);
                }
                #endregion

                #region Intro

                if (level == 0)
                {
                    cameraCol.Position = new Vector3(0, 250, 0);
                    Camera.Position = cameraCol.Position;
                    Camera.Pitch = 0; Camera.Yaw = 0;
                    if (!songstart)//Has the song not started yet?
                    {
                        seIntro = vIntro.CreateInstance();
                        seIntro.Play();//Start song               
                        songstart = true;//Confirm that the song has started
                    }

                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A) || (Keyboard.GetState().IsKeyDown(Keys.Enter)))
                    {
                        if (clicked_button == false)
                        {
                            level = 1;
                            seIntro.Stop();
                            startTime = DateTime.Now.Ticks / 10000;
                        }

                    }
                    else
                        clicked_button = false;
                    float alpha = nowTime - startTime;
                    #region Once they said this world was...

                    if (alpha < 7500)
                    {
                        if (alpha > 7000)
                        {
                            postEffect.Parameters["use_ov_tint"].SetValue(true);
                            postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 7000) / 500f);

                        }
                        if (alpha > 1300)
                        {
                            float curcolor = (alpha - 1300) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("Once they said this world was", new Vector2(20, 20), 0.5f);

                            curcolor = (alpha - 3200) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("beautiful . . .", new Vector2(490, 20), 1);
                        }

                        if (alpha > 4400)
                        {
                            float curcolor = (alpha - 4900) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString(". . . But I find that", new Vector2(75, 200), 0.5f);

                            curcolor = (alpha - 5600) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("hard", new Vector2(340, 200), 1);

                            curcolor = (alpha - 5900) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("to believe . . .", new Vector2(490, 230), 0.5f);
                        }
                    }
                    #endregion
                    #region These barren islands...

                    else if (alpha < 14500)
                    {
                        postEffect.Parameters["use_ov_tint"].SetValue(false);

                        if (alpha > 14000)
                        {
                            postEffect.Parameters["use_ov_tint"].SetValue(true);
                            postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 14000) / 500f);

                        }

                        if (alpha > 7000)
                        {
                            float curcolor = (alpha - 7500) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("These", new Vector2(20, 20), 0.5f);

                            curcolor = (alpha - 7800) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("barren", new Vector2(140, 20), 1f);

                            curcolor = (alpha - 8100) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("islands,", new Vector2(360, 50), 0.5f);

                            curcolor = (alpha - 9100) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("shattered parts of flowing green hills,", new Vector2(150, 125), 0.5f);
                        }
                        if (alpha > 11000)
                        {
                            float curcolor = (alpha - 11500) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("now lay in desolate", new Vector2(100, 200), 0.5f);

                            curcolor = (alpha - 12500) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("disrepair", new Vector2(410, 200), 1f);
                        }

                    }
                    #endregion
                    #region On Lofty wings soar birds...
                    else if (alpha < 31000)
                    {
                        postEffect.Parameters["use_ov_tint"].SetValue(false);

                        if (alpha > 30000)
                        {
                            postEffect.Parameters["use_ov_tint"].SetValue(true);
                            postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 30000) / 1000f);
                        }

                        if (alpha > 14500)
                        {
                            float curcolor = (alpha - 14500) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("On lofty wings soar birds in a sky, now", new Vector2(20, 20), 0.5f);

                            //curcolor = (alpha - 13500) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            //postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            //drawString("in a sky, ", new Vector2(120, 20), 0.5f);

                            curcolor = (alpha - 17000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("blackened,", new Vector2(310, 50), 1f);

                            curcolor = (alpha - 18000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("with the ashes of our mistakes", new Vector2(150, 125), 0.5f);

                            curcolor = (alpha - 21000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("their downy white wings", new Vector2(50, 170), 0.5f);

                            curcolor = (alpha - 22000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("tinged with the gray", new Vector2(430, 170), 0.5f);

                            curcolor = (alpha - 23100) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("smut", new Vector2(650, 200), 1f);

                            curcolor = (alpha - 25000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("The sun may still rise in the west,", new Vector2(50, 250), 0.5f);

                            curcolor = (alpha - 27500) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("but it feels like it set on this world", new Vector2(150, 300), 0.5f);

                            curcolor = (alpha - 29000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("a long time ago . . .", new Vector2(250, 350), 0.75f);
                        }

                    }
                    #endregion
                    #region For we are all...
                    else if (alpha < 38000)
                    {
                        postEffect.Parameters["use_ov_tint"].SetValue(false);

                        if (alpha > 37000)
                        {
                            postEffect.Parameters["use_ov_tint"].SetValue(true);
                            postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 37000) / 1000f);
                        }


                        float curcolor = (alpha - 31000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString(". . .  For we are all", new Vector2(50, 50), 0.5f);

                        curcolor = (alpha - 32000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("cloaked in this darkness", new Vector2(150, 100), 0.75f);

                        curcolor = (alpha - 35000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("Yet still we strive onwards", new Vector2(200, 250), 0.75f);



                    }
                    #endregion
                    #region A bleak almost meaningless existence
                    else if (alpha < 53000)
                    {
                        postEffect.Parameters["use_ov_tint"].SetValue(false);

                        if (alpha > 52000)
                        {
                            postEffect.Parameters["use_ov_tint"].SetValue(true);
                            postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 52000) / 1000f);
                        }

                        float curcolor = (alpha - 38000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("A bleak almost meaningless existence.", new Vector2(50, 50), 0.5f);

                        curcolor = (alpha - 40000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("There is no light at the end of the tunnel,", new Vector2(150, 100), 0.5f);

                        curcolor = (alpha - 42500) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("no silver lining.", new Vector2(450, 140), 0.5f);

                        curcolor = (alpha - 45000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("We live the lie of happiness", new Vector2(250, 220), 0.5f);

                        curcolor = (alpha - 48000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("fight these", new Vector2(50, 250), 0.5f);
                        drawString("smoke-stack monsters,", new Vector2(230, 250), 0.95f);

                        curcolor = (alpha - 51000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("till the silent soil.", new Vector2(450, 350), 0.65f);
                    }
                    #endregion
                    #region Hoping.  Praying...
                    else if (alpha < 54500)
                    {
                        postEffect.Parameters["use_ov_tint"].SetValue(false);

                        if (alpha > 54000)
                        {
                            postEffect.Parameters["use_ov_tint"].SetValue(true);
                            postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 54000) / 500f);
                        }

                        float curcolor = (alpha - 53000) / 500f; if (curcolor > 1.1f) curcolor = 1.1f;
                        postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("Hoping.", new Vector2(50, 50), 0.85f);
                    }
                    else if (alpha < 56000)
                    {
                        postEffect.Parameters["use_ov_tint"].SetValue(false);

                        if (alpha > 55500)
                        {
                            postEffect.Parameters["use_ov_tint"].SetValue(true);
                            postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 55500) / 500f);
                        }

                        float curcolor = (alpha - 54500) / 500f; if (curcolor > 1.1f) curcolor = 1.1f;
                        postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("Praying.", new Vector2(250, 150), 0.85f);
                    }


                    else if (alpha < 60000)
                    {
                        postEffect.Parameters["use_ov_tint"].SetValue(false);

                        if (alpha > 59000)
                        {
                            postEffect.Parameters["use_ov_tint"].SetValue(true);
                            postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 59000) / 1000f);
                        }

                        float curcolor = (alpha - 56000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("That from our efforts", new Vector2(150, 150), 0.55f);

                        curcolor = (alpha - 57000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        drawString("even a single life will blossom", new Vector2(300, 250), 0.55f);

                    }
                    #endregion
                    #region Even a solemn rose petal...
                    else if (alpha < 65000)
                    {
                        postEffect.Parameters["use_ov_tint"].SetValue(false);

                        if (alpha > 64000)
                        {
                            postEffect.Parameters["use_ov_tint"].SetValue(true);
                            postEffect.Parameters["ov_tint"].SetValue(1.1f - (alpha - 64000) / 1000f);
                        }

                        if (alpha > 60000)
                        {
                            float curcolor = (alpha - 60000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("Even a solemn rose petal", new Vector2(50, 50), 0.85f);

                            curcolor = (alpha - 62000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                            postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                            drawString("would lighten these dark hills", new Vector2(150, 200), 0.85f);
                        }
                    }
                    #endregion
                    else
                    {
                        startTime = DateTime.Now.Ticks / 10000;
                        level = 1;
                        seIntro.Stop();
                    }
                }


                #endregion
                #region Controls
                if (level == 1)
                {
                    postEffect.Parameters["use_ov_tint"].SetValue(false);
                    drawString("Controls", new Vector2(20, 20), 0.7f);
                    drawString("You can view the controls by pausing the game and selecting 'controls'", new Vector2(20, 70), 0.4f);

                    drawString("Left Thumbstick: Move Around", new Vector2(20, 150), 0.5f); drawString("ASDW Keys: Move Around", new Vector2(550, 150), 0.5f);
                    drawString("Right Thumbstick: Look Around", new Vector2(20, 180), 0.5f); drawString("Mouse: Look Around", new Vector2(550, 180), 0.5f);
                    drawString("Left Trigger: Boost", new Vector2(20, 210), 0.5f); drawString("Tab: Boost", new Vector2(550, 210), 0.5f);
                    drawString("Left/Right Shoulder: Rise/Drop", new Vector2(20, 270), 0.5f); drawString("Q/Z: Rise/Drop", new Vector2(550, 270), 0.5f);
                    drawString("Press B to continue", new Vector2(20, 450), 0.5f);

                    if (GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.B) || Keyboard.GetState().IsKeyDown(Keys.B))
                    {
                        level = 2;
                        startTime = DateTime.Now.Ticks / 10000;
                        MediaPlayer.Play(music[1]);
                        MediaPlayer.IsRepeating = true;
                    }
                }
                #endregion
                #region Part1.1
                if (level == 2)
                {
                    Mouse.SetPosition(200, 200);
                    float alpha = nowTime - startTime;
                    postEffect.Parameters["use_ov_tint"].SetValue(false);
                    if (alpha < 3000)
                    {
                        Vector2 text_size = SegoeUI.MeasureString("Part 1.1 - The Settling");
                        float curcolor = (alpha - 1000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                        drawString("Part 1.1 - The Settling", new Vector2(500 - (text_size.X / 2), 250 - (text_size.Y) / 2), 0.8f);
                        Camera.Pitch -= curcolor / 200;
                    }
                    else if (alpha < 4000)
                    {
                        Vector2 text_size = SegoeUI.MeasureString("Part 1.1 - The Settling");
                        float curcolor = 1 - (alpha - 3000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                        drawString("Part 1.1 - The Settling", new Vector2(500 - (text_size.X / 2), 250 - (text_size.Y) / 2), 0.8f);
                        Camera.Pitch -= curcolor / 150;
                    }
                    else
                        level = 3;

                    /*else if (alpha < 8000)
                    {
                        Vector2 text_size = SegoeUI.MeasureString("Part 1.1 - The Settling");
                        float curcolor = (alpha - 8000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                        drawString("New Objective: ", new Vector2(500 - (text_size.X / 2), 250 - (text_size.Y) / 2), 0.5f);
                    }
                    else if (alpha < 9000)
                    {
                        Vector2 text_size = SegoeUI.MeasureString("Part 1.1 - The Settling");
                        float curcolor = 1 - (alpha - 9000) / 1000f; if (curcolor > 1.1f) curcolor = 1.1f;
                        postEffect.Parameters["TintColor"].SetValue(new Vector3(curcolor, curcolor, curcolor));
                        postEffect.Parameters["pic_alpha"].SetValue(1 - curcolor);
                        drawString("New Objective: ", new Vector2(500 - (text_size.X / 2), 250 - (text_size.Y) / 2), 0.5f);
                    }*/
                }
                #endregion
                #region level3

                if (level == 3)
                {
                    /*Draw Health Bar*/
                    postEffect.Parameters["TintColor"].SetValue(new Vector3(1, 1, 1));
                    spriteBatch.Draw(healthBar, new Rectangle(20, 20, 26, 300), Color.White);
                    int temph = (int)(actions.Health * 202);

                    spriteBatch.Draw(healthBar_Green, new Rectangle(23, 239, 20, 20), Color.White);
                    spriteBatch.Draw(healthBar_Green, new Rectangle(43, 239 - temph, 20, 20), null, Color.White, (float)Math.PI, Vector2.Zero, SpriteEffects.None, 0);

                    spriteBatch.Draw(health_green, new Rectangle(26, 239 - temph, 14, temph), Color.White);

                }
                #endregion

            }


            public void drawString(String str, Vector2 Position, float scale)
            {
                spriteBatch.DrawString(SegoeUI, str, Position, Color.White, 0, new Vector2(0), scale, SpriteEffects.None, 0);
            }
            #endregion
    }


    public class SelectableObject
    {
        public Vector3 pos;
        public float range = -1;
        public string data = "";
        public string action = "";
        public int todo = -1;
        public StaticModel comp;
        public EntityModel ecomp;
        public float voltage = 0;//Voltage reeding
        public String technique = null;
        public Vector4 tint = new Vector4(1);

        public SelectableObject(Vector3 pos, float range, string top_data, string bottom_data, int todo, StaticModel comp)
        {
            this.pos = pos;
            this.range = range;
            this.data = top_data;
            this.action = bottom_data;
            this.todo = todo;
            this.comp = comp;
        }
        public SelectableObject(StaticModel comp)
        {
            this.comp = comp;
        }
        public SelectableObject(EntityModel ecomp)
        {
            this.ecomp = ecomp;
        }

        public bool inRange(Vector3 Position)
        {
            if (range == -1)
                return false;

            if (Vector3.Distance(Position, pos) < range)
            {
                return true;
            }

            return false;
        }

        public void setup(Vector3 pos, float range, string top_data, string bottom_data, int todo)
        {
            this.pos = pos;
            this.range = range;
            this.data = top_data;
            this.action = bottom_data;
            this.todo = todo;
        }

        public void resetData()
        {
            this.range = -1;
            this.data = "";
            this.action = "";
            this.todo = -1;
            this.comp.energy = 0;
        }
    }

    public class Models
    {
        public List<Model> models;

        public Models()
        {
            models = new List<Model>();
        }

        public void add(Model model)
        {
            models.Add(model);
        }

        public void add(Model[] model)
        {
            for(int i = 0; i < model.Length; i++)
            {
                models.Add(model[i]);
            }
           
        }

        public Model get(int i)
        {
            return models[i];
        }
    }

    public class Item
    {
        public Texture2D tex;
        public int actions;
        public bool flag;
        public double[] data = new double[10];
        public string tag;

        public Item(Texture2D tex, int actions, string tag)
        {
            this.tex = tex;
            this.actions = actions;
            this.tag = tag;
        }
    }

    public class Items
    {
        public List<Item> objects;
        public int selected = -1;       

        /*Constructors*/
        public Items()
        {
            objects = new List<Item>();
        }
        public Items(Item item)
        {
            selected = 0;
            objects = new List<Item>();
            objects.Add(item);
        }
        public Items(List<Item> items)
        {
            selected = 0;
            objects = items;
        }

        /*Functions*/
        public void draw(SpriteBatch spriteBatch, Game1 game)
        {
            (game as Game1).postEffect.CurrentTechnique = (game as Game1).postEffect.Techniques["AlphaBlending"];
            (game as Game1).GraphicsDevice.BlendState = BlendState.AlphaBlend;

            for (int i = 0; i < objects.Count; i++)
            {
                spriteBatch.Draw(objects[i].tex, new Rectangle(10+100*i, 400, 90, 50), Color.White);
            }

            if (selected != -1)
            {
                spriteBatch.Draw((game as Game1).object_pointer, new Rectangle(35 + 100 * selected, 500, 40, 40),null, Color.White,(float)(3*Math.PI/2), Vector2.Zero, SpriteEffects.None, 0);
                if (objects.Count != 0)
                {
                    if (objects[selected].tag == "Plasma")
                    {
                        spriteBatch.Draw((game as Game1).aim_cross, new Rectangle(400, 200, 100, 100), Color.White);
                    }
                }
            }
            else
            {
                if (objects.Count != 0)
                {
                    selected = 0;
                }
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed || Mouse.GetState().LeftButton == ButtonState.Pressed)
            {

                if ((game as Game1).userObjects.objects.Count != 0)//Make sure there is something in the inventory
                {
                    int actions = (game as Game1).userObjects.objects[(game as Game1).userObjects.selected].actions;

                    switch (actions)
                    {
                        case 1://VOLT METER   
                            float volt = 0.00f;
                            for (int i = 0; i < (game as Game1).SObjects.Count; i++)
                            {
                                if ((game as Game1).SObjects[i].inRange((game as Game1).cameraCol.Position))
                                {
                                    volt = (game as Game1).SObjects[i].voltage;
                                }
                            }
                            spriteBatch.DrawString((game as Game1).LED_REAL, "" + volt + " Volts", new Vector2(300, 300), Color.White);
                            break;
                        case 3:
                             Vector3 monsterPos = new Vector3(0);
                             Vector3 cameraPos = (game as Game1).Camera.Position;

                             int monster_id = -1;
                                
                             for (int i = 0; i < (game as Game1).SObjects.Count; i++)
                                {
                                    if ((game as Game1).SObjects[i].comp.id == "testmonster_head")
                                    {
                                        monsterPos = (game as Game1).SObjects[i].pos;
                                        monster_id = i;
                                    }
                                }
                             if (monster_id != -1)
                             {

                                 //spriteBatch.DrawString(SegoeUI, monsterPos.ToString(), new Vector2(0, 50), Color.White);
                                 Vector3 TestPos = monsterPos - cameraPos;
                                 Double Final = -1;

                                 Final = Math.Atan2(TestPos.Z, TestPos.X);
                                 Final += Math.PI / 2;
                                 if (Final > 3)
                                 {
                                     Final = Final - Math.PI * 2;
                                 }
                                 //spriteBatch.DrawString(SegoeUI, Final.ToString(), new Vector2(0, 100), Color.White);
                                 //spriteBatch.DrawString(SegoeUI, (Final+Camera.Yaw).ToString(), new Vector2(0, 150), Color.White);
                                 //spriteBatch.DrawString(SegoeUI, (Final+Camera.Yaw).ToString(), new Vector2(0, 150), Color.White);
                                 if (Math.Abs(Final + (game as Game1).Camera.Yaw) < 0.05f)
                                 {
                                     if ((game as Game1).SObjects[monster_id].inRange((game as Game1).Camera.Position))
                                     {
                                         //spriteBatch.DrawString((game as Game1).SegoeUI, "Target", new Vector2(0, 150), Color.White);
                                         (game as Game1).SObjects.RemoveAt(monster_id);
                                         (game as Game1).SObjects.RemoveAt(monster_id-1);
                                         (game as Game1).SObjects.RemoveAt(monster_id-2);                                         
                                     }
                                 }
                             }

                            break;
                        default:
                            break;
                    }
                }
                
            }
        
        }
        
        private bool keyhit = false;
        private bool ykeyhit = false;

        public void update(Game1 game)
        {
            //Add an object to the world
            if (GamePad.GetState(PlayerIndex.One).Buttons.Y == ButtonState.Pressed || Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                if (ykeyhit == false)
                {
                    if ((game as Game1).userObjects.objects.Count != 0)//Make sure there is something in the inventory
                    {
                        int actions = (game as Game1).userObjects.objects[(game as Game1).userObjects.selected].actions;

                        switch (actions)
                        {
                            case 0://BATTERY
                                (game as Game1).LoadStaticMeshWithoutCollisionBox((game as Game1).battery, (game as Game1).Content.Load<Texture2D>("Battery_tex"), "battery");
                                (game as Game1).SObjects[(game as Game1).SObjects.Count - 1].comp.Transform.Translation += (game as Game1).cameraCol.Position;//Reposition the object according to the position of the camera
                                (game as Game1).SObjects[(game as Game1).SObjects.Count - 1].pos = (game as Game1).cameraCol.Position;
                                (game as Game1).SObjects[(game as Game1).SObjects.Count - 1].setup((game as Game1).cameraCol.Position, 30, "Lithium Car Battery", "Press X to pick up", 2);
                                (game as Game1).MakeLastEnergy(0.5f);
                                (game as Game1).SObjects[(game as Game1).SObjects.Count - 1].voltage = (float)(game as Game1).userObjects.objects[(game as Game1).userObjects.selected].data[0];
                                (game as Game1).userObjects.objects.RemoveAt((game as Game1).userObjects.selected);

                                if ((game as Game1).userObjects.objects.Count > 0)
                                {
                                    (game as Game1).userObjects.selected = 0;
                                }
                                else
                                {
                                    (game as Game1).userObjects.selected = -1;
                                }

                                /*Sphere Around battery*/
                                //(game as Game1).LoadStaticMesh((game as Game1).GlowSphere, (game as Game1).Content.Load<Texture2D>("GlowSphere_white"), "glowsphere");
                                //(game as Game1).SObjects[(game as Game1).SObjects.Count - 1].comp.Transform.Translation += (game as Game1).cameraCol.Position;//Reposition the object according to the position of the camera
                            
                                break;
                            case 2://AIR BATTERY
                                (game as Game1).LoadStaticMeshWithoutCollisionBox((game as Game1).Content.Load<Model>("LiAirBattery"), (game as Game1).Content.Load<Texture2D>("Metal_1"), "LiAirBattery");
                                (game as Game1).SObjects[(game as Game1).SObjects.Count - 1].comp.Transform.Translation += (game as Game1).cameraCol.Position;//Reposition the object according to the position of the camera
                                (game as Game1).SObjects[(game as Game1).SObjects.Count - 1].pos = (game as Game1).cameraCol.Position;
                                (game as Game1).SObjects[(game as Game1).SObjects.Count - 1].setup((game as Game1).cameraCol.Position, 30, "Air Battery", "Press X to pick up", 4);
                                (game as Game1).MakeLastEnergy(0.5f);
                                (game as Game1).SObjects[(game as Game1).SObjects.Count - 1].voltage = (float)(game as Game1).userObjects.objects[(game as Game1).userObjects.selected].data[0];
                                (game as Game1).userObjects.objects.RemoveAt((game as Game1).userObjects.selected);

                                if ((game as Game1).userObjects.objects.Count > 0)
                                {
                                    (game as Game1).userObjects.selected = 0;
                                }
                                else
                                {
                                    (game as Game1).userObjects.selected = -1;
                                }
                               break;
                            default:
                                break;
                        }

                    }
                }
                else
                {

                }
                ykeyhit = true;
            }
            else
                ykeyhit = false;

            if ((GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Right))&& keyhit==false)
            {
                keyhit = true;
                if(selected!=-1)
                    selected++;

                if (selected+1 > objects.Count)
                {
                    selected = 0;
                }
            }
            else if ((GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Left))&& keyhit == false)
            {
                keyhit = true;
                if (selected != -1)
                {
                    selected--;

                    if (selected < 0)
                    {
                        selected = objects.Count-1;
                    }
                }
            }
            else if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Released && GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Released && Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right))
            {
                keyhit = false;
            }
        }
    }
}