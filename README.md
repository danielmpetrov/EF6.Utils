# EF6.Utils

[![Build status](https://ci.appveyor.com/api/projects/status/7kiwadlsw5ew8es2?svg=true)](https://ci.appveyor.com/project/danielmpetrov/ef6-utils)
[![GitHub](https://img.shields.io/github/license/danielmpetrov/ef6.utils)](https://github.com/danielmpetrov/ef6.utils/blob/master/LICENSE)
[![Feedback](https://img.shields.io/badge/feedback-welcome-orange)](https://github.com/danielmpetrov/EF6.Utils/issues)

A set of Entity Framework 6 extensions that reduce the boilerplate needed for common requirements.

## Features

### Timestamps

We often have the requirement to persist when records are created and modified. `EF6.Utils` provides an interface to add a `CreatedOn` and `UpdatedOn` fields to your entities. Inherit `UtilsDbContext` instead of `DbContext` in your application context to enable automatic timestamp updates whenever `SaveChanges` is called.

```csharp
public class Comment : ITimestampedEntity
{
    // ...
    public DateTime CreatedOn { get; set; }
    public DateTime UpdatedOn { get; set; }
}

public class AppDbContext : UtilsDbContext
{
    public DbSet<Comment> Comments { get; set; }
}

// ...

_context.Comments.Add(new Comment());

// for a new entity, CreatedOn and UpdatedOn will be the same
// for entities that have a modified state, UpdatedOn will be amended accordingly
_context.SaveChanges();
```

By default, the current time is being set through the `System.DateTime.Now` property. `EF6.Utils` provides an `EF6.Utils.Common.IClock` interface and constructor overloads so that `Now` can be anything you like.

```csharp
public class UtcClock : IClock
{
    public DateTime Now() => DateTime.UtcNow;
}

public class AppDbContext : UtilsDbContext
{
    public AppDbContext(IClock clock) : base(clock) { }

    public DbSet<Comment> Comments { get; set; }
}

using (var context = new AppDbContext(new UtcClock()))
{
    // with this context, timestamps will be in UTC
}
```

`EF6.Utils` also comes with extension methods for common timestamp-related queries.

```csharp
// query from the context
var latestCreated = _context.LatestCreated<Comment>();
var latestCreated = _context.LatestCreatedOrDefault<Comment>();
var latestUpdated = _context.LatestUpdated<Comment>();
var latestUpdated = _context.LatestUpdatedOrDefault<Comment>();

// query from the set
var latestCreated = _context.Comments.LatestCreated();
var latestCreated = _context.Comments.LatestCreatedOrDefault();
var latestUpdated = _context.Comments.LatestUpdated();
var latestUpdated = _context.Comments.LatestUpdatedOrDefault();
```

All methods have async counterparts.

```csharp
var latestCreated = await _context.LatestCreatedAsync<Comment>();
var latestCreated = await _context.LatestCreatedOrDefaultAsync<Comment>();
var latestUpdated = await _context.LatestUpdatedAsync<Comment>();
var latestUpdated = await _context.LatestUpdatedOrDefaultAsync<Comment>();

var latestCreated = await _context.Comments.LatestCreatedAsync();
var latestCreated = await _context.Comments.LatestCreatedOrDefaultAsync();
var latestUpdated = await _context.Comments.LatestUpdatedAsync();
var latestUpdated = await _context.Comments.LatestUpdatedOrDefaultAsync();
```

### Soft Delete

To enable soft delete functionality, implement the `ISoftDeletableEntity` interface on any entity, which requires a single `DeletedOn` property of type `Datetime?`. This property is set to `IClock.Now()` whenever an entity is marked for deletion, and the entity is updated instead of deleted.

```csharp
public class Comment : ISoftDeletableEntity
{
    // ...
    public DateTime? DeletedOn { get; set; }
}

public class AppDbContext : UtilsDbContext
{
    public DbSet<Comment> Comments { get; set; }
}

// ...
var comment = _context.Comments.First();
_context.Comments.Remove(comment);

// the comment will be updated, and DeletedOn will be set to IClock.Now()
_context.SaveChanges();
```

## Develop

> TODO
