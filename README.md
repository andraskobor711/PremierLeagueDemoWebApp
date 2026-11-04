# Premier League Demo

A demo web application for exploring Premier League teams, matches, and standings. It uses an Angular frontend, an ASP.NET Core Web API, SQLite, JWT authentication, and claim-based authorization.

## Live demo

Visit the [Premier League live demo](https://andraskobor711.github.io/PremierLeagueDemoWebApp/). Sign in with the read-only demo account:

| Username | Password | Access |
| --- | --- | --- |
| `demouser` | `demo_p@$$word` | View teams, matches, and standings |

The hosted demo does not provide an `adminuser` account. Data editing is not available in the live demo. To try editing data, clone the repository and run the app locally; local changes are stored in your local SQLite database and do not affect the hosted demo.

## Run locally

### Prerequisites

- .NET 10 SDK
- Node.js and npm
- Git

### Setup

Clone the repository and install the frontend dependencies:

```bash
git clone https://github.com/andraskobor711/PremierLeagueDemoWebApp.git
cd PremierLeagueDemoWebApp/frontend
npm ci
cd ..
```

Start the application from the repository root:

```bash
dotnet run --project backend/src/PremierLeagueDemoWebApp.csproj
```

The ASP.NET Core development server starts the Angular development server as well. Open [https://localhost:57412](https://localhost:57412) in your browser.

The checked-in SQLite database contains local demo accounts:

| Username | Password | Access |
| --- | --- | --- |
| `demouser` | `demo_p@$$word` | Read-only |
| `adminuser` | `admin_p@$$word` | Edit matches |

Sign in locally as `adminuser` to test data editing. Changes are saved to your local SQLite database and remain on your machine.

## Backend authentication

The API exchanges credentials for a JWT with `POST /auth/login`:

```json
{
  "username": "demouser",
  "password": "demo_p@$$word"
}
```

Use the returned `accessToken` as a bearer token in the request's `Authorization` header.

Identity stores password hashes in the database. The checked-in JWT signing key and demo accounts are for local development only. Before deploying, configure a private signing key, issuer, and audience through the deployment environment or secret store, and provision production users securely.
