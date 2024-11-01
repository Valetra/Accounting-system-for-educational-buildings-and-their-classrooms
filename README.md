# Accounting system for educational buildings and their classrooms

It is solution for accounting system of the enterprise's (university's) classroom fund is designed to store structured information about the premises of the enterprise and their characteristics.

Solution structure.

![alt text](https://i.ibb.co/2qxs91P/image.png)

## Prerequisites

+ [Git](https://git-scm.com/)
+ [Docker](https://www.docker.com/)

## Usage

1. Clone this repository.
1. Run Docker Engine.
1. Open solution folder from console.
1. Run command `docker-compose up --build` in the root of solution to assemble and run application.
1. Wait until docker containers will assemble and run.
1. Open browser on page "http://localhost:3000/swagger" for use Building API.
1. Open browser on page "http://localhost:3001/swagger" for use Classroom API.
1. Open browser on page "http://localhost:15673" to see RabbitMq UI.
