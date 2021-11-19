# Clean GraphQL API
[![Build Pipeline](https://github.com/stphnwlsh/CleanGraphQLApi/actions/workflows/build-pipeline.yml/badge.svg)](https://github.com/stphnwlsh/CleanGraphQLApi/actions/workflows/build-pipeline.yml)
[![Code Coverage](https://codecov.io/gh/stphnwlsh/CleanGraphQLApi/branch/main/graph/badge.svg?token=N3ZIADS1V2)](https://codecov.io/gh/stphnwlsh/CleanGraphQLApi)

This is a sample application to be an example of using [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) alongside a .net implementation of [GraphQL](https://github.com/graphql-dotnet/graphql-dotnet).

## Features
- Logging using [Serilog](https://github.com/serilog/serilog)
- Mediator Pattern using [Mediatr](https://github.com/jbogard/MediatR)
- Validation using [FluentValidation](https://github.com/FluentValidation/FluentValidation)
- Testing using [Shouldly](https://github.com/shouldly/shouldly) and [NSubstitute](https://github.com/nsubstitute/NSubstitute)
- GraphQL using [GraphQL for .NET](https://github.com/graphql-dotnet/graphql-dotnet)

## Docker
There's a dockerfile included in the build folder and serves the purpose of restoring, building, testing, publishing and then creating a runtime image of the API.  Works on my machine.....

```
docker build . -t cleangraphqlapi:latest --build-arg CACHE_BUST 1
```

You can pull the public version of this image using

```
docker pull stphnwlsh/cleangraphqlapi
```

## Prerequisites
This solution depends on the [pre-release SDK for .net 6](https://dotnet.microsoft.com/download/dotnet/6.0), you need to install that before it will work for you.

## Projects
This solution contains a few projects to follow the Clean Architecture patterns, it's by no means perfect.  But it's an example so cut me some slack.  Each project has a purpose and is separated from the others, for a far more well thought out solution please see [Jason Taylor's Clean Architecture Template](https://github.com/jasontaylordev/CleanArchitecture) 

## Resources
This sample would not have been possible without gaining inspiration from the following resources.  If you are on your own learning adventure please read the following blogs and documentation.
- [GraphQL.NET - Docs](https://graphql-dotnet.github.io/docs/getting-started/introduction)
- [Fiyaz Hasan - GraphQL Series](https://fiyazhasan.me/tag/graphql/)

## Support
I'm sharing some of my work here and if it helps you, I'd love it if you'd consider supporting me.

[!["Buy Me A Coffee"](https://www.buymeacoffee.com/assets/img/guidelines/download-assets-sm-1.svg)](https://www.buymeacoffee.com/stphnwlsh)
