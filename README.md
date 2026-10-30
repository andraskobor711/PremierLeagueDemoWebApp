# Premier League Demo

A demo web application for exploring Premier League teams and matches. It showcases an Angular frontend, an ASP.NET Core Web API, and SQLite for local data storage. Please, note this is a demo app, therefore things like user management, unit testing, proper frontend and backend validation, etc. are missing.

The project is currently intended for developers to run locally. The API is hosted on your machine, and the frontend connects to that local API.

## Run locally

### Prerequisites

- .NET 10 SDK
- Node.js and npm
- Git

### Setup

Clone the repository and install the frontend dependencies:

```bash
git clone https://github.com/andraskobor711/PremierLeagueWebApp.git
cd PremierLeagueWebApp/frontend
npm ci
cd ..
```

Start the application from the repository root:

```bash
dotnet run --project backend/src/PremierLeagueDemoWebApp.csproj
```

The ASP.NET Core development server starts the Angular development server as well. Open [https://localhost:57412](https://localhost:57412) in your browser.

The application uses a local SQLite database. Changes made while running the app are stored on your machine.

## Live demo

A hosted live demo is planned and will be available here when ready.
