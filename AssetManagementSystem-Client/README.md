# AssetManagementSystemClient

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 21.2.2.

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

## Running unit tests

To execute unit tests with the [Vitest](https://vitest.dev/) test runner, use the following command:

```bash
ng test
```

## Running end-to-end tests

For end-to-end (e2e) testing, run:

```bash
ng e2e
```

Angular CLI does not come with an end-to-end testing framework by default. You can choose one that suits your needs.

## Additional Resources

For more information on using the Angular CLI, including detailed command references, visit the [Angular CLI Overview and Command Reference](https://angular.dev/tools/cli) page.

## Run With Docker

This frontend can be containerized with Docker using a multi-stage build:

1. The app is built using Node.js.
2. The generated static files are served by Nginx.

### Build and run with Docker

```bash
docker build -t asset-management-system-client .
docker run -d -p 4200:80 --name asset-management-system-client asset-management-system-client
```

Open `http://localhost:4200` in your browser.

### Build and run with Docker Compose

```bash
docker compose up --build -d
```

To stop:

```bash
docker compose down
```

### Important API note

Service files currently use `https://localhost:62693` as the backend URL. When your frontend runs in Docker, API calls are still made by the browser, so they resolve from the machine where the browser is running.

If your backend is not reachable at `https://localhost:62693` from that browser, update the API base URL accordingly.
