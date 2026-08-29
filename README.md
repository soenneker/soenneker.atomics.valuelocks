[![](https://img.shields.io/nuget/v/soenneker.atomics.valuelocks.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.atomics.valuelocks/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.atomics.valuelocks/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.atomics.valuelocks/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.atomics.valuelocks.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.atomics.valuelocks/)

# Soenneker.Atomics.ValueLocks

Provides inline storage for a lazily created, atomically published `Lock`.

## Install

```bash
dotnet add package Soenneker.Atomics.ValueLocks
```

## What you get

- `ValueAtomicLock` — Provides inline storage for a lazily created, atomically published `Lock`.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `ValueAtomicLock.IsValueCreated` | Gets a value indicating whether the lock has been created. | Gets a value indicating whether the lock has been created. |
| `ValueAtomicLock.Value` | Gets the single published lock, creating it if necessary. | Gets the single published lock, creating it if necessary. |
| `ValueAtomicLock.Get()` | Gets the single published lock, creating and atomically publishing it when uninitialized. | The requested lock. |

## Important behavior

- `ValueAtomicLock`: The default value is ready to use and does not allocate until `Get` is first called. Concurrent callers may create temporary candidates, but every caller receives the single published lock. This is a mutable `struct` intended for use as a private field. Avoid copying it before initialization because each copy can publish a different lock and therefore establish a different lock domain.
