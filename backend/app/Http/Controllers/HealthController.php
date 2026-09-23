<?php

declare(strict_types=1);

namespace App\Http\Controllers;

use App\Services\TenantManager;
use Illuminate\Http\JsonResponse;

class HealthController
{
    public function __construct(private readonly TenantManager $tenants)
    {
    }

    public function health(): JsonResponse
    {
        return new JsonResponse(['status' => 'UP', 'timestamp' => gmdate('c')]);
    }

    public function ready(): JsonResponse
    {
        return new JsonResponse([
            'status' => 'READY',
            'tenant_databases' => count($this->tenants->databaseNames()),
        ]);
    }
}
