using Core.Querying.ExpressionBuilder.Models.Request;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace Core.Querying.ExpressionBuilder.FacetRegistry
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class FacetRegistryInitializationModule : IInitializableModule
    {
        public void Initialize(InitializationEngine context)
        {
            IFacetRegistry registry = ServiceLocator.Current.GetInstance<IFacetRegistry>();

            registry.Facets.Add(new FacetItem { PropertyId = "AvailableColors" });
            //registry.Facets.Add(new FacetItem { Field = "SockType" });
            //registry.Facets.Add(new FacetItem { Field = "Pairs" });
            //registry.Facets.Add(new FacetItem { Field = "Team" });
            //registry.Facets.Add(new FacetItem { Field = "ApparelFit" });
            //registry.Facets.Add(new FacetItem { Field = "BraSupports" });
            //registry.Facets.Add(new FacetItem { Field = "ApparelLength" });
            //registry.Facets.Add(new FacetItem { Field = "SleeveLength" });
            //registry.Facets.Add(new FacetItem { Field = "HoodyStyle" });
            //registry.Facets.Add(new FacetItem { Field = "JacketStyle" });
            //registry.Facets.Add(new FacetItem { Field = "Competition" });
            //registry.Facets.Add(new FacetItem { Field = "Capacity", DataType = GlobalConstants.FacetDataTypes.Number });
            //registry.Facets.Add(new FacetItem { Field = "Wheeled", DataType = GlobalConstants.FacetDataTypes.Boolean });
            //registry.Facets.Add(new FacetItem { Field = "LaptopSleeve", DataType = GlobalConstants.FacetDataTypes.Boolean });
            //registry.Facets.Add(new FacetItem { Field = "BallSize" });
            //registry.Facets.Add(new FacetItem { Field = "KickingTeeHeight" });
            //registry.Facets.Add(new FacetItem { Field = "Colour" });
            //registry.Facets.Add(new FacetItem { Field = "Material" });
            //registry.Facets.Add(new FacetItem { Field = "Fragrance" });
            //registry.Facets.Add(new FacetItem { Field = "FrameSize" });
            //registry.Facets.Add(new FacetItem { Field = "PartyStyle" });
            //registry.Facets.Add(new FacetItem { Field = "PartyColour" });
            //registry.Facets.Add(new FacetItem { Field = "SpecificUse" });
            //registry.Facets.Add(new FacetItem { Field = "BaggedBagless" });
            //registry.Facets.Add(new FacetItem { Field = "Resistance" });
            //registry.Facets.Add(new FacetItem { Field = "Surface" });
            //registry.Facets.Add(new FacetItem { Field = "Pronation" });
            //registry.Facets.Add(new FacetItem { Field = "SheetType" });
            //registry.Facets.Add(new FacetItem { Field = "GloveHand" });
            //registry.Facets.Add(new FacetItem { Field = "BallColour" });
            //registry.Facets.Add(new FacetItem { Field = "Gender" });
            //registry.Facets.Add(new FacetItem { Field = "Sport" });
            //registry.Facets.Add(new FacetItem { Field = "Room"});
            //registry.Facets.Add(new FacetItem { Field = "Brand" });
            //registry.Facets.Add(new FacetItem { Field = "Colours" });
            //registry.Facets.Add(new FacetItem { Field = "Sizes" });
            //registry.Facets.Add(new FacetItem { Field = FacetBuilderExtensions.PreDefineFacetField.COLOUR_NAME });
            //registry.Facets.Add(new FacetItem { Field = FacetBuilderExtensions.PreDefineFacetField.SIZE_DESCRIPTION });
            //registry.Facets.Add(new NumericFacetItem { Field = FacetBuilderExtensions.PreDefineFacetField.PRICE });
        }

        public void Uninitialize(InitializationEngine context)
        {
            //Add uninitialization logic
        }
    }
}