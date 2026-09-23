<?php

declare(strict_types=1);

namespace Database\Seeders;

use App\Models\Record;
use App\Models\Tenant;
use App\Services\TenantManager;
use Illuminate\Database\Seeder;

class TenantRecordSeeder extends Seeder
{
    private const CATEGORIES = ['billing', 'support', 'onboarding', 'compliance'];

    public function __construct(private readonly TenantManager $tenants)
    {
    }

    public function run(): void
    {
        foreach (Tenant::query()->get() as $tenant) {
            $this->tenants->switchTo($tenant);

            for ($i = 1; $i <= 12; $i++) {
                Record::query()->create([
                    'title' => sprintf('%s record #%02d', $tenant->name, $i),
                    'body' => 'Seeded row for analyser test data.',
                    'category' => self::CATEGORIES[$i % count(self::CATEGORIES)],
                    'amount_cents' => $i * 1250,
                    'status' => $i % 4 === 0 ? 'closed' : 'open',
                ]);
            }
        }

        $this->tenants->forget();
    }
}
