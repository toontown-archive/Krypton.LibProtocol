namespace Krypton.LibProtocol.Member.Statement
{
    public interface IStatement
    {
        /// <summary>
        /// The statement's alias. Used as a helper in templates
        /// </summary>
        string StatementAlias { get; }
    }
}
