# Premier League Demo

A demo web application for exploring Premier League teams and matches. It showcases an Angular frontend, an ASP.NET Core Web API, and SQLite for local data storage. The app uses JWT authentication and claim-based authorization.

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

## Login

Sign in through the frontend using one of the demo accounts below. The access token is kept in the current browser tab and cleared when you sign out or the token expires.

## Backend authentication

The checked-in SQLite database includes two demo accounts:

| Username | Password | Access |
| --- | --- | --- |
| `adminuser` | `admin_p@$$word` | Full system access |
| `demouser` | `demo_p@$$word` | Read-only access |

Exchange credentials for a JWT with `POST /auth/login`:

```json
{
  "username": "demouser",
  "password": "demo_p@$$word"
}
```

Use the returned `accessToken` as a bearer token in the request's `Authorization` header.

Identity stores password hashes in the database. The checked-in JWT signing key and demo accounts are for local development only. Before deploying, configure a private signing key, issuer, and audience through the deployment environment or secret store, and provision production users securely.

## Live demo

A hosted live demo is planned and will be available here when ready.
