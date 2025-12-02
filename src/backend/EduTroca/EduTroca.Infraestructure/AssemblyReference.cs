using System.Reflection;

namespace EduTroca.Infraestructure;
public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
