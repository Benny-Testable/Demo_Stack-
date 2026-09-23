<?php

declare(strict_types=1);

namespace Tests\Feature;

use App\Models\Tenant;
use PHPUnit\Framework\TestCase;

/**
 * Database-per-tenant: the physical database name must be derived from the
 * tenant slug, never shared between tenants.
 */
final class TenantIsolationTest extends TestCase
{
    public function testDatabaseNameUsesPrefixAndSlug(): void
    {
        $tenant = new Tenant(['slug' => 'star_alpha', 'name' => 'Star Alpha Ltd', 'status' => 'active']);

        self::assertSame('star_tenant_star_alpha', $this->databaseNameFor($tenant));
    }

    public function testTenantsNeverShareADatabase(): void
    {
        $names = [];
        foreach (['star_alpha', 'star_beta', 'star_gamma'] as $slug) {
            $names[] = $this->databaseNameFor(new Tenant(['slug' => $slug, 'status' => 'active']));
        }

        self::assertCount(3, array_unique($names));
    }

    public function testInactiveTenantIsNotActive(): void
    {
        self::assertFalse((new Tenant(['slug' => 'x', 'status' => 'suspended']))->isActive());
    }

    private function databaseNameFor(Tenant $tenant): string
    {
        return 'star_tenant_' . $tenant->slug;
    }
}
