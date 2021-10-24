using System.Collections.Generic;

namespace RecomendationForStartups
{
    public interface IHaveVersions
    {
        IEnumerable<SemanticVersioning.Version> Versions { get; }
    }
}
