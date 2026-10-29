# Clean GraphQL API

A `dotnet new` template for a GraphQL API built with a streamlined version of Clean Architecture and [Hot Chocolate](https://chillicream.com/docs/hotchocolate).

It's one of five templates, for Minimal APIs, Web API controllers, GraphQL, Model Context Protocol and AWS Lambda, that share the same Application and Infrastructure layers, tests and CI pipeline.

## Requirements

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/products/docker-desktop) (optional, to build and run the container image)

## Install

``` bash
dotnet new install CleanGraphQLApi.Template
```

## Create a solution

``` bash
dotnet new cleangraphqlapi --name {YOUR_SOLUTION_NAMESPACE} --authorName "{YOUR_NAME}"
```

| Option | Description |
| --- | --- |
| `--name` | The name of the solution, also used as the root namespace. |
| `--authorName` | The name of the author of the solution. |
| `--authority` | The OAuth authorization server that issues access tokens. Leave it empty to run the API without authentication. |
| `--audience` | The audience access tokens must be issued for, usually the API's own URL. |

With an authority set, the movie, author and review queries and the review mutations need a JWT bearer token, and the `version` query, the schema and Nitro stay open.

## Run it

From the new solution's folder, start the app and a Seq container for logs.

``` bash
docker compose up -d
```

Then open Nitro, the GraphQL IDE, at <http://localhost:8080/graphql>. Seq shows the logs at <http://localhost:5341>.

Or run it without Docker.

``` bash
dotnet run --project ./src/Presentation
```

Run the tests with `dotnet test`. Every way to run the solution is in `docs/RUN.md` inside it.

## What's inside

- **Presentation**: A [Hot Chocolate](https://chillicream.com/docs/hotchocolate) GraphQL schema with Relay paging, DataLoaders and the [Nitro](https://chillicream.com/docs/nitro) IDE
- **Application**: commands and queries through [Mediator](https://github.com/martinothamar/Mediator), validation with [FluentValidation](https://github.com/FluentValidation/FluentValidation) and mapping with [Mapperly](https://mapperly.riok.app)
- **Infrastructure**: [Entity Framework Core](https://learn.microsoft.com/ef/core) on SQLite with migrations and optional sample data
- **Tests**: [xUnit](https://xunit.net), [Shouldly](https://github.com/shouldly/shouldly) and [NSubstitute](https://github.com/nsubstitute/NSubstitute), each test on its own in-memory database
- A Dockerfile that runs as a non-root user, a `docker-compose.yml`, and a GitHub Actions pipeline with tests, a coverage gate, a container scan and CodeQL

## Updates

A new version is released on the 1st of each month with every change merged since the previous release, as long as something has changed. To update an installed template:

``` bash
dotnet new update
```

What changed in each version is in the [release notes](https://github.com/stphnwlsh/CleanGraphQLApi/releases). To remove the template, run `dotnet new uninstall CleanGraphQLApi.Template`.

## More help

- [README on GitHub](https://github.com/stphnwlsh/CleanGraphQLApi#readme) for the architecture, every feature and the Docker build options
- [Running guide](https://github.com/stphnwlsh/CleanGraphQLApi/blob/main/docs/RUN.md) for every way to run, configure and test the solution
- [GitHub issues](https://github.com/stphnwlsh/CleanGraphQLApi/issues) for bugs and ideas
- [Security policy](https://github.com/stphnwlsh/CleanGraphQLApi/security/policy) to report a vulnerability privately

## Support

If this template saves you time, please consider [sponsoring me on GitHub](https://github.com/sponsors/stphnwlsh).
