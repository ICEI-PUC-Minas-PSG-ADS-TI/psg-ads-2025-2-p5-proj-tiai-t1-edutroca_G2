using System.Reflection;

namespace EduTroca.UseCases;
public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
