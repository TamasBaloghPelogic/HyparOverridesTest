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
            Transform translate = new Transform();
            translate = translate.Moved(10,10,0);
            Polygon offsetRectangle = rectangle.TransformedPolygon(translate);
            Representation representation = new Representation(
              new[] {
                new Extrude(rectangle, height, Vector3.ZAxis, false)
                });

            Representation tRepr = new Representation(
              new[] {
                new Extrude(offsetRectangle, height, Vector3.ZAxis, false)
                });

            // 1. Create an intermediate collection to store any 
            // elements that might be overridden
            List<ServiceCore> allCores = new List<ServiceCore>();

            ServiceCore core = new ServiceCore(
              rectangle,
              elevation: 0,
              height,
              rectangle.Centroid(),
              new Transform(),
              BuiltInMaterials.Concrete,
              representation,
              false, Guid.NewGuid(), "Core");

            ServiceCore tCore = new ServiceCore(
              offsetRectangle,
              elevation: 0,
              height,
              offsetRectangle.Centroid(),
              new Transform(),
              BuiltInMaterials.Concrete,
              tRepr,
              false, Guid.NewGuid(), "Core 2");

            // 2. Add core to the intermediate collection
            allCores.Add(core);
            allCores.Add(tCore);

            // 3. Check if there are any overrides
            if (input.Overrides != null && input.Overrides.OverrideCores != null)
            {
 								// 4. Loop over the override collection
                foreach (OverrideCoresOverride coreOverride in input.Overrides.OverrideCores)
                {
                    // 5. find the matching element, based on identity
                    OverrideCoresIdentity identity = coreOverride.Identity;
                    // find the core whose Centroid is closest to our Identity
                    ServiceCore? matchingCore = allCores.OrderBy(
                      c => c.Centroid.DistanceTo(identity.Centroid))
                      .FirstOrDefault();
                    // 6. apply relevant changes
                    // note that the "Profile" property coming back is not a 
										// "Profile" element — it's a special "CoresProfile" class
                    // that only contains a subset of the properties of 
									  // a Profile.
                    matchingCore.Profile = new Profile(
												coreOverride.Value.Profile.Perimeter);
		                matchingCore.Representation = new Representation(
												new[] { new Extrude(
													matchingCore.Profile, height, Vector3.ZAxis, false) });
										// 7. Add Override Identity to the modified element.
										Identity.AddOverrideIdentity(
											matchingCore, 
											"Cores", 
											coreOverride.Id, 
											coreOverride.Identity);
								}

                bool weHaveCustomColors = false;
                foreach (ReprColorOverride colorOverride in input.Overrides.ReprColor)
                {
                    ReprColorIdentity identity = colorOverride.Identity;
                    ServiceCore? matchingCore = allCores.OrderBy(
                      c => c.Centroid.DistanceTo(identity.Centroid))
                      .FirstOrDefault();

                    Material newMaterial = colorOverride.Value.ObjectMaterial.ObjectColor;
                    matchingCore.Material = newMaterial;

                    Identity.AddOverrideIdentity(
                      matchingCore,
                      "CoresColored",
                      colorOverride.Id,
                      colorOverride.Identity);

                    weHaveCustomColors = true;
                }

                foreach (ReprColorGroupOverride grColorOverride in input.Overrides.ReprColorGroup)
                {
                    if (weHaveCustomColors)
                      continue;

                    IList<ReprColorGroupOverrideIdentities> identities = grColorOverride.Identities;
                    foreach(ReprColorGroupOverrideIdentities grIdentity in identities)
                    {
                      ServiceCore? matchingCore = allCores.OrderBy(
                        c => c.Centroid.DistanceTo(grIdentity.Centroid))
                        .FirstOrDefault();

                      matchingCore.Material = new Material(); 
                      matchingCore.Material.Color = new Color("Goldenrod");

                    Identity.AddOverrideIdentity(
                      matchingCore,
                      "CoresColoredSameWay",
                      grColorOverride.Id,
                      grColorOverride.GetIdentity());
                    }
                }
            }

            output.Model.AddElements(allCores);
            return output;
        }
      }
}