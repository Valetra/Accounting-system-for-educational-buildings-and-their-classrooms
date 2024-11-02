# Accounting system for educational buildings and their classrooms

It is solution for accounting system of the enterprise's (university's) classroom fund is designed to store structured information about the premises of the enterprise and their characteristics.

Solution structure.

![alt text](https://i.ibb.co/2qxs91P/image.png)

## Prerequisites

+ [VSCode](https://code.visualstudio.com/)
+ [Git](https://git-scm.com/)
+ [Docker](https://www.docker.com/)

## Usage

1. Clone this repository.
1. Start Docker Engine.
1. Open solution folder from console.
1. Run command `docker-compose up --build` in the root of solution to build and run application.
1. Wait until docker containers will be built and ran.
1. Open browser on page "http://localhost:3000/swagger" to use Building API.
1. Open browser on page "http://localhost:3001/swagger" to use Classroom API.
1. Open browser on page "http://localhost:15673" to see MQ UI.

## Development

1. Clone this repository.
1. Start Docker Engine.
1. Open solution folder in VSCode.
1. Run command `dotnet restore` in the root of solution to restore the dependencies and tools of a project.
1. Run command `docker compose -f docker-compose.infrastructure.yml up --build` in the root of solution to start infrastructure.
1. Wait until docker containers will be built and ran.
1. Run command `dotnet ef database update --startup-project BuildingApi && dotnet ef database update --startup-project ClassroomAPI` in the root of solution to apply database migrations.
