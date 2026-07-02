namespace AnkleBreaker.Core.MasterClasses
{
    /// <summary>
    /// Base class for networked Controllers (the "body" of the Manager/HandlerData/Controller
    /// triad). A Controller never subscribes to the event bus: registration is sealed empty,
    /// so overriding it is a compile error. It keeps the full network services of
    /// AnkleBreakerNetworkBehaviour (SyncVars, reset, owner/connection hooks); it is driven by
    /// its Manager through a direct intra-feature reference and pushes intent via the feature
    /// HandlerData Request helpers.
    /// </summary>
    public abstract class AnkleBreakerNetworkController : AnkleBreakerNetworkBehaviour
    {
        protected sealed override void EventHandlerRegister() { }
        protected sealed override void EventHandlerUnRegister() { }
    }
}
