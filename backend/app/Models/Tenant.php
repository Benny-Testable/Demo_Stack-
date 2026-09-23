<?php

declare(strict_types=1);

namespace App\Models;

use Illuminate\Database\Eloquent\Model;

/**
 * Landlord-side record. One row per tenant; each owns a physical MySQL database.
 */
class Tenant extends Model
{
    protected $connection = 'landlord';
    protected $table = 'tenants';
    public $incrementing = false;
    protected $keyType = 'string';

    protected $fillable = ['id', 'name', 'slug', 'domain', 'plan', 'status'];

    protected $casts = ['created_at' => 'datetime', 'updated_at' => 'datetime'];

    public function databaseName(): string
    {
        return config('tenancy.database_prefix') . $this->slug;
    }

    public function isActive(): bool
    {
        return $this->status === 'active';
    }
}
