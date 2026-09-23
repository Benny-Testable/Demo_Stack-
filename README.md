# Star — multi-tenant reference stack (test data)

Test-data repository for platform analysis runs.

| Layer | Stack |
|---|---|
| Backend | PHP 8.3 · Laravel 10 |
| Database | MySQL 8, **one database per tenant** |
| Frontend | Vue 3 · TypeScript · Vite · Tailwind CSS |

The tenants modelled here (`star_alpha`, `star_beta`, `star_gamma`) belong to this
sample application only. They are unrelated to any platform tenant.

## Layout

    backend/    Laravel 10 API, landlord + per-tenant connections, PHPUnit
    frontend/   Vue 3 SPA, Tailwind, Vitest + v8 coverage
    docker-compose.yml   MySQL 8

## Analyser expectations

Both workspaces emit coverage into `<workspace>/coverage/`:

* backend  — `coverage/cobertura.xml`, `coverage/clover.xml` (PHPUnit)
* frontend — `coverage/cobertura-coverage.xml`, `coverage/coverage-final.json`,
  `coverage/coverage-summary.json` (Vitest, v8 provider)

Dependencies are **not** committed. Run `composer install` and `npm ci` before any
tool that needs them (coverage, mutation, lint).

## Deliberate signals

`backend/app/Support/LegacyPricing.php` (complexity), `ReportBuilder`/`InvoiceBuilder`
(duplication), `frontend/src/utils/duplicate-*.ts` (duplication),
`frontend/src/utils/circular-*.ts` (cycle), `unused-export.ts` (dead code),
`fixtures/pii-sample.*` (PII). These exist so the analysers produce non-trivial values.

## ESLint configuration (deliberate)

The frontend ships **only** the legacy `.eslintrc.json`. No `eslint.config.js` is
present: on ESLint 8.57 the mere presence of a flat config makes the CLI reject
`--eslintrc` with exit 2, which is how analysers invoke it.

    npm run lint          # eslint --ext .ts,.vue src tests
    npm run lint:legacy   # eslint --eslintrc ...  (analyser style, exits 0)

## Verified locally

* frontend — 18 tests pass, 73.1% statement coverage, cobertura + json-summary emitted
* frontend lint (`--eslintrc`) — exit 0 with 43 warnings (real findings, no errors)
