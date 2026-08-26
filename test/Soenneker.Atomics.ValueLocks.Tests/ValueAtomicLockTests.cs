using AwesomeAssertions;
using Soenneker.Tests.Unit;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Atomics.ValueLocks.Tests;

public sealed class ValueAtomicLockTests : UnitTest
{
    [Test]
    public void Default_should_not_allocate_a_lock()
    {
        var valueLock = new ValueAtomicLock();

        valueLock.IsValueCreated.Should().BeFalse();
        Unsafe.SizeOf<ValueAtomicLock>().Should().Be(IntPtr.Size);
    }

    [Test]
    public void Get_should_publish_one_lock()
    {
        var valueLock = new ValueAtomicLock();

        Lock first = valueLock.Get();
        Lock second = valueLock.Get();

        valueLock.IsValueCreated.Should().BeTrue();
        (first == second).Should().BeTrue();
        (first == valueLock.Value).Should().BeTrue();
    }

    [Test]
    public void Concurrent_get_should_return_the_same_lock()
    {
        var holder = new Holder();
        var values = new Lock[128];

        Parallel.For(0, values.Length, i => values[i] = holder.Sync.Get());

        for (var i = 1; i < values.Length; i++)
            (values[0] == values[i]).Should().BeTrue();
    }

    [Test]
    public void Published_lock_should_protect_shared_state()
    {
        var holder = new Holder();
        var count = 0;

        Parallel.For(0, 10_000, _ =>
        {
            lock (holder.Sync.Get())
            {
                count++;
            }
        });

        count.Should().Be(10_000);
    }

    private sealed class Holder
    {
        public ValueAtomicLock Sync;
    }
}
