using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BEPUphysics.Entities;

namespace LeoniV0_3
{
    /// <summary>
    /// Component that draws a model following the position and orientation of a BEPUphysics entity.
    /// </summary>
    public class EntityModel : DrawableGameComponent
    {
        Entity entity;
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
        public float energy = 0f;
        Game game;
     
        public EntityModel(Entity entity, Model model, Matrix transform, Game game)
            : base(game)
        {
            this.entity = entity;
            this.model = model;
            this.Transform = transform;

            //Collect any bone transformations in the model itself.
            //The default cube model doesn't have any, but this allows the EntityModel to work with more complicated shapes.
            boneTransforms = new Matrix[model.Bones.Count];
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                }
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

                    effect.Parameters["World"].SetValue(boneTransforms[mesh.ParentBone.Index] * Transform);
                    effect.Parameters["View"].SetValue((Game as Game1).Camera.ViewMatrix);
                    effect.Parameters["Projection"].SetValue((Game as Game1).Camera.ProjectionMatrix);
                    effect.Parameters["Tex"].SetValue(tex);
                    effect.Parameters["energy"].SetValue(energy * (Game as Game1).blurFactor / 3);
                    effect.Parameters["Plasma"].SetValue((Game as Game1).plasma);
                    effect.Parameters["ViewVector"].SetValue((Game as Game1).Camera.ViewMatrix.Forward);

                    effect.Parameters["Colored"].SetValue((Game as Game1).colored);
                    effect.Parameters["mode"].SetValue((Game as Game1).mode);
                    effect.Parameters["bloom"].SetValue((Game as Game1).bloom);
                    effect.Parameters["bloomFactor"].SetValue((Game as Game1).bloomFactor);


                }
                mesh.Draw();
            }

            base.Draw(gameTime);
        }
    }
}
