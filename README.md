# Clean GraphQL API

[![GitHub Workflow Status](https://img.shields.io/github/workflow/status/stphnwlsh/CleanGraphQLApi/Build%20Pipeline?label=Build%20Pipeline&logo=github&logoColor=white&style=for-the-badge)](https://github.com/stphnwlsh/CleanGraphQLApi/actions/workflows/build-pipeline.yml)
[![Codecov](https://img.shields.io/codecov/c/github/stphnwlsh/CleanGraphQLApi?label=Code%20Coverage&logo=codecov&logoColor=white&style=for-the-badge)](https://codecov.io/gh/stphnwlsh/CleanGraphQLApi)
![Nuget](https://img.shields.io/nuget/v/CleanGraphQLApi.Template?label=nuget%20template&logo=nuget&logoColor=white&style=for-the-badge)

This is a template API using [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) alongside a .net implementation of [GraphQL](https://github.com/graphql-dotnet/graphql-dotnet).

## Prerequisites

This solution in built on the [.net 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0), you need to install that before it will work for you.  If you want to build the Dockerfile you will need to install  [Docker](https://www.docker.com/products/docker-desktop) as well.

## Installation

This is a template and you can install it using the [dotnet new cli](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new).  To install the lastest version of the template run the following command.

``` bash
dotnet new --install CleanGraphQLApi.Template
```

To create a new solution using this template run the following command

``` bash
dotnet new cleangraphqlapi --name {YOUR_SOLUTION_NAMESPACE} --au "{YOU_AUTHORS_NAME}"
```

## Docker

There's a dockerfile included in the build folder and serves the purpose of restoring, building, testing, publishing and then creating a runtime image of the API.  Works on my machine.....you can add a version prefix and suffix to version the service in the assembly.  The Dockerfile does have stages so you can just run the tests or publish the solution depending on your needs.  Review the `build-pipeline.yml` in the github folder for more detailed usage.

``` bash
docker build . -t cleangraphqlapi:latest --build-arg VERSION_PREFIX {VERSION_NUMBER} -- build-arg VERSION_SUFFIX {PRERELEASE_NAME}
```

The Github Action does publish an image of this API and you check it out for yourself by runnning this command in docker.

```bash
docker pull stphnwlsh/cleangraphqlapi
```

## Architecture

This solution is loosly based on Clean Architecture patterns, it's by no means perfect.  I prefer to call it "Lean Mean Clean Architecture".  Inspiration has been taken from [Jason Taylor's Clean Architecture Template](https://github.com/jasontaylordev/CleanArchitecture), but I have made some structural decisions to take some things further and scaled back others.

There's a little CQRS type stuff going on here but it's more in style than real separated functions for reading and writing as under the covers they are the same data source.

Breaking the Clean Architecture pattern is the fact that the Infrastructure project is referenced by the Presentation project.  This is for **Dependency Injection** purposes, so to protect this a little further, all classes in the Infrastructure project are `internal`.  This stops them being accidentally used in the Presentation project.

### Project Structure

It's streamlined into 3 functional projects.  All serve their own purpse and segregate aspects of the application to allow easier replacement and updating.

1. **Presentation** - Setting up the interactions between the Application layer and the consumer.  In the project that's via a Web API but it could be many other things.  The Web API uses controllers to funnel the actions to the layer that owns the domain.
1. **Application** - This project owns the domain and business logic.  There's validation of the Commands and Queries and handling of domain entities in their own separated structures.  Each domain type has it's own interface to a datasource downstream, this project doesn't care what fulfills this contract, as long as someone does.
1. **Infrastructure** - Here's where the database comes into play.  Infra owns the data objects and works with the repository interfaces to fetch, create, update and remove object from the source.  There's some entity mapping here to allow specific models with attributes to remain in this layer and not bleed through to the **Application** layer.

## Features

There are plenty of handy implementations of features throughout this solution, in no particular order here are some that might interest you.

- Logging using [Serilog](https://github.com/serilog/serilog)
- Mediator Pattern using [Mediatr](https://github.com/jbogard/MediatR)
- Validation using [FluentValidation](https://github.com/FluentValidation/FluentValidation)
- Testing using [Shouldly](https://github.com/shouldly/shouldly) and [NSubstitute](https://github.com/nsubstitute/NSubstitute)
- GraphQL using [GraphQL for .NET](https://github.com/graphql-dotnet/graphql-dotnet)

## Resources

This sample would not have been possible without gaining inspiration from the following resources.  If you are on your own learning adventure please read the following blogs and documentation.

- [GraphQL.NET - Docs](https://graphql-dotnet.github.io/docs/getting-started/introduction)
- [Fiyaz Hasan - GraphQL Series](https://fiyazhasan.me/tag/graphql/)
- [İlker Aytı - Dotnet Template](https://github.com/iayti/Matech.Sample.Template)

## Connect and Support

If you like this, or want to checkout my other work, please connect with me on [LinkedIn](https://www.linkedin.com/in/stphnwlsh), [Twitter](https://twitter.com/stphnwlsh) or [GitHub](https://github.com/stphnwlsh), and consider supporting me at [Buy Me a Coffee].

[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/guidelines/download-assets-sm-1.svg)](https://www.buymeacoffee.com/stphnwlsh)
