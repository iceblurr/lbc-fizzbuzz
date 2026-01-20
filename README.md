# FizzBuzz API Solution

This solution is a RESTful API built with ASP.NET Core that implements an advanced FizzBuzz service. It includes a `FizzBuzz` endpoint for generating the sequence with custom parameters and a `Metrics` endpoint to track the most frequently used request parameters.

## Purpose
The original fizz-buzz consists in writing all numbers from 1 to 100, and just replacing all multiples of 3 by "fizz", all multiples of 5 by "buzz", and all multiples of 15 by "fizzbuzz".
The output would look like this: "1,2,fizz,4,buzz,fizz,7,8,fizz,buzz,11,fizz,13,14,fizzbuzz,16,...".

Your goal is to implement a web server that will expose a REST API endpoint that:
- Accepts five parameters: three integers int1, int2 and limit, and two strings str1 and str2.
- Returns a list of strings with numbers from 1 to limit, where: all multiples of int1 are replaced by str1, all multiples of int2 are replaced by str2, all multiples of int1 and int2 are replaced by str1str2.The server needs to be:
- Ready for production
- Easy to maintain by other developers

Bonus: add a statistics endpoint allowing users to know what the most frequent request has been. This endpoint should:
- Accept no parameter
- Return the parameters corresponding to the most used request, as well as the number of hits for this request

## Architecture

The solution is structured following Clean Architecture principles:
- **FizzBuzz.Api**: The entry point, containing Controllers and API configuration.
- **FizzBuzz.Domain**: Contains business logic interfaces, services, and domain models.
- **FizzBuzz.Infrastructure**: Implements data access (using Entity Framework Core) and other external dependencies.
- **FizzBuzz.Tests**: Unit tests for the solution using xUnit, NSubstitute, and FluentAssertions.

## Prerequisites

- [Docker](https://www.docker.com/products/docker-desktop) and Docker Compose installed.
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) (if running locally without Docker).

## Getting Started

### Cloning the repository
```bash
git clone https://github.com/iceblurr/lbc-fizzbuzz.git
cd lbc-fizzbuzz
```

### Launching with Docker Compose

The easiest way to run the entire solution (API + Database) is using Docker Compose.

1.  **Build and Run:**
    Execute the following command in the root directory:
    ```bash
    docker compose up --build
    ```

    This command will:
    - Build the `fizzbuzz.api` image using the `FizzBuzz.Api/Dockerfile`.
    - Start a PostgreSQL container (`postgres-db`).
    - Initialize the database with the schema defined in `dbscripts/seed.sql`.
    - Start the API container.

2.  **Access the API:**
    - A Scalar UI for the API will be accessible at `http://localhost:8080/scalar/v1`(typically on port 8080 or the port exposed in the Dockerfile, check logs for `Now listening on: ...`).

    **Database Connection:**
    The API is configured to talk to the `postgres-db` service within the docker network `frontend`.

### Running Locally (Development Mode)

If you want to run the API via your IDE (like Rider or Visual Studio) or `dotnet run`:

1.  **Start the Database:**
    You still need the database running. You can start just the database service:
    ```bash
    docker compose up postgres-db -d
    ```
    This exposes Postgres on port `5432` on your local machine.

2.  **Configuration:**
    Ensure `FizzBuzz.Api/appsettings.Development.json` points to `localhost` port `5432` (or the mapped port).
    
    *Note: Ensure your docker-compose maps `5432:5432` or update the config to match your local setup.*

3.  **Run the API:**
    ```bash
    cd FizzBuzz.Api
    dotnet run
    ```

## API Endpoints

### 1. GET /FizzBuzz
Generates a FizzBuzz sequence based on parameters.

**Parameters:**
- `int1` (query, int): First divisor (default logic for "Fizz").
- `int2` (query, int): Second divisor (default logic for "Buzz").
- `limit` (query, int): The upper limit of numbers to process.
- `str1` (query, string): Replacement string for `int1`.
- `str2` (query, string): Replacement string for `int2`.

**Example:**
`GET /FizzBuzz?int1=3&int2=5&limit=15&str1=fizz&str2=buzz`

### 2. GET /FizzBuzz/most-hit
Returns the parameters of the most frequent request.

## Running Tests

To execute the unit tests:
```bash
cd FizzBuzz.Tests
dotnet test
```