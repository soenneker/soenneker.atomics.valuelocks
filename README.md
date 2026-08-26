[![](https://img.shields.io/nuget/v/soenneker.atomics.valuelocks.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.atomics.valuelocks/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.atomics.valuelocks/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.atomics.valuelocks/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.atomics.valuelocks.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.atomics.valuelocks/)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Atomics.ValueLocks
### Lightweight value-type synchronization with atomically published lock storage.

## Installation

```
dotnet add package Soenneker.Atomics.ValueLocks
```

## Usage

```csharp
using Soenneker.Atomics.ValueLocks;

public sealed class Cache
{
    private ValueAtomicLock _sync;

    public void Update()
    {
        lock (_sync.Get())
        {
            // Protected work
        }
    }
}
```

`ValueAtomicLock` occupies one reference-sized field and creates its `System.Threading.Lock` only on first use. It is a mutable struct intended to remain a private field; copying an uninitialized instance can create independent lock domains.
