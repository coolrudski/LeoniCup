using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace LeoniV0_3
{
    /// <summary>
    /// Component that draws a model.
    /// </summary>
    public class StaticModel : DrawableGameComponent
    {
        public Model model;
        /// <summary>
        /// Base transformation to apply to the model.
        /// </summary>
        public Matrix Transform;
        Matrix[] boneTransforms;
        Effect effect;
        public Vector3 rot;
        public Vector3 scale = new Vector3(1);
        public Texture2D tex;
        public string id;
        public float energy=0f;
        Game game;
        /// <summary>
        /// Creates a new StaticModel.
        /// </summary>
        /// <param name="model">Graphical representation to use for the entity.</param>
        /// <param name="transform">Base transformation to apply to the model before moving to the entity.</param>
        /// <param name="game">Game to which this component will belong.</param>
        public StaticModel(Model model, Matrix transform, Game game)
            : base(game)
        {
            this.model = model;
            this.Transform = transform;
            this.game = game;
            effect = game.Content.Load<Effect>("Effects\\Effect");  
            
            //Collect any bone transformations in the model itself.
            //The default cube model doesn't have any, but this allows the StaticModel to work with more complicated shapes.
            boneTransforms = new Matrix[model.Bones.Count];
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                    part.Effect = effect;
            }
        }

        public void Draw(GameTime gameTime, string tech)
        {  
            model.CopyAbsoluteBoneTransformsTo(boneTransforms);
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {

                    effect.CurrentTechnique = effect.Techniques[tech];

                    effect.Parameters["World"].SetValue(boneTransforms[mesh.ParentBone.Index] * Matrix.CreateScale(scale) * Matrix.CreateFromYawPitchRoll(rot.Y, rot.X, rot.Z) * Transform);
                    effect.Parameters["View"].SetValue((Game as Game1).Camera.ViewMatrix);
                    effect.Parameters["Projection"].SetValue((Game as Game1).Camera.ProjectionMatrix);
                    effect.Parameters["Tex"].SetValue(tex);
                    effect.Parameters["energy"].SetValue(energy * (Game as Game1).blurFactor/3);
                    effect.Parameters["Plasma"].SetValue((game as Game1).plasma);
                    effect.Parameters["ViewVector"].SetValue((Game as Game1).Camera.ViewMatrix.Forward);

                    effect.Parameters["Colored"].SetValue((Game as Game1).colored);
                    effect.Parameters["mode"].SetValue((Game as Game1).mode);
                    effect.Parameters["bloom"].SetValue((Game as Game1).bloom);
                    effect.Parameters["bloomFactor"].SetValue((Game as Game1).bloomFactor);

                    effect.Parameters["contrast"].SetValue((game as Game1).contrast);
                    
                    
                }
                mesh.Draw();
            }          

            base.Draw(gameTime);
        }
    }
}
