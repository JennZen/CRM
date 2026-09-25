# Mini CRM

A small client-server CRM system built to keep track of clients and the requests they send in — who's working on what, what stage a request is at, and who's carrying the heaviest load. It was built solo, from the database up to the UI, as a way to actually apply a layered .NET architecture end to end instead of just reading about one.

## Why this exists

Most small teams start out tracking client requests in a shared spreadsheet or a Slack channel, and it works fine — right up until it doesn't. Statuses get out of sync, nobody remembers who was supposed to follow up, and there's no real history of what happened to a request. Mini CRM is a deliberately small, focused answer to that problem: a place to store clients, log their requests, assign a manager, move a request through its lifecycle (New → Active → Finished/Solved/Rejected), and see who's handling what.

It's not trying to compete with Salesforce or HubSpot — it does far less, on purpose. The goal was a system sized correctly for a small organization, not a platform you have to configure your way down to something usable.

## What it does

- **Clients** — create, edit, search, and soft-archive client records (company name, contact person, phone, email, notes).
- **Requests** — full CRUD, with a dedicated status pipeline and priority levels, plus a separate endpoint for changing just the status or reassigning the manager without touching the rest of the request.
- **History tracking** — every status change on a request is logged with who made it and when, so there's an actual audit trail instead of "I think it was Maria."
- **Users & roles** — two roles, `Admin` and `Manager`. Admins manage user accounts and see everything; managers work within clients and requests.
- **Auth** — login with email/password, JWT issued on success, required on every protected endpoint.
- **Dashboard** — a quick read on the state of things: client/request counts, recent activity, and a breakdown of requests by manager so workload is visible at a glance.

## Architecture

The solution is split into six projects, following a fairly standard layered approach — each layer only knows about the one below it:

```
CRM.Domain        → entities & enums, zero dependencies
CRM.Application    → business logic, DTOs, service interfaces
CRM.DataAccess     → repositories, XPO models, JWT/password hashing
CRM.Api            → REST API (ASP.NET Core Web API), Swagger docs
CRM.Web            → MVC front end, talks to the API over HTTP — no direct DB access
CRM.Test           → unit tests for the Application layer (MSTest)
```

`CRM.Web` deliberately doesn't touch the database directly. It goes through the same API everything else would use, via a set of small HTTP client wrappers (`ICustomerApiClient`, `IRequestApiClient`, `IUserApiClient`). That keeps the "what can read/write data" surface to exactly one place — the API — and made the front end easy to reason about even without a JS framework in the mix.

## Tech stack

| Layer | Choice |
|---|---|
| Backend | ASP.NET Core Web API |
| Frontend | ASP.NET Core MVC (Razor Views, Bootstrap) |
| Database | PostgreSQL |
| ORM | DevExpress XPO |
| Auth | JWT + ASP.NET Core Identity's `PasswordHasher` |
| Object mapping | Mapperly (compile-time, no reflection) |
| API docs | Swagger / OpenAPI |
| Testing | MSTest |

## API

The API is documented via Swagger and grouped by resource:

- **Auth** — `POST /api/Auth/login`
- **Customer** — list / active / cards / by id / create / update / delete / count
- **Request** — full CRUD, plus `/my`, `/recent`, `/count`, `/my/count`, and dedicated `PUT` endpoints for status and manager reassignment
- **User** — list / active / with-request-count / by id / create / update / delete / count

Every endpoint requires a valid JWT in the `Authorization` header, except login itself.

## Project status

This was built as part of an internship at **Intelectsoft SRL** (Chișinău), as a solo project — one person handling design, backend, frontend, and tests. It covers the core CRM workflow end to end; it's not aiming to be feature-complete against commercial CRM platforms, and there's plenty of room to grow (notifications, richer reporting, multi-tenant support) if it were ever taken further.
