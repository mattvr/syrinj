using System;

namespace Syrinj.Provision
{
    public interface IProvider
    {
        Type Type { get; set; }
        string Tag { get; set; }

        object Get();
    }
}
