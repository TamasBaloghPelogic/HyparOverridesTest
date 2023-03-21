using Elements;
using Elements.Geometry;
using Elements.Geometry.Solids;
using System.Collections.Generic;

namespace OverrideTraining
{
      public static class OverrideTraining
    {
        /// <summary>
        /// Testing out overrides
        /// </summary>
        /// <param name="model">The input model.</param>
        /// <param name="input">The arguments to the execution.</param>
        /// <returns>A OverrideTrainingOutputs instance containing computed results and the model with any new elements.</returns>
        public static OverrideTrainingOutputs Execute(Dictionary<string, Model> inputModels, OverrideTrainingInputs input)
        {
            OverrideTrainingOutputs output = new OverrideTrainingOutputs();
						double height = 4.0;
            Polygon rectangle = Polygon.Rectangle(input.Length, input.Width);
            Representation representation = new Representation(
              new[] {
                new Extrude(rectangle, height, Vector3.ZAxis, false)
                });
            ServiceCore core = new ServiceCore(
              rectangle,
              elevation: 0,
              height,
              rectangle.Centroid(),
              new Transform(),
              BuiltInMaterials.Concrete,
              representation,
              false, Guid.NewGuid(), "Core");

            output.Model.AddElement(core);
            return output;
        }
      }
}