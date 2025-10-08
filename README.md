# Login and Signup API

A small ASP.NET Core Web API that provides user registration and login with JWT authentication and MongoDB as the data store. Passwords are hashed with BCrypt and JWT tokens are issued with a 1-hour expiry.

## Quick overview

- Base route: `/api/user`
- Endpoints:
	- `POST /api/user/signup` — register a new user
	- `POST /api/user/login` — login and receive a JWT

The project uses `DotNetEnv` to load environment variables from a `.env` file if present. Environment variables are also read from the process environment.

## Contracts (inputs / outputs)

- User model (signup request body):

```json
{
	"name": "string",
	"email": "user@example.com",
	"password": "plain-text-password"
}
```

- Login model (login request body):

```json
{
	"email": "user@example.com",
	"password": "plain-text-password"
}
```

- Responses
	- Signup (success): 200 OK with body: `"User registered successfully"`
	- Login (success): 200 OK with JSON body: `{ "token": "<JWT>" }`
	- Login failure: 401 Unauthorized (or 400/other on bad input)

## Required environment variables

Create a `.env` file in the project root or set these variables in your environment. The app will throw an error if any are missing at runtime.

- DB_NAME — MongoDB database name
- DB_CONNECTION — MongoDB connection string (URI)
- JWT_SECRET_KEY — secret used to sign JWTs (keep private)
- JWT_ISSUER — JWT issuer value
- JWT_AUDIENCE — JWT audience value

Example `.env` :

```
DB_NAME=LoginDb
DB_CONNECTION=mongodb://localhost:27017
JWT_SECRET_KEY=supersecretkey_here
JWT_ISSUER=your-app
JWT_AUDIENCE=your-app-audience
```

Note: The code loads environment variables via DotNetEnv (Env.Load()). If you prefer system environment variables, set them in your OS instead.

## Run locally (PowerShell)

1. Restore and build:

```powershell
dotnet restore
dotnet build
```

2. Run the API (from project root):

```powershell
dotnet run --project .\Login_and_Signup.csproj
```

The process will print the listening URLs (e.g. `https://localhost:xxxxx`). When running in Development the app registers Swagger; open the printed HTTPS URL and append `/swagger` to view the API UI.

## Try the endpoints (Postman)

Signup (register):

```Postman
Body

{"name": "Examplename", "email": "ExampleEmail@hotmail.com", "password": "Examplepassord123" }


```

Login (receive JWT):

```Postman
Body 

{ "email": "exampleEmail@hotmail.com", "password": "ExamplePassord123" }

=> Return a JWT
```



Use the returned token for protected endpoints by sending an `Authorization: Bearer <token>` header.

## Implementation notes & behaviour

- Passwords are hashed using BCrypt before saving to MongoDB.
- JWT tokens are generated with a 1-hour expiry.
- MongoDB collection name: `Users`.
- If required environment variables are missing, the app will throw an InvalidOperationException and the request that depends on them will fail. Typical error messages reference the missing key (for example `JWT_SECRET_KEY not found`).

