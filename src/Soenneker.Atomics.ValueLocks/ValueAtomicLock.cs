using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Soenneker.Atomics.ValueLocks;

/// <summary>
/// Provides inline storage for a lazily created, atomically published <see cref="Lock"/>.
/// </summary>
/// <remarks>
/// <para>
/// The default value is ready to use and does not allocate until <see cref="Get"/> is first called.
/// Concurrent callers may create temporary candidates, but every caller receives the single published lock.
/// </para>
/// <para>
/// This is a mutable <see langword="struct"/> intended for use as a private field. Avoid copying it before
/// initialization because each copy can publish a different lock and therefore establish a different lock domain.
/// </para>
/// </remarks>
[DebuggerDisplay("IsValueCreated = {IsValueCreated}")]
public struct ValueAtomicLock
{
    private Lock? _value;

    /// <summary>
    /// Gets a value indicating whether the lock has been created.
    /// </summary>
    public bool IsValueCreated
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Volatile.Read(ref _value) is not null;
    }

    /// <summary>
    /// Gets the single published lock, creating it if necessary.
    /// </summary>
    public Lock Value
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Get();
    }

    /// <summary>
    /// Gets the single published lock, creating and atomically publishing it when uninitialized.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Lock Get()
    {
        Lock? value = Volatile.Read(ref _value);
        if (value is not null)
            return value;

        var created = new Lock();
        return Interlocked.CompareExchange(ref _value, created, null) ?? created;
    }
}
