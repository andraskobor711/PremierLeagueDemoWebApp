# Frontend

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 22.2.1.

## Development server

To start a local development server, run:

```bash
ng serve
```

Once the server is running, open your browser and navigate to `http://localhost:4200/`. The application will automatically reload whenever you modify any of the source files.

## Code scaffolding

Angular CLI includes powerful code scaffolding tools. To generate a new component, run:

```bash
ng generate component component-name
```

For a complete list of available schematics (such as `components`, `directives`, or `pipes`), run:

```bash
ng generate --help
```

## Building

To build the project run:

```bash
ng build
```

This will compile your project and store the build artifacts in the `dist/` directory. By default, the production build optimizes your application for performance and speed.

### GitHub Pages build

For the repository's project Pages URL, build with:

```bash
npm run build:github-pages
```

This uses `/PremierLeagueWebApp/` as the base path and creates a production build
in `dist/frontend/browser`. The app uses hash-based routes, so Pages URLs look
like `https://andraskobor711.github.io/PremierLeagueWebApp/#/login`; refreshing
a routed page works without server-side route rewrites.

The `Deploy Angular frontend to GitHub Pages` workflow builds and publishes this
directory when frontend files are pushed to `main` or `hosting_in_github`, or
when the workflow is run manually. In the repository's **Settings > Pages**,
set the build and deployment source to **GitHub Actions**. After pushing the
workflow to one of those branches, the deployed site is available at
`https://andraskobor711.github.io/PremierLeagueWebApp/`.

## Running unit tests

To execute unit tests with the [Vitest](https://vitest.dev/) test runner, use the following command:

```bash
npm test -- frontend
```

## Authentication

The app signs in through the API's `/auth/login` endpoint. Access tokens are kept in
`sessionStorage` for the lifetime of the browser tab, attached only to requests to the
configured API origin, and removed on expiry, sign-out, or an API `401`. A `403` keeps
the session and displays the access-denied page.

`sessionStorage` is a usability/security compromise, not protection against cross-site
scripting: JavaScript running in the app can read the token. For a production deployment
that needs stronger token isolation, consider a backend-for-frontend with Secure,
HttpOnly, SameSite cookies and appropriate CSRF protections.

## Running end-to-end tests

For end-to-end (e2e) testing, run:

```bash
ng e2e
```

Angular CLI does not come with an end-to-end testing framework by default. You can choose one that suits your needs.

## Additional Resources

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.
