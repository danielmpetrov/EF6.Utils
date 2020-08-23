# EF6.Utils

A set of Entity Framework 6 extensions that reduce the boilerplate needed for common requirements.

## Features

### Timestamps

We often have the requirement to persist when records are created and modified. `EF6.Utils` provides an interface to add a `CreatedOn` and `UpdatedOn` fields to your entities and a way to save with timestamps.

```csharp
public class Comment : ITimestampedEntity
{
    // ...
    public DateTime CreatedOn { get; set; } // do not set manually
    public DateTime UpdatedOn { get; set; } // do not set manually
}

// ...

_context.Comments.Add(new Comment());

// for a new entity, CreatedOn and UpdatedOn will be the same
// for entities that have a modified state, UpdatedOn will be amended accordingly
_context.SaveChangesTimestamped();
```

`EF6.Utils` also comes with extension methods for common timestamp-related queries.

```csharp
// query from the context
var latestComment = _context.LatestCreated<Comment>(); // throws on empty set
var latestComment = _context.LatestCreatedOrDefault<Comment>(); // returns null on empty set

// query from the set
var latestComment = _context.Comments.LatestCreated(); // throws on empty set
var latestComment = _context.Comments.LatestCreatedOrDefault(); // returns null on empty set
```

## Develop

> TODO
