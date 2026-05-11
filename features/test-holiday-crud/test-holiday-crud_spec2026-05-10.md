# Feature Specification: Test Holiday CRUD

**Date**: 2026-05-10
**Status**: Approved

## Overview

A temporary test CRUD page that exercises the full application stack — MediatR, CQRS, EF Core, the repository pattern, and the React/MUI frontend — against a simple `TestHoliday` domain entity. The sole purpose is to validate that all architectural layers are wired together correctly before real feature work begins. All backend and frontend code for this feature is explicitly prefixed or namespaced with `Test` to signal its temporary nature and make surgical removal straightforward. This feature will be deleted once real features supersede it.

## Must-haves and stipulations

- All domain types, application types, infrastructure types, and API controllers introduced by this feature MUST be prefixed with `Test` (e.g. `TestHoliday`, `ITestHolidayRepository`, `TestHolidaysController`).
- All backend code MUST live under a `Test` namespace segment within its respective project layer — no test code pollutes the root namespace.
- The feature MUST follow the established architectural patterns (CQRS via MediatR, repository pattern, FluentValidation pipeline behavior) — it exists specifically to validate those patterns.
- The `Destination` field MUST be constrained to exactly five fixed values: Mexico, Japan, New Zealand, Iceland, Scotland. No free-text entry.
- The frontend page MUST be routed at `/test/holidays` and MUST NOT be linked from any permanent navigation.
- EF Core (`HolidayPlannerDbContext`) and the audit interceptor (`CreatedOn`/`ModifiedOn`) are first-time infrastructure that will be reused by real features — they are NOT prefixed with `Test`.
- The `TestHoliday` table MUST be managed by an EF Core migration.
- End date MUST be validated as after start date.

## User stories

As a developer,
I want to view a paginated list of test holidays,
so that I can confirm data is being read from the database correctly.

As a developer,
I want to create a new test holiday with a name, destination, start date, and end date,
so that I can confirm data is being written through the full CQRS → EF Core stack.

As a developer,
I want to edit an existing test holiday,
so that I can confirm update operations and re-validation work end-to-end.

As a developer,
I want to delete a test holiday,
so that I can confirm delete operations reach the database and the UI reflects the change.

As a developer,
I want destination to be a dropdown limited to Mexico, Japan, New Zealand, Iceland, and Scotland,
so that I can confirm enum-constrained field handling works on both the frontend and backend.

## Rationale

This feature exists to de-risk the architectural foundation before any production feature is built on top of it. A working CRUD cycle proves: MediatR pipeline dispatch, FluentValidation behavior, domain entity persistence via EF Core with SQL Server, the repository abstraction, AutoMapper DTO projection, the React/MUI component stack, TanStack Query, React Hook Form with Zod validation, and the full API contract round-trip. The deliberate `Test` prefix and namespace isolation mean the entire feature can be removed as a single clean operation when it has served its purpose.
