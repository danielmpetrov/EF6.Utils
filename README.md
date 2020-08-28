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
await _context.SaveChangesTimestampedAsync();

var latestCreated = await _context.LatestCreatedAsync<Comment>();
var latestCreated = await _context.LatestCreatedOrDefaultAsync<Comment>();
var latestUpdated = await _context.LatestUpdatedAsync<Comment>();
var latestUpdated = await _context.LatestUpdatedOrDefaultAsync<Comment>();

var latestCreated = await _context.Comments.LatestCreatedAsync();
var latestCreated = await _context.Comments.LatestCreatedOrDefaultAsync();
var latestUpdated = await _context.Comments.LatestUpdatedAsync();
var latestUpdated = await _context.Comments.LatestUpdatedOrDefaultAsync();
```

## Develop

> TODO
