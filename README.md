# EF6.Utils

A set of Entity Framework 6 extensions that reduce the boilerplate needed for common requirements.

## Features

### Timestamps

We often have the requirement to persist when records are created and modified. EF6.Utils provides an interface to add a `CreatedOn` and `UpdatedOn` fields to your entities, a `DbContext` extension method to save changes with timestamps, and extension methods for common queries against timestamps.

## Develop
