<?php

declare(strict_types=1);

return [
    // Physical database name = prefix . tenant slug
    'database_prefix' => env('TENANT_DB_PREFIX', 'star_tenant_'),

    // Connection used to resolve tenants, and the per-tenant template.
    'landlord_connection' => 'landlord',
    'tenant_connection' => 'tenant',

    // How an incoming request is attributed to a tenant.
    'resolvers' => ['header' => 'X-Tenant', 'domain' => true],

    'migrations_path' => 'database/migrations/tenant',
];
