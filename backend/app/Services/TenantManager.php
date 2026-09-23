<?php

declare(strict_types=1);

namespace App\Services;

use App\Models\Tenant;
use Illuminate\Database\DatabaseManager;
use Illuminate\Support\Facades\Config;
use RuntimeException;

/**
 * Switches the `tenant` connection to a given tenant's own MySQL database.
 * Database-per-tenant: no shared tables, no tenant_id columns.
 */
class TenantManager
{
    private ?Tenant $current = null;

    public function __construct(private readonly DatabaseManager $db)
    {
    }

    public function current(): ?Tenant
    {
        return $this->current;
    }

    public function resolveBySlug(string $slug): Tenant
    {
        $tenant = Tenant::query()->where('slug', $slug)->first();

        if ($tenant === null) {
            throw new RuntimeException("Unknown tenant: {$slug}");
        }

        if (! $tenant->isActive()) {
            throw new RuntimeException("Tenant is not active: {$slug}");
        }

        return $tenant;
    }

    public function switchTo(Tenant $tenant): void
    {
        $connection = config('tenancy.tenant_connection');

        Config::set("database.connections.{$connection}.database", $tenant->databaseName());
        $this->db->purge($connection);
        $this->db->reconnect($connection);

        $this->current = $tenant;
    }

    public function forget(): void
    {
        $connection = config('tenancy.tenant_connection');
        $this->db->purge($connection);
        $this->current = null;
    }

    /**
     * @return array<int, string>
     */
    public function databaseNames(): array
    {
        return Tenant::query()->get()->map(fn (Tenant $t): string => $t->databaseName())->all();
    }
}
