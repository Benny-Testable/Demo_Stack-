<?php

declare(strict_types=1);

namespace Database\Seeders;

use App\Models\Tenant;
use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;
use Ramsey\Uuid\Uuid;

/**
 * Sample tenants for this application. Each gets its own MySQL database,
 * created here and migrated with database/migrations/tenant.
 */
class LandlordSeeder extends Seeder
{
    private const TENANTS = [
        ['name' => 'Star Alpha Ltd', 'slug' => 'star_alpha', 'domain' => 'alpha.star.test', 'plan' => 'enterprise'],
        ['name' => 'Star Beta GmbH', 'slug' => 'star_beta', 'domain' => 'beta.star.test', 'plan' => 'growth'],
        ['name' => 'Star Gamma SARL', 'slug' => 'star_gamma', 'domain' => 'gamma.star.test', 'plan' => 'starter'],
    ];

    public function run(): void
    {
        foreach (self::TENANTS as $attributes) {
            $tenant = Tenant::query()->updateOrCreate(
                ['slug' => $attributes['slug']],
                $attributes + ['id' => Uuid::uuid4()->toString(), 'status' => 'active'],
            );

            DB::connection('landlord')->statement(
                sprintf('CREATE DATABASE IF NOT EXISTS `%s` CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci', $tenant->databaseName()),
            );
        }
    }
}
