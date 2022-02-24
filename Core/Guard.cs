using System.Runtime.CompilerServices;

namespace Core;
public static class Guard
{
    public static T NotNull<T>(this T? o, [CallerArgumentExpression("o")] string? argName = null)
    {
        if (o is null) throw new ArgumentNullException(argName);
        return o;
    }
}
