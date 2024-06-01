# IdealReating

# PersonDetailsTask

## How to Run

1. Clone the repository:
    ```sh
    git clone https://github.com/yourusername/PersonDetailsApi.git
    cd PersonDetailsApi
    ```

2. Build and run the application:
    ```sh
    dotnet build
    dotnet run
    ```

3. Alternatively, run using Docker:
    ```sh
    docker-compose up --build
    ```

## API Endpoint

- `GET /persons?filter={filter}`: Returns a list of person details from all data sources with optional filtering.

## Design Patterns Used

- **Repository Pattern**: To abstract data access logic and provide a flexible way to handle different data sources.
- **Dependency Injection**: To inject dependencies into controllers and services, promoting loose coupling and testability.

## Architecture

- **Controllers**: Handle HTTP requests and responses.
- **Services**: Contain business logic and interact with repositories.
- **Repositories**: Abstract data access logic for different data sources.
