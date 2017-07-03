namespace Krypton.LibProtocol.Target
{
    public abstract class TargetDefinable
    {
        /// <summary>
        /// Builds the definable from a parent context
        /// </summary>
        /// <param name="context"></param>
        public abstract void BuildFromContext(DefinitionContext context);

        /// <summary>
        /// Create context used by child definitions
        /// </summary>
        /// <returns></returns>
        public abstract DefinitionContext CreateChildContext();
    }
}
