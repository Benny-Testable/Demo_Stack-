<?php

declare(strict_types=1);

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

/**
 * Tenant-side record. Always read through the `tenant` connection, which
 * TenantManager has already pointed at the current tenant's database.
 */
class Record extends Model
{
    protected $connection = 'tenant';
    protected $table = 'records';

    protected $fillable = ['title', 'body', 'category', 'amount_cents', 'status'];

    protected $casts = ['amount_cents' => 'integer', 'created_at' => 'datetime'];

    public function amount(): float
    {
        return $this->amount_cents / 100;
    }
}
