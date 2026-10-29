# Clean GraphQL API

[![Build Pipeline](https://img.shields.io/github/actions/workflow/status/stphnwlsh/CleanGraphQLApi/build-pipeline.yml?branch=main&label=Build%20Pipeline&logo=github&style=for-the-badge)](https://github.com/stphnwlsh/CleanGraphQLApi/actions/workflows/build-pipeline.yml)
[![Code Coverage](https://img.shields.io/codecov/c/github/stphnwlsh/CleanGraphQLApi?label=Code%20Coverage&logo=codecov&logoColor=white&style=for-the-badge)](https://codecov.io/gh/stphnwlsh/CleanGraphQLApi)
[![OpenSSF Scorecard](https://img.shields.io/ossf-scorecard/github.com/stphnwlsh/CleanGraphQLApi?label=OpenSSF%20Scorecard&style=for-the-badge)](https://scorecard.dev/viewer/?uri=github.com/stphnwlsh/CleanGraphQLApi)
[![NuGet](https://img.shields.io/nuget/v/CleanGraphQLApi.Template?label=NuGet&logo=nuget&logoColor=white&style=for-the-badge)](https://www.nuget.org/packages/CleanGraphQLApi.Template)
[![NuGet Downloads](https://img.shields.io/nuget/dt/CleanGraphQLApi.Template?label=Downloads&logo=nuget&logoColor=white&style=for-the-badge)](https://www.nuget.org/packages/CleanGraphQLApi.Template)
[![License](https://img.shields.io/github/license/stphnwlsh/CleanGraphQLApi?label=License&style=for-the-badge)](LICENSE.md)
[![GitHub Sponsors](https://img.shields.io/github/sponsors/stphnwlsh?label=Sponsor&logo=githubsponsors&logoColor=white&color=ea4aaa&style=for-the-badge)](https://github.com/sponsors/stphnwlsh)

A `dotnet new` template for a GraphQL API built with a streamlined version of Clean Architecture and [Hot Chocolate](https://chillicream.com/docs/hotchocolate).

It's one of five templates that share the same Application and Infrastructure layers and differ only in how they present them. See [Related templates](#related-templates).

## Contents

- [Getting started](#getting-started)
- [Running the project](#running-the-project)
- [Architecture](#architecture)
- [Features](#features)
- [Releases](#releases)
- [Related templates](#related-templates)
- [Resources](#resources)
- [Contributing](#contributing)
- [Support](#support)
- [License](#license)

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/products/docker-desktop) (optional, to build and run the container image)

### Install the template

``` bash
dotnet new install CleanGraphQLApi.Template
```

### Create a solution

``` bash
dotnet new cleangraphqlapi --name {YOUR_SOLUTION_NAMESPACE} --authorName "{YOUR_NAME}"
```

### Protect the API with bearer tokens (optional)

The API runs without authentication unless you give it an authorization server. Pass one when you create the solution, or set `Authentication:Authority` and `Authentication:Audience` in `appsettings.json` later.

``` bash
dotnet new cleangraphqlapi --name {YOUR_SOLUTION_NAMESPACE} --authority https://{YOUR_AUTHORIZATION_SERVER} --audience https://{YOUR_API_URL}
```

With an authority set, the movie, author and review queries and the review mutations need a JWT bearer token issued by that server for the audience, and return an `AUTH_NOT_AUTHENTICATED` error without one. The `version` query, the schema and Nitro stay open. In Nitro, add the token under the connection's authentication settings.

## Running the project

``` bash
docker compose up -d
```

Then open Nitro, the GraphQL IDE, at <http://localhost:8080/graphql>. See [docs/RUN.md](docs/RUN.md) for every way to run it.

The `Dockerfile` has stages for testing, publishing and running, so you can build just the part you need. You can also stamp a version into the assembly.

``` bash
docker build . --target run --tag cleangraphqlapi:latest --build-arg VERSION_PREFIX={VERSION_NUMBER} --build-arg VERSION_SUFFIX={PRERELEASE_NAME}
```

## Architecture

This solution is loosely based on Clean Architecture. It's by no means perfect, so I prefer to call it "Lean Mean Clean Architecture". Inspiration comes from [Jason Taylor's Clean Architecture Template](https://github.com/jasontaylordev/CleanArchitecture), with some ideas taken further and others scaled back.

There's a little CQRS in the style of the commands and queries, but reads and writes use the same data source underneath.

The one deliberate break from Clean Architecture is that the Presentation project references the Infrastructure project, so it can wire up dependency injection. To protect that boundary, every class in Infrastructure is `internal`, so it can't be used from Presentation by accident.

### Project structure

1. **Presentation** - Exposes the Application layer as a GraphQL schema. Queries and mutations send commands and queries through Mediator, Application errors become GraphQL errors, and Nitro is included for exploring the schema.
1. **Application** - Owns the domain and business logic. Commands and queries are validated and handled here, and each domain type has its own repository interface. This layer doesn't care what fulfils those contracts, as long as something does.
1. **Infrastructure** - Owns the data. It implements the repository interfaces to fetch, create, update and remove data, and maps its own data models onto the Application entities so persistence details don't leak upwards.

## Features

- GraphQL using [Hot Chocolate](https://chillicream.com/docs/hotchocolate) with the [Nitro](https://chillicream.com/docs/nitro) IDE served from `/graphql`
- Schema descriptions generated from the XML documentation comments on the Application models and resolvers
- Relay-style cursor paging and search on the `movies`, `authors` and `reviews` queries (`first`, `after`, `pageInfo`, `totalCount`), with each cursor holding the offset the shared Application queries page by
- List items carry a review count and average stars worked out by the database, and their `reviews` field is loaded for the whole page at once by a [DataLoader](https://chillicream.com/docs/hotchocolate/fetching-data/dataloader), so a page of 50 movies with reviews takes three queries, not 52
- Optional JWT bearer authentication on the queries and mutations, turned on by setting an authority
- Logging using [Serilog](https://github.com/serilog/serilog), with [Seq](https://datalust.co/seq) when running in Docker
- Mediator pattern using [Mediator](https://github.com/martinothamar/Mediator)
- Validation using [FluentValidation](https://github.com/FluentValidation/FluentValidation) in an Application pipeline behaviour
- GraphQL errors mapped from Application exceptions, with validation failures and not-found codes in the error extensions
- Object mapping using [Mapperly](https://mapperly.riok.app)
- Data stored in [SQLite](https://learn.microsoft.com/ef/core/providers/sqlite/) using [Entity Framework Core](https://learn.microsoft.com/ef/core) migrations, seeded with [Bogus](https://github.com/bchavez/Bogus) only when `Database:SeedSampleData` is on. Each test uses its own throwaway in-memory SQLite database, so tests never touch a real one or each other's data
- Testing using [xUnit](https://xunit.net), [Shouldly](https://github.com/shouldly/shouldly) and [NSubstitute](https://github.com/nsubstitute/NSubstitute), with coverage from [Coverlet](https://github.com/coverlet-coverage/coverlet)
- Build rules in `Directory.Build.props`: code style and the recommended analyzers are checked on every build, warnings fail it, and package versions live in one place with [Central Package Management](https://learn.microsoft.com/nuget/consume-packages/central-package-management)
- CI with GitHub Actions: build and test, a `dotnet format` check, 95% coverage gate with at least 90% on the lines each pull request changes, [Trivy](https://trivy.dev) container scan, [CodeQL](https://codeql.github.com) static analysis, then a monthly NuGet release and GitHub release from `main` using [trusted publishing](https://learn.microsoft.com/nuget/nuget-org/trusted-publishing)
- Health checks at `/health/live` and `/health/ready`, using [ASP.NET Core health checks](https://learn.microsoft.com/aspnet/core/host-and-deploy/health-checks)
- Grouped dependency updates on the 15th of each month using [Dependabot](https://docs.github.com/code-security/dependabot), including the pinned Seq image
- A Docker image that runs as a non-root user with a health check

## Releases

The template is released once a month. On the 1st, at 9am US Central time, the pipeline checks for changes since the last release. If anything outside `.github/` has changed, it builds and tests `main`, publishes the new version to [NuGet](https://www.nuget.org/packages/CleanGraphQLApi.Template) and creates a [GitHub release](https://github.com/stphnwlsh/CleanGraphQLApi/releases) with notes for every pull request merged since the previous one. If nothing has changed, there's no release that month. Dependency updates arrive on the 15th, so they're merged in time for the next release.

Versions keep the `YYYY.MM.DD.RUN` format. Up to the end of October 2026, every merge to `main` was released, so the older releases are much smaller. To release outside the monthly cycle, run the **Build Pipeline** workflow by hand. Tick **force** to release even when only `.github/` has changed.

To update an installed template to the latest release:

``` bash
dotnet new update
```

## Related templates

All five templates share the same Application and Infrastructure layers, tests, CI pipeline and repository setup.

| Template | Presentation | Package |
| --- | --- | --- |
| [Clean Minimal API](https://github.com/stphnwlsh/CleanMinimalApi) | ASP.NET Core Minimal APIs | [CleanMinimalApi.Template](https://www.nuget.org/packages/CleanMinimalApi.Template) |
| [Clean Web API](https://github.com/stphnwlsh/CleanWebApi) | ASP.NET Core controllers | [CleanWebApi.Template](https://www.nuget.org/packages/CleanWebApi.Template) |
| **Clean GraphQL API** (this repo) | GraphQL | [CleanGraphQLApi.Template](https://www.nuget.org/packages/CleanGraphQLApi.Template) |
| [Clean Model Context Protocol](https://github.com/stphnwlsh/CleanModelContextProtocol) | Model Context Protocol server | [CleanModelContextProtocol.Template](https://www.nuget.org/packages/CleanModelContextProtocol.Template) |
| [Clean Lambda Function](https://github.com/stphnwlsh/CleanLamdaFunction) | AWS Lambda function | [CleanLambdaFunction.Template](https://www.nuget.org/packages/CleanLambdaFunction.Template) |

## Resources

This template wouldn't exist without these resources. If you're on your own learning adventure, they're well worth a read.

- [Hot Chocolate - Docs](https://chillicream.com/docs/hotchocolate)
- [Fiyaz Hasan - GraphQL Series](https://fiyazhasan.me/tag/graphql/)
- [İlker Aytı - Dotnet Template](https://github.com/iayti/Matech.Sample.Template)
- [Andrew Lock - Reducing log verbosity with Serilog RequestLogging](https://andrewlock.net/using-serilog-aspnetcore-in-asp-net-core-3-reducing-log-verbosity/)
- [Jason Taylor - Clean Architecture Solution Template](https://github.com/jasontaylordev/CleanArchitecture)
- [Robert C. Martin - The Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

## Contributing

Bug reports, ideas and pull requests are welcome. Please read the [contributing guide](CONTRIBUTING.md) and the [code of conduct](CODE_OF_CONDUCT.md) first. To report a security vulnerability, follow the [security policy](SECURITY.md) instead of opening an issue.

## Support

If this template saves you time, please consider [sponsoring me on GitHub](https://github.com/sponsors/stphnwlsh). It helps me keep all five templates up to date.

[![Please sponsor me](docs/sponsor.jpg)](https://github.com/sponsors/stphnwlsh)

You can also connect with me on [LinkedIn](https://www.linkedin.com/in/stphnwlsh), follow me on [Medium](https://stphnwlsh.medium.com) or [GitHub](https://github.com/stphnwlsh). For help using the template, see [SUPPORT.md](SUPPORT.md).

## License

Released under the [MIT License](LICENSE.md).
